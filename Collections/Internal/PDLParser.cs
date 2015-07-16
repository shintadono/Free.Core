using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Free.Core.Exceptions;

namespace Free.Core.Collections.Internal
{
	/// <summary>
	/// Parses PDL gramma formatted streams.
	/// </summary>
	class PDLParser
	{
		static CultureInfo NeutralCulture = new CultureInfo("");

		/// <summary>
		/// The <see cref="Stream"/> to be parsed, as <see cref="StreamReader"/>.
		/// </summary>
		StreamReader stream;

		/// <summary>
		/// The current line.
		/// </summary>
		int line;

		/// <summary>
		/// Parses a PDL gramma formatted stream to a <see cref="Parameters"/>.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to be parsed, as <see cref="StreamReader"/>.</param>
		/// <returns>Returns the content of the <paramref name="stream"/> as <see cref="Parameters"/>. If no block is found, a new and empty <see cref="Parameters"/> is returned.</returns>
		public static Parameters Parse(StreamReader stream)
		{
			if (stream == null) throw new ArgumentNullException("stream");

			return new PDLParser(stream).ConsumeNextBlock();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PDLParser"/> class with a <see cref="StreamReader"/>.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to be parsed, as <see cref="StreamReader"/>.</param>
		PDLParser(StreamReader stream)
		{
			if (stream == null) throw new ArgumentNullException("stream");
			this.stream = stream;
			line = 1;
		}

		/// <summary>
		/// Consumes one block ('{...}') from the <see cref="stream"/>.
		/// </summary>
		/// <returns>Returns the content of the block as <see cref="Parameters"/>. If no block is found, a new and empty <see cref="Parameters"/> is returned.</returns>
		Parameters ConsumeNextBlock()
		{
			if (stream == null) throw new ArgumentNullException("stream");

			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
					{
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					}
					line++;
					continue;
				}

				if (c == '{') return ConsumeBlock(false); // Found start of the next block, so let's consume it and return the result.

				if (c == '/') ConsumeComment();
				else throw new TextParserException("Invalid character.", line);
			}

			return new Parameters(); // No block found. Return a new and empty Parameters.
		}

		/// <summary>
		/// Consumes a C and C++ style comment.
		/// </summary>
		void ConsumeComment()
		{
			if (stream.EndOfStream) throw new TextParserException("Unexpected end of stream while parsing a comment.", line);

			char c = (char)stream.Read();

			if (c == '/') ConsumeCommentCpp();
			else if (c == '*') ConsumeCommentC();
			else throw new TextParserException("Invalid character while parsing a comment.", line);
		}

		/// <summary>
		/// Consumes a C++ style comment.
		/// </summary>
		void ConsumeCommentCpp()
		{
			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				// Handle non-whitespaces, whitespaces and count lines.
				if (c >= ' ' || c == '\t') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
					{
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					}
					line++;
					return;
				}

