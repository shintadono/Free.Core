using System;

namespace Free.Core.Extensions.CharExtensions
{
	/// <summary>
	/// Added methods to the <see cref="Char"/> class.
	/// </summary>
	/// <threadsafety static="true" instance="true"/>
	[CLSCompliant(false)]
	public static class CharExtensions
	{
		/// <summary>
		/// The minimum allowed radix value as argument in <see cref="Digit"/>.
		/// Only character in ['0' '1'] will return a non-minus-one value.
		/// </summary>
		public const uint MinimumRadix = 1;

		/// <summary>
		/// The minimum allowed radix value as argument in <see cref="Digit"/>.
		/// Only character in ['0'-'9', 'a'-'z', 'A'-'Z'] will return a non-minus-one value.
		/// </summary>
		public const uint MaximumRadix = 36;

		/// <summary>
		/// Determines whether a character is a hexadecimal digit character [0-9, a-f, A-F].
		/// </summary>
		/// <param name="ch">The <b>string</b> to test.</param>
		/// <returns><b>true</b> if the character is a hexadecimal digit character; otherwise, <b>false</b>.</returns>
		public static bool IsHexDigit(this char ch)
		{
			if ((ch < '0' || ch > '9') && (ch < 'A' || ch > 'F') && (ch < 'a' || ch > 'f')) return false;
			return true;
		}

		/// <summary>
		/// Converts a character to it number value limiting the result with the specified <paramref name="radix"/>.
		/// </summary>
		/// <param name="ch">The character to convert.</param>
		/// <param name="radix">The radix. The result value will be check against this. [<see cref="MinimumRadix"/> <see cref="MaximumRadix"/>]</param>
		/// <returns>The number value of the character regarding the radix; otherwise, minus one (-1).</returns>
		public static int Digit(this char ch, uint radix=10)
		{
			int value = -1;

			if (radix >= MinimumRadix && radix <= MaximumRadix)
			{
				if (ch >= '0' && ch <= '9') value = ch - '0';
				if (ch >= 'a' && ch <= 'z') value = 10 + ch - 'a';
				if (ch >= 'A' && ch <= 'Z') value = 10 + ch - 'A';
			}

			return (value < (int)radix) ? value : -1;
		}
	}

#if USE_NAMESPACE_DOC_CLASSES
	/// <summary>
	/// Added methods to the <see cref="Char"/> class.
	/// </summary>
	[System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
	class NamespaceDoc { }
#endif
}
