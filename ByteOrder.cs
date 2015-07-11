using System;

namespace Free.Core
{
	/// <summary>
	/// Class for changing the byte-order of values and arrays of value types.
	/// </summary>
	/// <threadsafety static="true" instance="true"/>
	public static class ByteOrder
	{
		#region Swap for values
		/// <summary>
		/// Reverses the byte-order of a value.
		/// </summary>
		/// <param name="value">The value which byte-order is to be reversed.</param>
		/// <returns>The byte-order reversed value.</returns>
		public static char Swap(char value)
		{
			return (char)(value<<8|(value>>8&255));
		}

		/// <summary>
		/// Reverses the byte-order of a value.
		/// </summary>
		/// <param name="value">The value which byte-order is to be reversed.</param>
		/// <returns>The byte-order reversed value.</returns>
		public static short Swap(short value)
		{
			return (short)(value<<8|(value>>8&255));
		}

		/// <summary>
		/// Reverses the byte-order of a value.
		/// </summary>
		/// <param name="value">The value which byte-order is to be reversed.</param>
		/// <returns>The byte-order reversed value.</returns>
		[CLSCompliant(false)]
		public static ushort Swap(ushort value)
		{
			return (ushort)(value<<8|(value>>8&255));
		}

		/// <summary>
		/// Reverses the byte-order of a value.
		/// </summary>
		/// <param name="value">The value which byte-order is to be reversed.</param>
		/// <returns>The byte-order reversed value.</returns>
		public static int Swap(int value)
		{
			return value<<24|(value&0xff00)<<8|(value>>8&0xff00)|(value>>24&0xff);
		}

		/// <summary>
		/// Reverses the byte-order of a value.
		/// </summary>
		/// <param name="value">The value which byte-order is to be reversed.</param>
		/// <returns>The byte-order reversed value.</returns>
		[CLSCompliant(false)]
		public static uint Swap(uint value)
		{
			return value<<24|(value&0xff00)<<8|(value>>8&0xff00)|(value>>24&0xff);
		}

		/// <summary>
		/// Reverses the byte-order of a value.
		/// </summary>
		/// <param name="value">The value which byte-order is to be reversed.</param>
		/// <returns>The byte-order reversed value.</returns>
		public static long Swap(long value)
		{
			// a tiny bit slower and unsafe
			//long ret;
			//int* pVal=(int*)&value;
			//int* pRet=(int*)&ret;
			//int a=*pVal;
			//pRet[1]=a<<24|(a&0xff00)<<8|(a>>8&0xff00)|(a>>24&0xff);
			//int b=pVal[1];
			//*pRet=b<<24|(b&0xff00)<<8|(b>>8&0xff00)|(b>>24&0xff);
			//return ret;

			uint a=(uint)value;
			a=a<<24|(a&0xff00)<<8|(a>>8&0xff00)|(a>>24&0xff);
			uint b=(uint)(value>>32);
			return (long)a<<32|b<<24|(b&0xff00)<<8|(b>>8&0xff00)|(b>>24&0xff);
		}

		/// <summary>
		/// Reverses the byte-order of a value.
		/// </summary>
		/// <param name="value">The value which byte-order is to be reversed.</param>
		/// <returns>The byte-order reversed value.</returns>
		[CLSCompliant(false)]
		public static ulong Swap(ulong value)
		{
			// a tiny bit slower and unsafe
			//ulong ret;
			//int* pVal=(int*)&value;
			//int* pRet=(int*)&ret;
			//int a=*pVal;
			//pRet[1]=a<<24|(a&0xff00)<<8|(a>>8&0xff00)|(a>>24&0xff);
			//int b=pVal[1];
			//*pRet=b<<24|(b&0xff00)<<8|(b>>8&0xff00)|(b>>24&0xff);
			//return ret;

			uint a=(uint)value;
			a=a<<24|(a&0xff00)<<8|(a>>8&0xff00)|(a>>24&0xff);
			uint b=(uint)(value>>32);
			return (ulong)a<<32|b<<24|(b&0xff00)<<8|(b>>8&0xff00)|(b>>24&0xff);
		}

