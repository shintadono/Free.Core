using System;
using System.IO;

namespace Free.Core.IO
{
	/// <summary>
	/// Describes a bitwise stream-reader. All operation are performed on the bits, not the bytes, of a stream (bit-stream), including seek operations.
	/// </summary>
	[CLSCompliant(false)]
	public interface IBitStreamReader : IDisposable
	{
		/// <summary>
		/// Gets whether the stream reader can seek.
		/// </summary>
		bool CanSeek { get; }

		/// <summary>
		/// Gets the length of the bit-stream in bits.
		/// </summary>
		long Length { get; }

		/// <summary>
		/// Gets the current location in the bit-stream in bits.
		/// </summary>
		long Position { get; }

		/// <summary>
		/// Returns the next available bit from the bit-stream.
		/// </summary>
		/// <returns>Zero (0) or one (1), depending on the value of the bit, if succesful; otherwise, minus one (-1).</returns>
		int ReadBit();

		/// <summary>
		/// Returns the next available bits from the bit-stream.
		/// </summary>
		/// <param name="bits">The number of bits to read. Must be positive. Default is one (1).</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="bits"/> is negative or zero (0).</exception>
		/// <exception cref="EndOfStreamException">If there are not enough bits left, to fullfill the request.</exception>
		/// <returns>An array containing the bits, starting at the least-significant-bit of the first byte; otherwise, <b>null</b>.</returns>
		byte[] ReadBits(long bits = 1);

		/// <summary>
		/// Returns the next available bits from the bit-stream as <b>byte</b>.
		/// </summary>
		/// <param name="bits">The number of bits to read. Only values one (1) to eight (8) are allowed. Default is one (1).</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="bits"/> is not in the range one (1) to eight (8).</exception>
		/// <exception cref="EndOfStreamException">If there are not enough bits left, to fullfill the request.</exception>
		/// <returns>An <b>byte</b> containing the bits, starting at the least-significant-bit of the <b>byte</b>.</returns>
		byte ReadBitsAsByte(int bits = 1);

		/// <summary>
		/// Returns the next available bits from the bit-stream as <b>sbyte</b>.
		/// </summary>
		/// <param name="bits">The number of bits to read. Only values one (1) to eight (8) are allowed. Default is one (1).</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="bits"/> is not in the range one (1) to eight (8).</exception>
		/// <exception cref="EndOfStreamException">If there are not enough bits left, to fullfill the request.</exception>
		/// <returns>An <b>sbyte</b> containing the bits, starting at the least-significant-bit of the <b>sbyte</b>.</returns>
		sbyte ReadBitsAsSByte(int bits = 1);

		/// <summary>
		/// Returns the next available bits from the bit-stream as <b>short</b>.
		/// </summary>
		/// <param name="bits">The number of bits to read. Only values one (1) to eight (16) are allowed. Default is one (1).</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="bits"/> is not in the range one (1) to eight (16).</exception>
		/// <exception cref="EndOfStreamException">If there are not enough bits left, to fullfill the request.</exception>
		/// <returns>An <b>short</b> containing the bits, starting at the least-significant-bit of the <b>short</b>.</returns>
		short ReadBitsAsInt16(int bits = 1);

		/// <summary>
		/// Returns the next available bits from the bit-stream as <b>ushort</b>.
		/// </summary>
		/// <param name="bits">The number of bits to read. Only values one (1) to eight (16) are allowed. Default is one (1).</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="bits"/> is not in the range one (1) to eight (16).</exception>
		/// <exception cref="EndOfStreamException">If there are not enough bits left, to fullfill the request.</exception>
		/// <returns>An <b>ushort</b> containing the bits, starting at the least-significant-bit of the <b>ushort</b>.</returns>
		ushort ReadBitsAsUInt16(int bits = 1);

		/// <summary>
		/// Returns the next available bits from the bit-stream as <b>int</b>.
		/// </summary>
		/// <param name="bits">The number of bits to read. Only values one (1) to eight (32) are allowed. Default is one (1).</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="bits"/> is not in the range one (1) to eight (32).</exception>
		/// <exception cref="EndOfStreamException">If there are not enough bits left, to fullfill the request.</exception>
		/// <returns>An <b>int</b> containing the bits, starting at the least-significant-bit of the <b>int</b>.</returns>
		int ReadBitsAsInt32(int bits = 1);

		/// <summary>
		/// Returns the next available bits from the bit-stream as <b>uint</b>.
		/// </summary>
		/// <param name="bits">The number of bits to read. Only values one (1) to eight (32) are allowed. Default is one (1).</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="bits"/> is not in the range one (1) to eight (32).</exception>
		/// <exception cref="EndOfStreamException">If there are not enough bits left, to fullfill the request.</exception>
		/// <returns>An <b>uint</b> containing the bits, starting at the least-significant-bit of the <b>uint</b>.</returns>
		uint ReadBitsAsUInt32(int bits = 1);

		/// <summary>
		/// Returns the next available bits from the bit-stream as <b>long</b>.
		/// </summary>
		/// <param name="bits">The number of bits to read. Only values one (1) to eight (64) are allowed. Default is one (1).</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="bits"/> is not in the range one (1) to eight (64).</exception>
		/// <exception cref="EndOfStreamException">If there are not enough bits left, to fullfill the request.</exception>
		/// <returns>An <b>long</b> containing the bits, starting at the least-significant-bit of the <b>long</b>.</returns>
		long ReadBitsAsInt64(int bits = 1);

		/// <summary>
		/// Returns the next available bits from the bit-stream as <b>ulong</b>.
		/// </summary>
		/// <param name="bits">The number of bits to read. Only values one (1) to eight (64) are allowed. Default is one (1).</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="bits"/> is not in the range one (1) to eight (64).</exception>
		/// <exception cref="EndOfStreamException">If there are not enough bits left, to fullfill the request.</exception>
		/// <returns>An <b>ulong</b> containing the bits, starting at the least-significant-bit of the <b>ulong</b>.</returns>
		ulong ReadBitsAsUInt64(int bits = 1);

		/// <summary>
		/// Returns the next available bit from the bit-stream, but does not consume it.
		/// </summary>
		/// <returns>Zero (0) or one (1), depending on the value of the bit, if succesful; otherwise, minus one (-1).</returns>
		int PeekBit();

		/// <summary>
		/// Skips the specified amount of bits.
		/// </summary>
		/// <param name="bits">The number of bits to skip. Must be positive. Default is one (1).</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="bits"/> is negative or zero (0).</exception>
		/// <exception cref="EndOfStreamException">If there are not enough bits left, to fullfill the request.</exception>
		/// <remarks>This is the same as preforming: SeekBits(bits, SeekOrigin.Current).</remarks>
		void SkipBits(long bits = 1);

		/// <summary>
		/// Set the current location in the bit-stream.
		/// </summary>
		/// <param name="offset">The new position or the offset, depending on the specified <paramref name="origin"/>.</param>
		/// <param name="origin">The origin, against which the <paramref name="offset"/> is given. Default is <see cref="SeekOrigin.Begin"/></param>
		/// <exception cref="EndOfStreamException">If there are not enough bits left, to fullfill the request.</exception>
		/// <exception cref="NotSupportedException">If the stream doesn't support seeking.</exception>
		void SeekBits(long offset, SeekOrigin origin = SeekOrigin.Begin);
	}
}
