using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Free.Core.Collections.Internal;

namespace Free.Core.Collections
{
	/// <summary>
	/// A hierarchical dictionary, using a <b>string</b> as keys, for storing parameters.
	/// </summary>
	[CLSCompliant(false)]
	public class Parameters : ICloneable, IEquatable<Parameters>, IEnumerable
	{
		const string InvalidNameMessage = "Must begin with a letter, a dollar-sign or an understroke, afterwards digits are allowed too.";
		const string TypeMismatchMessage = "Must be the type of the list to be added.";

		/// <summary>
		/// The storage for the data.
		/// </summary>
		Dictionary<string, object> items;

		#region Statics
		/// <summary>
		/// Determines whether a string is allowed as a name for <see cref="Parameters"/>.
		/// </summary>
		/// <remarks>
		/// <paramref name="name"/> must begin with [a-z], [A-Z], '_' or '$', afterwars digits are allowed, too.
		/// </remarks>
		/// <param name="name">The string to check.</param>
		/// <returns><b>true</b> if the string is allowed as a name for <see cref="Parameters"/>; otherwise, <b>false</b>.</returns>
		public static bool CheckName(string name)
		{
			if (name == null) throw new ArgumentNullException(nameof(name));

			int nameLength = name.Length;
			if (nameLength < 1) return false;

			char ch = name[0];

			if (!(ch >= 'a' && ch <= 'z') && !(ch >= 'A' && ch <= 'Z') && (ch != '$') && (ch != '_'))
				return false;

			int token = 1;

			while (token < nameLength)
			{
				ch = name[token++];

				if (!(ch >= 'a' && ch <= 'z') && !(ch >= 'A' && ch <= 'Z') &&
					!(ch >= '0' && ch <= '9') && (ch != '$') && (ch != '_'))
					return false;
			}

			return true;
		}

		/// <summary>
		/// Parses the list index from a string.
		/// </summary>
		/// <param name="name">The string to parse.</param>
		/// <param name="listname">Returns the name of the list.</param>
		/// <returns>The list index, if string contains a list index; otherwise, -1.</returns>
		static int ParseListIndex(string name, out string listname)
		{
			listname = string.Empty;

			if (name.Length < 4) return -1; // 4: name+index+brackets

			if (name[name.Length - 1] != ']') return -1; // Last char must be ']'.

			int index = name.IndexOf('[');
			if (index < 1) return -1; // Nothing found, or string begins with '['.

			string indexString = name.Substring(index + 1, name.Length - (index + 2));

			foreach (char ch in indexString) if (ch < '0' || ch > '9') return -1; // Only digits allowed, otherwise return -1.

			int ret = 0;
			if (!int.TryParse(indexString, out ret)) return -1;

			listname = name.Substring(0, index);
			return ret;
		}
		#endregion

		#region Factories
		/// <summary>
		/// Reads a PDL grammar formatted file and returns the content as <see cref="Parameters"/>.
		/// </summary>
		/// <param name="filename">The path to the file.</param>
		/// <returns>The parsed file content as <see cref="Parameters"/>.</returns>
		public static Parameters FromPDLFile(string filename)
		{
			if (filename == null) throw new ArgumentNullException(nameof(filename));

			if (!File.Exists(filename)) throw new FileNotFoundException("File does not exists.", filename);

			using (StreamReader streamReader = new StreamReader(filename, Encoding.UTF8, true))
			{
				return PDLParser.Parse(streamReader);
			}
		}

		/// <summary>
		/// Parses a PDL grammar formatted string and returns the content as <see cref="Parameters"/>.
		/// </summary>
		/// <param name="text">The text to be converted.</param>
		/// <param name="encoding">The text encoding to be used.</param>
		/// <returns>The parsed <paramref name="text"/> content as <see cref="Parameters"/>.</returns>
		public static Parameters FromPDLString(string text, Encoding encoding = null)
		{
			if (text == null) throw new ArgumentNullException(nameof(text));

			if (encoding == null) encoding = Encoding.Unicode;

			using (StreamReader streamReader = new StreamReader(new MemoryStream(encoding.GetBytes(text)), encoding))
			{
				return PDLParser.Parse(streamReader);
			}
		}

		/// <summary>
		/// Reads a PDL grammar formatted <see cref="Stream"/> and returns the content as <see cref="Parameters"/>.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to be parsed.</param>
		/// <param name="encoding">The text encoding to be used.</param>
		/// <returns>The parsed <paramref name="stream"/> content as <see cref="Parameters"/>.</returns>
		public static Parameters FromPDLStream(Stream stream, Encoding encoding = null)
		{
			if (stream == null) throw new ArgumentNullException(nameof(stream));

			if (encoding == null) encoding = Encoding.Unicode;

			using (StreamReader streamReader = new StreamReader(stream, encoding))
			{
				return PDLParser.Parse(streamReader);
			}
		}

		/// <summary>
		/// Reads a PDL grammar formatted <see cref="StreamReader"/> and returns the content as <see cref="Parameters"/>.
		/// </summary>
		/// <param name="stream">The <see cref="StreamReader"/> to be parsed.</param>
		/// <returns>The parsed <paramref name="stream"/> content as <see cref="Parameters"/>.</returns>
		public static Parameters FromPDLStream(StreamReader stream)
		{
			if (stream == null) throw new ArgumentNullException(nameof(stream));

			return PDLParser.Parse(stream);
		}

		/// <summary>
		/// Parses command line arguments into <see cref="Parameters"/>.
		/// </summary>
		/// <param name="args">The array of command line arguments.</param>
		/// <returns>The parsed command line as <see cref="Parameters"/>.</returns>
		public static Parameters InterpretCommandLine(string[] args)
		{
			if (args == null) throw new ArgumentNullException(nameof(args));

			Parameters ret = new Parameters();
			if (args.Length < 1) return ret;

			int filecounter = 0;
			foreach (string arg in args)
			{
				if (string.IsNullOrWhiteSpace(arg)) continue;

				if (arg[0] == '-')
				{
					int index = arg.IndexOf(':');
					if (index >= 0)
					{
						string key = arg.Substring(1, index - 1);
						if (!string.IsNullOrWhiteSpace(key)) ret.Add(key, arg.Substring(index + 1));
					}
					else
					{
						string key = arg.Substring(1);
						if (!string.IsNullOrWhiteSpace(key)) ret.Add(key, true);
					}
				}
				else
				{
					string key = string.Format("file{0:000}", filecounter++);
					ret.Add(key, arg);
				}
			}

			return ret;
		}