		/// <summary>
		/// Reverses the byte-order of a value.
		/// </summary>
		/// <param name="value">The value which byte-order is to be reversed.</param>
		/// <returns>The byte-order reversed value.</returns>
		[CLSCompliant(false)]
		public static UInt128 Swap(UInt128 value)
		{
			return new UInt128(Swap(value.Low), Swap(value.High));
		}

		/// <summary>
		/// Reverses the byte-order of a value.
		/// </summary>
		/// <param name="value">The value which byte-order is to be reversed.</param>
		/// <returns>The byte-order reversed value.</returns>
		public static unsafe float Swap(float value)
		{
			// slower
			//float ret;
			//byte* pVal=(byte*)&value;
			//byte* pRet=(byte*)&ret+3;
			//*pRet--=*pVal++;
			//*pRet--=*pVal++;
			//*pRet--=*pVal++;
			//*pRet=*pVal;
			//return ret;

			float ret;
			int v=*(int*)&value;
			*(int*)&ret=v<<24|(v&0xff00)<<8|(v>>8&0xff00)|(v>>24&0xff);
			return ret;
		}

		/// <summary>
		/// Reverses the byte-order of a value.
		/// </summary>
		/// <param name="value">The value which byte-order is to be reversed.</param>
		/// <returns>The byte-order reversed value.</returns>
		public static unsafe double Swap(double value)
		{
			// slower
			//double ret;
			//byte* pVal=(byte*)&value;
			//byte* pRet=(byte*)&ret+7;
			//*pRet--=*pVal++;
			//*pRet--=*pVal++;
			//*pRet--=*pVal++;
			//*pRet--=*pVal++;
			//*pRet--=*pVal++;
			//*pRet--=*pVal++;
			//*pRet--=*pVal++;
			//*pRet=*pVal;
			//return ret;

			double ret;
			int* pVal=(int*)&value;
			int* pRet=(int*)&ret;
			int a=*pVal;
			pRet[1]=a<<24|(a&0xff00)<<8|(a>>8&0xff00)|(a>>24&0xff);
			int b=pVal[1];
			*pRet=b<<24|(b&0xff00)<<8|(b>>8&0xff00)|(b>>24&0xff);
			return ret;
		}
		#endregion

		#region Swap for arrays of value types
		/// <summary>
		/// Reverses the byte-order of the values of an array.
		/// </summary>
		/// <param name="values">The array of values which byte-order is to be reversed.</param>
		/// <returns>The byte-order reversed values.</returns>
		/// <overloads>These functions reverse the byte-order of the values of an array.</overloads>
		public static unsafe char[] Swap(char[] values)
		{
			if(values==null) return null;
			if(values.Length==0) return new char[0];

			int length=values.Length;
			char[] ret=new char[length];

			fixed(char* pValue=values, pRet=ret)
			{
				byte* dest=(byte*)pRet;
				byte* src=(byte*)pValue;

				for(int i=0; i<length; i++)
				{
					dest[0]=src[1];
					dest[1]=src[0];
					dest+=2;
					src+=2;
				}
			}

			return ret;
		}

		/// <summary>
		/// Reverses the byte-order of the values of an array.
		/// </summary>
		/// <param name="values">The array of values which byte-order is to be reversed.</param>
		/// <returns>The byte-order reversed values.</returns>
		public static unsafe short[] Swap(short[] values)
		{
			if(values==null) return null;
			if(values.Length==0) return new short[0];

			int length=values.Length;
			short[] ret=new short[length];

			fixed(short* pValue=values, pRet=ret)
			{
				byte* dest=(byte*)pRet;
				byte* src=(byte*)pValue;

				for(int i=0; i<length; i++)
				{
					dest[0]=src[1];
					dest[1]=src[0];
					dest+=2;
					src+=2;
				}
			}

			return ret;
		}

