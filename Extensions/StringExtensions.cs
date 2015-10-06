using System;

namespace Free.Core.Extensions.StringExtensions
{
	/// <summary>
	/// Added methods to the <see cref="String"/> class.
	/// </summary>
	/// <threadsafety static="true" instance="true"/>
	public static class StringExtensions
	{
		/// <summary>
		/// Determines whether a <b>string</b> is completely consisting of the decimal digit characters [0-9].
		/// </summary>
		/// <param name="str">The <b>string</b> to test.</param>
		/// <returns><b>true</b> if the <b>string</b> is completely consisting of the decimal digit characters; otherwise, <b>false</b>.</returns>
		public static bool IsDigit(this string str)
		{
			if (string.IsNullOrWhiteSpace(str)) return false;

			for(int i=0; i<str.Length; i++)
			{
				char ch = str[i];
				if (ch < '0' || ch > '9') return false;
			}
			
			return true;
		}

		/// <summary>
		/// Determines whether a <b>string</b> is completely consisting of the hexadecimal digit characters [0-9, a-f, A-F].
		/// </summary>
		/// <param name="str">The <b>string</b> to test.</param>
		/// <returns><b>true</b> if the <b>string</b> is completely consisting of the hexadecimal digit characters; otherwise, <b>false</b>.</returns>
		public static bool IsHexDigit(this string str)
		{
			if (string.IsNullOrWhiteSpace(str)) return false;

			for (int i = 0; i < str.Length; i++)
			{
				char ch = str[i];
				if ((ch < '0' || ch > '9')&&(ch < 'A' || ch > 'F')&&(ch < 'a' || ch > 'f')) return false;
			}

			return true;
		}

		/// <summary>
		/// Determines whether a string is a number (floating-point with or without exponent, or integer in decimal or hexadecimal encoding).
		/// </summary>
		/// <param name="number">The <b>string</b> to test.</param>
		/// <returns><b>true</b> if the <b>string</b> is a number; otherwise, <b>false</b>.</returns>
		public static bool IsNumber(this string number)
		{
			if (string.IsNullOrWhiteSpace(number)) return false;

			// Handle sign
			if (number[0] == '+') // Integer only
				return number.Substring(1).IsIntegerWithoutSign();

			if (number[0] == '-')
			{
				if (number.Substring(1).IsIntegerWithoutSign()) return true;
				return number.Substring(1).IsFloatWithoutSign();
			}

			// No sign
			if (number.IsIntegerWithoutSign()) return true;
			return number.IsFloatWithoutSign();
		}

		/// <summary>
		/// Determines whether a string is an integer in decimal or hexadecimal encoding.
		/// </summary>
		/// <param name="number">The <b>string</b> to test.</param>
		/// <returns><b>true</b> if the <b>string</b> is an integer; otherwise, <b>false</b>.</returns>
		public static bool IsInteger(this string number)
		{
			int dummy = 0;
            		return Int32.TryParse(number, out dummy);
		}

		/// <summary>
		/// Determines whether a string is an integer in decimal or hexadecimal encoding; signs have to be trimmed prior to the call of this method.
		/// </summary>
		/// <param name="integerWithoutSign">The <b>string</b> to test.</param>
		/// <returns><b>true</b> if the <b>string</b> is an integer; otherwise, <b>false</b>.</returns>
		static bool IsIntegerWithoutSign(this string integerWithoutSign)
		{
			int integerlen = integerWithoutSign.Length;
			if (integerlen < 1) return false;

			// Hexadecimal encoding
			if (integerWithoutSign.IndexOf("0x") == 0) return integerWithoutSign.Substring(2).IsHexDigit();

			return integerWithoutSign.IsDigit();
		}

		/// <summary>
		/// Determines whether a string is a floating-point number with or without exponent.
		/// </summary>
		/// <param name="number">The <b>string</b> to test.</param>
		/// <returns><b>true</b> if the <b>string</b> is a floating-point number; otherwise, <b>false</b>.</returns>
		public static bool IsFloat(this string number)
		{
			double dummy = 0;
            		return Double.TryParse(number, out dummy);
		}

