using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Free.Core.Collections
{
	/// <summary>
	/// TODO
	/// </summary>
	[CLSCompliant(false)]
	public class Bitfield : ICollection, IEnumerable, IEnumerable<bool>
	{
		uint length;
		uint[] bitfield;

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="bits"></param>
		public Bitfield(Bitfield bits)
		{
			if(bits==null) throw new ArgumentNullException("bits");
			this.length=bits.length;
			bitfield=new uint[bits.bitfield.Length];
			Array.Copy(bits.bitfield, bitfield, bitfield.Length);
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="length"></param>
		/// <param name="defaultValue"></param>
		public Bitfield(uint length, bool defaultValue=false)
		{
			this.length=length;
			int uints=(int)(length+31)/32;
			bitfield=new uint[uints];

			if(defaultValue)
			{
				for(int i=0; i<uints; i++) bitfield[i]=uint.MaxValue;
			}
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="values"></param>
		public Bitfield(List<bool> values)
		{
			if(values==null) throw new ArgumentNullException("values");
			length=(uint)values.Count;
			int uints=(int)(length+31)/32;
			bitfield=new uint[uints];

			for(int i=0; i<length; i++) if(values[i]) Set(i);
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="values"></param>
		public Bitfield(bool[] values)
		{
			if(values==null) throw new ArgumentNullException("values");
			length=(uint)values.Length;
			int uints=(int)(length+31)/32;
			bitfield=new uint[uints];

			for(int i=0; i<length; i++) if(values[i]) Set(i);
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="bit"></param>
		/// <returns></returns>
		public bool Get(int bit)
		{
			if(bit<0||bit>=length) throw new ArgumentOutOfRangeException("bit", "Must be equal or greater than 0 and less than Length.");
			return (bitfield[bit/32]&(1u<<(bit%32)))!=0;
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="bit"></param>
		/// <returns></returns>
		public bool Get(uint bit)
		{
			if(bit>=length) throw new ArgumentOutOfRangeException("bit", "Must be less than Length.");
			return (bitfield[(int)(bit/32)]&(1u<<((int)(bit%32))))!=0;
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="bit"></param>
		public void Set(int bit)
		{
			if(bit<0||bit>=length) throw new ArgumentOutOfRangeException("bit", "Must be equal or greater than 0 and less than Length.");
			bitfield[bit/32]|=1u<<(bit%32);
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="bit"></param>
		public void Set(uint bit)
		{
			if(bit>=length) throw new ArgumentOutOfRangeException("bit", "Must be less than Length.");
			bitfield[(int)(bit/32)]|=1u<<((int)(bit%32));
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public void Set(int from, int to)
		{
			if(from<0||from>=length) throw new ArgumentOutOfRangeException("from", "Must be equal or greater than 0 and less than Length.");
			if(to<0||to>=length) throw new ArgumentOutOfRangeException("to", "Must be equal or greater than 0 and less than Length.");
			Set((uint)from, (uint)to);
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public void Set(uint from, uint to)
		{
			if(from>=length) throw new ArgumentOutOfRangeException("from", "Must be less than Length.");
			if(to>=length) throw new ArgumentOutOfRangeException("to", "Must be less than Length.");

			if(from==to) { Set(from); return; }

			int uintPos=(int)(from/32);
			int bit=(int)(from%32);
			uint count=to-from+1;

			if(bit!=0)
			{
				uint update=0;
				for(int i=bit; i<32&&count>0; i++, count--) update|=1u<<i;
				bitfield[uintPos]|=update;
				uintPos++;
				bit=0;
			}

			while(count>=32)
			{
				bitfield[uintPos++]=uint.MaxValue;
				count-=32;
			}

			if(count!=0)
			{
				uint update=0;
				for(int i=0; i<32&&count>0; i++, count--) update|=1u<<i;
				bitfield[uintPos]|=update;
			}
		}

		/// <summary>
		/// TODO
		/// </summary>
		public void SetAll()
		{
			for(int i=bitfield.Length-1; i>=0; i--) bitfield[i]=uint.MaxValue;
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="bit"></param>
		public void Reset(int bit)
		{
			if(bit<0||bit>=length) throw new ArgumentOutOfRangeException("bit", "Must be equal or greater than 0 and less than Length.");
			bitfield[bit/32]&=~(1u<<(bit%32));
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="bit"></param>
		public void Reset(uint bit)
		{
			if(bit>=length) throw new ArgumentOutOfRangeException("bit", "Must be less than Length.");
			bitfield[(int)(bit/32)]&=~(1u<<((int)(bit%32)));
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public void Reset(int from, int to)
		{
			if(from<0||from>=length) throw new ArgumentOutOfRangeException("from", "Must be equal or greater than 0 and less than Length.");
			if(to<0||to>=length) throw new ArgumentOutOfRangeException("to", "Must be equal or greater than 0 and less than Length.");
			Reset((uint)from, (uint)to);
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public void Reset(uint from, uint to)
		{
			if(from>=length) throw new ArgumentOutOfRangeException("from", "Must be less than Length.");
			if(to>=length) throw new ArgumentOutOfRangeException("to", "Must be less than Length.");

			if(from==to) { Reset(from); return; }

			int uintPos=(int)(from/32);
			int bit=(int)(from%32);
			uint count=to-from+1;

			if(bit!=0)
			{
				uint update=0;
				for(int i=bit; i<32&&count>0; i++, count--) update|=1u<<i;
				bitfield[uintPos]&=~update;
				uintPos++;
				bit=0;
			}

			while(count>=32)
			{
				bitfield[uintPos++]=0;
				count-=32;
			}

			if(count!=0)
			{
				uint update=0;
				for(int i=0; i<32&&count>0; i++, count--) update|=1u<<i;
				bitfield[uintPos]&=~update;
			}
		}

		/// <summary>
		/// TODO
		/// </summary>
		public void ResetAll()
		{
			for(int i=bitfield.Length-1; i>=0; i--) bitfield[i]=0;
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public bool this[int index]
		{
			get { return Get(index); }
			set { if(value) Set(index); else Reset(index); }
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public bool this[uint index]
		{
			get { return Get(index); }
			set { if(value) Set(index); else Reset(index); }
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="index"></param>
		public void CopyTo(Array destination, int index)
		{
			if(destination==null) throw new ArgumentNullException("destination");
			if(index<0) throw new ArgumentOutOfRangeException("index", "Must be not negative.");
			if(destination.Rank!=1) throw new ArgumentException("Must be a 1-dimensional array", "destination");

			bool[] dst=destination as bool[];
			if(dst==null) throw new NotSupportedException("Only bool[] supported.");

			if((long)dst.Length-index<length) throw new ArgumentException("Array destination (starting at index) is not long enough.");

			for(int i=0; i<length; i++) dst[index+i]=(bitfield[i/32]&(1u<<(i%32)))!=0;
		}

		/// <summary>
		/// TODO
		/// </summary>
		public int Count
		{
			get { return (int)length; }
		}

		/// <summary>
		/// TODO
		/// </summary>
		public bool IsSynchronized { get { return false; } }

		object syncRoot;

		/// <summary>
		/// TODO
		/// </summary>
		public object SyncRoot
		{
			get
			{
				if(syncRoot==null) Interlocked.CompareExchange(ref syncRoot, new object(), null);
				return syncRoot;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new BitfieldEnumerator(this);
		}

		IEnumerator<bool> IEnumerable<bool>.GetEnumerator()
		{
			return new BitfieldEnumerator(this);
		}

		/// <summary>
		/// TODO
		/// </summary>
		public BitCollection SetBits { get { return new BitCollection(this, true); } }

		/// <summary>
		/// TODO
		/// </summary>
		public BitCollection NotSetBits { get { return new BitCollection(this, false); } }

		class BitfieldEnumerator : IEnumerator, IEnumerator<bool>
		{
			Bitfield bits;
			bool cur;
			long index;

			public BitfieldEnumerator(Bitfield bits)
			{
				this.bits=bits;
				index=-1;
			}

			public virtual bool MoveNext()
			{
				if(index<bits.length-1)
				{
					index++;
					cur=bits.Get((uint)index);
					return true;
				}

				index=bits.Count;
				return false;
			}

			public void Reset()
			{
				index=-1;
			}

			object IEnumerator.Current
			{
				get
				{
					if(index==-1) throw new InvalidOperationException("Enumeration not started yet.");
					if(index>=bits.Count) throw new InvalidOperationException("Enumeration has ended");
					return cur;
				}
			}

			bool IEnumerator<bool>.Current
			{
				get
				{
					if(index==-1) throw new InvalidOperationException("Enumeration not started yet.");
					if(index>=bits.Count) throw new InvalidOperationException("Enumeration has ended");
					return cur;
				}
			}

			public void Dispose() { }
		}

		/// <summary>
		/// TODO
		/// </summary>
		public class BitCollection : IEnumerable<uint>
		{
			Bitfield bits;
			bool value;

			/// <summary>
			/// TODO
			/// </summary>
			/// <param name="bits"></param>
			/// <param name="value"></param>
			public BitCollection(Bitfield bits, bool value)
			{
				this.bits=bits;
				this.value=value;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new BitfieldSetEnumerator(bits, value);
			}

			IEnumerator<uint> IEnumerable<uint>.GetEnumerator()
			{
				return new BitfieldSetEnumerator(bits, value);
			}
		}

		class BitfieldSetEnumerator : IEnumerator<uint>
		{
			Bitfield bits;
			long index;
			bool value=true;

			public BitfieldSetEnumerator(Bitfield bits, bool value=true)
			{
				this.bits=bits;
				index=-1;
				this.value=value;
			}

			public virtual bool MoveNext()
			{
				while(index<bits.length-1)
				{
					index++;
					if(bits.Get((uint)index)==value) return true;
				}

				index=bits.Count;
				return false;
			}

			public void Reset()
			{
				index=-1;
			}

			public virtual uint Current
			{
				get
				{
					if(index==-1) throw new InvalidOperationException("Enumeration not started yet.");
					if(index>=bits.Count) throw new InvalidOperationException("Enumeration has ended");
					return (uint)index;
				}
			}

			public void Dispose() { }

			object IEnumerator.Current
			{
				get
				{
					if(index==-1) throw new InvalidOperationException("Enumeration not started yet.");
					if(index>=bits.Count) throw new InvalidOperationException("Enumeration has ended");
					return (uint)index;
				}
			}
		}
	}
}
