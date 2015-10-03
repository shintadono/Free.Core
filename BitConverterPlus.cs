using System;

namespace Free.Core
{
	/// <summary>
	/// BitConverter+
	/// </summary>
	/// <threadsafety static="true" instance="true"/>
	public static class BitConverterPlus
	{
		#region IsLittleEndian
		/// <summary>
		/// Helper, used to check byte-order and to initialize <see cref="IsLittleEndian"/>.
		/// </summary>
		/// <returns><c>true</c> for little-endian. <c>false</c> for big-endian.</returns>
		static unsafe bool GetIsLittleEndian()
		{
			ushort us=0xFF00;
			byte* b=(byte*)&us;
			return b[0]==0;
		}

		/// <summary>
		/// Indicates the byte-order used in this computer architecture.
		/// </summary>
		public static readonly bool IsLittleEndian=GetIsLittleEndian();
		#endregion

		#region GetBytes
		/// <summary>
		/// Returns the specified bool value as an array of bytes.
		/// </summary>
		/// <param name="value">A bool to convert.</param>
		/// <returns>An array of bytes with length 1.</returns>
		public static byte[] GetBytes(bool value)
		{
			return new byte[1] { value?(byte)1:(byte)0 };
		}

		/// <summary>
		/// Returns the specified Unicode character value as an array of bytes.
		/// </summary>
		/// <param name="value">An Unicode character to convert.</param>
		/// <returns>An array of bytes with length 2.</returns>
		public static unsafe byte[] GetBytes(char value)
		{
			byte* ptr=(byte*)&value;
			return new byte[2] { ptr[0], ptr[1] };
		}

		/// <summary>
		/// Returns the specified 16-bit signed integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 2.</returns>
		public static unsafe byte[] GetBytes(short value)
		{
			byte* ptr=(byte*)&value;
			return new byte[2] { ptr[0], ptr[1] };
		}

		/// <summary>
		/// Returns the specified 16-bit unsigned integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 2.</returns>
		[CLSCompliant(false)]
		public static unsafe byte[] GetBytes(ushort value)
		{
			byte* ptr=(byte*)&value;
			return new byte[2] { ptr[0], ptr[1] };
		}

		/// <summary>
		/// Returns the specified 32-bit signed integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 4.</returns>
		public static unsafe byte[] GetBytes(int value)
		{
			byte* ptr=(byte*)&value;
			return new byte[4] { ptr[0], ptr[1], ptr[2], ptr[3] };
		}

		/// <summary>
		/// Returns the specified 32-bit unsigned integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 4.</returns>
		[CLSCompliant(false)]
		public static unsafe byte[] GetBytes(uint value)
		{
			byte* ptr=(byte*)&value;
			return new byte[4] { ptr[0], ptr[1], ptr[2], ptr[3] };
		}

		/// <summary>
		/// Returns the specified 64-bit signed integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 8.</returns>
		public static unsafe byte[] GetBytes(long value)
		{
			byte* ptr=(byte*)&value;
			return new byte[8] { ptr[0], ptr[1], ptr[2], ptr[3], ptr[4], ptr[5], ptr[6], ptr[7] };
		}

		/// <summary>
		/// Returns the specified 64-bit unsigned integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 8.</returns>
		[CLSCompliant(false)]
		public static unsafe byte[] GetBytes(ulong value)
		{
			byte* ptr=(byte*)&value;
			return new byte[8] { ptr[0], ptr[1], ptr[2], ptr[3], ptr[4], ptr[5], ptr[6], ptr[7] };
		}

		/// <summary>
		/// Returns the specified 128-bit unsigned integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 16.</returns>
		[CLSCompliant(false)]
		public static unsafe byte[] GetBytes(UInt128 value)
		{
			byte* ptrHigh=(byte*)&value.High;
			byte* ptrLow=(byte*)&value.Low;
			return new byte[16] {
				ptrLow[0], ptrLow[1], ptrLow[2], ptrLow[3], ptrLow[4], ptrLow[5], ptrLow[6], ptrLow[7],
				ptrHigh[0], ptrHigh[1], ptrHigh[2], ptrHigh[3], ptrHigh[4], ptrHigh[5], ptrHigh[6], ptrHigh[7]};
		}

		/// <summary>
		/// Returns the specified single-precision floating point value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 4.</returns>
		public static unsafe byte[] GetBytes(float value)
		{
			byte* ptr=(byte*)&value;
			return new byte[4] { ptr[0], ptr[1], ptr[2], ptr[3] };
		}

		/// <summary>
		/// Returns the specified double-precision floating point value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 8.</returns>
		public static unsafe byte[] GetBytes(double value)
		{
			byte* ptr=(byte*)&value;
			return new byte[8] { ptr[0], ptr[1], ptr[2], ptr[3], ptr[4], ptr[5], ptr[6], ptr[7] };
		}
		#endregion

		#region GetBytesSwapped
		/// <summary>
		/// Returns the specified bool value as an array of bytes in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">A bool to convert.</param>
		/// <returns>An array of bytes with length 1 in reversed byte-order to this computer architecture.</returns>
		public static byte[] GetBytesSwapped(bool value)
		{
			return new byte[1] { value?(byte)1:(byte)0 };
		}

		/// <summary>
		/// Returns the specified Unicode character value as an array of bytes in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An Unicode character to convert.</param>
		/// <returns>An array of bytes with length 2 in reversed byte-order to this computer architecture.</returns>
		public static unsafe byte[] GetBytesSwapped(char value)
		{
			byte* ptr=(byte*)&value;
			return new byte[2] { ptr[1], ptr[0] };
		}

		/// <summary>
		/// Returns the specified 16-bit signed integer value as an array of bytes in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 2 in reversed byte-order to this computer architecture.</returns>
		public static unsafe byte[] GetBytesSwapped(short value)
		{
			byte* ptr=(byte*)&value;
			return new byte[2] { ptr[1], ptr[0] };
		}

		/// <summary>
		/// Returns the specified 16-bit unsigned integer value as an array of bytes in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 2 in reversed byte-order to this computer architecture.</returns>
		[CLSCompliant(false)]
		public static unsafe byte[] GetBytesSwapped(ushort value)
		{
			byte* ptr=(byte*)&value;
			return new byte[2] { ptr[1], ptr[0] };
		}

		/// <summary>
		/// Returns the specified 32-bit signed integer value as an array of bytes in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 4 in reversed byte-order to this computer architecture.</returns>
		public static unsafe byte[] GetBytesSwapped(int value)
		{
			byte* ptr=(byte*)&value;
			return new byte[4] { ptr[3], ptr[2], ptr[1], ptr[0] };
		}

		/// <summary>
		/// Returns the specified 32-bit unsigned integer value as an array of bytes in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 4 in reversed byte-order to this computer architecture.</returns>
		[CLSCompliant(false)]
		public static unsafe byte[] GetBytesSwapped(uint value)
		{
			byte* ptr=(byte*)&value;
			return new byte[4] { ptr[3], ptr[2], ptr[1], ptr[0] };
		}

		/// <summary>
		/// Returns the specified 64-bit signed integer value as an array of bytes in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 8 in reversed byte-order to this computer architecture.</returns>
		public static unsafe byte[] GetBytesSwapped(long value)
		{
			byte* ptr=(byte*)&value;
			return new byte[8] { ptr[7], ptr[6], ptr[5], ptr[4], ptr[3], ptr[2], ptr[1], ptr[0] };
		}

		/// <summary>
		/// Returns the specified 64-bit unsigned integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 8 in reversed byte-order to this computer architecture.</returns>
		[CLSCompliant(false)]
		public static unsafe byte[] GetBytesSwapped(ulong value)
		{
			byte* ptr=(byte*)&value;
			return new byte[8] { ptr[7], ptr[6], ptr[5], ptr[4], ptr[3], ptr[2], ptr[1], ptr[0] };
		}

