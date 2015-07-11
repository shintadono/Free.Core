using System;
using System.IO;

namespace Free.Core.IO
{
	/// <summary>
	/// 
	/// </summary>
	public interface IBitStreamReader : IDisposable
	{
		/// <summary>
		/// 
		/// </summary>
		bool CanSeek { get; }

		/// <summary>
		/// 
		/// </summary>
		long Length { get; }

		/// <summary>
		/// 
		/// </summary>
		long Position { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		int ReadBit();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bits"></param>
		/// <returns></returns>
		byte[] ReadBits(long bits=1);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bits"></param>
		/// <returns></returns>
		byte ReadBitsAsByte(int bits=1);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bits"></param>
		/// <returns></returns>
		sbyte ReadBitsAsSByte(int bits=1);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bits"></param>
		/// <returns></returns>
		short ReadBitsAsInt16(int bits=1);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bits"></param>
		/// <returns></returns>
		ushort ReadBitsAsUInt16(int bits=1);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bits"></param>
		/// <returns></returns>
		int ReadBitsAsInt32(int bits=1);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bits"></param>
		/// <returns></returns>
		uint ReadBitsAsUInt32(int bits=1);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bits"></param>
		/// <returns></returns>
		long ReadBitsAsInt64(int bits=1);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bits"></param>
		/// <returns></returns>
		ulong ReadBitsAsUInt64(int bits=1);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		int PeekBit();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bits"></param>
		void SkipBits(long bits=1);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="offset"></param>
		/// <param name="origin"></param>
		void SeekBits(long offset, SeekOrigin origin);
	}
}
