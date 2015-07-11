using System;
using System.Runtime.InteropServices;

namespace Free.Core
{
	/// <summary>
	/// Implements an unsigned 128-bit integer value type.
	/// </summary>
	/// <threadsafety static="false" instance="false"/>
	[StructLayout(LayoutKind.Sequential, Pack=1)]
	[CLSCompliant(false)]
	public struct UInt128 : IEquatable<UInt128>
	{
		#region Pseudo-constants
		/// <summary>
		/// A 128-bit unsigned zero (0).
		/// </summary>
		public static readonly UInt128 Zero=0;

		/// <summary>
		/// A 128-bit unsigned one (1).
		/// </summary>
		public static readonly UInt128 One=1;

		/// <summary>
		/// Represents the smallest possible value of <see cref="UInt128"/>.
		/// </summary>
		public static readonly UInt128 MinValue=0;

		/// <summary>
		/// Represents the largest possible value of <see cref="UInt128"/>.
		/// </summary>
		public static readonly UInt128 MaxValue=new UInt128(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF);
		#endregion

		/// <summary>
		/// The value.
		/// </summary>
		public ulong Low, High;

		#region Constructors
		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value to initialize the instance with.</param>
		public UInt128(byte value) { High=0; Low=value; }

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value to initialize the instance with.</param>
		public UInt128(sbyte value) { High=value<0?0xFFFFFFFFFFFFFFFF:0; Low=(ulong)value; }

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value to initialize the instance with.</param>
		public UInt128(short value) { High=value<0?0xFFFFFFFFFFFFFFFF:0; Low=(ulong)value; }
		
		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value to initialize the instance with.</param>
		public UInt128(ushort value) { High=0; Low=value; }

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value to initialize the instance with.</param>
		public UInt128(int value) { High=value<0?0xFFFFFFFFFFFFFFFF:0; Low=(ulong)value; }
		
		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value to initialize the instance with.</param>
		public UInt128(uint value) { High=0; Low=value; }

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value to initialize the instance with.</param>
		public UInt128(long value) { High=value<0?0xFFFFFFFFFFFFFFFF:0; Low=(ulong)value; }

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="value">The value to initialize the instance with.</param>
		public UInt128(ulong value) { High=0; Low=value; }

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="high">The higher 64 bits of value to initialize the instance with.</param>
		/// <param name="low">The lower 64 bits of value to initialize the instance with.</param>
		public UInt128(ulong high, ulong low) { High=high; Low=low; }
		#endregion

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return Low.GetHashCode();
		}

		/// <summary>
		/// Returns a value indicating whether this instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns><b>true</b> if <paramref name="obj"/> is an instance of <see cref="UInt128"/> and equals the value of this instance; otherwise, <b>false</b>.</returns>
		public override bool Equals(object obj)
		{
			if(!(obj is UInt128)) return false;

			UInt128 other=(UInt128)obj;
			return High==other.High&&Low==other.Low;
		}

		/// <summary>
		/// Returns a value indicating whether this instance is equal to a specified object.
		/// </summary>
		/// <param name="other">An object to compare with this instance.</param>
		/// <returns><b>true</b> if <paramref name="other"/> equals the value of this instance; otherwise, <b>false</b>.</returns>
		public bool Equals(UInt128 other)
		{
			return High==other.High&&Low==other.Low;
		}

		#region public static implicit operator UInt128(<Type> value) { return new UInt128(value); }
		/// <summary>
		/// Converts a value to <see cref="UInt128"/>.
		/// </summary>
		/// <param name="value">The value to initialize with.</param>
		/// <returns>The converted value.</returns>
		public static implicit operator UInt128(byte value) { return new UInt128(value); }

		/// <summary>
		/// Converts a value to <see cref="UInt128"/>.
		/// </summary>
		/// <param name="value">The value to initialize with.</param>
		/// <returns>The converted value.</returns>
		public static implicit operator UInt128(sbyte value) { return new UInt128(value); }

		/// <summary>
		/// Converts a value to <see cref="UInt128"/>.
		/// </summary>
		/// <param name="value">The value to initialize with.</param>
		/// <returns>The converted value.</returns>
		public static implicit operator UInt128(short value) { return new UInt128(value); }

