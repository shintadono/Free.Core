using System;

namespace Free.Core.Extensions.StringSplitExtensions
{
	/// <summary>
	/// A more convenient set of <see cref="O:System.String.Split">string.Split(...)</see> methods where you don't need to create the separator arrays beforehand.
	/// </summary>
	/// <threadsafety static="true" instance="true"/>
	public static class StringSplitExtensions
	{
		/// <summary>
		/// A more convenient version of <see cref="string.Split(char[], StringSplitOptions)"/> where you don't need to set the separator array to <c>null</c>.
		/// </summary>
		/// <param name="that">The new <c>this</c>.</param>
		/// <param name="options"><see cref="StringSplitOptions.RemoveEmptyEntries"/> to omit empty array elements from the array returned; or <see cref="StringSplitOptions.None"/> to include empty array elements in the array returned.</param>
		/// <returns>An array with the split up elements of the string. See <see cref="string.Split(char[], StringSplitOptions)"/> for more informations.</returns>
		public static string[] Split(this string that, StringSplitOptions options)
		{
			return that.Split((char[])null, options);
		}

		/// <summary>
		/// A more convenient version of <see cref="string.Split(char[], int, StringSplitOptions)"/> where you don't need to set the separator array to <c>null</c>.
		/// </summary>
		/// <param name="that">The new <c>this</c>.</param>
		/// <param name="options"><see cref="StringSplitOptions.RemoveEmptyEntries"/> to omit empty array elements from the array returned; or <see cref="StringSplitOptions.None"/> to include empty array elements in the array returned.</param>
		/// <param name="count">The maximum number of substrings to return.</param>
		/// <returns>An array with the split up elements of the string. See <see cref="string.Split(char[], int, StringSplitOptions)"/> for more informations.</returns>
		public static string[] Split(this string that, StringSplitOptions options, int count)
		{
			return that.Split((char[])null, count, options);
		}

		/// <summary>
		/// A more convenient version of <see cref="string.Split(char[], StringSplitOptions)"/> where you don't need to create the separator arrays beforehand.
		/// </summary>
		/// <param name="that">The new <c>this</c>.</param>
		/// <param name="options"><see cref="StringSplitOptions.RemoveEmptyEntries"/> to omit empty array elements from the array returned; or <see cref="StringSplitOptions.None"/> to include empty array elements in the array returned.</param>
		/// <param name="separator">One or more Unicode characters that delimit the substrings in this string.</param>
		/// <returns>An array with the split up elements of the string. See <see cref="string.Split(char[], StringSplitOptions)"/> for more informations.</returns>
		public static string[] Split(this string that, StringSplitOptions options, params char[] separator)
		{
			return that.Split(separator, options);
		}

		/// <summary>
		/// A more convenient version of <see cref="string.Split(char[], int, StringSplitOptions)"/> where you don't need to create the separator arrays beforehand.
		/// </summary>
		/// <param name="that">The new <c>this</c>.</param>
		/// <param name="options"><see cref="StringSplitOptions.RemoveEmptyEntries"/> to omit empty array elements from the array returned; or <see cref="StringSplitOptions.None"/> to include empty array elements in the array returned.</param>
		/// <param name="count">The maximum number of substrings to return.</param>
		/// <param name="separator">One or more Unicode characters that delimit the substrings in this string.</param>
		/// <returns>An array with the split up elements of the string. See <see cref="string.Split(char[], int, StringSplitOptions)"/> for more informations.</returns>
		public static string[] Split(this string that, StringSplitOptions options, int count, params char[] separator)
		{
			return that.Split(separator, count, options);
		}

		/// <summary>
		/// A more convenient version of <see cref="string.Split(string[], StringSplitOptions)"/> where you don't need to create the separator arrays beforehand.
		/// </summary>
		/// <param name="that">The new <c>this</c>.</param>
		/// <param name="options"><see cref="StringSplitOptions.RemoveEmptyEntries"/> to omit empty array elements from the array returned; or <see cref="StringSplitOptions.None"/> to include empty array elements in the array returned.</param>
		/// <param name="separator">One or more strings that delimit the substrings in this string.</param>
		/// <returns>An array with the split up elements of the string. See <see cref="string.Split(string[], StringSplitOptions)"/> for more informations.</returns>
		public static string[] Split(this string that, StringSplitOptions options, params string[] separator)
		{
			return that.Split(separator, options);
		}

		/// <summary>
		/// A more convenient version of <see cref="string.Split(string[], int, StringSplitOptions)"/> where you don't need to create the separator arrays beforehand.
		/// </summary>
		/// <param name="that">The new <c>this</c>.</param>
		/// <param name="options"><see cref="StringSplitOptions.RemoveEmptyEntries"/> to omit empty array elements from the array returned; or <see cref="StringSplitOptions.None"/> to include empty array elements in the array returned.</param>
		/// <param name="count">The maximum number of substrings to return.</param>
		/// <param name="separator">One or more strings that delimit the substrings in this string.</param>
		/// <returns>An array with the split up elements of the string. See <see cref="string.Split(string[], int, StringSplitOptions)"/> for more informations.</returns>
		public static string[] Split(this string that, StringSplitOptions options, int count, params string[] separator)
		{
			return that.Split(separator, count, options);
		}
	}

#if USE_NAMESPACE_DOC_CLASSES
	/// <summary>
	/// A more convenient set of <see cref="O:System.String.Split">string.Split(...)</see> methods where you don't need to create the separator arrays beforehand.
	/// </summary>
	[System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
	class NamespaceDoc { }
#endif
}
