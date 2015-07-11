using System;

namespace Free.Core
{
	/// <summary>
	/// Class providing useful bit operation methods.
	/// </summary>
	/// <threadsafety static="true" instance="true"/>
	public static class Bits
	{
		#region CeilingPowerOf2
		/// <summary>
		/// Returns the ceiling power of 2 for a given value.
		/// </summary>
		/// <param name="value">The value for which to find the ceiling power of 2.</param>
		/// <returns>The ceiling power of 2 for the <paramref name="value"/>.</returns>
		[CLSCompliant(false)]
		public static byte CeilingPowerOf2(byte value)
		{
			byte ret=(byte)(value-1);
			ret|=(byte)(ret>>1);
			ret|=(byte)(ret>>2);
			ret|=(byte)(ret>>4);
			return (byte)(ret+1);
		}

		/// <summary>
		/// Returns the ceiling power of 2 for a given value.
		/// </summary>
		/// <param name="value">The value for which to find the ceiling power of 2.</param>
		/// <returns>The ceiling power of 2 for the <paramref name="value"/>.</returns>
		[CLSCompliant(false)]
		public static ushort CeilingPowerOf2(ushort value)
		{
			ushort ret=(ushort)(value-1);
			ret|=(ushort)(ret>>1);
			ret|=(ushort)(ret>>2);
			ret|=(ushort)(ret>>4);
			ret|=(ushort)(ret>>8);
			return (ushort)(ret+1);
		}

		/// <summary>
		/// Returns the ceiling power of 2 for a given value.
		/// </summary>
		/// <param name="value">The value for which to find the ceiling power of 2.</param>
		/// <returns>The ceiling power of 2 for the <paramref name="value"/>.</returns>
		[CLSCompliant(false)]
		public static uint CeilingPowerOf2(uint value)
		{
			uint ret=value-1;
			ret|=ret>>1;
			ret|=ret>>2;
			ret|=ret>>4;
			ret|=ret>>8;
			ret|=ret>>16;
			return ret+1;
		}

		/// <summary>
		/// Returns the ceiling power of 2 for a given value.
		/// </summary>
		/// <param name="value">The value for which to find the ceiling power of 2.</param>
		/// <returns>The ceiling power of 2 for the <paramref name="value"/>.</returns>
		[CLSCompliant(false)]
		public static ulong CeilingPowerOf2(ulong value)
		{
			ulong ret=value-1;
			ret|=ret>>1;
			ret|=ret>>2;
			ret|=ret>>4;
			ret|=ret>>8;
			ret|=ret>>16;
			ret|=ret>>32;
			return ret+1;
		}
		#endregion

		#region FloorPowerOf2
		/// <summary>
		/// Returns the floor power of 2 for a given value.
		/// </summary>
		/// <param name="value">The value for which to find the floor power of 2.</param>
		/// <returns>The floor power of 2 for the <paramref name="value"/>.</returns>
		[CLSCompliant(false)]
		public static byte FloorPowerOf2(byte value)
		{
			return (byte)(NextLargestPowerOf2(value)>>1);
		}

		/// <summary>
		/// Returns the floor power of 2 for a given value.
		/// </summary>
		/// <param name="value">The value for which to find the floor power of 2.</param>
		/// <returns>The floor power of 2 for the <paramref name="value"/>.</returns>
		[CLSCompliant(false)]
		public static ushort FloorPowerOf2(ushort value)
		{
			return (ushort)(NextLargestPowerOf2(value)>>1);
		}

		/// <summary>
		/// Returns the floor power of 2 for a given value.
		/// </summary>
		/// <param name="value">The value for which to find the floor power of 2.</param>
		/// <returns>The floor power of 2 for the <paramref name="value"/>.</returns>
		[CLSCompliant(false)]
		public static uint FloorPowerOf2(uint value)
		{
			return NextLargestPowerOf2(value)>>1;
		}

		/// <summary>
		/// Returns the floor power of 2 for a given value.
		/// </summary>
		/// <param name="value">The value for which to find the floor power of 2.</param>
		/// <returns>The floor power of 2 for the <paramref name="value"/>.</returns>
		[CLSCompliant(false)]
		public static ulong FloorPowerOf2(ulong value)
		{
			return NextLargestPowerOf2(value)>>1;
		}
		#endregion

