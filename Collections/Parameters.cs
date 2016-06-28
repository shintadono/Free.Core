using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Free.Core.Collections.Internal;

namespace Free.Core.Collections
{
	/// <summary>
	/// A hierarchical dictionary, using string as keys, for storing parameters.
	/// </summary>
	[CLSCompliant(false)]
	public class Parameters : ICloneable, IEquatable<Parameters>, IEnumerable
	{
		/// <summary>
		/// The store for the data.
		/// </summary>
		Dictionary<string, object> items;

		#region Statics
		/// <summary>
		/// Determines whether a string is allowed as a name of <see cref="Parameters"/>.
		/// </summary>
		/// <remarks>
		/// <paramref name="name"/> must begin with [a-z], [A-Z], '_' or '$', afterwars digits a allowed, too.
		/// </remarks>
		/// <param name="name">The string to check.</param>
		/// <returns><b>true</b> if the string is allowed as a name of <see cref="Parameters"/>; otherwise, <b>false</b>.</returns>
		public static bool CheckName(string name)
		{
			int namelen = name.Length;
			if (namelen < 1) return false;
			char c = name[0];

			if (!(c >= 'a' && c <= 'z') && !(c >= 'A' && c <= 'Z') && (c != '$') && (c != '_'))
				return false;

			int token = 1;

			while (token < namelen)
			{
				c = name[token++];

				if (!(c >= 'a' && c <= 'z') && !(c >= 'A' && c <= 'Z') &&
					!(c >= '0' && c <= '9') && (c != '$') && (c != '_'))
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
			listname = "";

			if (name.Length < 4) return -1; // 4: name+index+brackets

			if (name[name.Length - 1] != ']') return -1; // Last char must be ']'

			int ind = name.IndexOf('[');
			if (ind < 1) return -1; // Nothing found, or string begins with '['

			string indStr = name.Substring(ind + 1, name.Length - (ind + 2));

			foreach (char c in indStr) if (c < '0' || c > '9') return -1; // only digits allowed, otherwise return -1

			int ret = 0;
			if (!int.TryParse(indStr, out ret)) return -1;

			listname = name.Substring(0, ind);
			return ret;
		}
		#endregion

		#region Factories
		/// <summary>
		/// Reads a PDL gramma formatted file and returns the content as <see cref="Parameters"/>.
		/// </summary>
		/// <param name="filename">The path to the file.</param>
		/// <returns>The parsed file content as <see cref="Parameters"/>.</returns>
		public static Parameters FromPDLFile(string filename)
		{
			if (!File.Exists(filename)) throw new FileNotFoundException(string.Format("File not found. ('{0}')", filename));

			using (StreamReader streamReader = new StreamReader(filename, Encoding.UTF8, true))
			{
				return PDLParser.Parse(streamReader);
			}
		}

		/// <summary>
		/// Converts a PDL gramma formatted string and returns the content as <see cref="Parameters"/>.
		/// </summary>
		/// <param name="text">The text to be converted.</param>
		/// <param name="encoding">The text encoding to be used.</param>
		/// <returns>The parsed text content as <see cref="Parameters"/>.</returns>
		public static Parameters FromPDLString(string text, Encoding encoding = null)
		{
			if (encoding == null) encoding = Encoding.Unicode;

			using (StreamReader streamReader = new StreamReader(new MemoryStream(encoding.GetBytes(text)), encoding))
			{
				return PDLParser.Parse(streamReader);
			}
		}

		/// <summary>
		/// Reads a PDL gramma formatted <see cref="Stream"/> and returns the content as <see cref="Parameters"/>.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to be parsed.</param>
		/// <param name="encoding">The text encoding to be used.</param>
		/// <returns>The parsed <paramref name="stream"/> content as <see cref="Parameters"/>.</returns>
		public static Parameters FromPDLStream(Stream stream, Encoding encoding = null)
		{
			if (encoding == null) encoding = Encoding.Unicode;

			using (StreamReader streamReader = new StreamReader(stream, encoding))
			{
				return PDLParser.Parse(streamReader);
			}
		}

		/// <summary>
		/// Reads a PDL gramma formatted <see cref="StreamReader"/> and returns the content as <see cref="Parameters"/>.
		/// </summary>
		/// <param name="stream">The <see cref="StreamReader"/> to be parsed.</param>
		/// <returns>The parsed <paramref name="stream"/> content as <see cref="Parameters"/>.</returns>
		public static Parameters FromPDLStream(StreamReader stream)
		{
			return PDLParser.Parse(stream);
		}

		/// <summary>
		/// Converts command line arguments into <see cref="Parameters"/>.
		/// </summary>
		/// <param name="args">The command line arguments.</param>
		/// <returns>The parsed command line as <see cref="Parameters"/>.</returns>
		public static Parameters InterpretCommandLine(string[] args)
		{
			Parameters ret = new Parameters();
			if (args.Length < 1) return ret;

			int filecounter = 0;
			foreach (string str in args)
			{
				if (str[0] == '-')
				{
					int idx = str.IndexOf(':');
					if (idx >= 0)
					{
						string key = str.Substring(1, idx - 1);
						if (key != "") ret.Add(key, str.Substring(idx + 1));
					}
					else
					{
						string key = str.Substring(1);
						if (key != "") ret.Add(key, true);
					}
				}
				else
				{
					string key = String.Format("file{0:000}", filecounter++);
					ret.Add(key, str);
				}
			}

			return ret;
		}

		#region MakeKeyValuePair
		/// <summary>
		/// Creates a key-value-pair <see cref="Parameters"/>.
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
		/// Creates a key-value-pair <see cref="Parameters"/>.
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
		/// Creates a key-value-pair <see cref="Parameters"/>.
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
		/// Creates a key-value-pair <see cref="Parameters"/>.
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
		/// Creates a key-value-pair <see cref="Parameters"/>.
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
		/// Creates a key-value-pair <see cref="Parameters"/>.
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
		/// Creates a key-value-pair <see cref="Parameters"/>.
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
		/// Creates a key-value-pair <see cref="Parameters"/>.
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
		/// Creates a key-value-pair <see cref="Parameters"/>.
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
		/// Creates a key-value-pair <see cref="Parameters"/>.
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
		/// Creates a key-value-pair <see cref="Parameters"/>.
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
		/// Creates an empty instance.
		/// </summary>
		public Parameters()
		{
			items=new Dictionary<string, object>();
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
		public bool IsEmpty()
		{
			return items.Count == 0;
		}

		#region Get
		/// <summary>
		/// Retrieves an object from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the object.</param>
		/// <param name="std">Fallback value.</param>
		/// <returns>The requested object, if successful; otherwise, <paramref name="std"/>.</returns>
		public object Get(string name, object std = null)
		{
			// Canonize key
			string key = name.Replace('\\', '/');
			while (key.IndexOf("//") != -1) key = key.Replace("//", "/");
			key = key.Trim('/');

			// Check key
			if (key.Length == 0) return std;

			return GetSub(key, std);
		}

		object GetSub(string name, object std)
		{
			int ind = name.IndexOf('/');

			if (ind == -1) return GetLast(name, std);

			string key = name.Substring(0, ind);
			string subkey = name.Substring(ind + 1);

			string listname = "";
			int idx = ParseListIndex(key, out listname);

			Parameters p = null;
			if (idx == -1)
			{
				if (!CheckName(key)) return std;
				if (!items.ContainsKey(key)) return std;

				p = items[key] as Parameters;
				if (p == null) return std;

				return p.GetSub(subkey, std);
			}

			if (!CheckName(listname)) return std;
			if (!items.ContainsKey(listname)) return std;

			List<Parameters> pl = items[listname] as List<Parameters>;
			if (pl == null) return std;

			if (idx >= pl.Count) return std;

			p = pl[idx];
			return p.GetSub(subkey, std);
		}

		object GetLast(string name, object std)
		{
			string listname = "";
			int idx = ParseListIndex(name, out listname);

			if (idx == -1)
			{
				if (!CheckName(name)) return std;
				if (!items.ContainsKey(name)) return std;

				return items[name];
			}
			else
			{
				if (!CheckName(listname)) return std;
				if (!items.ContainsKey(listname)) return std;

				ICollection list = Get(listname) as ICollection;
				if (list == null) return std;

				if (idx >= list.Count) return std;

				IList ilist = list as IList;
				return ilist[idx];
			}
		}
		#endregion

		#region Put
		/// <summary>
		/// Puts a value into a (sub-parameters) store. Called recursive for each sub-parameters in <paramref name="path"/>.
		/// </summary>
		/// <param name="path">The path (name) for the value.</param>
		/// <param name="val">The value to be stored.</param>
		void Put(string path, object val)
		{
			// Canonize key
			string key=path.Replace('\\', '/');
			while(key.IndexOf("//")!=-1) key=key.Replace("//", "/");
			key=key.Trim('/');

			// Check key
			if(key.Length==0)
				throw new ArgumentException("Must begin with an alpha-char, a dollar-sign or an understroke, afterwards digits are allowed too.", nameof(path));

			int ind=key.IndexOf('/');

			if(ind==-1)
			{
				PutLast(key, val);
				return;
			}

			string mainkey=key.Substring(0, ind);
			string subkey=key.Substring(ind+1);

			Parameters p=Get(mainkey) as Parameters;
			if(p!=null) p.Put(subkey, val);
			else PutSub(key, val);
		}

		/// <summary>
		/// Puts a value into a new sub-parameter store. Called recursive for each sub-parameters in <paramref name="path"/>.
		/// </summary>
		/// <param name="path">The path (name) for the value.</param>
		/// <param name="val">The value to be stored.</param>
		void PutSub(string path, object val)
		{
			int ind=path.IndexOf('/');

			// Sub-Parameters?
			if(ind!=-1)
			{
				string mainkey=path.Substring(0, ind);
				string subkey=path.Substring(ind+1);

				Parameters newp=new Parameters();
				newp.PutSub(subkey, val);

				PutLast(mainkey, newp);
				return;
			}

			PutLast(path, val);
		}

		/// <summary>
		/// Puts a value into the store.
		/// </summary>
		/// <param name="name">The name for the value.</param>
		/// <param name="val">The value to be stored.</param>
		void PutLast(string name, object val)
		{
			string listname="";
			int idx=ParseListIndex(name, out listname);

			if(idx==-1)
			{
				#region Not [index]
				string nameBrackets="";
				if(name.Length>2) nameBrackets=name.Substring(name.Length-2);

				// A list?
				if(nameBrackets=="[]")
				{
					string name2=name.Substring(0, name.Length-2);

					if(!CheckName(name2))
						throw new ArgumentException("Must begin with an alpha-char, a dollar-sign or an understroke, afterwards digits are allowed too.", nameof(name));

					if(items.ContainsKey(name2))
					{
						IList ilist=items[name2] as IList;

						if(ilist!=null)
						{
							if(ilist is List<bool>&&val is bool)
							{
								List<bool> list=(List<bool>)ilist;
								list.Add((bool)val);
							}
							else if(ilist is List<long>&&val is long)
							{
								List<long> list=(List<long>)ilist;
								list.Add((long)val);
							}
							else if(ilist is List<double>&&val is double)
							{
								List<double> list=(List<double>)ilist;
								list.Add((double)val);
							}
							else if(ilist is List<double>&&val is long) // ints too
							{
								List<double> list=(List<double>)ilist;
								list.Add((long)val);
							}
							else if(ilist is List<string>&&val is string)
							{
								List<string> list=(List<string>)ilist;
								list.Add((string)val);
							}
							else if(ilist is List<Parameters>&&val is Parameters)
							{
								List<Parameters> list=(List<Parameters>)ilist;
								list.Add((Parameters)val);
							}
							else throw new ArgumentException("Must be of the same type as the typed list to be added to the list.", nameof(val));

							return;
						}

						// if not a list remove the item (will be overwritten below)
						items.Remove(name2);
					}

					if(val is bool)
					{
						List<bool> list=new List<bool>();
						list.Add((bool)val);
						items.Add(name2, list);
					}
					else if(val is long)
					{
						List<long> list=new List<long>();
						list.Add((long)val);
						items.Add(name2, list);
					}
					else if(val is double)
					{
						List<double> list=new List<double>();
						list.Add((double)val);
						items.Add(name2, list);
					}
					else if(val is string)
					{
						List<string> list=new List<string>();
						list.Add((string)val);
						items.Add(name2, list);
					}
					else if(val is Parameters)
					{
						List<Parameters> list=new List<Parameters>();
						list.Add((Parameters)val);
						items.Add(name2, list);
					}
					else throw new ArgumentException("Must be a type of a typed list to be added to a list.", nameof(val));
				}
				else // not a list
				{
					if(!CheckName(name))
						throw new ArgumentException("Must begin with an alpha-char, a dollar-sign or an understroke, afterwards digits are allowed too.", nameof(name));

					if(items.ContainsKey(name)) items.Remove(name);
					items.Add(name, val);
				}
				#endregion
			}
			else
			{
				#region [index]
				if(!CheckName(listname))
					throw new ArgumentException("Must begin with an alpha-char, a dollar-sign or an understroke, afterwards digits are allowed too.", nameof(name));

				if(items.ContainsKey(listname))
				{
					ICollection icoll=items[listname] as ICollection;

					if(icoll!=null)
					{
						if(idx>icoll.Count)
							throw new IndexOutOfRangeException("Argument: name contains index greater than count of list.");

						// replace or add
						if(icoll is List<bool>&&val is bool)
						{
							List<bool> list=(List<bool>)icoll;
							if(idx<list.Count) list[idx]=(bool)val;
							else list.Add((bool)val);
						}
						else if(icoll is List<long>&&val is long)
						{
							List<long> list=(List<long>)icoll;
							if(idx<list.Count) list[idx]=(long)val;
							else list.Add((long)val);
						}
						else if(icoll is List<double>&&val is double)
						{
							List<double> list=(List<double>)icoll;
							if(idx<list.Count) list[idx]=(double)val;
							else list.Add((double)val);
						}
						else if(icoll is List<double>&&val is long) // ints too
						{
							List<double> list=(List<double>)icoll;
							if(idx<list.Count) list[idx]=(long)val;
							else list.Add((long)val);
						}
						else if(icoll is List<string>&&val is string)
						{
							List<string> list=(List<string>)icoll;
							if(idx<list.Count) list[idx]=(string)val;
							else list.Add((string)val);
						}
						else if(icoll is List<Parameters>&&val is Parameters)
						{
							List<Parameters> list=(List<Parameters>)icoll;
							if(idx<list.Count) list[idx]=(Parameters)val;
							else list.Add((Parameters)val);
						}
						else throw new ArgumentException("Must be of the same type as the typed list to be added to the list.", nameof(val));

						return;
					}
				}

				// Not a list => remove and create a new one
				if(idx!=0) // only '[0]' is allow
					throw new IndexOutOfRangeException("Argument: name contains index(greater 0) to an object that is not a list.");

				items.Remove(listname);

				if(val is bool)
				{
					List<bool> list=new List<bool>();
					list.Add((bool)val);
					items.Add(listname, list);
				}
				else if(val is long)
				{
					List<long> list=new List<long>();
					list.Add((long)val);
					items.Add(listname, list);
				}
				else if(val is double)
				{
					List<double> list=new List<double>();
					list.Add((double)val);
					items.Add(listname, list);
				}
				else if(val is string)
				{
					List<string> list=new List<string>();
					list.Add((string)val);
					items.Add(listname, list);
				}
				else if(val is Parameters)
				{
					List<Parameters> list=new List<Parameters>();
					list.Add((Parameters)val);
					items.Add(listname, list);
				}
				else throw new ArgumentException("Must be a type of a typed list to be added to a list.", nameof(val));

				#endregion
			}
		}
		#endregion

		#region Contains
		/// <summary>
		/// Determines whether a parameter with a specified (path) name exists in the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name of the parameter.</param>
		/// <returns><b>true</b> the the parameter exists in the <see cref="Parameters"/>; otherwise, <b>false</b>.</returns>
		public bool Contains(string name)
		{
			// Canonize key
			string key = name.Replace('\\', '/');
			while (key.IndexOf("//") != -1) key = key.Replace("//", "/");
			key = key.Trim('/');

			// Check key
			if (key.Length == 0) return false;

			return ContainsSub(key);
		}

		bool ContainsSub(string name)
		{
			int ind = name.IndexOf('/');

			if (ind == -1) return ContainsLast(name);

			string key = name.Substring(0, ind);
			string subkey = name.Substring(ind + 1);

			string listname = "";
			int idx = ParseListIndex(key, out listname);

			Parameters p = null;
			if (idx == -1)
			{
				if (!CheckName(key)) return false;
				if (!items.ContainsKey(key)) return false;

				p = items[key] as Parameters;
				if (p == null) return false;

				return p.ContainsSub(subkey);
			}

			if (!CheckName(listname)) return false;
			if (!items.ContainsKey(listname)) return false;

			List<Parameters> pl = items[listname] as List<Parameters>;
			if (pl == null) return false;

			if (idx >= pl.Count) return false;

			p = pl[idx];
			return p.ContainsSub(subkey);
		}

		bool ContainsLast(string name)
		{
			string listname = "";
			int idx = ParseListIndex(name, out listname);

			if (idx == -1)
			{
				if (!CheckName(name)) return false;
				return items.ContainsKey(name);
			}
			else
			{
				if (!CheckName(listname)) return false;
				if (!items.ContainsKey(listname)) return false;

				ICollection list = Get(listname) as ICollection;
				if (list == null) return false;

				return (idx < list.Count);
			}
		}
		#endregion

		#region Typed Add methods
		/// <summary>
		/// Adds a value to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="val">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		/// <overloads>These functions add values to the <see cref="Parameters"/>.</overloads>
		public Parameters Add(string name, bool val) { Put(name, val); return this; }

		/// <summary>
		/// Adds a value to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="val">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, sbyte val) { Put(name, (long)val); return this; }

		/// <summary>
		/// Adds a value to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="val">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, short val) { Put(name, (long)val); return this; }

		/// <summary>
		/// Adds a value to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="val">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, int val) { Put(name, (long)val); return this; }

		/// <summary>
		/// Adds a value to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="val">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, long val) { Put(name, val); return this; }

		/// <summary>
		/// Adds a value to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="val">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, byte val) { Put(name, (long)val); return this; }

		/// <summary>
		/// Adds a value to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="val">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, ushort val) { Put(name, (long)val); return this; }

		/// <summary>
		/// Adds a value to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="val">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, uint val) { Put(name, (long)val); return this; }

		/// <summary>
		/// Adds a value to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="val">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, ulong val) { Put(name, (long)val); return this; }

		/// <summary>
		/// Adds a value to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="val">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, float val) { Put(name, (double)val); return this; }

		/// <summary>
		/// Adds a value to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="val">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, double val) { Put(name, val); return this; }

		/// <summary>
		/// Adds a value to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="val">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, string val) { Put(name, val); return this; }

		/// <summary>
		/// Adds a value to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="val">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, Parameters val) { Put(name, val); return this; }

		/// <summary>
		/// Adds a value to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="val">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, byte[] val) { Put(name, val); return this; }

		/// <summary>
		/// Adds a value to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="val">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, List<bool> val) { Put(name, val); return this; }

		/// <summary>
		/// Adds a value to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="val">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, List<long> val) { Put(name, val); return this; }

		/// <summary>
		/// Adds a value to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="val">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, List<double> val) { Put(name, val); return this; }

		/// <summary>
		/// Adds a value to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="val">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, List<string> val) { Put(name, val); return this; }

		/// <summary>
		/// Adds a value to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="val">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, string[] val) { Put(name, new List<string>(val)); return this; }

		/// <summary>
		/// Adds a value to the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name for the value.</param>
		/// <param name="val">The value to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(string name, List<Parameters> val) { Put(name, val); return this; }

		/// <summary>
		/// Inserts (shallow add) all values from an Parameters into this Parameter.
		/// </summary>
		/// <param name="values">The values to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters Add(Parameters values)
		{
			foreach (string key in values.items.Keys) Put(key, values.items[key]);

			return this;
		}

		/// <summary>
		/// Recursivly add all values from an Parameters into this Parameter.
		/// Existing values (except Parameters) will be overwritten.
		/// </summary>
		/// <param name="values">The values to add.</param>
		/// <returns>The <see cref="Parameters"/>.</returns>
		public Parameters AddDeep(Parameters values)
		{
			foreach (string key in values.items.Keys)
			{
				if (!items.ContainsKey(key))
				{
					Put(key, values.items[key]);
					continue;
				}

				object valThis = items[key];
				bool fPThis = (valThis is Parameters);

				object valThat = values.items[key];
				bool fPThat = (valThat is Parameters);

				// Both Parameters => add recursivly
				if (fPThis && fPThat)
				{
					((Parameters)valThis).AddDeep((Parameters)valThat);
					continue;
				}

				// Both non-Parameters => replace
				if (!fPThis && !fPThat)
				{
					Put(key, valThat);
					continue;
				}

				Parameters p;

				if (fPThis)
				{
					p = (Parameters)valThis;
					object o = valThat;

					// Store non-Parameters as "$default$" entry in the Parameters
					p.Put("$default$", o);
				}
				else
				{
					p = (Parameters)valThat;
					object o = valThis;

					// Store non-Parameters as "$default$" entry in the Parameters
					p.Put("$default$", o);

					Add(key, p);
				}
			}

			return this;
		}
		#endregion

		#region Typed Get methods
		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the object.</param>
		/// <param name="std">Fallback value.</param>
		/// <returns>The requested object, if successful; otherwise, <paramref name="std"/>.</returns>
		public bool GetBool(string name, bool std=false)
		{
			object o=Get(name, std);

			if(o is bool) return (bool)o;
			if(o is long) return (long)o!=0;
			if(o is double) return (double)o!=0;

			if(o is string)
			{
				string os=(string)o;
				os=os.ToLower();
				if(os=="true") return true;
				else if(os=="false") return false;
			}

			return std;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the object.</param>
		/// <param name="std">Fallback value.</param>
		/// <returns>The requested object, if successful; otherwise, <paramref name="std"/>.</returns>
		public int GetInt32(string name, int std=0)
		{
			return (int)GetInt(name, std);
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the object.</param>
		/// <param name="std">Fallback value.</param>
		/// <returns>The requested object, if successful; otherwise, <paramref name="std"/>.</returns>
		public long GetInt(string name, long std=0)
		{
			object o=Get(name, std);

			if(o is long) return (long)o;
			if(o is double) return (long)(double)o;

			return std;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the object.</param>
		/// <param name="std">Fallback value.</param>
		/// <returns>The requested object, if successful; otherwise, <paramref name="std"/>.</returns>
		public double GetDouble(string name, double std=0)
		{
			object o=Get(name, std);

			if(o is long) return (double)(long)o;
			if(o is double) return (double)o;

			return std;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the object.</param>
		/// <param name="std">Fallback value.</param>
		/// <returns>The requested object, if successful; otherwise, <paramref name="std"/>.</returns>
		public string GetString(string name, string std="")
		{
			object o=Get(name, std);

			if(o is string) return o.ToString();

			return std;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the object.</param>
		/// <param name="std">Fallback value.</param>
		/// <returns>The requested object, if successful; otherwise, <paramref name="std"/>.</returns>
		public Parameters GetParameters(string name, Parameters std=null)
		{
			object o=Get(name, std);

			if(o is Parameters) return (Parameters)o;

			return std;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the object.</param>
		/// <param name="std">Fallback value.</param>
		/// <returns>The requested object, if successful; otherwise, <paramref name="std"/>.</returns>
		public byte[] GetDataBlock(string name, byte[] std=null)
		{
			object o=Get(name, std);

			if(o is byte[]) return (byte[])o;

			return std;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the object.</param>
		/// <param name="std">Fallback value.</param>
		/// <returns>The requested object, if successful; otherwise, <paramref name="std"/>.</returns>
		public List<bool> GetBoolList(string name, List<bool> std=null)
		{
			object o=Get(name, std);

			if(o is List<bool>) return (List<bool>)o;

			return std;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the object.</param>
		/// <param name="std">Fallback value.</param>
		/// <returns>The requested object, if successful; otherwise, <paramref name="std"/>.</returns>
		public List<long> GetIntList(string name, List<long> std=null)
		{
			object o=Get(name, std);

			if(o is List<long>) return (List<long>)o;

			return std;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the object.</param>
		/// <param name="std">Fallback value.</param>
		/// <returns>The requested object, if successful; otherwise, <paramref name="std"/>.</returns>
		public List<double> GetDoubleList(string name, List<double> std=null)
		{
			object o=Get(name, std);

			if(o is List<double>) return (List<double>)o;

			return std;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the object.</param>
		/// <param name="std">Fallback value.</param>
		/// <returns>The requested object, if successful; otherwise, <paramref name="std"/>.</returns>
		public List<string> GetStringList(string name, List<string> std=null)
		{
			object o=Get(name, std);

			if(o is List<string>) return (List<string>)o;

			return std;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the object.</param>
		/// <param name="std">Fallback value.</param>
		/// <returns>The requested object, if successful; otherwise, <paramref name="std"/>.</returns>
		public List<Parameters> GetParametersList(string name, List<Parameters> std=null)
		{
			object o=Get(name, std);

			if(o is List<Parameters>) return (List<Parameters>)o;

			return std;
		}

		/// <summary>
		/// Retrieves a key-value-pair <see cref="Parameters"/> from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the object.</param>
		/// <returns>A dictionary containing the key-value-pairs.</returns>
		public Dictionary<string, object> GetKeyValueList(string name)
		{
			Dictionary<string, object> ret=new Dictionary<string, object>();

			List<Parameters> parameterList=GetParametersList(name);

			if (parameterList!=null)
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
		/// <param name="name">The path to the object.</param>
		/// <param name="std">Fallback value.</param>
		/// <returns>The requested object, if successful; otherwise, <paramref name="std"/>.</returns>
		public List<double> GetAsDoubleList(string name, List<double> std=null)
		{
			object o=Get(name, std);

			if (o is List<double>) return (List<double>)o;
			if (o is List<long>)
			{
				List<long> ll=(List<long>)o;
				List<double> ret=new List<double>(ll.Count);
				for (int i=0; i<ll.Count; i++) ret.Add(ll[i]);
				return ret;
			}

			return std;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The path to the object.</param>
		/// <returns>The requested object, if successful; otherwise, an empty string is returned.</returns>
		public string GetAsString(string name)
		{
			object o=Get(name, string.Empty);
			return o.ToString();
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
				if (i.StartsWith("file"))
				{
					ret.Add(GetString(i));
				}
			}

			return ret;
		}
		#endregion

		#region GetHash
		static long GetHash(double value, long hc)
		{
			unchecked
			{
				hc+=Math.Abs(((int)value)+17);
				hc*=1234597;
				hc=Math.Abs(hc);
			}
			return hc;
		}

		static long GetHash(long value, long hc)
		{
			unchecked
			{
				hc+=Math.Abs(((int)value)+17);
				hc*=1234597;
				hc=Math.Abs(hc);
			}
			return hc;
		}

		static long GetHash(string value, long hc)
		{
			unchecked
			{
				int h=0;
				int off=0;
				int len=value.Length;

				if(len<16)
				{
					for(int i=len; i>0; i--) h=(h*37)+value[off++];
				}
				else
				{
					// only sample some characters
					int skip=len/8;
					for(int i=len; i>0; i-=skip, off+=skip) h=(h*39)+value[off];
				}

				hc+=Math.Abs(h);

				hc*=1234597;
				hc=Math.Abs(hc);
			}
			return hc;
		}

		static long GetHash(bool value, long hc)
		{
			unchecked
			{
				hc+=Math.Abs(value?1231:1237);
				hc*=1234597;
				hc=Math.Abs(hc);
			}
			return hc;
		}

		static int StringComparer(string a, string b)
		{
			if(a==null&&b==null) return 0;
			if(a==null) return -1;

			int len=Math.Min(a.Length, b.Length);
			for(int i=0; i<len; i++)
			{
				if(a[i]<b[i]) return -1;
				if(b[i]<a[i]) return 1;
			}

			if(a.Length==b.Length) return 0;

			return a.Length<b.Length?-1:+1;
		}

		static long GetHash(Parameters value, long hc)
		{
			unchecked
			{
				if(value==null) hc+=1345;
				else
				{
					List<string> keys=new List<string>(value.items.Keys);
					keys.Sort(StringComparer);
					foreach(string key in keys)
					{
						hc=GetHash(key, hc);
						object v=value.Get(key);
						if(v==null) hc=GetHash((Parameters)null, hc);
						else if(v is bool) hc=GetHash((bool)v, hc);
						else if(v is double) hc=GetHash((double)v, hc);
						else if(v is long) hc=GetHash((long)v, hc);
						else if(v is string) hc=GetHash((string)v, hc);
						else if(v is Parameters) hc=GetHash((Parameters)v, hc);
						else
						{
							hc*=1234597;
							hc=Math.Abs(hc);
						}
					}
				}

				hc*=1234597;
				hc=Math.Abs(hc);
			}

			return hc;
		}

		static string GetHashString(Parameters value)
		{
			long hc=GetHash(value, 13);

			string ret="1-";
			while(hc>0)
			{
				ret+=(char)(hc%26+'a');
				hc/=17;
			}
			return ret;
		}

		long GetHash(long hc)
		{
			return GetHash(this, hc);
		}

		/// <summary>
		/// Generate a hash string of the Parameters.
		/// </summary>
		/// <returns></returns>
		public string GetHashString()
		{
			return GetHashString(this);
		}
		#endregion

		#region Remove & Clear
		/// <summary>
		/// Clears the <see cref="Parameters"/>.
		/// </summary>
		public void Clear() { items.Clear(); }

		/// <summary>
		/// Removes a parameter from the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="name">The (path) name of the parameter.</param>
		/// <returns><b>true</b> if successful; otherwise, <b>false</b>.</returns>
		public bool Remove(string name)
		{
			// Canonize key
			string key=name.Replace('\\', '/');
			while(key.IndexOf("//")!=-1) key=key.Replace("//", "/");
			key=key.Trim('/');

			// Check key
			if(key.Length==0) return false;

			int ind=key.LastIndexOf('/');

			if(ind==-1)
			{
				string listname="";
				int idx=ParseListIndex(key, out listname);

				if(idx==-1)
				{
					if(!CheckName(key)) return false;
					return items.Remove(key);
				}
				else
				{
					if(!CheckName(listname)) return false;
					if(!items.ContainsKey(listname)) return false;

					ICollection list=Get(listname) as ICollection;
					if(list==null) return false;

					if(idx!=list.Count-1) return false;
					IList ilist=list as IList;

					try
					{
						ilist.RemoveAt(idx);
					}
					catch
					{
						return false;
					}
					return true;
				}
			}

			string mainkey=key.Substring(0, ind);
			string subkey=key.Substring(ind+1);

			Parameters p=Get(mainkey) as Parameters;
			if(p==null) return false;

			return p.Remove(subkey);
		}
		#endregion

		#region PDL
		/// <summary>
		/// Retrieves a PDL gramma encoded text representation of the <see cref="Parameters"/>.
		/// </summary>
		/// <param name="cooked">Set <b>true</b> to create a string formatted for human reading.</param>
		/// <returns>The <b>string</b> containing the converted <see cref="Parameters"/>.</returns>
		public string ToPDLString(bool cooked = true)
		{
			return ParametersToPDLConverter.ParametersToPDL(this, cooked);
		}

		/// <summary>
		/// Saves the content of the <see cref="Parameters"/> into a file using PDL gramma.
		/// </summary>
		/// <param name="filename">The path and filename to the file.</param>
		/// <param name="cooked">Set <b>true</b> to create a string formatted for human reading.</param>
		/// <returns>The number of chars save to file.</returns>
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
				object o = Get(key);
				if (o is byte[])
				{
					byte[] arr = (byte[])o;
					byte[] val = new byte[arr.Length];
					arr.CopyTo(val, 0); // Copy of the byte[]
					ret.Add(key, val);
				}
				else if (o is List<string>)
				{
					List<string> list = (List<string>)o;
					List<string> val = new List<string>(list); // Copy of the List<string>
					ret.Add(key, val);
				}
				else if (o is List<long>)
				{
					List<long> list = (List<long>)o;
					List<long> val = new List<long>(list); // Copy of the List<long>
					ret.Add(key, val);
				}
				else if (o is List<bool>)
				{
					List<bool> list = (List<bool>)o;
					List<bool> val = new List<bool>(list); // Copy of the List<bool>
					ret.Add(key, val);
				}
				else if (o is List<double>)
				{
					List<double> list = (List<double>)o;
					List<double> val = new List<double>(list); // Copy of the List<double>
					ret.Add(key, val);
				}
				else if (o is List<Parameters>)
				{
					List<Parameters> list = (List<Parameters>)o;
					List<Parameters> val = new List<Parameters>(list.Count);
					foreach (Parameters p in list) val.Add((Parameters)p.Clone()); // Clone of the Parameters in the List<Parameters>
					ret.Add(key, val);
				}
				else if (o is Parameters)
				{
					Parameters p = (Parameters)o;
					ret.Add(key, (Parameters)(p.Clone())); // Clonee of the Parameters
				}
				else
				{
					// Use Put instead of Add, since 'o' is of unknown type
					ret.Put(key, o); // Copy of all other types
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
		/// <returns><b>true</b> if the current object is equal to the other parameter; otherwise, <b>false</b>.</returns>
		public bool Equals(Parameters other)
		{
			if (Count != other.Count) return false;

			foreach (string key in items.Keys)
			{
				if (!other.Contains(key)) return false;

				object oa = items[key];
				object ob = other.items[key];

				if (oa is bool)
				{
					if (!(ob is bool)) return false;
					if ((bool)oa != (bool)ob) return false;
				}
				else if (oa is long)
				{
					if (!(ob is long)) return false;
					if ((long)oa != (long)ob) return false;
				}
				else if (oa is double)
				{
					if (!(ob is double)) return false;
					if ((double)oa != (double)ob) return false;
				}
				else if (oa is string)
				{
					if (!(ob is string)) return false;
					if ((string)oa != (string)ob) return false;
				}
				else if (oa is Parameters)
				{
					if (!(ob is Parameters)) return false;
					if (!((Parameters)oa).Equals((Parameters)ob)) return false;
				}
				else if (oa is IList)
				{
					if (!(ob is IList)) return false;

					IList la = oa as IList;
					IList lb = ob as IList;

					if (la.Count != lb.Count) return false;

					if (oa is List<bool>)
					{
						if (!(ob is List<bool>)) return false;

						List<bool> ia = oa as List<bool>;
						List<bool> ib = ob as List<bool>;

						for (int i = 0; i < ia.Count; i++)
							if (ia[i] != ib[i]) return false;
					}
					else if (oa is List<long>)
					{
						if (!(ob is List<long>)) return false;

						List<long> ia = oa as List<long>;
						List<long> ib = ob as List<long>;

						for (int i = 0; i < ia.Count; i++)
							if (ia[i] != ib[i]) return false;
					}
					else if (oa is List<double>)
					{
						if (!(ob is List<double>)) return false;

						List<double> ia = oa as List<double>;
						List<double> ib = ob as List<double>;

						for (int i = 0; i < ia.Count; i++)
							if (ia[i] != ib[i]) return false;
					}
					else if (oa is List<string>)
					{
						if (!(ob is List<string>)) return false;

						List<string> ia = oa as List<string>;
						List<string> ib = ob as List<string>;

						for (int i = 0; i < ia.Count; i++)
							if (ia[i] != ib[i]) return false;
					}
					else if (oa is List<Parameters>)
					{
						if (!(ob is List<Parameters>)) return false;

						List<Parameters> ia = oa as List<Parameters>;
						List<Parameters> ib = ob as List<Parameters>;

						for (int i = 0; i < ia.Count; i++)
							if (!ia[i].Equals(ib[i])) return false;
					}
				}
				else if (oa is byte[])
				{
					if (!(ob is byte[])) return false;

					byte[] ia = oa as byte[];
					byte[] ib = ob as byte[];

					for (int i = 0; i < ia.Length; i++)
						if (ia[i] != ib[i]) return false;
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