		/// <summary>
		/// Returns the specified 128-bit unsigned integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 16 in reversed byte-order to this computer architecture.</returns>
		[CLSCompliant(false)]
		public static unsafe byte[] GetBytesSwapped(UInt128 value)
		{
			byte* ptrHigh=(byte*)&value.High;
			byte* ptrLow=(byte*)&value.Low;
			return new byte[16] {
				ptrHigh[7], ptrHigh[6], ptrHigh[5], ptrHigh[4], ptrHigh[3], ptrHigh[2], ptrHigh[1], ptrHigh[0],
				ptrLow[7], ptrLow[6], ptrLow[5], ptrLow[4], ptrLow[3], ptrLow[2], ptrLow[1], ptrLow[0] };
		}

		/// <summary>
		/// Returns the specified single-precision floating point value as an array of bytes in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 4 in reversed byte-order to this computer architecture.</returns>
		public static unsafe byte[] GetBytesSwapped(float value)
		{
			byte* ptr=(byte*)&value;
			return new byte[4] { ptr[3], ptr[2], ptr[1], ptr[0] };
		}

		/// <summary>
		/// Returns the specified double-precision floating point value as an array of bytes in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 8 in reversed byte-order to this computer architecture.</returns>
		public static unsafe byte[] GetBytesSwapped(double value)
		{
			byte* ptr=(byte*)&value;
			return new byte[8] { ptr[7], ptr[6], ptr[5], ptr[4], ptr[3], ptr[2], ptr[1], ptr[0] };
		}
		#endregion

		#region GetBytes + bool bigEndian
		/// <summary>
		/// Returns the specified bool value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">A bool to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <returns>An array of bytes with length 1 in the given byte-order.</returns>
		public static byte[] GetBytes(bool value, bool bigEndian)
		{
			return IsLittleEndian^bigEndian?GetBytes(value):GetBytesSwapped(value);
		}

		/// <summary>
		/// Returns the specified Unicode character value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">An Unicode character to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <returns>An array of bytes with length 2 in the given byte-order.</returns>
		public static byte[] GetBytes(char value, bool bigEndian)
		{
			return IsLittleEndian^bigEndian?GetBytes(value):GetBytesSwapped(value);
		}

		/// <summary>
		/// Returns the specified 16-bit signed integer value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <returns>An array of bytes with length 2 in the given byte-order.</returns>
		public static byte[] GetBytes(short value, bool bigEndian)
		{
			return IsLittleEndian^bigEndian?GetBytes(value):GetBytesSwapped(value);
		}

		/// <summary>
		/// Returns the specified 16-bit unsigned integer value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <returns>An array of bytes with length 2 in the given byte-order.</returns>
		[CLSCompliant(false)]
		public static byte[] GetBytes(ushort value, bool bigEndian)
		{
			return IsLittleEndian^bigEndian?GetBytes(value):GetBytesSwapped(value);
		}

		/// <summary>
		/// Returns the specified 32-bit signed integer value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <returns>An array of bytes with length 4 in the given byte-order.</returns>
		public static byte[] GetBytes(int value, bool bigEndian)
		{
			return IsLittleEndian^bigEndian?GetBytes(value):GetBytesSwapped(value);
		}

		/// <summary>
		/// Returns the specified 32-bit unsigned integer value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <returns>An array of bytes with length 4 in the given byte-order.</returns>
		[CLSCompliant(false)]
		public static byte[] GetBytes(uint value, bool bigEndian)
		{
			return IsLittleEndian^bigEndian?GetBytes(value):GetBytesSwapped(value);
		}

		/// <summary>
		/// Returns the specified 64-bit signed integer value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <returns>An array of bytes with length 8 in the given byte-order.</returns>
		public static byte[] GetBytes(long value, bool bigEndian)
		{
			return IsLittleEndian^bigEndian?GetBytes(value):GetBytesSwapped(value);
		}

		/// <summary>
		/// Returns the specified 64-bit unsigned integer value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <returns>An array of bytes with length 8 in the given byte-order.</returns>
		[CLSCompliant(false)]
		public static byte[] GetBytes(ulong value, bool bigEndian)
		{
			return IsLittleEndian^bigEndian?GetBytes(value):GetBytesSwapped(value);
		}

		/// <summary>
		/// Returns the specified 128-bit unsigned integer value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <returns>An array of bytes with length 16 in the given byte-order.</returns>
		[CLSCompliant(false)]
		public static byte[] GetBytes(UInt128 value, bool bigEndian)
		{
			return IsLittleEndian^bigEndian?GetBytes(value):GetBytesSwapped(value);
		}

		/// <summary>
		/// Returns the specified single-precision floating point value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <returns>An array of bytes with length 4 in the given byte-order.</returns>
		public static byte[] GetBytes(float value, bool bigEndian)
		{
			return IsLittleEndian^bigEndian?GetBytes(value):GetBytesSwapped(value);
		}

		/// <summary>
		/// Returns the specified double-precision floating point value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <returns>An array of bytes with length 8 in the given byte-order.</returns>
		public static byte[] GetBytes(double value, bool bigEndian)
		{
			return IsLittleEndian^bigEndian?GetBytes(value):GetBytesSwapped(value);
		}
		#endregion

		#region To*
		/// <summary>
		/// Returns a bool value converted from one byte at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns><c>true</c> if the byte at <paramref name="index"/> in <paramref name="value"/> is nonzero; otherwise <c>false</c>.</returns>
		public static bool ToBoolean(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>=value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value.");

			return value[index]!=0;
		}

		/// <summary>
		/// Returns an Unicode character converted from two bytes at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A character formed by two bytes beginning at <paramref name="index"/>.</returns>
		public static unsafe char ToChar(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index+2>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value minus 1.");

			fixed(byte* ptr=&value[index]) return *(char*)ptr;
		}

		/// <summary>
		/// Returns a 16-bit signed integer converted from two bytes at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 16-bit signed integer formed by two bytes beginning at <paramref name="index"/>.</returns>
		public static unsafe short ToInt16(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index+2>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value minus 1.");

			fixed(byte* ptr=&value[index]) return *(short*)ptr;
		}

		/// <summary>
		/// Returns a 16-bit unsigned integer converted from two bytes at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 16-bit unsigned integer formed by two bytes beginning at <paramref name="index"/>.</returns>
		[CLSCompliant(false)]
		public static unsafe ushort ToUInt16(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index+2>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value minus 1.");

			fixed(byte* ptr=&value[index]) return *(ushort*)ptr;
		}

		/// <summary>
		/// Returns a 32-bit signed integer converted from four bytes at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 32-bit signed integer formed by four bytes beginning at <paramref name="index"/>.</returns>
		public static unsafe int ToInt32(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index+4>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value minus 3.");

			fixed(byte* ptr=&value[index]) return *(int*)ptr;
		}

		/// <summary>
		/// Returns a 32-bit unsigned integer converted from four bytes at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 32-bit unsigned integer formed by four bytes beginning at <paramref name="index"/>.</returns>
		[CLSCompliant(false)]
		public static unsafe uint ToUInt32(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index+4>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value minus 3.");

			fixed(byte* ptr=&value[index]) return *(uint*)ptr;
		}

		/// <summary>
		/// Returns a 64-bit signed integer converted from eight bytes at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 64-bit signed integer formed by eight bytes beginning at <paramref name="index"/>.</returns>
		public static unsafe long ToInt64(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index+8>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value minus 7.");

			fixed(byte* ptr=&value[index]) return *(long*)ptr;
		}