		#region MakeKeyValuePair
		/// <summary>
		/// Creates a key-value-pair as <see cref="Parameters"/>.
		/// </summary>
		/// <remarks>
		/// <code>
		/// {
		///    $name$=<paramref name="name"/>;
		///    $value$=<paramref name="value"/>;
		/// }
		/// </code>
		/// </remarks>
		/// <param name="name">The name of the key-value-pair.</param>
		/// <param name="value">The value of the key-value-pair.</param>
		/// <returns>A <see cref="Parameters"/> containing the key-value-pair.</returns>
		/// <overloads>These functions create a key-value-pair <see cref="Parameters"/>.</overloads>
		public static Parameters MakeKeyValuePair(string name, bool value)
		{
			Parameters keyValuePair = new Parameters();
			keyValuePair.Add("$name$", name);
			keyValuePair.Add("$value$", value);
			return keyValuePair;
		}

		/// <summary>
		/// Creates a key-value-pair as <see cref="Parameters"/>.
		/// </summary>
		/// <remarks>
		/// <code>
		/// {
		///    $name$=<paramref name="name"/>;
		///    $value$=<paramref name="value"/>;
		/// }
		/// </code>
		/// </remarks>
		/// <param name="name">The name of the key-value-pair.</param>
		/// <param name="value">The value of the key-value-pair.</param>
		/// <returns>A <see cref="Parameters"/> containing the key-value-pair.</returns>
		public static Parameters MakeKeyValuePair(string name, List<bool> value)
		{
			Parameters keyValuePair = new Parameters();
			keyValuePair.Add("$name$", name);
			keyValuePair.Add("$value$", value);
			return keyValuePair;
		}

		/// <summary>
		/// Creates a key-value-pair as <see cref="Parameters"/>.
		/// </summary>
		/// <remarks>
		/// <code>
		/// {
		///    $name$=<paramref name="name"/>;
		///    $value$=<paramref name="value"/>;
		/// }
		/// </code>
		/// </remarks>
		/// <param name="name">The name of the key-value-pair.</param>
		/// <param name="value">The value of the key-value-pair.</param>
		/// <returns>A <see cref="Parameters"/> containing the key-value-pair.</returns>
		public static Parameters MakeKeyValuePair(string name, string value)
		{
			Parameters keyValuePair = new Parameters();
			keyValuePair.Add("$name$", name);
			keyValuePair.Add("$value$", value);
			return keyValuePair;
		}

		/// <summary>
		/// Creates a key-value-pair as <see cref="Parameters"/>.
		/// </summary>
		/// <remarks>
		/// <code>
		/// {
		///    $name$=<paramref name="name"/>;
		///    $value$=<paramref name="value"/>;
		/// }
		/// </code>
		/// </remarks>
		/// <param name="name">The name of the key-value-pair.</param>
		/// <param name="value">The value of the key-value-pair.</param>
		/// <returns>A <see cref="Parameters"/> containing the key-value-pair.</returns>
		public static Parameters MakeKeyValuePair(string name, List<string> value)
		{
			Parameters keyValuePair = new Parameters();
			keyValuePair.Add("$name$", name);
			keyValuePair.Add("$value$", value);
			return keyValuePair;
		}

		/// <summary>
		/// Creates a key-value-pair as <see cref="Parameters"/>.
		/// </summary>
		/// <remarks>
		/// <code>
		/// {
		///    $name$=<paramref name="name"/>;
		///    $value$=<paramref name="value"/>;
		/// }
		/// </code>
		/// </remarks>
		/// <param name="name">The name of the key-value-pair.</param>
		/// <param name="value">The value of the key-value-pair.</param>
		/// <returns>A <see cref="Parameters"/> containing the key-value-pair.</returns>
		public static Parameters MakeKeyValuePair(string name, double value)
		{
			Parameters keyValuePair = new Parameters();
			keyValuePair.Add("$name$", name);
			keyValuePair.Add("$value$", value);
			return keyValuePair;
		}

		/// <summary>
		/// Creates a key-value-pair as <see cref="Parameters"/>.
		/// </summary>
		/// <remarks>
		/// <code>
		/// {
		///    $name$=<paramref name="name"/>;
		///    $value$=<paramref name="value"/>;
		/// }
		/// </code>
		/// </remarks>
		/// <param name="name">The name of the key-value-pair.</param>
		/// <param name="value">The value of the key-value-pair.</param>
		/// <returns>A <see cref="Parameters"/> containing the key-value-pair.</returns>
		public static Parameters MakeKeyValuePair(string name, List<double> value)
		{
			Parameters keyValuePair = new Parameters();
			keyValuePair.Add("$name$", name);
			keyValuePair.Add("$value$", value);
			return keyValuePair;
		}

		/// <summary>
		/// Creates a key-value-pair as <see cref="Parameters"/>.
		/// </summary>
		/// <remarks>
		/// <code>
		/// {
		///    $name$=<paramref name="name"/>;
		///    $value$=<paramref name="value"/>;
		/// }
		/// </code>
		/// </remarks>
		/// <param name="name">The name of the key-value-pair.</param>
		/// <param name="value">The value of the key-value-pair.</param>
		/// <returns>A <see cref="Parameters"/> containing the key-value-pair.</returns>
		public static Parameters MakeKeyValuePair(string name, int value)
		{
			Parameters keyValuePair = new Parameters();
			keyValuePair.Add("$name$", name);
			keyValuePair.Add("$value$", value);
			return keyValuePair;
		}

		/// <summary>
		/// Creates a key-value-pair as <see cref="Parameters"/>.
		/// </summary>
		/// <remarks>
		/// <code>
		/// {
		///    $name$=<paramref name="name"/>;
		///    $value$=<paramref name="value"/>;
		/// }
		/// </code>
		/// </remarks>
		/// <param name="name">The name of the key-value-pair.</param>
		/// <param name="value">The value of the key-value-pair.</param>
		/// <returns>A <see cref="Parameters"/> containing the key-value-pair.</returns>
		public static Parameters MakeKeyValuePair(string name, long value)
		{
			Parameters keyValuePair = new Parameters();
			keyValuePair.Add("$name$", name);
			keyValuePair.Add("$value$", value);
			return keyValuePair;
		}