		#region IsPowerOf2
		/// <summary>
		/// Indicates whether a given value is a power of 2.
		/// </summary>
		/// <param name="value">The value for which to find if it is a power of 2.</param>
		/// <returns><b>true</b> if <paramref name="value"/> is a power of 2, <b>false</b> otherwise.</returns>
		[CLSCompliant(false)]
		public static bool IsPowerOf2(uint value)
		{
			return value==1&&(value&(value-1))!=0;
		}

		/// <summary>
		/// Indicates whether a given value is a power of 2.
		/// </summary>
		/// <param name="value">The value for which to find if it is a power of 2.</param>
		/// <returns><b>true</b> if <paramref name="value"/> is a power of 2, <b>false</b> otherwise.</returns>
		[CLSCompliant(false)]
		public static bool IsPowerOf2(ulong value)
		{
			return value==1&&(value&(value-1))!=0;
		}
		#endregion

		#region LeastSignificantOneBit
		/// <summary>
		/// Returns the least significant 1 bit (lowest numbered element of a bit set) for a given value.
		/// </summary>
		/// <param name="value">The value for which to find the least significant 1 bit.</param>
		/// <returns>The value with only the the least significant 1 bit set, or zero (0).</returns>
		[CLSCompliant(false)]
		public static byte LeastSignificantOneBit(byte value)
		{
			return (byte)(value^(value&(value-1)));
		}

		/// <summary>
		/// Returns the least significant 1 bit (lowest numbered element of a bit set) for a given value.
		/// </summary>
		/// <param name="value">The value for which to find the least significant 1 bit.</param>
		/// <returns>The value with only the the least significant 1 bit set, or zero (0).</returns>
		[CLSCompliant(false)]
		public static ushort LeastSignificantOneBit(ushort value)
		{
			return (ushort)(value^(value&(value-1)));
		}

		/// <summary>
		/// Returns the least significant 1 bit (lowest numbered element of a bit set) for a given value.
		/// </summary>
		/// <param name="value">The value for which to find the least significant 1 bit.</param>
		/// <returns>The value with only the the least significant 1 bit set, or zero (0).</returns>
		[CLSCompliant(false)]
		public static uint LeastSignificantOneBit(uint value)
		{
			return value^(value&(value-1));
		}

		/// <summary>
		/// Returns the least significant 1 bit (lowest numbered element of a bit set) for a given value.
		/// </summary>
		/// <param name="value">The value for which to find the least significant 1 bit.</param>
		/// <returns>The value with only the the least significant 1 bit set, or zero (0).</returns>
		[CLSCompliant(false)]
		public static ulong LeastSignificantOneBit(ulong value)
		{
			return value^(value&(value-1));
		}
		#endregion

		#region Log2
		/// <summary>
		/// Returns the base 2 logarithm for a given number. This is the number of filled bits (bits right of the last leading zero).
		/// </summary>
		/// <param name="value">The value for which to give the base 2 logarithm.</param>
		/// <returns>The base 2 logarithm for the <paramref name="value"/>. Zero (0) for <paramref name="value"/>==zero (0).</returns>
		[CLSCompliant(false)]
		public static int Log2(byte value)
		{
			byte ret=value;
			ret|=(byte)(ret>>1);
			ret|=(byte)(ret>>2);
			ret|=(byte)(ret>>4);
			return Ones((byte)(ret>>1));
		}

		/// <summary>
		/// Returns the base 2 logarithm for a given number. This is the number of filled bits (bits right of the last leading zero).
		/// </summary>
		/// <param name="value">The value for which to give the base 2 logarithm.</param>
		/// <returns>The base 2 logarithm for the <paramref name="value"/>. Zero (0) for <paramref name="value"/>==zero (0).</returns>
		[CLSCompliant(false)]
		public static int Log2(ushort value)
		{
			ushort ret=value;
			ret|=(ushort)(ret>>1);
			ret|=(ushort)(ret>>2);
			ret|=(ushort)(ret>>4);
			ret|=(ushort)(ret>>8);
			return Ones((ushort)(ret>>1));
		}