		/// <summary>
		/// Returns a 64-bit unsigned integer converted from eight bytes at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 64-bit unsigned integer formed by eight bytes beginning at <paramref name="index"/>.</returns>
		[CLSCompliant(false)]
		public static unsafe ulong ToUInt64(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index+8>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value minus 7.");

			fixed(byte* ptr=&value[index]) return *(ulong*)ptr;
		}

		/// <summary>
		/// Returns a 128-bit unsigned integer converted from sixteen bytes at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 128-bit unsigned integer formed by sixteen bytes beginning at <paramref name="index"/>.</returns>
		[CLSCompliant(false)]
		public static unsafe UInt128 ToUInt128(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index+16>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value minus 15.");

			fixed(byte* ptr=&value[index]) return new UInt128(*(ulong*)(ptr+8), *(ulong*)ptr);
		}

		/// <summary>
		/// Returns a single-precision floating point number converted from four bytes at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A single-precision floating point number formed by four bytes beginning at <paramref name="index"/>.</returns>
		public static unsafe float ToSingle(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index+4>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value minus 3.");

			fixed(byte* ptr=&value[index]) return *(float*)ptr;
		}

		/// <summary>
		/// Returns a double-precision floating point number converted from eight bytes at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A double-precision floating point number formed by eight bytes beginning at <paramref name="index"/>.</returns>
		public static unsafe double ToDouble(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index+8>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value minus 7.");

			fixed(byte* ptr=&value[index]) return *(double*)ptr;
		}
		#endregion

		#region To*Swapped
		/// <summary>
		/// Returns a bool value converted from one byte at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns><c>true</c> if the byte at <paramref name="index"/> in <paramref name="value"/> is nonzero; otherwise <c>false</c>.</returns>
		public static bool ToBooleanSwapped(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>=value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value.");

			return value[index]!=0;
		}

		/// <summary>
		/// Returns an Unicode character converted from two bytes at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A character formed by two bytes beginning at <paramref name="index"/> in reversed byte-order to this computer architecture.</returns>
		public static char ToCharSwapped(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index+2>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value minus 1.");

			return (char)(value[index]<<8|value[index+1]);
		}

		/// <summary>
		/// Returns a 16-bit signed integer converted from two bytes at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 16-bit signed integer formed by two bytes beginning at <paramref name="index"/> in reversed byte-order to this computer architecture.</returns>
		public static short ToInt16Swapped(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index+2>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value minus 1.");

			return (short)(value[index]<<8|value[index+1]);
		}

		/// <summary>
		/// Returns a 16-bit unsigned integer converted from two bytes at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 16-bit unsigned integer formed by two bytes beginning at <paramref name="index"/> in reversed byte-order to this computer architecture.</returns>
		[CLSCompliant(false)]
		public static ushort ToUInt16Swapped(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index+2>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value minus 1.");

			return (ushort)(value[index]<<8|value[index+1]);
		}

		/// <summary>
		/// Returns a 32-bit signed integer converted from four bytes at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 32-bit signed integer formed by four bytes beginning at <paramref name="index"/> in reversed byte-order to this computer architecture.</returns>
		public static int ToInt32Swapped(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index+4>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value minus 3.");

			return value[index]<<24|value[index+1]<<16|value[index+2]<<8|value[index+3];
		}

		/// <summary>
		/// Returns a 32-bit unsigned integer converted from four bytes at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 32-bit unsigned integer formed by four bytes beginning at <paramref name="index"/> in reversed byte-order to this computer architecture.</returns>
		[CLSCompliant(false)]
		public static uint ToUInt32Swapped(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index+4>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value minus 3.");

			return (uint)(value[index]<<24|value[index+1]<<16|value[index+2]<<8|value[index+3]);
		}

		/// <summary>
		/// Returns a 64-bit signed integer converted from eight bytes at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 64-bit signed integer formed by eight bytes beginning at <paramref name="index"/> in reversed byte-order to this computer architecture.</returns>
		public static long ToInt64Swapped(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index+8>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value minus 7.");

			uint a=(uint)(value[index]<<24|value[index+1]<<16|value[index+2]<<8|value[index+3]);
			uint b=(uint)(value[index+4]<<24|value[index+5]<<16|value[index+6]<<8|value[index+7]);
			return (long)a<<32|b;
		}

		/// <summary>
		/// Returns a 64-bit unsigned integer converted from eight bytes at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 64-bit unsigned integer formed by eight bytes beginning at <paramref name="index"/> in reversed byte-order to this computer architecture.</returns>
		[CLSCompliant(false)]
		public static ulong ToUInt64Swapped(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index+8>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value minus 7.");

			uint a=(uint)(value[index]<<24|value[index+1]<<16|value[index+2]<<8|value[index+3]);
			uint b=(uint)(value[index+4]<<24|value[index+5]<<16|value[index+6]<<8|value[index+7]);
			return (ulong)a<<32|b;
		}

		/// <summary>
		/// Returns a 128-bit unsigned integer converted from sixteen bytes at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 128-bit unsigned integer formed by sixteen bytes beginning at <paramref name="index"/> in reversed byte-order to this computer architecture.</returns>
		[CLSCompliant(false)]
		public static UInt128 ToUInt128Swapped(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index+16>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value minus 15.");

			return new UInt128(ToUInt64Swapped(value, index), ToUInt64Swapped(value, index+8));
		}

		/// <summary>
		/// Returns a single-precision floating point number converted from four bytes at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A single-precision floating point number formed by four bytes beginning at <paramref name="index"/> in reversed byte-order to this computer architecture.</returns>
		public static unsafe float ToSingleSwapped(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index+4>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value minus 3.");

			float ret;
			fixed(byte* pValue=&value[index])
			{
				int v=*(int*)pValue;
				*(int*)&ret=v<<24|(v&0xff00)<<8|(v>>8&0xff00)|(v>>24&0xff);
			}
			return ret;
		}

		/// <summary>
		/// Returns a double-precision floating point number converted from eight bytes at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A double-precision floating point number formed by eight bytes beginning at <paramref name="index"/> in reversed byte-order to this computer architecture.</returns>
		public static unsafe double ToDoubleSwapped(byte[] value, int index=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index+8>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value minus 7.");

			double ret;
			byte* pRet=(byte*)&ret;
			fixed(byte* pValue=&value[index])
			{
				pRet[0]=pValue[7];
				pRet[1]=pValue[6];
				pRet[2]=pValue[5];
				pRet[3]=pValue[4];
				pRet[4]=pValue[3];
				pRet[5]=pValue[2];
				pRet[6]=pValue[1];
				pRet[7]=pValue[0];
			}
			return ret;
		}
		#endregion

		#region To* + bool bigEndian
		/// <summary>
		/// Returns a bool value converted from one byte at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns><c>true</c> if the byte at <paramref name="index"/> in <paramref name="value"/> is nonzero; otherwise <c>false</c>.</returns>
		public static bool ToBoolean(byte[] value, bool bigEndian, int index=0)
		{
			return IsLittleEndian^bigEndian?ToBoolean(value, index):ToBooleanSwapped(value, index);
		}

		/// <summary>
		/// Returns an Unicode character converted from two bytes at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A character formed by two bytes beginning at <paramref name="index"/> in a given byte-order.</returns>
		public static char ToChar(byte[] value, bool bigEndian, int index=0)
		{
			return IsLittleEndian^bigEndian?ToChar(value, index):ToCharSwapped(value, index);
		}

		/// <summary>
		/// Returns a 16-bit signed integer converted from two bytes at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 16-bit signed integer formed by two bytes beginning at <paramref name="index"/> in a given byte-order.</returns>
		public static short ToInt16(byte[] value, bool bigEndian, int index=0)
		{
			return IsLittleEndian^bigEndian?ToInt16(value, index):ToInt16Swapped(value, index);
		}