		/// <summary>
		/// Converts a value to <see cref="UInt128"/>.
		/// </summary>
		/// <param name="value">The value to initialize with.</param>
		/// <returns>The converted value.</returns>
		public static implicit operator UInt128(ushort value) { return new UInt128(value); }

		/// <summary>
		/// Converts a value to <see cref="UInt128"/>.
		/// </summary>
		/// <param name="value">The value to initialize with.</param>
		/// <returns>The converted value.</returns>
		public static implicit operator UInt128(int value) { return new UInt128(value); }

		/// <summary>
		/// Converts a value to <see cref="UInt128"/>.
		/// </summary>
		/// <param name="value">The value to initialize with.</param>
		/// <returns>The converted value.</returns>
		public static implicit operator UInt128(uint value) { return new UInt128(value); }

		/// <summary>
		/// Converts a value to <see cref="UInt128"/>.
		/// </summary>
		/// <param name="value">The value to initialize with.</param>
		/// <returns>The converted value.</returns>
		public static implicit operator UInt128(long value) { return new UInt128(value); }

		/// <summary>
		/// Converts a value to <see cref="UInt128"/>.
		/// </summary>
		/// <param name="value">The value to initialize with.</param>
		/// <returns>The converted value.</returns>
		public static implicit operator UInt128(ulong value) { return new UInt128(value); }
		#endregion

		#region public static explicit operator <Type>(UInt128 val) {...}
		/// <summary>
		/// Converts <see cref="UInt128"/> to <b>byte</b>.
		/// </summary>
		/// <param name="value">The <see cref="UInt128"/> to convert.</param>
		/// <returns>The converted value.</returns>
		public static explicit operator byte(UInt128 value) { return (byte)value.Low; }

		/// <summary>
		/// Converts <see cref="UInt128"/> to <b>sbyte</b>.
		/// </summary>
		/// <param name="value">The <see cref="UInt128"/> to convert.</param>
		/// <returns>The converted value.</returns>
		public static explicit operator sbyte(UInt128 value) { return (sbyte)value.Low; }

		/// <summary>
		/// Converts <see cref="UInt128"/> to <b>short</b>.
		/// </summary>
		/// <param name="value">The <see cref="UInt128"/> to convert.</param>
		/// <returns>The converted value.</returns>
		public static explicit operator short(UInt128 value) { return (short)value.Low; }

		/// <summary>
		/// Converts <see cref="UInt128"/> to <b>ushort</b>.
		/// </summary>
		/// <param name="value">The <see cref="UInt128"/> to convert.</param>
		/// <returns>The converted value.</returns>
		public static explicit operator ushort(UInt128 value) { return (ushort)value.Low; }

		/// <summary>
		/// Converts <see cref="UInt128"/> to <b>int</b>.
		/// </summary>
		/// <param name="value">The <see cref="UInt128"/> to convert.</param>
		/// <returns>The converted value.</returns>
		public static explicit operator int(UInt128 value) { return (int)value.Low; }

		/// <summary>
		/// Converts <see cref="UInt128"/> to <b>uint</b>.
		/// </summary>
		/// <param name="value">The <see cref="UInt128"/> to convert.</param>
		/// <returns>The converted value.</returns>
		public static explicit operator uint(UInt128 value) { return (uint)value.Low; }

		/// <summary>
		/// Converts <see cref="UInt128"/> to <b>long</b>.
		/// </summary>
		/// <param name="value">The <see cref="UInt128"/> to convert.</param>
		/// <returns>The converted value.</returns>
		public static explicit operator long(UInt128 value) { return (long)value.Low; }

		/// <summary>
		/// Converts <see cref="UInt128"/> to <b>ulong</b>.
		/// </summary>
		/// <param name="value">The <see cref="UInt128"/> to convert.</param>
		/// <returns>The converted value.</returns>
		public static explicit operator ulong(UInt128 value) { return value.Low; }
		#endregion

		#region Unary operators
		/// <summary>
		/// Identity operation.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The value.</returns>
		public static UInt128 operator+(UInt128 value) { return value; }
		
		// not allowed on unsigned types
		//public static int operator-(UInt128 value) { return 0; }

		// not allowed on non-boolean types
		//public static int operator!(UInt128 value) { return 0; }

		/// <summary>
		/// Bitwise negation operation.
		/// </summary>
		/// <param name="value">The value to be negated bitwise.</param>
		/// <returns>The bitwise negated value.</returns>
		public static UInt128 operator~(UInt128 value) { return new UInt128(~value.High, ~value.Low); }