		/// <summary>
		/// Returns the base 2 logarithm for a given number. This is the number of filled bits (bits right of the last leading zero).
		/// </summary>
		/// <param name="value">The value for which to give the base 2 logarithm.</param>
		/// <returns>The base 2 logarithm for the <paramref name="value"/>. Zero (0) for <paramref name="value"/>==zero (0).</returns>
		[CLSCompliant(false)]
		public static int Log2(uint value)
		{
			uint ret=value;
			ret|=ret>>1;
			ret|=ret>>2;
			ret|=ret>>4;
			ret|=ret>>8;
			ret|=ret>>16;
			return Ones(ret>>1);
		}

		/// <summary>
		/// Returns the base 2 logarithm for a given number. This is the number of filled bits (bits right of the last leading zero).
		/// </summary>
		/// <param name="value">The value for which to give the base 2 logarithm.</param>
		/// <returns>The base 2 logarithm for the <paramref name="value"/>. Zero (0) for <paramref name="value"/>==zero (0).</returns>
		[CLSCompliant(false)]
		public static int Log2(ulong value)
		{
			ulong ret=value;
			ret|=ret>>1;
			ret|=ret>>2;
			ret|=ret>>4;
			ret|=ret>>8;
			ret|=ret>>16;
			ret|=ret>>32;
			return Ones(ret>>1);
		}
		#endregion

		#region MostSignificantOneBit
		/// <summary>
		/// Returns the most significant 1 bit (highest numbered element of a bit set) for a given value.
		/// </summary>
		/// <param name="value">The value for which to find the most significant 1 bit.</param>
		/// <returns>The value with only the the most significant 1 bit set, or zero (0).</returns>
		[CLSCompliant(false)]
		public static byte MostSignificantOneBit(byte value)
		{
			byte ret=value;
			ret|=(byte)(ret>>1);
			ret|=(byte)(ret>>2);
			ret|=(byte)(ret>>4);
			return (byte)(ret&~(ret>>1));
		}

		/// <summary>
		/// Returns the most significant 1 bit (highest numbered element of a bit set) for a given value.
		/// </summary>
		/// <param name="value">The value for which to find the most significant 1 bit.</param>
		/// <returns>The value with only the the most significant 1 bit set, or zero (0).</returns>
		[CLSCompliant(false)]
		public static ushort MostSignificantOneBit(ushort value)
		{
			ushort ret=value;
			ret|=(ushort)(ret>>1);
			ret|=(ushort)(ret>>2);
			ret|=(ushort)(ret>>4);
			ret|=(ushort)(ret>>8);
			return (ushort)(ret&~(ret>>1));
		}

		/// <summary>
		/// Returns the most significant 1 bit (highest numbered element of a bit set) for a given value.
		/// </summary>
		/// <param name="value">The value for which to find the most significant 1 bit.</param>
		/// <returns>The value with only the the most significant 1 bit set, or zero (0).</returns>
		[CLSCompliant(false)]
		public static uint MostSignificantOneBit(uint value)
		{
			uint ret=value;
			ret|=ret>>1;
			ret|=ret>>2;
			ret|=ret>>4;
			ret|=ret>>8;
			ret|=ret>>16;
			return ret&~(ret>>1);
		}

		/// <summary>
		/// Returns the most significant 1 bit (highest numbered element of a bit set) for a given value.
		/// </summary>
		/// <param name="value">The value for which to find the most significant 1 bit.</param>
		/// <returns>The value with only the the most significant 1 bit set, or zero (0).</returns>
		[CLSCompliant(false)]
		public static ulong MostSignificantOneBit(ulong value)
		{
			ulong ret=value;
			ret|=ret>>1;
			ret|=ret>>2;
			ret|=ret>>4;
			ret|=ret>>8;
			ret|=ret>>16;
			ret|=ret>>32;
			return ret&~(ret>>1);
		}
		#endregion

		#region NextLargestPowerOf2
		/// <summary>
		/// Returns the next largest power of 2 for a given value.
		/// </summary>
		/// <param name="value">The value for which to find the next largest power of 2.</param>
		/// <returns>The next largest power of 2 for the <paramref name="value"/>.</returns>
		[CLSCompliant(false)]
		public static byte NextLargestPowerOf2(byte value)
		{
			byte ret=value;
			ret|=(byte)(ret>>1);
			ret|=(byte)(ret>>2);
			ret|=(byte)(ret>>4);
			return (byte)(ret+1);
		}

