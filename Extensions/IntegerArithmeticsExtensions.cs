using System;

namespace Free.Core.Extensions.IntegerArithmeticsExtensions
{
	/// <summary>
	/// Contains extensions to the integer value types.
	/// </summary>
	/// <threadsafety static="true" instance="true"/>
	[CLSCompliant(false)]
	public static class IntegerArithmeticsExtensions
	{
		/// <summary>
		/// Multiplies one <see cref="Byte"/> value with another, returns the result, and returns the overflow in an output parameter.
		/// </summary>
		/// <param name="a">The multiplicand.</param>
		/// <param name="b">The multiplier.</param>
		/// <param name="highBits">The overflowing higher bits.</param>
		/// <returns>The product of the multiplication.</returns>
		public static byte BigMul(this byte a, byte b, out byte highBits)
		{
			uint tmp=(uint)a*b;
			highBits=(byte)(tmp>>8);
			return (byte)tmp;
		}

		/// <summary>
		/// Multiplies one <see cref="UInt16"/> value with another, returns the result, and returns the overflow in an output parameter.
		/// </summary>
		/// <param name="a">The multiplicand.</param>
		/// <param name="b">The multiplier.</param>
		/// <param name="highBits">The overflowing higher bits.</param>
		/// <returns>The product of the multiplication.</returns>
		public static ushort BigMul(this ushort a, ushort b, out ushort highBits)
		{
			uint tmp=(uint)a*b;
			highBits=(ushort)(tmp>>16);
			return (ushort)tmp;
		}

		/// <summary>
		/// Multiplies one <see cref="UInt32"/> value with another, returns the result, and returns the overflow in an output parameter.
		/// </summary>
		/// <param name="a">The multiplicand.</param>
		/// <param name="b">The multiplier.</param>
		/// <param name="highBits">The overflowing higher bits.</param>
		/// <returns>The product of the multiplication.</returns>
		public static uint BigMul(this uint a, uint b, out uint highBits)
		{
			ulong tmp=(ulong)a*b;
			highBits=(uint)(tmp>>32);
			return (uint)tmp;
		}

		/// <summary>
		/// Multiplies one <see cref="UInt64"/> value with another, returns the result, and returns the overflow in an output parameter.
		/// </summary>
		/// <param name="a">The multiplicand.</param>
		/// <param name="b">The multiplier.</param>
		/// <param name="highBits">The overflowing higher bits.</param>
		/// <returns>The product of the multiplication.</returns>
		public static ulong BigMul(this ulong a, ulong b, out ulong highBits)
		{
			uint a0, a1, b0, b1;
			if(a<b)
			{
				a0=(uint)a; a1=(uint)(a>>32);
				b0=(uint)b; b1=(uint)(b>>32);
			}
			else
			{
				b0=(uint)a; b1=(uint)(a>>32);
				a0=(uint)b; a1=(uint)(b>>32);
			}

			uint r0=0, r1=0, r2=0, r3=0;
			if(a1!=0)
			{
				ulong tmp=(ulong)a1*b0;
				r1=(uint)tmp;

				tmp=(ulong)a1*b1+(uint)(tmp>>32);
				r2=(uint)tmp;

				r3=(uint)(tmp>>32);
			}

			if(a0!=0)
			{
				ulong tmp=(ulong)a0*b0;
				r0=(uint)tmp;

				tmp=(ulong)a0*b1+r1+(uint)(tmp>>32);
				r1=(uint)tmp;

				tmp=(ulong)r2+(uint)(tmp>>32);
				r2=(uint)tmp;

				r3+=(uint)(tmp>>32);
			}

			highBits=(ulong)r3<<32|r2;
			return (ulong)r1<<32|r0;
		}
	}

#if USE_NAMESPACE_DOC_CLASSES
	/// <summary>
	/// Contains extensions to the integer value types.
	/// </summary>
	[System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
	class NamespaceDoc { }
#endif
}