		/// <summary>
		/// Integral incrementation operation.
		/// </summary>
		/// <param name="value">The value to be incremented.</param>
		/// <returns>The incremented value.</returns>
		public static UInt128 operator++(UInt128 value) { return value+1; }

		/// <summary>
		/// Integral decrementation operation.
		/// </summary>
		/// <param name="value">The value to be decremented.</param>
		/// <returns>The decremented value.</returns>
		public static UInt128 operator--(UInt128 value) { return value-1; }

#if false
		/// <summary>
		/// The true operation.
		/// </summary>
		/// <param name="value">The value to be checked.</param>
		/// <returns><b>true</b> if <paramref name="value"/> is non-zero; <b>false</b> otherwise.</returns>
		public static bool operator true(UInt128 value) { return value.High!=0||value.Low!=0; }

		/// <summary>
		/// The false operation.
		/// </summary>
		/// <param name="value">The value to be checked.</param>
		/// <returns><b>true</b> if <paramref name="value"/> is zero; <b>false</b> otherwise.</returns>
		public static bool operator false(UInt128 value) { return value.High==0&&value.Low==0; }
#endif
		#endregion

		#region Binary operators
		#region Operators for integer arithmetics
		/// <summary>
		/// Addition operation.
		/// </summary>
		/// <param name="a">The 1st summand.</param>
		/// <param name="b">The 2nd summand.</param>
		/// <returns>The <see cref="UInt128"/> result of the addition operation (the sum).</returns>
		public static UInt128 operator+(UInt128 a, UInt128 b)
		{
			ulong low=a.Low+b.Low;
			return new UInt128(a.High+b.High+(low<a.Low?1ul:0ul), low);
		}

		/// <summary>
		/// Substraction operation.
		/// </summary>
		/// <param name="a">The minuend.</param>
		/// <param name="b">The subtrahend.</param>
		/// <returns>The <see cref="UInt128"/> result of the substraction operation (the difference).</returns>
		public static UInt128 operator-(UInt128 a, UInt128 b)
		{
			ulong low=a.Low-b.Low;
			return new UInt128(a.High-b.High-(low>a.Low?1ul:0ul), low);
		}
		
		/// <summary>
		/// Multiplies one <see cref="UInt128"/> value with another and returns the result.
		/// </summary>
		/// <param name="a">The multiplicand.</param>
		/// <param name="b">The multiplier.</param>
		/// <returns>The product of the multiplication.</returns>
		public static UInt128 operator*(UInt128 a, UInt128 b)
		{
			uint a0, a1, a2, a3, b0, b1, b2, b3;
			if(a<b)
			{
				a0=(uint)a.Low; a1=(uint)(a.Low>>32); a2=(uint)a.High; a3=(uint)(a.High>>32);
				b0=(uint)b.Low; b1=(uint)(b.Low>>32); b2=(uint)b.High; b3=(uint)(b.High>>32);
			}
			else
			{
				b0=(uint)a.Low; b1=(uint)(a.Low>>32); b2=(uint)a.High; b3=(uint)(a.High>>32);
				a0=(uint)b.Low; a1=(uint)(b.Low>>32); a2=(uint)b.High; a3=(uint)(b.High>>32);
			}

			uint r0=0, r1=0, r2=0, r3=0;
			if(a3!=0) r3=a3*b0;

			if(a2!=0)
			{
				ulong tmp=(ulong)a2*b0;
				r2=(uint)tmp;

				r3+=a2*b1+(uint)(tmp>>32);
			}

			if(a1!=0)
			{
				ulong tmp=(ulong)a1*b0;
				r1=(uint)tmp;

				tmp=(ulong)a1*b1+r2+(uint)(tmp>>32);
				r2=(uint)tmp;

				r3+=a1*b2+(uint)(tmp>>32);
			}

			if(a0!=0)
			{
				ulong tmp=(ulong)a0*b0;
				r0=(uint)tmp;

				tmp=(ulong)a0*b1+r1+(uint)(tmp>>32);
				r1=(uint)tmp;

				tmp=(ulong)a0*b2+r2+(uint)(tmp>>32);
				r2=(uint)tmp;

				r3+=a0*b3+(uint)(tmp>>32);
			}

			return new UInt128((ulong)r3<<32|r2, (ulong)r1<<32|r0);
		}