		/// <summary>
		/// Creates a key-value-pair as <see cref="Parameters"/>.
		/// </summary>
		/// <remarks>
		/// <code>
		/// {
		///    $name$=<paramref name="name"/>;
		///    $value$=<paramref name="value"/>;
		/// }
		/// </code>
		/// </remarks>
		/// <param name="name">The name of the key-value-pair.</param>
		/// <param name="value">The value of the key-value-pair.</param>
		/// <returns>A <see cref="Parameters"/> containing the key-value-pair.</returns>
		public static Parameters MakeKeyValuePair(string name, List<long> value)
		{
			Parameters keyValuePair = new Parameters();
			keyValuePair.Add("$name$", name);
			keyValuePair.Add("$value$", value);
			return keyValuePair;
		}

		/// <summary>
		/// Creates a key-value-pair as <see cref="Parameters"/>.
		/// </summary>
		/// <remarks>
		/// <code>
		/// {
		///    $name$=<paramref name="name"/>;
		///    $value$=<paramref name="value"/>;
		/// }
		/// </code>
		/// </remarks>
		/// <param name="name">The name of the key-value-pair.</param>
		/// <param name="value">The value of the key-value-pair.</param>
		/// <returns>A <see cref="Parameters"/> containing the key-value-pair.</returns>
		public static Parameters MakeKeyValuePair(string name, Parameters value)
		{
			Parameters keyValuePair = new Parameters();
			keyValuePair.Add("$name$", name);
			keyValuePair.Add("$value$", value);
			return keyValuePair;
		}

		/// <summary>
		/// Creates a key-value-pair as <see cref="Parameters"/>.
		/// </summary>
		/// <remarks>
		/// <code>
		/// {
		///    $name$=<paramref name="name"/>;
		///    $value$=<paramref name="value"/>;
		/// }
		/// </code>
		/// </remarks>
		/// <param name="name">The name of the key-value-pair.</param>
		/// <param name="value">The value of the key-value-pair.</param>
		/// <returns>A <see cref="Parameters"/> containing the key-value-pair.</returns>
		public static Parameters MakeKeyValuePair(string name, List<Parameters> value)
		{
			Parameters keyValuePair = new Parameters();
			keyValuePair.Add("$name$", name);
			keyValuePair.Add("$value$", value);
			return keyValuePair;
		}
		#endregion
		#endregion

		/// <summary>
		/// Creates an empty instance of <see cref="Parameters"/>.
		/// </summary>
		public Parameters()
		{
			items = new Dictionary<string, object>();
		}

		/// <summary>
		/// Retrieves the names of all parameters in the <see cref="Parameters"/>.
		/// </summary>
		/// <returns>A list of the names of all parameters in the <see cref="Parameters"/>.</returns>
		public string[] GetNames()
		{
			string[] ret = new string[items.Keys.Count];
			items.Keys.CopyTo(ret, 0);
			return ret;
		}

		/// <summary>
		/// Retrieves the number of parameters in the <see cref="Parameters"/>.
		/// </summary>
		public int Count { get { return items.Count; } }

		/// <summary>
		/// Retrieves an enumerable for the names in the <see cref="Parameters"/>.
		/// </summary>
		public IEnumerable<string> Keys { get { return items.Keys; } }

		/// <summary>
		/// Determines whether the <see cref="Parameters"/> is empty.
		/// </summary>
		/// <returns><b>true</b> if the <see cref="Parameters"/> is empty; otherwise, <b>false</b>.</returns>
		public bool IsEmpty() { return items.Count == 0; }

		#region Get
		/// <summary>
		/// Retrieves an object from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the object.</param>
		/// <param name="defaultValue">Fallback value.</param>
		/// <returns>The requested object, if successful; otherwise, <paramref name="defaultValue"/>.</returns>
		public object Get(string name, object defaultValue = null)
		{
			if (string.IsNullOrWhiteSpace(name)) return defaultValue;

			// Canonize key.
			string key = name.Replace('\\', '/');
			while (key.IndexOf("//") != -1) key = key.Replace("//", "/");
			key = key.Trim('/');

			// Check key.
			if (key.Length == 0) return defaultValue;

			return GetInternal(key, defaultValue);
		}

		object GetInternal(string name, object defaultValue)
		{
			int indexSubKey = name.IndexOf('/');

			if (indexSubKey == -1) return GetEntry(name, defaultValue);

			string key = name.Substring(0, indexSubKey);
			string subKey = name.Substring(indexSubKey + 1);

			string listname = string.Empty;
			int index = ParseListIndex(key, out listname);

			Parameters parameters = null;
			if (index == -1)
			{
				if (!CheckName(key)) return defaultValue;
				if (!items.ContainsKey(key)) return defaultValue;

				parameters = items[key] as Parameters;
				if (parameters == null) return defaultValue;

				return parameters.GetInternal(subKey, defaultValue);
			}

			if (!CheckName(listname)) return defaultValue;
			if (!items.ContainsKey(listname)) return defaultValue;

			List<Parameters> parameterslist = items[listname] as List<Parameters>;
			if (parameterslist == null) return defaultValue;

			if (index >= parameterslist.Count) return defaultValue;

			parameters = parameterslist[index];
			return parameters.GetInternal(subKey, defaultValue);
		}

		object GetEntry(string name, object defaultValue)
		{
			string listname = string.Empty;
			int index = ParseListIndex(name, out listname);

			if (index == -1)
			{
				if (!CheckName(name)) return defaultValue;
				if (!items.ContainsKey(name)) return defaultValue;

				return items[name];
			}

			if (!CheckName(listname)) return defaultValue;
			if (!items.ContainsKey(listname)) return defaultValue;

			IList list = Get(listname) as IList;
			if (list == null) return defaultValue;

			if (index >= list.Count) return defaultValue;

			return list[index];
		}
		#endregion