		/// <summary>
		/// Reverses the byte-order of the values of an array.
		/// </summary>
		/// <param name="values">The array of values which byte-order is to be reversed.</param>
		/// <returns>The byte-order reversed values.</returns>
		[CLSCompliant(false)]
		public static unsafe ushort[] Swap(ushort[] values)
		{
			if(values==null) return null;
			if(values.Length==0) return new ushort[0];

			int length=values.Length;
			ushort[] ret=new ushort[length];

			fixed(ushort* pValue=values, pRet=ret)
			{
				byte* dest=(byte*)pRet;
				byte* src=(byte*)pValue;

				for(int i=0; i<length; i++)
				{
					dest[0]=src[1];
					dest[1]=src[0];
					dest+=2;
					src+=2;
				}
			}

			return ret;
		}

		/// <summary>
		/// Reverses the byte-order of the values of an array.
		/// </summary>
		/// <param name="values">The array of values which byte-order is to be reversed.</param>
		/// <returns>The byte-order reversed values.</returns>
		public static unsafe int[] Swap(int[] values)
		{
			if(values==null) return null;
			if(values.Length==0) return new int[0];

			int length=values.Length;
			int[] ret=new int[length];

			fixed(int* pValue=values, pRet=ret)
			{
				byte* dest=(byte*)pRet;
				byte* src=(byte*)pValue;

				for(int i=0; i<length; i++)
				{
					dest[0]=src[3];
					dest[1]=src[2];
					dest[2]=src[1];
					dest[3]=src[0];
					dest+=4;
					src+=4;
				}
			}

			return ret;
		}

		/// <summary>
		/// Reverses the byte-order of the values of an array.
		/// </summary>
		/// <param name="values">The array of values which byte-order is to be reversed.</param>
		/// <returns>The byte-order reversed values.</returns>
		[CLSCompliant(false)]
		public static unsafe uint[] Swap(uint[] values)
		{
			if(values==null) return null;
			if(values.Length==0) return new uint[0];

			int length=values.Length;
			uint[] ret=new uint[length];

			fixed(uint* pValue=values, pRet=ret)
			{
				byte* dest=(byte*)pRet;
				byte* src=(byte*)pValue;

				for(int i=0; i<length; i++)
				{
					dest[0]=src[3];
					dest[1]=src[2];
					dest[2]=src[1];
					dest[3]=src[0];
					dest+=4;
					src+=4;
				}
			}

			return ret;
		}

		/// <summary>
		/// Reverses the byte-order of the values of an array.
		/// </summary>
		/// <param name="values">The array of values which byte-order is to be reversed.</param>
		/// <returns>The byte-order reversed values.</returns>
		public static unsafe long[] Swap(long[] values)
		{
			if(values==null) return null;
			if(values.Length==0) return new long[0];

			int length=values.Length;
			long[] ret=new long[length];

			fixed(long* pValue=values, pRet=ret)
			{
				byte* dest=(byte*)pRet;
				byte* src=(byte*)pValue;

				for(int i=0; i<length; i++)
				{
					dest[0]=src[7];
					dest[1]=src[6];
					dest[2]=src[5];
					dest[3]=src[4];
					dest[4]=src[3];
					dest[5]=src[2];
					dest[6]=src[1];
					dest[7]=src[0];
					dest+=8;
					src+=8;
				}
			}

			return ret;
		}

		/// <summary>
		/// Reverses the byte-order of the values of an array.
		/// </summary>
		/// <param name="values">The array of values which byte-order is to be reversed.</param>
		/// <returns>The byte-order reversed values.</returns>
		[CLSCompliant(false)]
		public static unsafe ulong[] Swap(ulong[] values)
		{
			if(values==null) return null;
			if(values.Length==0) return new ulong[0];

			int length=values.Length;
			ulong[] ret=new ulong[length];

			fixed(ulong* pValue=values, pRet=ret)
			{
				byte* dest=(byte*)pRet;
				byte* src=(byte*)pValue;

				for(int i=0; i<length; i++)
				{
					dest[0]=src[7];
					dest[1]=src[6];
					dest[2]=src[5];
					dest[3]=src[4];
					dest[4]=src[3];
					dest[5]=src[2];
					dest[6]=src[1];
					dest[7]=src[0];
					dest+=8;
					src+=8;
				}
			}

			return ret;
		}