		/// <summary>
		/// Divides one <see cref="UInt128"/> value by another and returns the result.
		/// </summary>
		/// <param name="a">The value to be divided.</param>
		/// <param name="b">The value to divide by.</param>
		/// <returns>The quotient of the division.</returns>
		public static UInt128 operator/(UInt128 a, UInt128 b) { UInt128 rem; return DivRem(a, b, out rem); }

		/// <summary>
		/// Divides one <see cref="UInt128"/> value by another and returns the remainder. (Modulo)
		/// </summary>
		/// <param name="a">The value to be divided.</param>
		/// <param name="b">The value to divide by.</param>
		/// <returns>The remainder of the division. (Modulo)</returns>
		public static UInt128 operator%(UInt128 a, UInt128 b) { UInt128 rem; DivRem(a, b, out rem); return rem; }
		#endregion

		#region Operators for bit arithmetics
		/// <summary>
		/// Bitwise AND operation.
		/// </summary>
		/// <param name="a">The 1st value.</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns>The <see cref="UInt128"/> result of the bitwise AND operation.</returns>
		public static UInt128 operator&(UInt128 a, UInt128 b) { return new UInt128(a.High&b.High, a.Low&b.Low); }

		/// <summary>
		/// Bitwise AND operation.
		/// </summary>
		/// <param name="a">The 1st value (as <b>ulong</b>).</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns>The <see cref="UInt128"/> result of the bitwise AND operation.</returns>
		public static UInt128 operator&(ulong a, UInt128 b) { return new UInt128(0, a&b.Low); }

		/// <summary>
		/// Bitwise AND operation.
		/// </summary>
		/// <param name="a">The 1st value (as <b>uint</b> for performance reasons).</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns>The <see cref="UInt128"/> result of the bitwise AND operation.</returns>
		public static UInt128 operator&(uint a, UInt128 b) { return new UInt128(0, a&b.Low); }

		/// <summary>
		/// Bitwise AND operation.
		/// </summary>
		/// <param name="a">The 1st value (as <b>ushort</b> for performance reasons).</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns>The <see cref="UInt128"/> result of the bitwise AND operation.</returns>
		public static UInt128 operator&(ushort a, UInt128 b) { return new UInt128(0, a&b.Low); }

		/// <summary>
		/// Bitwise AND operation.
		/// </summary>
		/// <param name="a">The 1st value (as <b>byte</b> for performance reasons).</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns>The <see cref="UInt128"/> result of the bitwise AND operation.</returns>
		public static UInt128 operator&(byte a, UInt128 b) { return new UInt128(0, a&b.Low); }

		/// <summary>
		/// Bitwise AND operation.
		/// </summary>
		/// <param name="a">The 1st value (as <b>long</b>).</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns>The <see cref="UInt128"/> result of the bitwise AND operation.</returns>
		public static UInt128 operator&(long a, UInt128 b) { return new UInt128(a<0?b.High:0, (ulong)a&b.Low); }

		/// <summary>
		/// Bitwise OR operation.
		/// </summary>
		/// <param name="a">The 1st value.</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns>The <see cref="UInt128"/> result of the bitwise OR operation.</returns>
		public static UInt128 operator|(UInt128 a, UInt128 b) { return new UInt128(a.High|b.High, a.Low|b.Low); }

		/// <summary>
		/// Bitwise OR operation.
		/// </summary>
		/// <param name="a">The 1st value (as <b>ulong</b>).</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns>The <see cref="UInt128"/> result of the bitwise OR operation.</returns>
		public static UInt128 operator|(ulong a, UInt128 b) { return new UInt128(b.High, a|b.Low); }

		/// <summary>
		/// Bitwise OR operation.
		/// </summary>
		/// <param name="a">The 1st value (as <b>uint</b> for performance reasons).</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns>The <see cref="UInt128"/> result of the bitwise OR operation.</returns>
		public static UInt128 operator|(uint a, UInt128 b) { return new UInt128(b.High, a|b.Low); }

		/// <summary>
		/// Bitwise OR operation.
		/// </summary>
		/// <param name="a">The 1st value (as <b>ushort</b> for performance reasons).</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns>The <see cref="UInt128"/> result of the bitwise OR operation.</returns>
		public static UInt128 operator|(ushort a, UInt128 b) { return new UInt128(b.High, a|b.Low); }