				throw new TextParserException("Invalid character while parsing a comment.", line);
			}
		}

		/// <summary>
		/// Consumes a C style comment.
		/// </summary>
		void ConsumeCommentC()
		{
			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				// '*?' or '*/' ?
				if (c == '*')
				{
					if (stream.EndOfStream) throw new TextParserException("Unexpected end of stream while parsing a comment.", line);

					if ((char)stream.Read() == '/') return;
					continue;
				}

				// Handle non-whitespaces, whitespaces and count lines.
				if (c >= ' ' || c == '\t') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
					{
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					}
					line++;
					continue;
				}

				throw new TextParserException("Unexpected end of stream while parsing a comment.", line);
			}

			throw new TextParserException("Unexpected end of stream while parsing a comment.", line);
		}

		/// <summary>
		/// Consumes a block with or without an addition semicolon.
		/// </summary>
		/// <param name="consumeSemicolon">Set <b>true</b>, if the block is the value parts of a parameter (Key=Value;) and the semicolon must be parsed.</param>
		/// <returns>A <see cref="Parameters"/> containing the parameters parsed for the block.</returns>
		Parameters ConsumeBlock(bool consumeSemicolon)
		{
			Parameters ret = new Parameters();

			bool done = false; // Will be set true when the closing curly bracket is found, and a semicolon must be parsed.

			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				if (c == '}')
				{
					if (!consumeSemicolon) return ret; // No semicolon to read, so we're done here.
					done = true;
					break;
				}

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
					{
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					}
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				// First character of the name of a parameter (Key=Value;).
				if ((c >= 'A' && (c <= 'Z')) || (c >= 'a' && (c <= 'z')) || c == '_' || c == '$' || c == '"') ConsumeParameter(c, ret);
				else throw new TextParserException("Invalid character while parsing a block.", line);
			}

			// No closing curly bracket parsed, so we're at the end of the stream.
			if (!done) throw new TextParserException("Unexpected end of stream while parsing a block.", line);

			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				if (c == ';') return ret; // End of the block-parameter (Key={...};).

				// Handle non-whitespaces, whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				throw new TextParserException("Invalid character while parsing a block.", line);
			}

			// No closing curly bracket parsed, so we're at the end of the stream.
			throw new TextParserException("Unexpected end of stream while parsing a block.", line);
		}

		/// <summary>
		/// Consumes a parameter (Key=Value) and adds it to a <see cref="Parameters"/>.
		/// </summary>
		/// <param name="c0">The already consumed first character of the name of a parameter.</param>
		/// <param name="ret">The <see cref="Parameters"/> in which the result has to be added.</param>
		void ConsumeParameter(char c0, Parameters ret)
		{
			// Parse the name of the parameter (til the '=').
			string name = ConsumeParameterName(c0);

			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
					{
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					}
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				// Handle bool value: false.
				if (c == 'F' || c == 'f')
				{
					ConsumeBoolFalse(true);
					ret.Add(name, false);
					return;
				}

				// Handle bool value: true.
				if (c == 'T' || c == 't')
				{
					ConsumeBoolTrue(true);
					ret.Add(name, true);
					return;
				}

				// Handle number (long or double).
				if (c == '+' || c == '-' || (c >= '0' && c <= '9'))
				{
					ConsumeNumber(c, ret, name);
					return;
				}

				// Handle string.
				if (c == '"')
				{
					string value = ConsumeString();
					ret.Add(name, value);
					return;
				}

				// Handle block.
				if (c == '{')
				{
					Parameters value = ConsumeBlock(true);
					ret.Add(name, value);
					return;
				}

				// Handle list.
				if (c == '(')
				{
					ConsumeList(ret, name);
					return;
				}

				// Handle hexcoded data.
				if (c == '#')
				{
					byte[] value = ConsumeHexBlob();
					ret.Add(name, value);
					return;
				}

				// Handle verbatim string.
				if (c == '@')
				{
					string value = ConsumeVerbatimString();
					ret.Add(name, value);
					return;
				}

				throw new TextParserException("Invalid character while parsing the value of a parameter.", line);
			}

			throw new TextParserException("Unexpected end of stream while parsing a parameter.", line);
		}

		/// <summary>
		/// Consumes a name of a parameter (every allowed character until the next '=').
		/// </summary>
		/// <param name="c0">The already consumed first character of the name of a parameter.</param>
		/// <returns>The name of the parameter as <b>string</b>.</returns>
		string ConsumeParameterName(char c0)
		{
			StringBuilder ret = new StringBuilder();
			
			// If name starts with a double-quote, consume as regular string. Invalid characters will be detected be Parameter.Add().
			if (c0 == '"') ret.Append(ConsumeStringWithoutSemicolon());
			else ret.Append(c0);

			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				if (c == '=') return ret.ToString(); // Done.

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
					{
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					}
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				if ((c >= 'A' && (c <= 'Z')) || (c >= 'a' && (c <= 'z')) || c == '_' || c == '$' || (c >= '0' && c <= '9')) ret.Append(c);
				else if (c == '"') ret.Append(ConsumeStringWithoutSemicolon()); // Consume as regular string. Invalid characters will be detected be Parameter.Add().
				else throw new TextParserException("unexpected character while parsing a name", line);
			}

			throw new TextParserException("unexpected end of stream while parsing a name", line);
		}

		/// <summary>
		/// Consumes 'alse' (first character of 'false' is already consumed) case-insensitive with or without a semicolon.
		/// </summary>
		/// <param name="consumeSemicolon">Set <b>true</b>, if the value is parts of a parameter (Key=Value;) and the semicolon must be parsed.</param>
		void ConsumeBoolFalse(bool consumeSemicolon)
		{
			int letter = 1;

			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
					{
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					}
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				if (letter == 1 && (c == 'A' || c == 'a')) letter++;
				else if (letter == 2 && (c == 'L' || c == 'l')) letter++;
				else if (letter == 3 && (c == 'S' || c == 's')) letter++;
				else if (letter == 4 && (c == 'E' || c == 'e'))
				{
					if (consumeSemicolon) letter++; // Still need a ';'.
					else return; // Done.
				}
				else if (letter == 5 && c == ';') return; // Done.
				else throw new TextParserException("Invalid character while parsing the value of a parameter.", line);
			}

			throw new TextParserException("Unexpected end of stream while parsing the value of a parameter.", line);
		}

		/// <summary>
		/// Consumes 'rue' (first character of 'true' is already consumed) case-insensitive with or without a semicolon.
		/// </summary>
		/// <param name="consumeSemicolon">Set <b>true</b>, if the value is parts of a parameter (Key=Value;) and the semicolon must be parsed.</param>
		void ConsumeBoolTrue(bool consumeSemicolon)
		{
			int letter = 1;

			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
					{
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					}
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				if (letter == 1 && (c == 'R' || c == 'r')) letter++;
				else if (letter == 2 && (c == 'U' || c == 'u')) letter++;
				else if (letter == 3 && (c == 'E' || c == 'e'))
				{
					if (consumeSemicolon) letter++; // Still need a ';'.
					else return; // Done.
				}
				else if (letter == 4 && c == ';') return; // Done.
				else throw new TextParserException("Invalid character while parsing the value of a parameter.", line);
			}

			throw new TextParserException("Unexpected end of stream while parsing the value of a parameter.", line);
		}

		/// <summary>
		/// Consumes a number
		/// </summary>
		/// <param name="c0">The already consumed first character of the number.</param>
		/// <param name="ret">The <see cref="Parameters"/> in which the result has to be added.</param>
		/// <param name="name">The name of the parameter.</param>
		void ConsumeNumber(char c0, Parameters ret, string name)
		{
			StringBuilder numberSB = new StringBuilder();
			numberSB.Append(c0);

			bool wasSemicolon = false; // Will be set true when the closing semicolon is found.

			// Collect all characters allowed in a number, until the closing semicolon.
			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				if (c == ';') { wasSemicolon = true; break; }

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
					{
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					}
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				if (c == '+' || c == '-' || (c >= '0' && c <= '9') || c == '.' || c == 'x' || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')) numberSB.Append(c);
				else throw new TextParserException("Invalid character while parsing the value of a parameter.", line);
			}

			// No closing semicolon parsed, so we're at the end of the stream.
			if (!wasSemicolon) throw new TextParserException("Unexpected end of stream while parsing the value of a parameter.", line);

			string number = numberSB.ToString();

			if (number.StartsWith("0x"))
			{
				// Parse as hexadecimal integer.
				long val = 0;
				if (!long.TryParse(number.Substring(2), NumberStyles.AllowHexSpecifier, NeutralCulture, out val))
					throw new TextParserException("Invalid character while parsing the value of a parameter.", line);
				ret.Add(name, val);
			}
			else
			{
				// Parse as decimal integer.
				long ival = 0;
				if (long.TryParse(number, out ival))
				{
					ret.Add(name, ival);
					return;
				}

				// Parse as floating-point.
				double dval = 0;
				if (!double.TryParse(number, NumberStyles.Float, NeutralCulture, out dval))
					throw new TextParserException("Invalid character while parsing the value of a parameter.", line);
				ret.Add(name, dval);
			}
		}

		/// <summary>
		/// Consumes a <b>string</b> ("...") that can contain escape sequences for backslash and double-quote.
		/// Reads all characters until the next not-escaped double-quote.
		/// </summary>
		/// <returns>The consumed string without the closing double-quote.</returns>
		string ConsumeStringWithoutSemicolon()
		{
			StringBuilder ret = new StringBuilder();

			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				if (c == '"') return ret.ToString();

				// Handle escape sequences.
				if (c == '\\')
				{
					if (stream.EndOfStream) throw new TextParserException("Unexpected end of stream while parsing a string.", line);

					char a = (char)stream.Read();
					if (a != '\\' && a != '"') throw new TextParserException("Invalid character while parsing  a string.", line);
					c = a;
				}

				ret.Append(c);
			}

			throw new TextParserException("Unexpected end of stream while parsing a string.", line);
		}

		/// <summary>
		/// Consumes a <b>string</b> value ("...";) that can contain escape sequences for backslash and double-quote.
		/// Reads all characters until the closing semicolon.
		/// </summary>
		/// <returns>The consumed string without the closing double-quote.</returns>
		string ConsumeString()
		{
			// Consume the string until the next not-escaped double-quote.
			string ret = ConsumeStringWithoutSemicolon();

			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				if (c == ';') return ret; // Done.

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
					{
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					}
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				throw new TextParserException("Invalid character while parsing the value of a parameter.", line);
			}

			throw new TextParserException("Unexpected end of stream while parsing the value of a parameter.", line);
		}

		/// <summary>
		/// Consumes a verbatim <b>string</b> (@"...").
		/// Reads all characters until the closing double-quote.
		/// </summary>
		/// <returns>The consumed string without the closing double-quote.</returns>
		string ConsumeVerbatimStringWithoutSemicolon()
		{
			// Find the start of the string. The first double-quote.
			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
					{
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					}
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				if (c == '"') break; // Found.

				throw new TextParserException("Invalid character while parsing a string.", line);
			}

			StringBuilder ret = new StringBuilder();

			// Collect all characters until the closing double-quote.
			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				// End or escape sequence (two double-quotes become one)?
				if (c == '"')
				{
					if (stream.EndOfStream) throw new TextParserException("Unexpected end of stream while parsing a string.", line);

					char a = (char)stream.Peek();
					if (a != '"') return ret.ToString(); // Closing double-quote.
					stream.Read(); // Double-quote escape sequence.
				}

				ret.Append(c);
			}

			throw new TextParserException("Unexpected end of stream while parsing a string.", line);
		}

		/// <summary>
		/// Consumes a verbatim <b>string</b> value (@"...";).
		/// Reads all characters until the closing semicolon.
		/// </summary>
		/// <returns>The consumed string without the closing double-quote.</returns>
		string ConsumeVerbatimString()
		{
			// Consume the verbatim string until the closing double-quote.
			string ret = ConsumeVerbatimStringWithoutSemicolon();

			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				if (c == ';') return ret; // Done.

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
					{
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					}
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				throw new TextParserException("Invalid character while parsing the value of a parameter.", line);
			}

			throw new TextParserException("Unexpected end of stream while parsing the value of a parameter.", line);
		}

		/// <summary>
		/// Consumes a hexadecimal encoded data blob.
		/// </summary>
		/// <returns>The byte array containing the parsed data.</returns>
		byte[] ConsumeHexBlob()
		{
			List<byte> ret = new List<byte>();

			bool first = true; // Indicates the hexadecimal digit to be parsed next.
			byte a = 0; // Stores the parsed hexadecimal digit of the current byte.

			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				if (first && c == ';') return ret.ToArray(); // Done.

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
					{
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					}
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F'))
				{
					byte b = (byte)c;
					if (c >= '0' && c <= '9') b -= 48;
					else if (c >= 'A' && c <= 'F') b -= 55;
					else if (c >= 'a' && c <= 'f') b -= 87;
					else throw new Exception("This should be unreachable.");

					if (first) a = b;
					else ret.Add((byte)(a * 16 + b));

					first = !first;
				}
				else throw new TextParserException("Invalid character while parsing the value of a parameter.", line);
			}

			throw new TextParserException("Unexpected end of stream while parsing the value of a parameter.", line);
		}

		/// <summary>
		/// Consumes a list value.
		/// </summary>
		/// <param name="ret">The <see cref="Parameters"/> in which the result has to be added.</param>
		/// <param name="name">The name of the parameter.</param>
		void ConsumeList(Parameters ret, string name)
		{
			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
					{
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					}
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				// Handle a list of booleans.
				if (c == 'F' || c == 'f' || c == 'T' || c == 't')
				{
					ConsumeBoolList(c, ret, name);
					return;
				}

				// Handle a list of numbers (long or double).
				if (c == '+' || c == '-' || (c >= '0' && c <= '9'))
				{
					ConsumeNumberList(c, ret, name);
					return;
				}

				// Handle a list of strings.
				if (c == '"')
				{
					ConsumeStringList('"', ret, name);
					return;
				}

				// Handle a list of strings.
				if (c == '@')
				{
					ConsumeStringList('@', ret, name);
					return;
				}

				// Handle a list of blocks.
				if (c == '{')
				{
					ConsumeBlockList(ret, name);
					return;
				}

				// Handle empty list.
				if (c == ')')
				{
					ConsumeEmptyList(ret, name);
					return;
				}

				throw new TextParserException("Invalid character while parsing a list.", line);
			}

			throw new TextParserException("Unexpected end of stream while parsing a list.", line);
		}

		/// <summary>
		/// Consumes a list of boolean values.
		/// </summary>
		/// <param name="c0">The already consumed first character of the first list entry.</param>
		/// <param name="ret">The <see cref="Parameters"/> in which the result has to be added.</param>
		/// <param name="name">The name of the parameter.</param>
		void ConsumeBoolList(char c0, Parameters ret, string name)
		{
			List<bool> result = new List<bool>();

			// Handle the first list entry
			if (c0 == 'F' || c0 == 'f')
			{
				ConsumeBoolFalse(false);
				result.Add(false);
			}
			else if (c0 == 'T' || c0 == 't')
			{
				ConsumeBoolTrue(false);
				result.Add(true);
			}
			else throw new ArgumentException("Must be either 't', 'T', 'f' or 'F'.", "c0");

			bool commaNext = true; // Indicates whether the next character must be a comma (ignoring the whitespaces and comments).
			bool done = false;
			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
					{
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					}
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				// Handle comma or done, or the next list entry.
				if (commaNext)
				{
					if (c == ')') { done = true; break; }
					if (c == ',') commaNext = false;
				}
				else if (c == 'F' || c == 'f')
				{
					ConsumeBoolFalse(false);
					result.Add(false);
					commaNext = true;
				}
				else if (c == 'T' || c == 't')
				{
					ConsumeBoolTrue(false);
					result.Add(true);
					commaNext = true;
				}
				else throw new TextParserException("Invalid character while parsing the value of a parameter.", line);
			}

			// No closing round bracket parsed, so we're at the end of the stream.
			if (!done) throw new TextParserException("Unexpected end of stream while parsing the value of a parameter.", line);

			// Find the closing semicolon.
			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				if (c == ';')
				{
					ret.Add(name, result);
					return;
				}

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
					{
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					}
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				throw new TextParserException("Invalid character while parsing the value of a parameter.", line);
			}

			throw new TextParserException("Unexpected end of stream while parsing the value of a parameter.", line);
		}

		/// <summary>
		/// Consumes a list of number values (<b>long</b> or <b>double</b>). If all values are integer a
		/// list of <b>long</b> values will be created, otherwise a list of <b>double</b> values.
		/// </summary>
		/// <param name="c0">The already consumed first character of the first list entry.</param>
		/// <param name="ret">The <see cref="Parameters"/> in which the result has to be added.</param>
		/// <param name="name">The name of the parameter.</param>
		void ConsumeNumberList(char c0, Parameters ret, string name)
		{
			List<long> iList = new List<long>();
			List<double> dList = new List<double>();

			if (!(c0 == '+' || c0 == '-' || (c0 >= '0' && c0 <= '9')))
				throw new ArgumentException("Must be either '+', '-', or between '0' and '9'.", "c0");

			StringBuilder numberSB = new StringBuilder();
			numberSB.Append(c0);

			bool isInteger = true; // Indicates whether the all values are integers or not.
			bool done = false;
			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				if (c == '+' || c == '-' || (c >= '0' && c <= '9') || c == '.' || c == 'x' || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')) numberSB.Append(c);
				else if (numberSB.Length > 0 && (c == ',' || c == ')'))
				{
					string number = numberSB.ToString();

					if (isInteger && number.StartsWith("0x"))
					{
						// Parse as hexadecimal integer.
						long val = 0;
						if (!long.TryParse(number.Substring(2), NumberStyles.AllowHexSpecifier, NeutralCulture, out val))
							throw new TextParserException("Invalid character while parsing the value of a parameter.", line);

						iList.Add(val);
						dList.Add(val);
					}
					else
					{
						if (isInteger)
						{
							// Parse as decimal integer.
							long val = 0;
							if (long.TryParse(number, out val))
							{
								iList.Add(val);
								dList.Add(val);
							}
							else isInteger = false;
						}

						if (!isInteger)
						{
							// Parse as floating-point.
							double dval = 0;
							if (!double.TryParse(number, NumberStyles.Float, NeutralCulture, out dval))
								throw new TextParserException("Invalid character while parsing the value of a parameter.", line);

							dList.Add(dval);
						}
					}

					if (c == ')') { done = true; break; } // Done.

					numberSB.Clear();
				}
				else throw new TextParserException("Invalid character while parsing the value of a parameter.", line);
			}

			// No closing round bracket parsed, so we're at the end of the stream.
			if (!done) throw new TextParserException("Unexpected end of stream while parsing the value of a parameter.", line);

			// Find the closing semicolon.
			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				if (c == ';')
				{
					if (isInteger) ret.Add(name, iList);
					else ret.Add(name, dList);
					return;
				}

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				throw new TextParserException("Invalid character while parsing the value of a parameter.", line);
			}

			throw new TextParserException("Unexpected end of stream while parsing the value of a parameter.", line);
		}

		/// <summary>
		/// Consumes a list of string values.
		/// </summary>
		/// <param name="c0">The already consumed first character of the first list entry.</param>
		/// <param name="ret">The <see cref="Parameters"/> in which the result has to be added.</param>
		/// <param name="name">The name of the parameter.</param>
		void ConsumeStringList(char c0, Parameters ret, string name)
		{
			List<string> tmp = new List<string>();

			string str = "";

			// Handle the first list entry
			if (c0 == '"')
			{
				str = ConsumeStringWithoutSemicolon();
				tmp.Add(str);
			}
			else if (c0 == '@')
			{
				str = ConsumeVerbatimStringWithoutSemicolon();
				tmp.Add(str);
			}
			else throw new ArgumentException("Must be either '\"' or '@'.", "c0");

			bool commaNext = true; // Indicates whether the next character must be a comma (ignoring the whitespaces and comments).
			bool done = false;
			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
					{
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					}
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				// Handle comma or done, or the next list entry.
				if (commaNext)
				{
					if (c == ')') { done = true; break; }
					if (c == ',') commaNext = false;
				}
				else if (c == '"')
				{
					str = ConsumeStringWithoutSemicolon();
					tmp.Add(str);
					commaNext = true;
				}
				else if (c == '@')
				{
					str = ConsumeVerbatimStringWithoutSemicolon();
					tmp.Add(str);
					commaNext = true;
				}
				else throw new TextParserException("Invalid character while parsing the value of a parameter.", line);
			}

			// No closing round bracket parsed, so we're at the end of the stream.
			if (!done) throw new TextParserException("Unexpected end of stream while parsing the value of a parameter.", line);

			// Find the closing semicolon.
			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				if (c == ';')
				{
					ret.Add(name, tmp);
					return;
				}

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
					{
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					}
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				throw new TextParserException("Invalid character while parsing the value of a parameter.", line);
			}

			throw new TextParserException("Unexpected end of stream while parsing the value of a parameter.", line);
		}

		/// <summary>
		/// Consumes a list of block values.
		/// </summary>
		/// <param name="ret">The <see cref="Parameters"/> in which the result has to be added.</param>
		/// <param name="name">The name of the parameter.</param>
		void ConsumeBlockList(Parameters ret, string name)
		{
			List<Parameters> tmp = new List<Parameters>();

			// Handle the first list entry
			Parameters param = ConsumeBlock(false);
			tmp.Add(param);

			bool commaNext = true; // Indicates whether the next character must be a comma (ignoring the whitespaces and comments).
			bool done = false;
			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
					{
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					}
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				// Handle comma or done, or the next list entry.
				if (commaNext)
				{
					if (c == ')') { done = true; break; }
					if (c == ',') commaNext = false;
				}
				else if (c == '{')
				{
					param = ConsumeBlock(false);
					tmp.Add(param);
					commaNext = true;
				}
				else throw new TextParserException("Invalid character while parsing the value of a parameter.", line);
			}

			// No closing round bracket parsed, so we're at the end of the stream.
			if (!done) throw new TextParserException("Unexpected end of stream while parsing the value of a parameter.", line);

			// Find the closing semicolon.
			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				if (c == ';')
				{
					ret.Add(name, tmp);
					return;
				}

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
					{
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					}
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				throw new TextParserException("Invalid character while parsing the value of a parameter.", line);
			}

			throw new TextParserException("Unexpected end of stream while parsing the value of a parameter.", line);
		}

		/// <summary>
		/// Consumes an empty list.
		/// </summary>
		/// <param name="ret">The <see cref="Parameters"/> in which the result has to be added.</param>
		/// <param name="name">The name of the parameter.</param>
		void ConsumeEmptyList(Parameters ret, string name)
		{
			// Find the closing semicolon.
			while (!stream.EndOfStream)
			{
				char c = (char)stream.Read();

				if (c == ';') return;

				// Handle whitespaces and count lines.
				if (c == '\t' || c == ' ') continue;
				if (c == '\n' || c == '\r')
				{
					if (c == '\r')
						if (!stream.EndOfStream && stream.Peek() == '\n') stream.Read();
					line++;
					continue;
				}

				// Handle comments.
				if (c == '/') { ConsumeComment(); continue; }

				throw new TextParserException("Invalid character while parsing the value of a parameter.", line);
			}

			throw new TextParserException("Unexpected end of stream while parsing the value of a parameter.", line);
		}
	}
}
