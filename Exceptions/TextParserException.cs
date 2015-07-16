using System;

namespace Free.Core.Exceptions
{
	/// <summary>
	/// The exception that is thrown when a text stream is in an invalid format.
	/// </summary>
	public class TextParserException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TextParserException"/> class with a specified error message.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		public TextParserException(string message) : base(message) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="TextParserException"/> class with a specified error message and information about where in the text stream the error occurred.
		/// </summary>
		/// <remarks>
		/// The information, about where in the text stream the error occurred, will be merged with the <paramref name="message"/> and is not separatly available.
		/// </remarks>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="line">The line in the text stream the error occurred.</param>
		public TextParserException(string message, int line) : base(string.Format("{0} (line: {1})", message, line)) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="TextParserException"/> class with a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not <b>null</b>, the current
		/// exception is raised in a <b>catch</b> block that handles the inner exception.</param>
		public TextParserException(string message, Exception innerException) : base(message, innerException) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="TextParserException"/> class with a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <remarks>
		/// The information, about where in the text stream the error occurred, will be merged with the <paramref name="message"/> and is not separatly available.
		/// </remarks>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="line">The line in the text stream the error occurred.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not <b>null</b>, the current
		/// exception is raised in a <b>catch</b> block that handles the inner exception.</param>
		public TextParserException(string message, int line, Exception innerException) : base(string.Format("{0} (line: {1})", message, line), innerException) { }
	}
}