		/// <summary>
		/// Bitwise OR operation.
		/// </summary>
		/// <param name="a">The 1st value (as <b>byte</b> for performance reasons).</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns>The <see cref="UInt128"/> result of the bitwise OR operation.</returns>
		public static UInt128 operator|(byte a, UInt128 b) { return new UInt128(b.High, a|b.Low); }

		/// <summary>
		/// Bitwise OR operation.
		/// </summary>
		/// <param name="a">The 1st value (as <b>long</b>).</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns>The <see cref="UInt128"/> result of the bitwise OR operation.</returns>
		public static UInt128 operator|(long a, UInt128 b) { return new UInt128(a<0?0xFFFFFFFF:b.High, (ulong)a|b.Low); }

		/// <summary>
		/// Bitwise exclusive-OR operation.
		/// </summary>
		/// <param name="a">The 1st value.</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns>The <see cref="UInt128"/> result of the bitwise exclusive-OR operation.</returns>
		public static UInt128 operator^(UInt128 a, UInt128 b) { return new UInt128(a.High^b.High, a.Low^b.Low); }

		/// <summary>
		/// Bitwise exclusive-OR operation.
		/// </summary>
		/// <param name="a">The 1st value (as <b>ulong</b>).</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns>The <see cref="UInt128"/> result of the bitwise exclusive-OR operation.</returns>
		public static UInt128 operator^(ulong a, UInt128 b) { return new UInt128(b.High, a^b.Low); }

		/// <summary>
		/// Bitwise exclusive-OR operation.
		/// </summary>
		/// <param name="a">The 1st value (as <b>uint</b> for performance reasons).</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns>The <see cref="UInt128"/> result of the bitwise exclusive-OR operation.</returns>
		public static UInt128 operator^(uint a, UInt128 b) { return new UInt128(b.High, a^b.Low); }

		/// <summary>
		/// Bitwise exclusive-OR operation.
		/// </summary>
		/// <param name="a">The 1st value (as <b>ushort</b> for performance reasons).</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns>The <see cref="UInt128"/> result of the bitwise exclusive-OR operation.</returns>
		public static UInt128 operator^(ushort a, UInt128 b) { return new UInt128(b.High, a^b.Low); }

		/// <summary>
		/// Bitwise exclusive-OR operation.
		/// </summary>
		/// <param name="a">The 1st value (as <b>byte</b> for performance reasons).</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns>The <see cref="UInt128"/> result of the bitwise exclusive-OR operation.</returns>
		public static UInt128 operator^(byte a, UInt128 b) { return new UInt128(b.High, a^b.Low); }

		/// <summary>
		/// Bitwise exclusive-OR operation.
		/// </summary>
		/// <param name="a">The 1st value (as <b>long</b>).</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns>The <see cref="UInt128"/> result of the bitwise exclusive-OR operation.</returns>
		public static UInt128 operator^(long a, UInt128 b) { return new UInt128(a<0?~b.High:b.High, (ulong)a^b.Low); }

		/// <summary>
		/// Bit-shift operation to the left (into the higher order bits).
		/// </summary>
		/// <param name="value">The value to be shifted.</param>
		/// <param name="bits">The number of bit positions <paramref name="value"/> is to be shifted.</param>
		/// <returns>The <see cref="UInt128"/> result of the bit-shift operation.</returns>
		public static UInt128 operator<<(UInt128 value, int bits)
		{
			bits&=0x7F; // only low-order seven bits needed/allowed
			if(bits==0) return value;
			if(bits==64) return new UInt128(value.Low, 0);
			if(bits>64) return new UInt128(value.Low<<(bits-64), 0);
			return new UInt128(value.High<<bits|value.Low>>(64-bits), value.Low<<bits);
		}

		/// <summary>
		/// Bit-shift operation to the right (into the lower order bits).
		/// </summary>
		/// <param name="value">The value to be shifted.</param>
		/// <param name="bits">The number of bit positions <paramref name="value"/> is to be shifted.</param>
		/// <returns>The <see cref="UInt128"/> result of the bit-shift operation.</returns>
		public static UInt128 operator>>(UInt128 value, int bits)
		{
			bits&=0x7F; // only low-order seven bits needed/allowed
			if(bits==0) return value;
			if(bits==64) return new UInt128(0, value.High);
			if(bits>64) return new UInt128(0, value.High>>(bits-64));
			return new UInt128(value.High>>bits, value.High<<(64-bits)|value.Low>>bits);
		}
		#endregion
		#endregion

