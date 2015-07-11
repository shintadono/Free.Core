namespace Free.Core.IO
{
	/// <summary>
	/// <para>Provides the fields that represent reference points (first bit) in bit streams.</para>
	/// <para>Example - 3 Bits (a, b and c; a is the first and c the last bit; '?' unused bits) in a byte:</para>
	/// <code language="none">
	/// |7|6|5|4|3|2|1|0| - Bits of the byte
	/// +-+-+-+-+-+-+-+-+
	/// |a|b|c|?|?|?|?|?| - MostSignificantBitHigh/MSBHigh
	/// |?|?|?|?|?|c|b|a| - LeastSignificantBitLow/LSBLow
	/// |?|?|?|?|?|a|b|c| - MostSignificantBitLow/MSBLow
	/// |c|b|a|?|?|?|?|?| - LeastSignificantBitHigh/LSBHigh
	/// </code>
	/// </summary>
	public enum BitStreamBitArrangement
	{
		/// <summary>
		/// The first bit is placed at the "Most Significant Bit" (Bit 7) of the first byte of an byte array or at the "Most Significant Bit" of the integer.
		/// </summary>
		MostSignificantBitHigh,

		/// <summary>
		/// The first bit is placed at the "Least Significant Bit" (Bit 0) of the last byte of an byte array or at the "Least Significant Bit" of the integer.
		/// </summary>
		LeastSignificantBitLow,

		/// <summary>
		/// The last bit is placed at the "Least Significant Bit" (Bit 0) of the last byte of an byte array or at the "Least Significant Bit" of the integer.
		/// </summary>
		MostSignificantBitLow,

		/// <summary>
		/// The last bit is placed at the "Most Significant Bit" (Bit 7) of the first byte of an byte array or at the "Most Significant Bit" of the integer.
		/// </summary>
		LeastSignificantBitHigh,

		/// <summary>
		/// Abbreviation for <see cref="MostSignificantBitHigh"/>.
		/// </summary>
		MSBHigh=MostSignificantBitHigh,

		/// <summary>
		/// Abbreviation for <see cref="LeastSignificantBitLow"/>.
		/// </summary>
		LSBLow=LeastSignificantBitLow,

		/// <summary>
		/// Abbreviation for <see cref="MostSignificantBitLow"/>.
		/// </summary>
		MSBLow=MostSignificantBitLow,

		/// <summary>
		/// Abbreviation for <see cref="LeastSignificantBitHigh"/>.
		/// </summary>
		LSBHigh=LeastSignificantBitHigh,
	}
}