		/// <summary>
		/// Returns a 16-bit unsigned integer converted from two bytes at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 16-bit unsigned integer formed by two bytes beginning at <paramref name="index"/> in a given byte-order.</returns>
		[CLSCompliant(false)]
		public static ushort ToUInt16(byte[] value, bool bigEndian, int index=0)
		{
			return IsLittleEndian^bigEndian?ToUInt16(value, index):ToUInt16Swapped(value, index);
		}

		/// <summary>
		/// Returns a 32-bit signed integer converted from four bytes at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 32-bit signed integer formed by four bytes beginning at <paramref name="index"/> in a given byte-order.</returns>
		public static int ToInt32(byte[] value, bool bigEndian, int index=0)
		{
			return IsLittleEndian^bigEndian?ToInt32(value, index):ToInt32Swapped(value, index);
		}

		/// <summary>
		/// Returns a 32-bit unsigned integer converted from four bytes at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 32-bit unsigned integer formed by four bytes beginning at <paramref name="index"/> in a given byte-order.</returns>
		[CLSCompliant(false)]
		public static uint ToUInt32(byte[] value, bool bigEndian, int index=0)
		{
			return IsLittleEndian^bigEndian?ToUInt32(value, index):ToUInt32Swapped(value, index);
		}

		/// <summary>
		/// Returns a 64-bit signed integer converted from eight bytes at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 64-bit signed integer formed by eight bytes beginning at <paramref name="index"/> in a given byte-order.</returns>
		public static long ToInt64(byte[] value, bool bigEndian, int index=0)
		{
			return IsLittleEndian^bigEndian?ToInt64(value, index):ToInt64Swapped(value, index);
		}

		/// <summary>
		/// Returns a 64-bit unsigned integer converted from eight bytes at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 64-bit unsigned integer formed by eight bytes beginning at <paramref name="index"/> in a given byte-order.</returns>
		[CLSCompliant(false)]
		public static ulong ToUInt64(byte[] value, bool bigEndian, int index=0)
		{
			return IsLittleEndian^bigEndian?ToUInt64(value, index):ToUInt64Swapped(value, index);
		}

		/// <summary>
		/// Returns a 128-bit unsigned integer converted from sixteen bytes at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A 128-bit unsigned integer formed by sixteen bytes beginning at <paramref name="index"/> in a given byte-order.</returns>
		[CLSCompliant(false)]
		public static UInt128 ToUInt128(byte[] value, bool bigEndian, int index=0)
		{
			return IsLittleEndian^bigEndian?ToUInt128(value, index):ToUInt128Swapped(value, index);
		}

		/// <summary>
		/// Returns a single-precision floating point number converted from four bytes at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A single-precision floating point number formed by four bytes beginning at <paramref name="index"/> in a given byte-order.</returns>
		public static float ToSingle(byte[] value, bool bigEndian, int index=0)
		{
			return IsLittleEndian^bigEndian?ToSingle(value, index):ToSingleSwapped(value, index);
		}

		/// <summary>
		/// Returns a double-precision floating point number converted from eight bytes at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A double-precision floating point number formed by eight bytes beginning at <paramref name="index"/> in a given byte-order.</returns>
		public static double ToDouble(byte[] value, bool bigEndian, int index=0)
		{
			return IsLittleEndian^bigEndian?ToDouble(value, index):ToDoubleSwapped(value, index);
		}
		#endregion

		#region GetBytes[]
		/// <summary>
		/// Returns the specified bool value as an array of bytes.
		/// </summary>
		/// <param name="value">A bool to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes.</returns>
		public static byte[] GetBytes(bool[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count];
			for(int i=0; i<count; i++) ret[i]=value[i]?(byte)1:(byte)0;
			return ret;
		}