		#region Comparison operators
		/// <summary>
		/// Equality operation.
		/// </summary>
		/// <param name="a">The 1st value.</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns><b>true</b> if <paramref name="a"/> and <paramref name="b"/> have the same value; <b>false</b> otherwise.</returns>
		public static bool operator==(UInt128 a, UInt128 b) { return a.High==b.High&&a.Low==b.Low; }

		/// <summary>
		/// Inequality operation.
		/// </summary>
		/// <param name="a">The 1st value.</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns><b>true</b> if <paramref name="a"/> and <paramref name="b"/> does not have the same value; <b>false</b> otherwise.</returns>
		public static bool operator!=(UInt128 a, UInt128 b) { return a.High!=b.High||a.Low!=b.Low; }

		/// <summary>
		/// "Less than" relational operation.
		/// </summary>
		/// <param name="a">The 1st value.</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns><b>true</b> if <paramref name="a"/> is less than <paramref name="b"/>; <b>false</b> otherwise.</returns>
		public static bool operator<(UInt128 a, UInt128 b) { return a.High<b.High||(a.High==b.High&&a.Low<b.Low); }

		/// <summary>
		/// "Greater than" relational operation.
		/// </summary>
		/// <param name="a">The 1st value.</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns><b>true</b> if <paramref name="a"/> is greater than <paramref name="b"/>; <b>false</b> otherwise.</returns>
		public static bool operator>(UInt128 a, UInt128 b) { return a.High>b.High||(a.High==b.High&&a.Low>b.Low); }

		/// <summary>
		/// "Less than or equal" relational operation.
		/// </summary>
		/// <param name="a">The 1st value.</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns><b>true</b> if <paramref name="a"/> is less than or equal to <paramref name="b"/>; <b>false</b> otherwise.</returns>
		public static bool operator<=(UInt128 a, UInt128 b) { return a.High<b.High||(a.High==b.High&&a.Low<=b.Low); }

		/// <summary>
		/// "Greater than or equal" relational operation.
		/// </summary>
		/// <param name="a">The 1st value.</param>
		/// <param name="b">The 2nd value.</param>
		/// <returns><b>true</b> if <paramref name="a"/> is greater than or equal to <paramref name="b"/>; <b>false</b> otherwise.</returns>
		public static bool operator>=(UInt128 a, UInt128 b) { return a.High>b.High||(a.High==b.High&&a.Low>=b.Low); }
		#endregion