		/// <summary>
		/// Reverses the byte-order of the values of an array.
		/// </summary>
		/// <param name="values">The array of values which byte-order is to be reversed.</param>
		/// <returns>The byte-order reversed values.</returns>
		[CLSCompliant(false)]
		public static unsafe UInt128[] Swap(UInt128[] values)
		{
			if(values==null) return null;
			if(values.Length==0) return new UInt128[0];

			int length=values.Length;
			UInt128[] ret=new UInt128[length];

			fixed(UInt128* pValue=values, pRet=ret)
			{
				byte* dest=(byte*)pRet;
				byte* src=(byte*)pValue;

				for(int i=0; i<length; i++)
				{
					dest[0]=src[15];
					dest[1]=src[14];
					dest[2]=src[13];
					dest[3]=src[12];
					dest[4]=src[11];
					dest[5]=src[10];
					dest[6]=src[9];
					dest[7]=src[8];
					dest[8]=src[7];
					dest[9]=src[6];
					dest[10]=src[5];
					dest[11]=src[4];
					dest[12]=src[3];
					dest[13]=src[2];
					dest[14]=src[1];
					dest[15]=src[0];
					dest+=16;
					src+=16;
				}
			}

			return ret;
		}

		/// <summary>
		/// Reverses the byte-order of the values of an array.
		/// </summary>
		/// <param name="values">The array of values which byte-order is to be reversed.</param>
		/// <returns>The byte-order reversed values.</returns>
		public static unsafe float[] Swap(float[] values)
		{
			if(values==null) return null;
			if(values.Length==0) return new float[0];

			int length=values.Length;
			float[] ret=new float[length];

			fixed(float* pValue=values, pRet=ret)
			{
				byte* dest=(byte*)pRet;
				byte* src=(byte*)pValue;

				for(int i=0; i<length; i++)
				{
					dest[0]=src[3];
					dest[1]=src[2];
					dest[2]=src[1];
					dest[3]=src[0];
					dest+=4;
					src+=4;
				}
			}

			return ret;
		}

		/// <summary>
		/// Reverses the byte-order of the values of an array.
		/// </summary>
		/// <param name="values">The array of values which byte-order is to be reversed.</param>
		/// <returns>The byte-order reversed values.</returns>
		public static unsafe double[] Swap(double[] values)
		{
			if(values==null) return null;
			if(values.Length==0) return new double[0];

			int length=values.Length;
			double[] ret=new double[length];

			fixed(double* pValue=values, pRet=ret)
			{
				byte* dest=(byte*)pRet;
				byte* src=(byte*)pValue;

				for(int i=0; i<length; i++)
				{
					dest[0]=src[7];
					dest[1]=src[6];
					dest[2]=src[5];
					dest[3]=src[4];
					dest[4]=src[3];
					dest[5]=src[2];
					dest[6]=src[1];
					dest[7]=src[0];
					dest+=8;
					src+=8;
				}
			}

			return ret;
		}
		#endregion

		/// <summary>
		/// Reverses the order of the bytes of an array.
		/// </summary>
		/// <param name="bytes">The byte array which order is to be reversed.</param>
		/// <returns>The order reversed byte array.</returns>
		public static byte[] Reverse(byte[] bytes)
		{
			byte[] ret=new byte[bytes.Length];
			for(int i=0, j=bytes.Length-1; i<bytes.Length; i++, j--) ret[i]=bytes[j];
			return ret;
		}
	}
}
