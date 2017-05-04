namespace Free.Core.Drawing
{
	/// <summary>
	/// Describes the data type of the channel in an image / raster instances.
	/// </summary>
	public enum ChannelType
	{
		/// <summary>
		/// The data type is not specified.
		/// </summary>
		Unknown,

		/// <summary>
		/// Unsigned 8-bit integer.
		/// </summary>
		Byte,

		/// <summary>
		/// Signed 8-bit integer.
		/// </summary>
		SByte,

		/// <summary>
		/// Unsigned 16-bit integer.
		/// </summary>
		UShort,

		/// <summary>
		/// Signed 16-bit integer.
		/// </summary>
		SShort,

		/// <summary>
		/// Unsigned 32-bit integer.
		/// </summary>
		UInt,

		/// <summary>
		/// Signed 32-bit integer.
		/// </summary>
		SInt,

		/// <summary>
		/// Unsigned 64-bit integer.
		/// </summary>
		ULong,

		/// <summary>
		/// Signed 64-bit integer.
		/// </summary>
		SLong,

		/// <summary>
		/// Signed 32-bit floating point.
		/// </summary>
		Single,

		/// <summary>
		/// Signed 64-bit floating point.
		/// </summary>
		Double,

		/// <summary>
		/// The data type is a structure of zero or more arbitrarily typed elements.
		/// </summary>
		Structure,
	}
}