		#region Put
		/// <summary>
		/// Puts a value into a (sub-parameters) storage. Called recursive for each sub-parameters in <paramref name="path"/>.
		/// </summary>
		/// <param name="path">The path (name) for the value.</param>
		/// <param name="value">The value to be stored.</param>
		void Put(string path, object value)
		{
			// Canonize key.
			string key = path.Replace('\\', '/');
			while (key.IndexOf("//") != -1) key = key.Replace("//", "/");
			key = key.Trim('/');

			// Check key.
			if (key.Length == 0) throw new ArgumentException(InvalidNameMessage, nameof(path));

			int index = key.IndexOf('/');

			if (index == -1)
			{
				PutEntry(key, value);
				return;
			}

			string name = key.Substring(0, index);
			string subKey = key.Substring(index + 1);

			// If name is a Parameters, put value in it; otherwise create new sub-parameters.
			Parameters parameter = Get(name) as Parameters;
			if (parameter != null) parameter.Put(subKey, value);
			else PutIntoNewSubParameters(key, value);
		}

		/// <summary>
		/// Puts a value into a new sub-parameter storage. Called recursive for each sub-parameters in <paramref name="path"/>.
		/// </summary>
		/// <param name="path">The path (name) for the value.</param>
		/// <param name="value">The value to be stored.</param>
		void PutIntoNewSubParameters(string path, object value)
		{
			int index = path.IndexOf('/');

			// Need another sub-parameters?
			if (index != -1)
			{
				string name = path.Substring(0, index);
				string subKey = path.Substring(index + 1);

				Parameters newParameters = new Parameters();
				newParameters.PutIntoNewSubParameters(subKey, value);

				PutEntry(name, newParameters);
				return;
			}

			PutEntry(path, value);
		}

		/// <summary>
		/// Puts a value into the storage.
		/// </summary>
		/// <param name="name">The name for the value.</param>
		/// <param name="value">The value to be stored.</param>
		void PutEntry(string name, object value)
		{
			string listname = string.Empty;
			int index = ParseListIndex(name, out listname);

			if (index == -1)
			{
				#region Not ???[index]
				string nameBrackets = string.Empty;
				if (name.Length > 2) nameBrackets = name.Substring(name.Length - 2);

				// A index-less list?
				if (nameBrackets == "[]")
				{
					string nameWithoutBrackets = name.Substring(0, name.Length - 2);

					if (!CheckName(nameWithoutBrackets)) throw new ArgumentException(InvalidNameMessage, nameof(name));

					if (items.ContainsKey(nameWithoutBrackets))
					{
						IList iList = items[nameWithoutBrackets] as IList;

						if (iList != null)
						{
							if (iList is List<bool> && value is bool)
							{
								List<bool> list = (List<bool>)iList;
								list.Add((bool)value);
							}
							else if (iList is List<long> && value is long)
							{
								List<long> list = (List<long>)iList;
								list.Add((long)value);
							}
							else if (iList is List<double> && value is double)
							{
								List<double> list = (List<double>)iList;
								list.Add((double)value);
							}
							else if (iList is List<double> && value is long) // ints too
							{
								List<double> list = (List<double>)iList;
								list.Add((long)value);
							}
							else if (iList is List<string> && value is string)
							{
								List<string> list = (List<string>)iList;
								list.Add((string)value);
							}
							else if (iList is List<Parameters> && value is Parameters)
							{
								List<Parameters> list = (List<Parameters>)iList;
								list.Add((Parameters)value);
							}
							else throw new ArgumentException(TypeMismatchMessage, nameof(value));

							return;
						}

						// If not a list remove the item (new list will be added below).
						items.Remove(nameWithoutBrackets);
					}

					// Not found (or removed, because of wrong type)? Add new list.
					if (value is bool)
					{
						List<bool> list = new List<bool>();
						list.Add((bool)value);
						items.Add(nameWithoutBrackets, list);
					}
					else if (value is long)
					{
						List<long> list = new List<long>();
						list.Add((long)value);
						items.Add(nameWithoutBrackets, list);
					}
					else if (value is double)
					{
						List<double> list = new List<double>();
						list.Add((double)value);
						items.Add(nameWithoutBrackets, list);
					}
					else if (value is string)
					{
						List<string> list = new List<string>();
						list.Add((string)value);
						items.Add(nameWithoutBrackets, list);
					}
					else if (value is Parameters)
					{
						List<Parameters> list = new List<Parameters>();
						list.Add((Parameters)value);
						items.Add(nameWithoutBrackets, list);
					}
					else throw new ArgumentException(TypeMismatchMessage, nameof(value));
				}
				else // Not a list.
				{
					if (!CheckName(name)) throw new ArgumentException(InvalidNameMessage, nameof(name));
					items[name]=value;
				}
				#endregion
			}
			else
			{
				#region ???[index]
				if (!CheckName(listname)) throw new ArgumentException(InvalidNameMessage, nameof(name));

				if (items.ContainsKey(listname))
				{
					ICollection collection = items[listname] as ICollection;

					if (collection != null)
					{
						if (index > collection.Count) throw new IndexOutOfRangeException("Argument: name contains index greater than count of list.");

						// Replace or add.
						if (collection is List<bool> && value is bool)
						{
							List<bool> list = (List<bool>)collection;
							if (index < list.Count) list[index] = (bool)value;
							else list.Add((bool)value);
						}
						else if (collection is List<long> && value is long)
						{
							List<long> list = (List<long>)collection;
							if (index < list.Count) list[index] = (long)value;
							else list.Add((long)value);
						}
						else if (collection is List<double> && value is double)
						{
							List<double> list = (List<double>)collection;
							if (index < list.Count) list[index] = (double)value;
							else list.Add((double)value);
						}
						else if (collection is List<double> && value is long) // ints too
						{
							List<double> list = (List<double>)collection;
							if (index < list.Count) list[index] = (long)value;
							else list.Add((long)value);
						}
						else if (collection is List<string> && value is string)
						{
							List<string> list = (List<string>)collection;
							if (index < list.Count) list[index] = (string)value;
							else list.Add((string)value);
						}
						else if (collection is List<Parameters> && value is Parameters)
						{
							List<Parameters> list = (List<Parameters>)collection;
							if (index < list.Count) list[index] = (Parameters)value;
							else list.Add((Parameters)value);
						}
						else throw new ArgumentException(TypeMismatchMessage, nameof(value));

						return;
					}
				}

				// Not a list (or not in parameters yet) => remove and create a new list.
				if (index != 0) // Only '[0]' is allow.
					throw new IndexOutOfRangeException("Argument: name contains index (greater 0) to an object that is not a list, or not in Parameters.");

				items.Remove(listname);

				if (value is bool)
				{
					List<bool> list = new List<bool>();
					list.Add((bool)value);
					items.Add(listname, list);
				}
				else if (value is long)
				{
					List<long> list = new List<long>();
					list.Add((long)value);
					items.Add(listname, list);
				}
				else if (value is double)
				{
					List<double> list = new List<double>();
					list.Add((double)value);
					items.Add(listname, list);
				}
				else if (value is string)
				{
					List<string> list = new List<string>();
					list.Add((string)value);
					items.Add(listname, list);
				}
				else if (value is Parameters)
				{
					List<Parameters> list = new List<Parameters>();
					list.Add((Parameters)value);
					items.Add(listname, list);
				}
				else throw new ArgumentException(TypeMismatchMessage, nameof(value));

				#endregion
			}
		}
		#endregion