		/// <summary>
		/// Multiplies one <see cref="UInt128"/> value with another, returns the result, and returns the overflow in an output parameter.
		/// </summary>
		/// <param name="a">The multiplicand.</param>
		/// <param name="b">The multiplier.</param>
		/// <param name="highBits">The overflowing higher bits.</param>
		/// <returns>The product of the multiplication.</returns>
		public static UInt128 BigMul(UInt128 a, UInt128 b, out UInt128 highBits)
		{
			uint a0, a1, a2, a3, b0, b1, b2, b3;
			if(a<b)
			{
				a0=(uint)a.Low; a1=(uint)(a.Low>>32); a2=(uint)a.High; a3=(uint)(a.High>>32);
				b0=(uint)b.Low; b1=(uint)(b.Low>>32); b2=(uint)b.High; b3=(uint)(b.High>>32);
			}
			else
			{
				b0=(uint)a.Low; b1=(uint)(a.Low>>32); b2=(uint)a.High; b3=(uint)(a.High>>32);
				a0=(uint)b.Low; a1=(uint)(b.Low>>32); a2=(uint)b.High; a3=(uint)(b.High>>32);
			}

			uint r0=0, r1=0, r2=0, r3=0, r4=0, r5=0, r6=0, r7=0;
			if(a3!=0)
			{
				ulong tmp=(ulong)a3*b0;
				r3=(uint)tmp;

				tmp=(ulong)a3*b1+(uint)(tmp>>32);
				r4=(uint)tmp;

				tmp=(ulong)a3*b2+(uint)(tmp>>32);
				r5=(uint)tmp;

				tmp=(ulong)a3*b3+(uint)(tmp>>32);
				r6=(uint)tmp;

				r7=(uint)(tmp>>32);
			}

			if(a2!=0)
			{
				ulong tmp=(ulong)a2*b0;
				r2=(uint)tmp;

				tmp=(ulong)a2*b1+r3+(uint)(tmp>>32);
				r3=(uint)tmp;

				tmp=(ulong)a2*b2+r4+(uint)(tmp>>32);
				r4=(uint)tmp;

				tmp=(ulong)a2*b3+r5+(uint)(tmp>>32);
				r5=(uint)tmp;

				tmp=(ulong)r6+(uint)(tmp>>32);
				r6=(uint)tmp;

				r7+=(uint)(tmp>>32);
			}

			if(a1!=0)
			{
				ulong tmp=(ulong)a1*b0;
				r1=(uint)tmp;

				tmp=(ulong)a1*b1+r2+(uint)(tmp>>32);
				r2=(uint)tmp;

				tmp=(ulong)a1*b2+r3+(uint)(tmp>>32);
				r3=(uint)tmp;

				tmp=(ulong)a1*b3+r4+(uint)(tmp>>32);
				r4=(uint)tmp;

				tmp=(ulong)r5+(uint)(tmp>>32);
				r5=(uint)tmp;

				tmp=(ulong)r6+(uint)(tmp>>32);
				r6=(uint)tmp;

				r7+=(uint)(tmp>>32);
			}

			if(a0!=0)
			{
				ulong tmp=(ulong)a0*b0;
				r0=(uint)tmp;

				tmp=(ulong)a0*b1+r1+(uint)(tmp>>32);
				r1=(uint)tmp;

				tmp=(ulong)a0*b2+r2+(uint)(tmp>>32);
				r2=(uint)tmp;

				tmp=(ulong)a0*b3+r3+(uint)(tmp>>32);
				r3=(uint)tmp;

				tmp=(ulong)r4+(uint)(tmp>>32);
				r4=(uint)tmp;

				tmp=(ulong)r5+(uint)(tmp>>32);
				r5=(uint)tmp;

				tmp=(ulong)r6+(uint)(tmp>>32);
				r6=(uint)tmp;

				r7+=(uint)(tmp>>32);
			}

			highBits=new UInt128((ulong)r7<<32|r6, (ulong)r5<<32|r4);
			return new UInt128((ulong)r3<<32|r2, (ulong)r1<<32|r0);
		}

		/// <summary>
		/// Divides one <see cref="UInt128"/> value by another, returns the result, and returns the remainder in an output parameter.
		/// </summary>
		/// <param name="dividend">The value to be divided.</param>
		/// <param name="divisor">The value to divide by.</param>
		/// <param name="remainder">When this method returns, contains a <see cref="UInt128"/> value that represents the remainder from the division. This parameter is passed uninitialized.</param>
		/// <returns>The quotient of the division.</returns>
		public static UInt128 DivRem(UInt128 dividend, UInt128 divisor, out UInt128 remainder)
		{
			if(divisor.High==0&&divisor.Low==0) throw new DivideByZeroException();

			if(divisor.High==0&&divisor.Low==1) { remainder=0; return dividend; }
			if(dividend<divisor) { remainder=dividend; return 0; }
			if(dividend==divisor) { remainder=0; return 1; }

			UInt128 ret=0, multiplier=1, shiftedDivisor=divisor;
			int shifts=0;

			// special case - no bit left for regular '>='-search of divisor multiplier
			if((dividend.High&0x8000000000000000ul)!=0)
			{
				while((shiftedDivisor.High&0x8000000000000000ul)==0)
				{
					shiftedDivisor<<=1;
					multiplier<<=1;
					shifts++;
				}
			}
			else
			{
				while(dividend>=(shiftedDivisor<<1))
				{
					shiftedDivisor<<=1;
					multiplier<<=1;
					shifts++;
				}
			}

			remainder=dividend-shiftedDivisor;
			ret|=multiplier;
			if(remainder<divisor) return ret;

			do
			{
				shiftedDivisor>>=1;
				multiplier>>=1;

				if(remainder>=shiftedDivisor)
				{
					remainder-=shiftedDivisor;
					ret|=multiplier;
					if(remainder<divisor) return ret;
				}

				shifts--;
			} while(shifts>0);

			return ret;
		}
	}
}