		/// <summary>
		/// Returns the specified Unicode character value as an array of bytes.
		/// </summary>
		/// <param name="value">An Unicode character to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 2.</returns>
		public static unsafe byte[] GetBytes(char[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count*2];
			fixed(byte* pRet=ret)
			{
				byte* iRet=pRet;
				char buffer;
				byte* ptr=(byte*)&buffer;
				for(int i=0; i<count; i++)
				{
					buffer=value[index+i];
					*(iRet++)=ptr[0];
					*(iRet++)=ptr[1];
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns the specified 16-bit signed integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 2.</returns>
		public static unsafe byte[] GetBytes(short[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count*2];
			fixed(byte* pRet=ret)
			{
				byte* iRet=pRet;
				short buffer;
				byte* ptr=(byte*)&buffer;
				for(int i=0; i<count; i++)
				{
					buffer=value[index+i];
					*(iRet++)=ptr[0];
					*(iRet++)=ptr[1];
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns the specified 16-bit unsigned integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 2.</returns>
		[CLSCompliant(false)]
		public static unsafe byte[] GetBytes(ushort[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count*2];
			fixed(byte* pRet=ret)
			{
				byte* iRet=pRet;
				ushort buffer;
				byte* ptr=(byte*)&buffer;
				for(int i=0; i<count; i++)
				{
					buffer=value[index+i];
					*(iRet++)=ptr[0];
					*(iRet++)=ptr[1];
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns the specified 32-bit signed integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 4.</returns>
		public static unsafe byte[] GetBytes(int[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count*4];
			fixed(byte* pRet=ret)
			{
				byte* iRet=pRet;
				int buffer;
				byte* ptr=(byte*)&buffer;
				for(int i=0; i<count; i++)
				{
					buffer=value[index+i];
					*(iRet++)=ptr[0];
					*(iRet++)=ptr[1];
					*(iRet++)=ptr[2];
					*(iRet++)=ptr[3];
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns the specified 32-bit unsigned integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 4.</returns>
		[CLSCompliant(false)]
		public static unsafe byte[] GetBytes(uint[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count*4];
			fixed(byte* pRet=ret)
			{
				byte* iRet=pRet;
				uint buffer;
				byte* ptr=(byte*)&buffer;
				for(int i=0; i<count; i++)
				{
					buffer=value[index+i];
					*(iRet++)=ptr[0];
					*(iRet++)=ptr[1];
					*(iRet++)=ptr[2];
					*(iRet++)=ptr[3];
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns the specified 64-bit signed integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 8.</returns>
		public static unsafe byte[] GetBytes(long[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count*8];
			fixed(byte* pRet=ret)
			{
				byte* iRet=pRet;
				long buffer;
				byte* ptr=(byte*)&buffer;
				for(int i=0; i<count; i++)
				{
					buffer=value[index+i];
					*(iRet++)=ptr[0];
					*(iRet++)=ptr[1];
					*(iRet++)=ptr[2];
					*(iRet++)=ptr[3];
					*(iRet++)=ptr[4];
					*(iRet++)=ptr[5];
					*(iRet++)=ptr[6];
					*(iRet++)=ptr[7];
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns the specified 64-bit unsigned integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 8.</returns>
		[CLSCompliant(false)]
		public static unsafe byte[] GetBytes(ulong[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count*8];
			fixed(byte* pRet=ret)
			{
				byte* iRet=pRet;
				ulong buffer;
				byte* ptr=(byte*)&buffer;
				for(int i=0; i<count; i++)
				{
					buffer=value[index+i];
					*(iRet++)=ptr[0];
					*(iRet++)=ptr[1];
					*(iRet++)=ptr[2];
					*(iRet++)=ptr[3];
					*(iRet++)=ptr[4];
					*(iRet++)=ptr[5];
					*(iRet++)=ptr[6];
					*(iRet++)=ptr[7];
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns the specified 128-bit unsigned integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 16.</returns>
		[CLSCompliant(false)]
		public static unsafe byte[] GetBytes(UInt128[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count*16];
			fixed(byte* pRet=ret)
			{
				byte* iRet=pRet;
				ulong buffer;
				byte* ptr=(byte*)&buffer;
				for(int i=0; i<count; i++)
				{
					buffer=value[index+i].Low;
					*(iRet++)=ptr[0];
					*(iRet++)=ptr[1];
					*(iRet++)=ptr[2];
					*(iRet++)=ptr[3];
					*(iRet++)=ptr[4];
					*(iRet++)=ptr[5];
					*(iRet++)=ptr[6];
					*(iRet++)=ptr[7];

					buffer=value[index+i].High;
					*(iRet++)=ptr[0];
					*(iRet++)=ptr[1];
					*(iRet++)=ptr[2];
					*(iRet++)=ptr[3];
					*(iRet++)=ptr[4];
					*(iRet++)=ptr[5];
					*(iRet++)=ptr[6];
					*(iRet++)=ptr[7];
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns the specified single-precision floating point value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 4.</returns>
		public static unsafe byte[] GetBytes(float[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count*4];
			fixed(byte* pRet=ret)
			{
				byte* iRet=pRet;
				float buffer;
				byte* ptr=(byte*)&buffer;
				for(int i=0; i<count; i++)
				{
					buffer=value[index+i];
					*(iRet++)=ptr[0];
					*(iRet++)=ptr[1];
					*(iRet++)=ptr[2];
					*(iRet++)=ptr[3];
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns the specified double-precision floating point value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 8.</returns>
		public static unsafe byte[] GetBytes(double[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count*8];
			fixed(byte* pRet=ret)
			{
				byte* iRet=pRet;
				double buffer;
				byte* ptr=(byte*)&buffer;
				for(int i=0; i<count; i++)
				{
					buffer=value[index+i];
					*(iRet++)=ptr[0];
					*(iRet++)=ptr[1];
					*(iRet++)=ptr[2];
					*(iRet++)=ptr[3];
					*(iRet++)=ptr[4];
					*(iRet++)=ptr[5];
					*(iRet++)=ptr[6];
					*(iRet++)=ptr[7];
				}
			}
			return ret;
		}
		#endregion

		#region GetBytesSwapped[]
		/// <summary>
		/// Returns the specified bool value as an array of bytes in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">A bool to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes in reversed byte-order to this computer architecture.</returns>
		public static byte[] GetBytesSwapped(bool[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count];
			for(int i=0; i<count; i++) ret[i]=value[i]?(byte)1:(byte)0;
			return ret;
		}

		/// <summary>
		/// Returns the specified Unicode character value as an array of bytes in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An Unicode character to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 2 in reversed byte-order to this computer architecture.</returns>
		public static unsafe byte[] GetBytesSwapped(char[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count*2];
			fixed(byte* pRet=ret)
			{
				byte* iRet=pRet;
				char buffer;
				byte* ptr=(byte*)&buffer;
				for(int i=0; i<count; i++)
				{
					buffer=value[index+i];
					*(iRet++)=ptr[1];
					*(iRet++)=ptr[0];
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns the specified 16-bit signed integer value as an array of bytes in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 2 in reversed byte-order to this computer architecture.</returns>
		public static unsafe byte[] GetBytesSwapped(short[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count*2];
			fixed(byte* pRet=ret)
			{
				byte* iRet=pRet;
				short buffer;
				byte* ptr=(byte*)&buffer;
				for(int i=0; i<count; i++)
				{
					buffer=value[index+i];
					*(iRet++)=ptr[1];
					*(iRet++)=ptr[0];
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns the specified 16-bit unsigned integer value as an array of bytes in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 2 in reversed byte-order to this computer architecture.</returns>
		[CLSCompliant(false)]
		public static unsafe byte[] GetBytesSwapped(ushort[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count*2];
			fixed(byte* pRet=ret)
			{
				byte* iRet=pRet;
				ushort buffer;
				byte* ptr=(byte*)&buffer;
				for(int i=0; i<count; i++)
				{
					buffer=value[index+i];
					*(iRet++)=ptr[1];
					*(iRet++)=ptr[0];
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns the specified 32-bit signed integer value as an array of bytes in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 4 in reversed byte-order to this computer architecture.</returns>
		public static unsafe byte[] GetBytesSwapped(int[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count*4];
			fixed(byte* pRet=ret)
			{
				byte* iRet=pRet;
				int buffer;
				byte* ptr=(byte*)&buffer;
				for(int i=0; i<count; i++)
				{
					buffer=value[index+i];
					*(iRet++)=ptr[3];
					*(iRet++)=ptr[2];
					*(iRet++)=ptr[1];
					*(iRet++)=ptr[0];
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns the specified 32-bit unsigned integer value as an array of bytes in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 4 in reversed byte-order to this computer architecture.</returns>
		[CLSCompliant(false)]
		public static unsafe byte[] GetBytesSwapped(uint[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count*4];
			fixed(byte* pRet=ret)
			{
				byte* iRet=pRet;
				uint buffer;
				byte* ptr=(byte*)&buffer;
				for(int i=0; i<count; i++)
				{
					buffer=value[index+i];
					*(iRet++)=ptr[3];
					*(iRet++)=ptr[2];
					*(iRet++)=ptr[1];
					*(iRet++)=ptr[0];
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns the specified 64-bit signed integer value as an array of bytes in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 8 in reversed byte-order to this computer architecture.</returns>
		public static unsafe byte[] GetBytesSwapped(long[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count*8];
			fixed(byte* pRet=ret)
			{
				byte* iRet=pRet;
				long buffer;
				byte* ptr=(byte*)&buffer;
				for(int i=0; i<count; i++)
				{
					buffer=value[index+i];
					*(iRet++)=ptr[7];
					*(iRet++)=ptr[6];
					*(iRet++)=ptr[5];
					*(iRet++)=ptr[4];
					*(iRet++)=ptr[3];
					*(iRet++)=ptr[2];
					*(iRet++)=ptr[1];
					*(iRet++)=ptr[0];
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns the specified 64-bit unsigned integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 8 in reversed byte-order to this computer architecture.</returns>
		[CLSCompliant(false)]
		public static unsafe byte[] GetBytesSwapped(ulong[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count*8];
			fixed(byte* pRet=ret)
			{
				byte* iRet=pRet;
				ulong buffer;
				byte* ptr=(byte*)&buffer;
				for(int i=0; i<count; i++)
				{
					buffer=value[index+i];
					*(iRet++)=ptr[7];
					*(iRet++)=ptr[6];
					*(iRet++)=ptr[5];
					*(iRet++)=ptr[4];
					*(iRet++)=ptr[3];
					*(iRet++)=ptr[2];
					*(iRet++)=ptr[1];
					*(iRet++)=ptr[0];
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns the specified 128-bit unsigned integer value as an array of bytes.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 16 in reversed byte-order to this computer architecture.</returns>
		[CLSCompliant(false)]
		public static unsafe byte[] GetBytesSwapped(UInt128[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count*16];
			fixed(byte* pRet=ret)
			{
				byte* iRet=pRet;
				ulong buffer;
				byte* ptr=(byte*)&buffer;
				for(int i=0; i<count; i++)
				{
					buffer=value[index+i].High;
					*(iRet++)=ptr[7];
					*(iRet++)=ptr[6];
					*(iRet++)=ptr[5];
					*(iRet++)=ptr[4];
					*(iRet++)=ptr[3];
					*(iRet++)=ptr[2];
					*(iRet++)=ptr[1];
					*(iRet++)=ptr[0];

					buffer=value[index+i].Low;
					*(iRet++)=ptr[7];
					*(iRet++)=ptr[6];
					*(iRet++)=ptr[5];
					*(iRet++)=ptr[4];
					*(iRet++)=ptr[3];
					*(iRet++)=ptr[2];
					*(iRet++)=ptr[1];
					*(iRet++)=ptr[0];
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns the specified single-precision floating point value as an array of bytes in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 4 in reversed byte-order to this computer architecture.</returns>
		public static unsafe byte[] GetBytesSwapped(float[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count*4];
			fixed(byte* pRet=ret)
			{
				byte* iRet=pRet;
				float buffer;
				byte* ptr=(byte*)&buffer;
				for(int i=0; i<count; i++)
				{
					buffer=value[index+i];
					*(iRet++)=ptr[3];
					*(iRet++)=ptr[2];
					*(iRet++)=ptr[1];
					*(iRet++)=ptr[0];
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns the specified double-precision floating point value as an array of bytes in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 8 in reversed byte-order to this computer architecture.</returns>
		public static unsafe byte[] GetBytesSwapped(double[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of value minus the index argument.");

			if(count==0) count=value.Length-index;

			byte[] ret=new byte[count*8];
			fixed(byte* pRet=ret)
			{
				byte* iRet=pRet;
				double buffer;
				byte* ptr=(byte*)&buffer;
				for(int i=0; i<count; i++)
				{
					buffer=value[index+i];
					*(iRet++)=ptr[7];
					*(iRet++)=ptr[6];
					*(iRet++)=ptr[5];
					*(iRet++)=ptr[4];
					*(iRet++)=ptr[3];
					*(iRet++)=ptr[2];
					*(iRet++)=ptr[1];
					*(iRet++)=ptr[0];
				}
			}
			return ret;
		}
		#endregion

		#region GetBytes[] + bool bigEndian
		/// <summary>
		/// Returns the specified bool value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">A bool to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes in the given byte-order.</returns>
		public static byte[] GetBytes(bool[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?GetBytes(value, index, count):GetBytesSwapped(value, index, count);
		}

		/// <summary>
		/// Returns the specified Unicode character value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">An Unicode character to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 2 in the given byte-order.</returns>
		public static byte[] GetBytes(char[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?GetBytes(value, index, count):GetBytesSwapped(value, index, count);
		}

		/// <summary>
		/// Returns the specified 16-bit signed integer value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 2 in the given byte-order.</returns>
		public static byte[] GetBytes(short[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?GetBytes(value, index, count):GetBytesSwapped(value, index, count);
		}

		/// <summary>
		/// Returns the specified 16-bit unsigned integer value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 2 in the given byte-order.</returns>
		[CLSCompliant(false)]
		public static byte[] GetBytes(ushort[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?GetBytes(value, index, count):GetBytesSwapped(value, index, count);
		}

		/// <summary>
		/// Returns the specified 32-bit signed integer value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 4 in the given byte-order.</returns>
		public static byte[] GetBytes(int[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?GetBytes(value, index, count):GetBytesSwapped(value, index, count);
		}

		/// <summary>
		/// Returns the specified 32-bit unsigned integer value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 4 in the given byte-order.</returns>
		[CLSCompliant(false)]
		public static byte[] GetBytes(uint[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?GetBytes(value, index, count):GetBytesSwapped(value, index, count);
		}

		/// <summary>
		/// Returns the specified 64-bit signed integer value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 8 in the given byte-order.</returns>
		public static byte[] GetBytes(long[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?GetBytes(value, index, count):GetBytesSwapped(value, index, count);
		}

		/// <summary>
		/// Returns the specified 64-bit unsigned integer value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 8 in the given byte-order.</returns>
		[CLSCompliant(false)]
		public static byte[] GetBytes(ulong[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?GetBytes(value, index, count):GetBytesSwapped(value, index, count);
		}

		/// <summary>
		/// Returns the specified 128-bit unsigned integer value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 16 in the given byte-order.</returns>
		[CLSCompliant(false)]
		public static byte[] GetBytes(UInt128[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?GetBytes(value, index, count):GetBytesSwapped(value, index, count);
		}

		/// <summary>
		/// Returns the specified single-precision floating point value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 4 in the given byte-order.</returns>
		public static byte[] GetBytes(float[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?GetBytes(value, index, count):GetBytesSwapped(value, index, count);
		}

		/// <summary>
		/// Returns the specified double-precision floating point value as an array of bytes in a given byte-order.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <param name="bigEndian">Specifies the byte-order the resulting array will be in. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>An array of bytes with length multiple of 8 in the given byte-order.</returns>
		public static byte[] GetBytes(double[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?GetBytes(value, index, count):GetBytesSwapped(value, index, count);
		}
		#endregion

		#region To*Array
		/// <summary>
		/// Returns an array of bool values, each converted from the bytes starting at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of bool values.</returns>
		public static bool[] ToBooleanArray(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the index argument.");

			if(count==0) count=value.Length-index;

			bool[] ret=new bool[count];
			for(int i=0; i<count; i++) ret[i]=value[index+i]!=0;
			return ret;
		}

		/// <summary>
		/// Returns an array of Unicode characters, each converted from two bytes starting at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of Unicode characters.</returns>
		public static unsafe char[] ToCharArray(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count*2>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the offset argument divided by 2.");

			if(count==0) count=(value.Length-index)/2;

			char[] ret=new char[count];
			fixed(byte* ptr=&value[index])
				for(int i=0; i<count; i++) ret[i]=*((char*)ptr+i);
			return ret;
		}

		/// <summary>
		/// Returns an array of 16-bit signed integers, each converted from two bytes starting at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 16-bit signed integers.</returns>
		public static unsafe short[] ToInt16Array(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count*2>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the offset argument divided by 2.");

			if(count==0) count=(value.Length-index)/2;

			short[] ret=new short[count];
			fixed(byte* ptr=&value[index])
				for(int i=0; i<count; i++) ret[i]=*((short*)ptr+i);
			return ret;
		}

		/// <summary>
		/// Returns an array of 16-bit unsigned integers, each converted from two bytes starting at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 16-bit unsigned integers.</returns>
		[CLSCompliant(false)]
		public static unsafe ushort[] ToUInt16Array(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count*2>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the offset argument divided by 2.");

			if(count==0) count=(value.Length-index)/2;

			ushort[] ret=new ushort[count];
			fixed(byte* ptr=&value[index])
				for(int i=0; i<count; i++) ret[i]=*((ushort*)ptr+i);
			return ret;
		}

		/// <summary>
		/// Returns an array of 32-bit signed integers, each converted from four bytes starting at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 32-bit signed integers.</returns>
		public static unsafe int[] ToInt32Array(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count*4>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the offset argument divided by 4.");

			if(count==0) count=(value.Length-index)/4;

			int[] ret=new int[count];
			fixed(byte* ptr=&value[index])
				for(int i=0; i<count; i++) ret[i]=*((int*)ptr+i);
			return ret;
		}

		/// <summary>
		/// Returns an array of 32-bit unsigned integers, each converted from four bytes starting at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 32-bit unsigned integers.</returns>
		[CLSCompliant(false)]
		public static unsafe uint[] ToUInt32Array(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count*4>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the offset argument divided by 4.");

			if(count==0) count=(value.Length-index)/4;

			uint[] ret=new uint[count];
			fixed(byte* ptr=&value[index])
				for(int i=0; i<count; i++) ret[i]=*((uint*)ptr+i);
			return ret;
		}

		/// <summary>
		/// Returns an array of 64-bit signed integers, each converted from eight bytes starting at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 64-bit signed integers.</returns>
		public static unsafe long[] ToInt64Array(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count*8>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the offset argument divided by 8.");

			if(count==0) count=(value.Length-index)/8;

			long[] ret=new long[count];
			fixed(byte* ptr=&value[index])
				for(int i=0; i<count; i++) ret[i]=*((long*)ptr+i);
			return ret;
		}

		/// <summary>
		/// Returns an array of 64-bit unsigned integers, each converted from eight bytes starting at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 64-bit unsigned integers.</returns>
		[CLSCompliant(false)]
		public static unsafe ulong[] ToUInt64Array(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count*8>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the offset argument divided by 8.");

			if(count==0) count=(value.Length-index)/8;

			ulong[] ret=new ulong[count];
			fixed(byte* ptr=&value[index])
				for(int i=0; i<count; i++) ret[i]=*((ulong*)ptr+i);
			return ret;
		}

		/// <summary>
		/// Returns an array of 128-bit unsigned integers, each converted from sixteen bytes starting at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 128-bit unsigned integers.</returns>
		[CLSCompliant(false)]
		public static unsafe UInt128[] ToUInt128Array(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count*16>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the offset argument divided by 16.");

			if(count==0) count=(value.Length-index)/16;

			UInt128[] ret=new UInt128[count];
			fixed(byte* ptr=&value[index])
				for(int i=0; i<count; i++) ret[i]=new UInt128(*((ulong*)ptr+2*i+1), *((ulong*)ptr+2*i));
			return ret;
		}

		/// <summary>
		/// Returns an array of single-precision floating point numbers, each converted from four bytes starting at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of single-precision floating point numbers.</returns>
		public static unsafe float[] ToSingleArray(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count*4>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the offset argument divided by 4.");

			if(count==0) count=(value.Length-index)/4;

			float[] ret=new float[count];
			fixed(byte* ptr=&value[index])
				for(int i=0; i<count; i++) ret[i]=*((float*)ptr+i);
			return ret;
		}

		/// <summary>
		/// Returns an array of double-precision floating point numbers, each converted from eight bytes starting at a specified position in a byte array.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of double-precision floating point numbers.</returns>
		public static unsafe double[] ToDoubleArray(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count*8>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the offset argument divided by 8.");

			if(count==0) count=(value.Length-index)/8;

			double[] ret=new double[count];
			fixed(byte* ptr=&value[index])
				for(int i=0; i<count; i++) ret[i]=*((double*)ptr+i);
			return ret;
		}
		#endregion

		#region To*ArraySwapped
		/// <summary>
		/// Returns an array of bool values, each converted from the bytes starting at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of bool values.</returns>
		public static bool[] ToBooleanArraySwapped(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the index argument.");

			if(count==0) count=value.Length-index;

			bool[] ret=new bool[count];
			for(int i=0; i<count; i++) ret[i]=value[index+i]!=0;
			return ret;
		}

		/// <summary>
		/// Returns an array of Unicode characters, each converted from two bytes starting at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of Unicode characters.</returns>
		public static char[] ToCharArraySwapped(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count*2>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the offset argument divided by 2.");

			if(count==0) count=(value.Length-index)/2;

			char[] ret=new char[count];
			for(int i=0; i<count; i++, index+=2) ret[i]=(char)(value[index]<<8|value[index+1]);
			return ret;
		}

		/// <summary>
		/// Returns an array of 16-bit signed integers, each converted from two bytes starting at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 16-bit signed integers.</returns>
		public static short[] ToInt16ArraySwapped(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count*2>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the offset argument divided by 2.");

			if(count==0) count=(value.Length-index)/2;

			short[] ret=new short[count];
			for(int i=0; i<count; i++, index+=2) ret[i]=(short)(value[index]<<8|value[index+1]);
			return ret;
		}

		/// <summary>
		/// Returns an array of 16-bit unsigned integers, each converted from two bytes starting at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 16-bit unsigned integers.</returns>
		[CLSCompliant(false)]
		public static ushort[] ToUInt16ArraySwapped(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count*2>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the offset argument divided by 2.");

			if(count==0) count=(value.Length-index)/2;

			ushort[] ret=new ushort[count];
			for(int i=0; i<count; i++, index+=2) ret[i]=(ushort)(value[index]<<8|value[index+1]);
			return ret;
		}

		/// <summary>
		/// Returns an array of 32-bit signed integers, each converted from four bytes starting at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 32-bit signed integers.</returns>
		public static int[] ToInt32ArraySwapped(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count*4>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the offset argument divided by 4.");

			if(count==0) count=(value.Length-index)/4;

			int[] ret=new int[count];
			for(int i=0; i<count; i++, index+=4) ret[i]=value[index]<<24|value[index+1]<<16|value[index+2]<<8|value[index+3];
			return ret;
		}

		/// <summary>
		/// Returns an array of 32-bit unsigned integers, each converted from four bytes starting at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 32-bit unsigned integers.</returns>
		[CLSCompliant(false)]
		public static uint[] ToUInt32ArraySwapped(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count*4>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the offset argument divided by 4.");

			if(count==0) count=(value.Length-index)/4;

			uint[] ret=new uint[count];
			for(int i=0; i<count; i++, index+=4) ret[i]=(uint)(value[index]<<24|value[index+1]<<16|value[index+2]<<8|value[index+3]);
			return ret;
		}

		/// <summary>
		/// Returns an array of 64-bit signed integers, each converted from eight bytes starting at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 64-bit signed integers.</returns>
		public static long[] ToInt64ArraySwapped(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count*8>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the offset argument divided by 8.");

			if(count==0) count=(value.Length-index)/8;

			long[] ret=new long[count];
			for(int i=0; i<count; i++, index+=8)
			{
				uint a=(uint)(value[index]<<24|value[index+1]<<16|value[index+2]<<8|value[index+3]);
				uint b=(uint)(value[index+4]<<24|value[index+5]<<16|value[index+6]<<8|value[index+7]);
				ret[i]=(long)a<<32|b;
			}
			return ret;
		}

		/// <summary>
		/// Returns an array of 64-bit unsigned integers, each converted from eight bytes starting at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 64-bit unsigned integers.</returns>
		[CLSCompliant(false)]
		public static ulong[] ToUInt64ArraySwapped(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count*8>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the offset argument divided by 8.");

			if(count==0) count=(value.Length-index)/8;

			ulong[] ret=new ulong[count];
			for(int i=0; i<count; i++, index+=8)
			{
				uint a=(uint)(value[index]<<24|value[index+1]<<16|value[index+2]<<8|value[index+3]);
				uint b=(uint)(value[index+4]<<24|value[index+5]<<16|value[index+6]<<8|value[index+7]);
				ret[i]=(ulong)a<<32|b;
			}
			return ret;
		}

		/// <summary>
		/// Returns an array of 128-bit unsigned integers, each converted from sixteen bytes starting at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 128-bit unsigned integers.</returns>
		[CLSCompliant(false)]
		public static UInt128[] ToUInt128ArraySwapped(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count*16>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the offset argument divided by 16.");

			if(count==0) count=(value.Length-index)/16;

			UInt128[] ret=new UInt128[count];
			for(int i=0; i<count; i++, index+=16)
				ret[i]=new UInt128(ToUInt64Swapped(value, index), ToUInt64Swapped(value, index+8));

			return ret;
		}

		/// <summary>
		/// Returns an array of single-precision floating point numbers, each converted from four bytes starting at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of single-precision floating point numbers.</returns>
		public static unsafe float[] ToSingleArraySwapped(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count*4>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the offset argument divided by 4.");

			if(count==0) count=(value.Length-index)/4;

			float[] ret=new float[count];
			fixed(float* pRet=ret)
			{
				int* iRet=(int*)pRet;
				for(int i=0; i<count; i++, index+=8)
				{
					fixed(byte* pValue=&value[index])
					{
						int v=*(int*)pValue;
						*(iRet++)=v<<24|(v&0xff00)<<8|(v>>8&0xff00)|(v>>24&0xff);
					}
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns an array of double-precision floating point numbers, each converted from eight bytes starting at a specified position in a byte array in reversed byte-order to this computer architecture.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of double-precision floating point numbers.</returns>
		public static unsafe double[] ToDoubleArraySwapped(byte[] value, int index=0, int count=0)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||index>value.Length)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than or equal to the length of value.");

			if(count<0) throw new ArgumentOutOfRangeException("count", "Must be non-negative.");
			if(index+count*8>value.Length) throw new ArgumentOutOfRangeException("count", "Must be less than or equal to the length of the byte array minus the offset argument divided by 8.");

			if(count==0) count=(value.Length-index)/8;

			double[] ret=new double[count];
			fixed(double* pRet=ret)
			{
				byte* bRet=(byte*)pRet;
				for(int i=0; i<count; i++, index+=8, bRet+=8)
				{
					fixed(byte* pValue=&value[index])
					{
						bRet[0]=pValue[7];
						bRet[1]=pValue[6];
						bRet[2]=pValue[5];
						bRet[3]=pValue[4];
						bRet[4]=pValue[3];
						bRet[5]=pValue[2];
						bRet[6]=pValue[1];
						bRet[7]=pValue[0];
					}
				}
			}
			return ret;
		}
		#endregion

		#region To*Array + bool bigEndian
		/// <summary>
		/// Returns an array of bool values, each converted from the bytes starting at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of bool values.</returns>
		public static bool[] ToBooleanArray(byte[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?ToBooleanArray(value, index, count):ToBooleanArraySwapped(value, index, count);
		}

		/// <summary>
		/// Returns an array of Unicode characters, each converted from two bytes starting at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of Unicode characters.</returns>
		public static char[] ToCharArray(byte[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?ToCharArray(value, index, count):ToCharArraySwapped(value, index, count);
		}

		/// <summary>
		/// Returns an array of 16-bit signed integers, each converted from two bytes starting at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 16-bit signed integers.</returns>
		public static short[] ToInt16Array(byte[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?ToInt16Array(value, index, count):ToInt16ArraySwapped(value, index, count);
		}

		/// <summary>
		/// Returns an array of 16-bit unsigned integers, each converted from two bytes starting at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 16-bit unsigned integers.</returns>
		[CLSCompliant(false)]
		public static ushort[] ToUInt16Array(byte[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?ToUInt16Array(value, index, count):ToUInt16ArraySwapped(value, index, count);
		}

		/// <summary>
		/// Returns an array of 32-bit signed integers, each converted from four bytes starting at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 32-bit signed integers.</returns>
		public static int[] ToInt32Array(byte[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?ToInt32Array(value, index, count):ToInt32ArraySwapped(value, index, count);
		}

		/// <summary>
		/// Returns an array of 32-bit unsigned integers, each converted from four bytes starting at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 32-bit unsigned integers.</returns>
		[CLSCompliant(false)]
		public static uint[] ToUInt32Array(byte[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?ToUInt32Array(value, index, count):ToUInt32ArraySwapped(value, index, count);
		}

		/// <summary>
		/// Returns an array of 64-bit signed integers, each converted from eight bytes starting at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 64-bit signed integers.</returns>
		public static long[] ToInt64Array(byte[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?ToInt64Array(value, index, count):ToInt64ArraySwapped(value, index, count);
		}

		/// <summary>
		/// Returns an array of 64-bit unsigned integers, each converted from eight bytes starting at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 64-bit unsigned integers.</returns>
		[CLSCompliant(false)]
		public static ulong[] ToUInt64Array(byte[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?ToUInt64Array(value, index, count):ToUInt64ArraySwapped(value, index, count);
		}

		/// <summary>
		/// Returns an array of 128-bit unsigned integers, each converted from sixteen bytes starting at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of 128-bit unsigned integers.</returns>
		[CLSCompliant(false)]
		public static UInt128[] ToUInt128Array(byte[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?ToUInt128Array(value, index, count):ToUInt128ArraySwapped(value, index, count);
		}

		/// <summary>
		/// Returns an array of single-precision floating point numbers, each converted from four bytes starting at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of single-precision floating point numbers.</returns>
		public static float[] ToSingleArray(byte[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?ToSingleArray(value, index, count):ToSingleArraySwapped(value, index, count);
		}

		/// <summary>
		/// Returns an array of double-precision floating point numbers, each converted from eight bytes starting at a specified position in a byte array in a given byte-order.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="bigEndian">Specifies the byte-order the in the array. <c>true</c> for big-endian, <c>false</c> for little-endian.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="count">Number of elements to convert.</param>
		/// <returns>The array of double-precision floating point numbers.</returns>
		public static double[] ToDoubleArray(byte[] value, bool bigEndian, int index=0, int count=0)
		{
			return IsLittleEndian^bigEndian?ToDoubleArray(value, index, count):ToDoubleArraySwapped(value, index, count);
		}
		#endregion

		#region ToString
		/// <summary>
		/// Converts the numeric value of each element of a specified array of bytes to its equivalent hexadecimal string representation.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <returns>A string of hexadecimal pairs separated by hyphens, where each pair represents the corresponding element in <paramref name="value"/>.</returns>
		public static string ToString(byte[] value)
		{
			if(value==null) throw new ArgumentNullException("value");
			return ToString(value, 0, value.Length);
		}

		/// <summary>
		/// Converts the numeric value of each element of a specified array of bytes to its equivalent hexadecimal string representation.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <returns>A string of hexadecimal pairs separated by hyphens, where each pair represents the corresponding element in <paramref name="value"/>.</returns>
		public static string ToString(byte[] value, int index)
		{
			if(value==null) throw new ArgumentNullException("value");
			return ToString(value, index, value.Length-index);
		}

		/// <summary>
		/// Converts the numeric value of each element of a specified array of bytes to its equivalent hexadecimal string representation.
		/// </summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="index">The starting position within <paramref name="value"/>.</param>
		/// <param name="length">The number of array elements in <paramref name="value"/> to convert.</param>
		/// <returns>A string of hexadecimal pairs separated by hyphens, where each pair represents the corresponding element in <paramref name="value"/>.</returns>
		public static string ToString(byte[] value, int index, int length)
		{
			if(value==null) throw new ArgumentNullException("value");

			if(index<0||(index>=value.Length&&index>0)) // ignore special case (value.Length==index==length==0)
				throw new ArgumentOutOfRangeException("index", "Must be non-negative and less than the length of value.");

			if(length<0||index>value.Length-length||length>715827882)
				throw new ArgumentOutOfRangeException("length", "Must be non-negative, less than or equal to the length of value minus index and less than 715827883.");

			if(length==0/*&&index==0&&value.Length==0*/) return string.Empty;

			char[] ret=new char[length*3-1];
			int pos=0;

			byte val=value[index];
			int h=val/16;
			int l=val%16;
			ret[pos++]=h<10?(char)('0'+h):(char)('A'+h-10);
			ret[pos++]=l<10?(char)('0'+l):(char)('A'+l-10);

			int end=index+length;
			for(int i=index+1; i<end; i++)
			{
				ret[pos++]='-';
				val=value[i];
				h=val/16;
				l=val%16;
				ret[pos++]=h<10?(char)('0'+h):(char)('A'+h-10);
				ret[pos++]=l<10?(char)('0'+l):(char)('A'+l-10);
			}

			return new string(ret, 0, ret.Length);
		}
		#endregion

		#region double <=> long
		/// <summary>
		/// Converts the specified double-precision floating point number to a 64-bit signed integer.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>A 64-bit signed integer whose value is equivalent to <paramref name="value"/>.</returns>
		public unsafe static long DoubleToInt64Bits(double value)
		{
			return *(long*)&value;
		}

		/// <summary>
		/// Converts the specified 64-bit signed integer to a double-precision floating point number.
		/// </summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>A double-precision floating point number whose value is equivalent to <paramref name="value"/>.</returns>
		public unsafe static double Int64BitsToDouble(long value)
		{
			return *(double*)&value;
		}
		#endregion
	}
}