		#region Contains
		/// <summary>
		/// Determines whether a value with a specified (path) name exists in the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name of the value.</param>
		/// <returns><b>true</b> if the value exists in the <see cref="Parameters"/>; otherwise, <b>false</b>.</returns>
		public bool Contains(string name)
		{
			if (string.IsNullOrWhiteSpace(name)) return false;

			// Canonize key.
			string key = name.Replace('\\', '/');
			while (key.IndexOf("//") != -1) key = key.Replace("//", "/");
			key = key.Trim('/');

			// Check key.
			if (key.Length == 0) return false;

			return ContainsInternal(key);
		}

		bool ContainsInternal(string name)
		{
			int indexSubKey = name.IndexOf('/');

			if (indexSubKey == -1) return ContainsEntry(name);

			string key = name.Substring(0, indexSubKey);
			string subkey = name.Substring(indexSubKey + 1);

			string listname = string.Empty;
			int index = ParseListIndex(key, out listname);

			Parameters parameters = null;
			if (index == -1)
			{
				if (!CheckName(key)) return false;
				if (!items.ContainsKey(key)) return false;

				parameters = items[key] as Parameters;
				if (parameters == null) return false;

				return parameters.ContainsInternal(subkey);
			}

			if (!CheckName(listname)) return false;
			if (!items.ContainsKey(listname)) return false;

			List<Parameters> parameterslist = items[listname] as List<Parameters>;
			if (parameterslist == null) return false;

			if (index >= parameterslist.Count) return false;

			parameters = parameterslist[index];
			return parameters.ContainsInternal(subkey);
		}

		bool ContainsEntry(string name)
		{
			string listname = string.Empty;
			int index = ParseListIndex(name, out listname);

			if (index == -1)
			{
				if (!CheckName(name)) return false;
				return items.ContainsKey(name);
			}

			if (!CheckName(listname)) return false;
			if (!items.ContainsKey(listname)) return false;

			IList list = Get(listname) as IList;
			if (list == null) return false;

			return index < list.Count;
		}
		#endregion

		#region Typed Add methods
		/// <summary>
		/// Adds a <paramref name="value"/> to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		/// <overloads>These functions add values to the <see cref="Parameters"/>.</overloads>
		public Parameters Add(string name, bool value) { Put(name, value); return this; }

		/// <summary>
		/// Adds a <paramref name="value"/> to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, sbyte value) { Put(name, (long)value); return this; }

		/// <summary>
		/// Adds a <paramref name="value"/> to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, short value) { Put(name, (long)value); return this; }

		/// <summary>
		/// Adds a <paramref name="value"/> to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, int value) { Put(name, (long)value); return this; }

		/// <summary>
		/// Adds a <paramref name="value"/> to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, long value) { Put(name, value); return this; }

		/// <summary>
		/// Adds a <paramref name="value"/> to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, byte value) { Put(name, (long)value); return this; }

		/// <summary>
		/// Adds a <paramref name="value"/> to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, ushort value) { Put(name, (long)value); return this; }

		/// <summary>
		/// Adds a <paramref name="value"/> to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, uint value) { Put(name, (long)value); return this; }

		/// <summary>
		/// Adds a <paramref name="value"/> to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, ulong value) { Put(name, (long)value); return this; }

		/// <summary>
		/// Adds a <paramref name="value"/> to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, float value) { Put(name, (double)value); return this; }

		/// <summary>
		/// Adds a <paramref name="value"/> to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, double value) { Put(name, value); return this; }

		/// <summary>
		/// Adds a <paramref name="value"/> to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, string value) { Put(name, value); return this; }

		/// <summary>
		/// Adds a <paramref name="value"/> to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, Parameters value) { Put(name, value); return this; }

		/// <summary>
		/// Adds a <paramref name="value"/> to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, byte[] value) { Put(name, value); return this; }

		/// <summary>
		/// Adds a <paramref name="value"/> to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, List<bool> value) { Put(name, value); return this; }

		/// <summary>
		/// Adds a <paramref name="value"/> to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, List<long> value) { Put(name, value); return this; }

		/// <summary>
		/// Adds a <paramref name="value"/> to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, List<double> value) { Put(name, value); return this; }

		/// <summary>
		/// Adds a <paramref name="value"/> to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, List<string> value) { Put(name, value); return this; }

		/// <summary>
		/// Adds a <paramref name="value"/> to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, string[] value) { Put(name, new List<string>(value)); return this; }

		/// <summary>
		/// Adds a <paramref name="value"/> to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, List<Parameters> value) { Put(name, value); return this; }

		/// <summary>
		/// Inserts (shallow add) all <paramref name="values"/> from an <see cref="Parameters"/> into this <see cref="Parameters"/>.
		/// </summary>
		/// <param name="values">The values to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(Parameters values)
		{
			foreach (string key in values.items.Keys) Put(key, values.items[key]);

			return this;
		}

