using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Free.Core.Collections.Internal
{
	/// <summary>
	/// Converts a <see cref="Parameters"/> to a PDL gramma formatted <b>string</b>.
	/// </summary>
	static class ParametersToPDLConverter
	{
		static readonly CultureInfo NeutralCulture = new CultureInfo("");
		static char[] chars = new char[] { '\\', '"' };
		static char[] hexchars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

		/// <summary>
		/// Converts a <see cref="Parameters"/> to a PDL gramma formatted <b>string</b>.
		/// </summary>
		/// <param name="data">The <see cref="Parameters"/> to be converted.</param>
		/// <param name="cooked">Set <b>true</b> to create a string formatted for human reading.</param>
		/// <param name="indent">The indent for human readability.</param>
		/// <returns>The <b>string</b> containing the converted <paramref name="data"/>.</returns>
		public static string ParametersToPDL(Parameters data, bool cooked = false, string indent = "")
		{
			StringBuilder ret = new StringBuilder();

			if (cooked) ret.Append("{\r\n");
			else ret.Append('{');

			bool wasItem = false;

			string[] names = data.GetNames();
			for(int i=0; i<names.Length; i++)
			{
				string name = names[i];
				object value = data.Get(name);

				if (cooked)
				{
					string newIndent = indent + '\t';

					try
					{
						string val = "";
						if (value is long) val = value.ToString();
						else if (value is double) { val = ((double)value).ToString("G16", NeutralCulture); }
						else if (value is string) val = StringToPDL((string)value);
						else if (value is bool) val = ((bool)value) ? "true" : "false";
						else if (value is byte[]) val = DataBlobToPDL((byte[])value, true);
						else if (value is Parameters) val = ParametersToPDL((Parameters)value, true, newIndent);
						else if (value is List<string>) val = StringListToPDL((List<string>)value, true, newIndent);
						else if (value is List<Parameters>) val = ParametersListToPDL((List<Parameters>)value, true, newIndent);
						else if (value is List<long>) val = IntegerListToPDL((List<long>)value, true, newIndent);
						else if (value is List<double>) val = FloatListToPDL((List<double>)value, true, newIndent);
						else if (value is List<bool>) val = BooleanListToPDL((List<bool>)value, true, newIndent);

						if (val.Length > 0)
						{
							ret.Append(newIndent);
							ret.Append(name);
							ret.Append('=');
							ret.Append(val);
							ret.Append(";\r\n");
							wasItem = true;
						}
					}
					catch { } // ignore
				}
				else
				{
					try
					{
						string val = "";
						if (value is long) val = value.ToString();
						else if (value is double) { val = value.ToString(); val = val.Replace(',', '.'); }
						else if (value is string) val = StringToPDL((string)value);
						else if (value is bool) val = ((bool)value) ? "true" : "false";
						else if (value is byte[]) val = DataBlobToPDL((byte[])value, false);
						else if (value is Parameters) val = ParametersToPDL((Parameters)value);
						else if (value is List<string>) val = StringListToPDL((List<string>)value, false, indent);
						else if (value is List<Parameters>) val = ParametersListToPDL((List<Parameters>)value, false, indent);
						else if (value is List<long>) val = IntegerListToPDL((List<long>)value, false, indent);
						else if (value is List<double>) val = FloatListToPDL((List<double>)value, false, indent);
						else if (value is List<bool>) val = BooleanListToPDL((List<bool>)value, false, indent);

						if (val.Length > 0)
						{
							ret.Append(name);
							ret.Append('=');
							ret.Append(val);
							ret.Append(';');
							wasItem = true;
						}
					}
					catch { } // ignore
				}
			}

			if (!wasItem) return "";

			if (cooked) ret.Append(indent);
			ret.Append('}');

			return ret.ToString();
		}

		/// <summary>
		/// Converts a <b>string</b> to PDL gramma.
		/// </summary>
		/// <param name="text">The <b>string</b> to be converted.</param>
		/// <returns>The converted <b>string</b>.</returns>
		static string StringToPDL(string text)
		{
			int len = text.Length;
			StringBuilder ret = new StringBuilder(len + 2);
			ret.Append('"');

			int token = 0;
			while (token < len)
			{
				// Determine whether we need a escape sequence.
				int esc = text.IndexOfAny(chars, token);
				if (esc > -1)
				{
					ret.Append(text.Substring(token, esc - token));
					ret.Append('\\');
					ret.Append(text[esc]);
					token = esc + 1;
					continue;
				}

				ret.Append(text.Substring(token));
				break;
			}

			ret.Append('"');
			return ret.ToString();
		}

		/// <summary>
		/// Converts an array of <b>byte</b>s to PDL gramma.
		/// </summary>
		/// <param name="data">The array of <b>byte</b>s to be converted.</param>
		/// <param name="cooked">Set <b>true</b> to create a string formatted for human reading.</param>
		/// <returns>The converted array of <b>byte</b>s.</returns>
		static string DataBlobToPDL(byte[] data, bool cooked)
		{
			int len = data.Length;
			if (len == 0) throw new ArgumentException("Array must not be empty.", "data");

			StringBuilder ret = new StringBuilder(1 + data.Length * (cooked ? 3 : 2));
			ret.Append('#');
			for(int i=0; i<data.Length; i++)
			{
				byte b=data[i];
				ret.Append(hexchars[b >> 4]);
				ret.Append(hexchars[b & 0xf]);
				if (cooked) ret.Append(' ');
			}
			return ret.ToString();
		}

		/// <summary>
		/// Converts a list of <b>string</b>s to PDL gramma.
		/// </summary>
		/// <param name="list">The list to be converted.</param>
		/// <param name="cooked">Set <b>true</b> to create a string formatted for human reading.</param>
		/// <param name="indent">The indent for human readability.</param>
		/// <returns>The converted list.</returns>
		static string StringListToPDL(List<string> list, bool cooked, string indent)
		{
			StringBuilder ret = new StringBuilder();
			if (list.Count == 0) return "";

			string newIndent = indent + '\t';

			if (cooked) ret.Append("(\r\n");
			else ret.Append('(');

			for (int i = 0; i < list.Count; i++)
			{
				if (cooked) ret.Append(newIndent);

				ret.Append(StringToPDL(list[i]));

				if (i < (list.Count - 1)) ret.Append(',');
				if (cooked) ret.Append("\r\n");
			}

			if (cooked) ret.Append(indent);
			ret.Append(')');

			return ret.ToString();
		}

		/// <summary>
		/// Converts a list of <b>double</b>s to PDL gramma.
		/// </summary>
		/// <param name="list">The list to be converted.</param>
		/// <param name="cooked">Set <b>true</b> to create a string formatted for human reading.</param>
		/// <param name="indent">The indent for human readability.</param>
		/// <returns>The converted list.</returns>
		static string FloatListToPDL(List<double> list, bool cooked, string indent)
		{
			StringBuilder ret = new StringBuilder();
			if (list.Count == 0) return "";

			string newIndent = indent + '\t';

			if (cooked) ret.Append("(\r\n");
			else ret.Append('(');

			for (int i = 0; i < list.Count; i++)
			{
				if (cooked) ret.Append(newIndent);

				string val = list[i].ToString("G16", NeutralCulture);
				ret.Append(val);

				if (i < (list.Count - 1)) ret.Append(',');
				if (cooked) ret.Append("\r\n");
			}

			if (cooked) ret.Append(indent);
			ret.Append(')');

			return ret.ToString();
		}

		/// <summary>
		/// Converts a list of <b>long</b>s to PDL gramma.
		/// </summary>
		/// <param name="list">The list to be converted.</param>
		/// <param name="cooked">Set <b>true</b> to create a string formatted for human reading.</param>
		/// <param name="indent">The indent for human readability.</param>
		/// <returns>The converted list.</returns>
		static string IntegerListToPDL(List<long> list, bool cooked, string indent)
		{
			StringBuilder ret = new StringBuilder();
			if (list.Count == 0) return "";

			string newIndent = indent + '\t';

			if (cooked) ret.Append("(\r\n");
			else ret.Append('(');

			for (int i = 0; i < list.Count; i++)
			{
				if (cooked) ret.Append(newIndent);

				ret.Append(list[i].ToString());

				if (i < (list.Count - 1)) ret.Append(',');
				if (cooked) ret.Append("\r\n");
			}

			if (cooked) ret.Append(indent);
			ret.Append(')');

			return ret.ToString();
		}

		/// <summary>
		/// Converts a list of <b>bool</b>s to PDL gramma.
		/// </summary>
		/// <param name="list">The list to be converted.</param>
		/// <param name="cooked">Set <b>true</b> to create a string formatted for human reading.</param>
		/// <param name="indent">The indent for human readability.</param>
		/// <returns>The converted list.</returns>
		static string BooleanListToPDL(List<bool> list, bool cooked, string indent)
		{
			StringBuilder ret = new StringBuilder();
			if (list.Count == 0) return "";

			string newIndent = indent + '\t';

			if (cooked) ret.Append("(\r\n");
			else ret.Append('(');

			for (int i = 0; i < list.Count; i++)
			{
				if (cooked) ret.Append(newIndent);

				ret.Append(list[i] ? "true" : "false");

				if (i < (list.Count - 1)) ret.Append(',');
				if (cooked) ret.Append("\r\n");
			}

			if (cooked) ret.Append(indent);
			ret.Append(')');

			return ret.ToString();
		}

		/// <summary>
		/// Converts a list of <b>Parameters</b>s to PDL gramma.
		/// </summary>
		/// <param name="list">The list to be converted.</param>
		/// <param name="cooked">Set <b>true</b> to create a string formatted for human reading.</param>
		/// <param name="indent">The indent for human readability.</param>
		/// <returns>The converted list.</returns>
		static string ParametersListToPDL(List<Parameters> list, bool cooked, string indent)
		{
			StringBuilder ret = new StringBuilder();
			if (list.Count == 0) return "";

			string newIndent = indent + '\t';

			if (cooked) ret.Append("(\r\n");
			else ret.Append('(');

			bool wasItem = false;

			for (int i = 0; i < list.Count; i++)
			{
				string parastr = ParametersToPDL(list[i], cooked, newIndent);
				if (parastr == "") continue;

				if (cooked) ret.Append(newIndent);

				ret.Append(parastr);

				if (i < (list.Count - 1)) ret.Append(',');
				if (cooked) ret.Append("\r\n");
				wasItem = true;
			}

			if (!wasItem) return "";

			if (cooked) ret.Append(indent);
			ret.Append(')');

			return ret.ToString();
		}
	}
}