		/// <summary>
		/// Returns the next largest power of 2 for a given value.
		/// </summary>
		/// <param name="value">The value for which to find the next largest power of 2.</param>
		/// <returns>The next largest power of 2 for the <paramref name="value"/>.</returns>
		[CLSCompliant(false)]
		public static ushort NextLargestPowerOf2(ushort value)
		{
			ushort ret=value;
			ret|=(ushort)(ret>>1);
			ret|=(ushort)(ret>>2);
			ret|=(ushort)(ret>>4);
			ret|=(ushort)(ret>>8);
			return (ushort)(ret+1);
		}

		/// <summary>
		/// Returns the next largest power of 2 for a given value.
		/// </summary>
		/// <param name="value">The value for which to find the next largest power of 2.</param>
		/// <returns>The next largest power of 2 for the <paramref name="value"/>.</returns>
		[CLSCompliant(false)]
		public static uint NextLargestPowerOf2(uint value)
		{
			uint ret=value;
			ret|=ret>>1;
			ret|=ret>>2;
			ret|=ret>>4;
			ret|=ret>>8;
			ret|=ret>>16;
			return ret+1;
		}

		/// <summary>
		/// Returns the next largest power of 2 for a given value.
		/// </summary>
		/// <param name="value">The value for which to find the next largest power of 2.</param>
		/// <returns>The next largest power of 2 for the <paramref name="value"/>.</returns>
		[CLSCompliant(false)]
		public static ulong NextLargestPowerOf2(ulong value)
		{
			ulong ret=value;
			ret|=ret>>1;
			ret|=ret>>2;
			ret|=ret>>4;
			ret|=ret>>8;
			ret|=ret>>16;
			ret|=ret>>32;
			return ret+1;
		}
		#endregion

		#region Ones
		/// <summary>
		/// Gets the number of 1-bits in a value.
		/// </summary>
		/// <param name="value">The value for which to count the ones.</param>
		/// <returns>The number of 1-bits in <paramref name="value"/>.</returns>
		[CLSCompliant(false)]
		public static int Ones(byte value)
		{
			uint ret=value;
			ret-=(ret>>1)&0x55555555;
			ret=((ret>>2)&0x33333333)+(ret&0x33333333);
			ret=((ret>>4)+ret)&0x0f0f0f0f;
			return (int)(ret&0xf);
		}

		/// <summary>
		/// Gets the number of 1-bits in a value.
		/// </summary>
		/// <param name="value">The value for which to count the ones.</param>
		/// <returns>The number of 1-bits in <paramref name="value"/>.</returns>
		[CLSCompliant(false)]
		public static int Ones(ushort value)
		{
			uint ret=value;
			ret-=(ret>>1)&0x55555555;
			ret=((ret>>2)&0x33333333)+(ret&0x33333333);
			ret=((ret>>4)+ret)&0x0f0f0f0f;
			ret+=ret>>8;
			return (int)(ret&0x1f);
		}

		/// <summary>
		/// Gets the number of 1-bits in a value.
		/// </summary>
		/// <param name="value">The value for which to count the ones.</param>
		/// <returns>The number of 1-bits in <paramref name="value"/>.</returns>
		[CLSCompliant(false)]
		public static int Ones(uint value)
		{
			uint ret=value;
			ret-=(ret>>1)&0x55555555;
			ret=((ret>>2)&0x33333333)+(ret&0x33333333);
			ret=((ret>>4)+ret)&0x0f0f0f0f;
			ret+=ret>>8;
			ret+=ret>>16;
			return (int)(ret&0x3f);
		}

		/// <summary>
		/// Gets the number of 1-bits in a value.
		/// </summary>
		/// <param name="value">The value for which to count the ones.</param>
		/// <returns>The number of 1-bits in <paramref name="value"/>.</returns>
		[CLSCompliant(false)]
		public static int Ones(ulong value)
		{
			return Ones((uint)value)+Ones((uint)(value>>32));
		}
		#endregion
	}
}