		/// <summary>
		/// Recursive adds all <paramref name="values"/> from an <see cref="Parameters"/> into this <see cref="Parameters"/>.
		/// Existing values (except <see cref="Parameters"/>) will be overwritten.
		/// </summary>
		/// <param name="values">The values to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters AddDeep(Parameters values)
		{
			foreach (string key in values.items.Keys)
			{
				// If not existsing yet, just add it.
				if (!items.ContainsKey(key))
				{
					Put(key, values.items[key]);
					continue;
				}

				object thisValue = items[key];
				bool isThisValueParameters = thisValue is Parameters;

				object thatValue = values.items[key];
				bool isThatValueParameters = thatValue is Parameters;

				// Both Parameters => add recursive.
				if (isThisValueParameters && isThatValueParameters)
				{
					((Parameters)thisValue).AddDeep((Parameters)thatValue);
					continue;
				}

				// Both non-Parameters => replace.
				if (!isThisValueParameters && !isThatValueParameters)
				{
					Put(key, thatValue);
					continue;
				}

				Parameters parameters;

				if (isThisValueParameters)
				{
					parameters = (Parameters)thisValue;
					object value = thatValue;

					// Store non-Parameters as "$default$" entry in the Parameters.
					parameters.Put("$default$", value);
				}
				else
				{
					parameters = (Parameters)thatValue;
					object value = thisValue;

					// Store non-Parameters as "$default$" entry in the Parameters.
					parameters.Put("$default$", value);

					Add(key, parameters);
				}
			}

			return this;
		}
		#endregion