		/// <summary>
		/// Determines whether a string is a floating-point number; signs have to be trimmed prior to the call of this method.
		/// </summary>
		/// <param name="numberWithoutSign">The <b>string</b> to test.</param>
		/// <returns><b>true</b> if the <b>string</b> is a floating-point number; otherwise, <b>false</b>.</returns>
		static bool IsFloatWithoutSign(this string numberWithoutSign)
		{
			if (numberWithoutSign.Length < 3) return false;

			char c = numberWithoutSign[0];
			if (c < '0' || c > '9') return false; // need at least one digit at the beginning

			c = numberWithoutSign[1];
			if (c == '.')
			{
				if (numberWithoutSign.IsSimpleFloat()) return true;
				return numberWithoutSign.IsExpFloat();
			}

			if (c == 'e' || c == 'E') return numberWithoutSign.Insert(1, ".0").IsExpFloat();

			return numberWithoutSign.IsSimpleFloat();
		}

		/// <summary>
		/// Determines whether a string is a floating-point number without exponent; signs have to be trimmed and the first char tested as digit prior to the call of this method.
		/// </summary>
		/// <param name="numberWithoutSign">The <b>string</b> to test.</param>
		/// <returns><b>true</b> if the <b>string</b> is a floating-point number without exponent; otherwise, <b>false</b>.</returns>
		static bool IsSimpleFloat(this string numberWithoutSign)
		{
			if (string.IsNullOrWhiteSpace(numberWithoutSign)) return false;

			int len = numberWithoutSign.Length;
			int token = 1;
			bool foundDecimalPoint = false;
			bool foundDigitsAfterDecimalPoint = false;

			while (token < len)
			{
				char c = numberWithoutSign[token++];
				if (c == '.')
				{
					if (foundDecimalPoint) return false; // to many decimal points
					if (token == len) return false; // decimal points is not allowed at the end
					foundDecimalPoint = true;
					continue;
				}
				if (c < '0' || c > '9') return false;
				if (foundDecimalPoint) foundDigitsAfterDecimalPoint = true;
			}

			return foundDigitsAfterDecimalPoint;
		}

		/// <summary>
		/// Determines whether a string is a floating-point number with exponent; signs have to be trimmed and the first two chars tested as digit and decimal point prior to the call of this method.
		/// </summary>
		/// <param name="numberWithoutSign">The <b>string</b> to test.</param>
		/// <returns><b>true</b> if the <b>string</b> is a floating-point number with exponent; otherwise, <b>false</b>.</returns>
		static bool IsExpFloat(this string numberWithoutSign)
		{
			if (string.IsNullOrWhiteSpace(numberWithoutSign)) return false;

			int numberlen = numberWithoutSign.Length;

			char c = numberWithoutSign[2]; // first two chars alreasy tested as digit and decimal point
			if (c < '0' || c > '9') return false; // need at least one digit after decimal point before the exponent starts

			int token = 3;

			while (token < numberlen)
			{
				c = numberWithoutSign[token++];

				// Handle exponent
				if (c == 'e' || c == 'E')
				{
					// minimum 1 and maximum 3 digits and an optional sign allowed in the exponent
					if ((numberlen - token) > 4) return false;

					c = numberWithoutSign[token];
					if (c == '+' || c == '-') token++;

					// minimum 1 and maximum 3 digits allowed
					if ((numberlen - token) > 3) return false;
					if ((numberlen - token) == 0) return false;

					int len = numberlen - token;
					for (int i = 0; i < len; i++)
					{
						c = numberWithoutSign[token++];
						if (c < '0' || c > '9') return false;
					}

					return true;
				}

				if (c < '0' || c > '9') return false;
			}

			return true;
		}
	}

#if USE_NAMESPACE_DOC_CLASSES
	/// <summary>
	/// Added methods to the <see cref="String"/> class.
	/// </summary>
	[System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
	class NamespaceDoc { }
#endif
}