		#region Typed Get methods
		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the value.</param>
		/// <param name="defaultValue">Fallback value.</param>
		/// <returns>The requested value, if successful; otherwise, <paramref name="defaultValue"/>.</returns>
		public bool GetBool(string name, bool defaultValue = false)
		{
			object value = Get(name, defaultValue);

			if (value is bool) return (bool)value;
			if (value is long) return (long)value != 0;
			if (value is double) return (double)value != 0;

			if (value is string)
			{
				string stringValue = (string)value;
				stringValue = stringValue.ToLower();
				if (stringValue == "true") return true;
				else if (stringValue == "false") return false;
			}

			return defaultValue;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the value.</param>
		/// <param name="defaultValue">Fallback value.</param>
		/// <returns>The requested value, if successful; otherwise, <paramref name="defaultValue"/>.</returns>
		public int GetInt32(string name, int defaultValue = 0)
		{
			return (int)GetInt(name, defaultValue);
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the value.</param>
		/// <param name="defaultValue">Fallback value.</param>
		/// <returns>The requested value, if successful; otherwise, <paramref name="defaultValue"/>.</returns>
		public long GetInt(string name, long defaultValue = 0)
		{
			object value = Get(name, defaultValue);

			if (value is long) return (long)value;
			if (value is double) return (long)(double)value;

			return defaultValue;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the value.</param>
		/// <param name="defaultValue">Fallback value.</param>
		/// <returns>The requested value, if successful; otherwise, <paramref name="defaultValue"/>.</returns>
		public double GetDouble(string name, double defaultValue = 0)
		{
			object value = Get(name, defaultValue);

			if (value is long) return (long)value;
			if (value is double) return (double)value;

			return defaultValue;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the value.</param>
		/// <param name="defaultValue">Fallback value.</param>
		/// <returns>The requested value, if successful; otherwise, <paramref name="defaultValue"/>.</returns>
		public string GetString(string name, string defaultValue = "")
		{
			object value = Get(name, defaultValue);

			if (value is string) return value.ToString();

			return defaultValue;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the value.</param>
		/// <param name="defaultValue">Fallback value.</param>
		/// <returns>The requested value, if successful; otherwise, <paramref name="defaultValue"/>.</returns>
		public Parameters GetParameters(string name, Parameters defaultValue = null)
		{
			object value = Get(name, defaultValue);

			if (value is Parameters) return (Parameters)value;

			return defaultValue;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the value.</param>
		/// <param name="defaultValue">Fallback value.</param>
		/// <returns>The requested value, if successful; otherwise, <paramref name="defaultValue"/>.</returns>
		public byte[] GetDataBlock(string name, byte[] defaultValue = null)
		{
			object value = Get(name, defaultValue);

			if (value is byte[]) return (byte[])value;

			return defaultValue;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the value.</param>
		/// <param name="defaultValue">Fallback value.</param>
		/// <returns>The requested value, if successful; otherwise, <paramref name="defaultValue"/>.</returns>
		public List<bool> GetBoolList(string name, List<bool> defaultValue = null)
		{
			object value = Get(name, defaultValue);

			if (value is List<bool>) return (List<bool>)value;

			return defaultValue;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the value.</param>
		/// <param name="defaultValue">Fallback value.</param>
		/// <returns>The requested value, if successful; otherwise, <paramref name="defaultValue"/>.</returns>
		public List<long> GetIntList(string name, List<long> defaultValue = null)
		{
			object value = Get(name, defaultValue);

			if (value is List<long>) return (List<long>)value;

			return defaultValue;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the value.</param>
		/// <param name="defaultValue">Fallback value.</param>
		/// <returns>The requested value, if successful; otherwise, <paramref name="defaultValue"/>.</returns>
		public List<double> GetDoubleList(string name, List<double> defaultValue = null)
		{
			object value = Get(name, defaultValue);

			if (value is List<double>) return (List<double>)value;

			return defaultValue;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the value.</param>
		/// <param name="defaultValue">Fallback value.</param>
		/// <returns>The requested value, if successful; otherwise, <paramref name="defaultValue"/>.</returns>
		public List<string> GetStringList(string name, List<string> defaultValue = null)
		{
			object value = Get(name, defaultValue);

			if (value is List<string>) return (List<string>)value;

			return defaultValue;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the value.</param>
		/// <param name="defaultValue">Fallback value.</param>
		/// <returns>The requested value, if successful; otherwise, <paramref name="defaultValue"/>.</returns>
		public List<Parameters> GetParametersList(string name, List<Parameters> defaultValue = null)
		{
			object value = Get(name, defaultValue);

			if (value is List<Parameters>) return (List<Parameters>)value;

			return defaultValue;
		}

		/// <summary>
		/// Retrieves a key-value-pair <see cref="Parameters"/> from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the object.</param>
		/// <returns>A dictionary containing the key-value-pairs.</returns>
		public Dictionary<string, object> GetKeyValueList(string name)
		{
			Dictionary<string, object> ret = new Dictionary<string, object>();

			List<Parameters> parameterList = GetParametersList(name);

			if (parameterList != null)
			{
				foreach (Parameters param in parameterList)
				{
					ret.Add(param.GetString("$name$"), param.Get("$value$", null));
				}
			}

			return ret;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the value.</param>
		/// <param name="defaultValue">Fallback value.</param>
		/// <returns>The requested value, if successful; otherwise, <paramref name="defaultValue"/>.</returns>
		public List<double> GetAsDoubleList(string name, List<double> defaultValue = null)
		{
			object value = Get(name, defaultValue);

			if (value is List<double>) return (List<double>)value;
			if (value is List<long>)
			{
				List<long> longList = (List<long>)value;
				List<double> ret = new List<double>(longList.Count);
				for (int i = 0; i < longList.Count; i++) ret.Add(longList[i]);
				return ret;
			}

			return defaultValue;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the value.</param>
		/// <returns>The requested value, if successful; otherwise, an empty string is returned.</returns>
		public string GetAsString(string name)
		{
			object value = Get(name, string.Empty);
			return value.ToString();
		}

		/// <summary>
		/// Retrieves all values, that start with "file".
		/// </summary>
		/// <returns>A list of all values, that start with "file".</returns>
		public List<string> GetFilesFromParameters()
		{
			List<string> ret = new List<string>();

			foreach (string i in GetNames())
			{
				if (i.StartsWith("file")) ret.Add(GetString(i));
			}

			return ret;
		}
		#endregion

		#region GetHash
		static long GetHash(double value, long hashCode)
		{
			unchecked
			{
				hashCode += Math.Abs(((int)value) + 17);
				return Math.Abs(hashCode * 1234597);
			}
		}

		static long GetHash(long value, long hashCode)
		{
			unchecked
			{
				hashCode += Math.Abs(((int)value) + 17);
				return Math.Abs(hashCode * 1234597);
			}
		}

		static long GetHash(string value, long hashCode)
		{
			unchecked
			{
				int hc = 0;
				int offset = 0;
				int length = value.Length;

				if (length < 16)
				{
					for (int i = length; i > 0; i--) hc = (hc * 37) + value[offset++];
				}
				else
				{
					// only sample some characters
					int skip = length / 8;
					for (int i = length; i > 0; i -= skip, offset += skip) hc = (hc * 39) + value[offset];
				}

				hashCode += Math.Abs(hc);
				return Math.Abs(hashCode * 1234597);
			}
		}

		static long GetHash(bool value, long hashCode)
		{
			unchecked
			{
				hashCode += Math.Abs(value ? 1231 : 1237);
				return Math.Abs(hashCode* 1234597);
			}
		}

		static long GetHash(Parameters value, long hashCode)
		{
			unchecked
			{
				if (value == null) hashCode += 1345;
				else
				{
					List<string> keys = new List<string>(value.items.Keys);
					keys.Sort(string.CompareOrdinal);
					foreach (string key in keys)
					{
						hashCode = GetHash(key, hashCode);
						object v = value.Get(key);
						if (v == null) hashCode = GetHash((Parameters)null, hashCode);
						else if (v is bool) hashCode = GetHash((bool)v, hashCode);
						else if (v is double) hashCode = GetHash((double)v, hashCode);
						else if (v is long) hashCode = GetHash((long)v, hashCode);
						else if (v is string) hashCode = GetHash((string)v, hashCode);
						else if (v is Parameters) hashCode = GetHash((Parameters)v, hashCode);
						else hashCode = Math.Abs(hashCode * 1234597);
					}
				}

				return Math.Abs(hashCode* 1234597);
			}
		}

		static string GetHashString(Parameters value)
		{
			long hashCode = GetHash(value, 13);

			string ret = "1-";
			while (hashCode > 0)
			{
				ret += (char)(hashCode % 26 + 'a');
				hashCode /= 17;
			}
			return ret;
		}

		long GetHash(long hashCode)
		{
			return GetHash(this, hashCode);
		}

		/// <summary>
		/// Generate a hash string of the <see cref="Parameters"/>.
		/// </summary>
		/// <returns>The hash as <b>string</b>.</returns>
		public string GetHashString()
		{
			return GetHashString(this);
		}
		#endregion

		#region Remove & Clear
		/// <summary>
		/// Removes all values from a <see cref="Parameters"/>.
		/// </summary>
		public void Clear() { items.Clear(); }

		/// <summary>
		/// Removes a value from a <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name of the value.</param>
		/// <returns><b>true</b> if successful; otherwise, <b>false</b>.</returns>
		public bool Remove(string name)
		{
			if (string.IsNullOrWhiteSpace(name)) return false;

			// Canonize key.
			string key = name.Replace('\\', '/');
			while (key.IndexOf("//") != -1) key = key.Replace("//", "/");
			key = key.Trim('/');

			// Check key.
			if (key.Length == 0) return false;

			int index = key.LastIndexOf('/');

			if (index == -1)
			{
				string listname = string.Empty;
				int listIndex = ParseListIndex(key, out listname);

				if (listIndex == -1)
				{
					if (!CheckName(key)) return false;
					return items.Remove(key);
				}

				if (!CheckName(listname)) return false;
				if (!items.ContainsKey(listname)) return false;

				IList list = Get(listname) as IList;
				if (list == null) return false;

				if (listIndex != list.Count - 1) return false;

				try
				{
					list.RemoveAt(listIndex);
				}
				catch
				{
					return false;
				}

				return true;
			}

			name = key.Substring(0, index);
			string subKey = key.Substring(index + 1);

			Parameters parameters = Get(name) as Parameters;
			if (parameters == null) return false;

			return parameters.Remove(subKey);
		}
		#endregion

		#region PDL
		/// <summary>
		/// Retrieves a PDL grammar encoded text representation of the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="cooked">Set <b>true</b> to create a string formatted for human reading.</param>
		/// <returns>The <b>string</b> containing the converted <see cref="Parameters"/>.</returns>
		public string ToPDLString(bool cooked = true)
		{
			return ParametersToPDLConverter.ParametersToPDL(this, cooked);
		}

		/// <summary>
		/// Saves the content of the <see cref="Parameters"/> into a file using PDL grammar.
		/// </summary>
		/// <param name="filename">The path and filename to the file.</param>
		/// <param name="cooked">Set <b>true</b> to create a string formatted for human reading.</param>
		/// <returns>The number of characters saved to file.</returns>
		public int SaveToPDLFile(string filename, bool cooked = true)
		{
			string pdlString = ParametersToPDLConverter.ParametersToPDL(this, cooked);
			if (pdlString.Length == 0) pdlString = "{}";
			if (cooked) pdlString += "\r\n";

			try
			{
				File.WriteAllText(filename, pdlString, new UTF8Encoding());
				return pdlString.Length;
			}
			catch
			{
				throw new Exception(string.Format("WriteToPDLFile: Error creating file. Maybe a wrong path, illegal filename or no permission to (over)write. ('{0}')", filename));
			}
		}
		#endregion

		#region ICloneable Members
		/// <summary>
		/// Returns a deep-clone of a <see cref="Parameters"/>.
		/// </summary>
		/// <returns>The clone of the <see cref="Parameters"/>.</returns>
		public object Clone()
		{
			Parameters ret = (Parameters)MemberwiseClone();

			ret.items = new Dictionary<string, object>();
			foreach (string key in items.Keys)
			{
				object obj = Get(key);
				if (obj is byte[])
				{
					byte[] arr = (byte[])obj;
					byte[] value = new byte[arr.Length];
					arr.CopyTo(value, 0); // Copy of the byte[].
					ret.Add(key, value);
				}
				else if (obj is List<string>)
				{
					List<string> list = (List<string>)obj;
					List<string> value = new List<string>(list); // Copy of the List<string>.
					ret.Add(key, value);
				}
				else if (obj is List<long>)
				{
					List<long> list = (List<long>)obj;
					List<long> value = new List<long>(list); // Copy of the List<long>.
					ret.Add(key, value);
				}
				else if (obj is List<bool>)
				{
					List<bool> list = (List<bool>)obj;
					List<bool> value = new List<bool>(list); // Copy of the List<bool>.
					ret.Add(key, value);
				}
				else if (obj is List<double>)
				{
					List<double> list = (List<double>)obj;
					List<double> value = new List<double>(list); // Copy of the List<double>.
					ret.Add(key, value);
				}
				else if (obj is List<Parameters>)
				{
					List<Parameters> list = (List<Parameters>)obj;
					List<Parameters> value = new List<Parameters>(list.Count);
					foreach (Parameters element in list) value.Add((Parameters)element.Clone()); // Clone of the Parameters in the List<Parameters>.
					ret.Add(key, value);
				}
				else if (obj is Parameters)
				{
					Parameters parameters = (Parameters)obj;
					ret.Add(key, (Parameters)(parameters.Clone())); // Clone of the Parameters.
				}
				else
				{
					// Use Put instead of Add, since 'obj' is of unknown type.
					ret.Put(key, obj); // Copy of all other types.
				}
			}

			return ret;
		}
		#endregion

		#region IEquatable<Parameters> Members
		/// <summary>
		/// Indicates whether the current object is equal to another object of this type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns><b>true</b> if the current object is equal to the <paramref name="other"/> <see cref="Parameters"/>; otherwise, <b>false</b>.</returns>
		public bool Equals(Parameters other)
		{
			if (ReferenceEquals(other, null)) return false;
			if (Count != other.Count) return false;

			foreach (string key in items.Keys)
			{
				if (!other.Contains(key)) return false;

				object objectA = items[key];
				object objectB = other.items[key];

				if (objectA is bool)
				{
					if (!(objectB is bool)) return false;
					if ((bool)objectA != (bool)objectB) return false;
				}
				else if (objectA is long)
				{
					if (!(objectB is long)) return false;
					if ((long)objectA != (long)objectB) return false;
				}
				else if (objectA is double)
				{
					if (!(objectB is double)) return false;
					if ((double)objectA != (double)objectB) return false;
				}
				else if (objectA is string)
				{
					if (!(objectB is string)) return false;
					if ((string)objectA != (string)objectB) return false;
				}
				else if (objectA is Parameters)
				{
					if (!(objectB is Parameters)) return false;
					if (!((Parameters)objectA).Equals((Parameters)objectB)) return false;
				}
				else if (objectA is IList)
				{
					if (!(objectB is IList)) return false;

					IList iListA = objectA as IList;
					IList iListB = objectB as IList;

					if (iListA.Count != iListB.Count) return false;

					if (objectA is List<bool>)
					{
						if (!(objectB is List<bool>)) return false;

						List<bool> listA = objectA as List<bool>;
						List<bool> listB = objectB as List<bool>;

						for (int i = 0; i < listA.Count; i++)
							if (listA[i] != listB[i]) return false;
					}
					else if (objectA is List<long>)
					{
						if (!(objectB is List<long>)) return false;

						List<long> listA = objectA as List<long>;
						List<long> listB = objectB as List<long>;

						for (int i = 0; i < listA.Count; i++)
							if (listA[i] != listB[i]) return false;
					}
					else if (objectA is List<double>)
					{
						if (!(objectB is List<double>)) return false;

						List<double> listA = objectA as List<double>;
						List<double> listB = objectB as List<double>;

						for (int i = 0; i < listA.Count; i++)
							if (listA[i] != listB[i]) return false;
					}
					else if (objectA is List<string>)
					{
						if (!(objectB is List<string>)) return false;

						List<string> listA = objectA as List<string>;
						List<string> listB = objectB as List<string>;

						for (int i = 0; i < listA.Count; i++)
							if (listA[i] != listB[i]) return false;
					}
					else if (objectA is List<Parameters>)
					{
						if (!(objectB is List<Parameters>)) return false;

						List<Parameters> listA = objectA as List<Parameters>;
						List<Parameters> listB = objectB as List<Parameters>;

						for (int i = 0; i < listA.Count; i++)
							if (!listA[i].Equals(listB[i])) return false;
					}
				}
				else if (objectA is byte[])
				{
					if (!(objectB is byte[])) return false;

					byte[] arrayA = objectA as byte[];
					byte[] arrayB = objectB as byte[];

					for (int i = 0; i < arrayA.Length; i++)
						if (arrayA[i] != arrayB[i]) return false;
				}
				else return false;
			}

			return true;
		}
		#endregion

		#region IEnumerable Members
		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
		public IEnumerator GetEnumerator()
		{
			return items.GetEnumerator();
		}
		#endregion
	}
}
