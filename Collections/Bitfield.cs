using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Free.Core.Collections
{
	/// <summary>
	/// Describes an object the holds a field of bits.
	/// </summary>
	[CLSCompliant(false)]
	public class Bitfield : ICollection, IEnumerable, IEnumerable<bool>
	{
		uint length;
		uint[] bitfield;

		/// <summary>
		/// Initializes an instance by copying from a given <see cref="Bitfield"/>.
		/// </summary>
		/// <param name="bits">The <see cref="Bitfield"/> to initalize with.</param>
		public Bitfield(Bitfield bits)
		{
			if (bits == null) throw new ArgumentNullException(nameof(bits));
			this.length = bits.length;
			bitfield = new uint[bits.bitfield.Length];
			Array.Copy(bits.bitfield, bitfield, bitfield.Length);
		}

		/// <summary>
		/// Initialzes an instance with a given length and default values.
		/// </summary>
		/// <param name="length">The number of bits in the <see cref="Bitfield"/>.</param>
		/// <param name="defaultValue">The value to set the bits with.</param>
		public Bitfield(uint length, bool defaultValue = false)
		{
			this.length = length;
			int uints = (int)(length + 31) / 32;
			bitfield = new uint[uints];

			if (defaultValue)
			{
				for (int i = 0; i < uints; i++) bitfield[i] = uint.MaxValue;
			}
		}

		/// <summary>
		/// Initialzes an instance with a list for boolean values.
		/// </summary>
		/// <param name="values">The list of values to initialize with.</param>
		public Bitfield(List<bool> values)
		{
			if (values == null) throw new ArgumentNullException(nameof(values));
			length = (uint)values.Count;
			int uints = (int)(length + 31) / 32;
			bitfield = new uint[uints];

			for (int i = 0; i < length; i++) if (values[i]) Set(i);
		}

		/// <summary>
		/// Initialzes an instance with an array for boolean values.
		/// </summary>
		/// <param name="values">The array of values to initialize with.</param>
		public Bitfield(bool[] values)
		{
			if (values == null) throw new ArgumentNullException(nameof(values));
			length = (uint)values.Length;
			int uints = (int)(length + 31) / 32;
			bitfield = new uint[uints];

			for (int i = 0; i < length; i++) if (values[i]) Set(i);
		}

		/// <summary>
		/// Gets the current value of the specified bit.
		/// </summary>
		/// <param name="bit">The index of the bit.</param>
		/// <returns>The value of the bit.</returns>
		public bool Get(int bit)
		{
			if (bit < 0 || bit >= length) throw new ArgumentOutOfRangeException(nameof(bit), "Must be equal or greater than 0 and less than Length.");
			return (bitfield[bit / 32] & (1u << (bit % 32))) != 0;
		}

		/// <summary>
		/// Gets the current value of the specified bit.
		/// </summary>
		/// <param name="bit">The index of the bit.</param>
		/// <returns>The value of the bit.</returns>
		public bool Get(uint bit)
		{
			if (bit >= length) throw new ArgumentOutOfRangeException(nameof(bit), "Must be less than Length.");
			return (bitfield[(int)(bit / 32)] & (1u << ((int)(bit % 32)))) != 0;
		}

		/// <summary>
		/// Sets the specified bit.
		/// </summary>
		/// <param name="bit">The index of the bit.</param>
		public void Set(int bit)
		{
			if (bit < 0 || bit >= length) throw new ArgumentOutOfRangeException(nameof(bit), "Must be equal or greater than 0 and less than Length.");
			bitfield[bit / 32] |= 1u << (bit % 32);
		}

		/// <summary>
		/// Sets the specified bit.
		/// </summary>
		/// <param name="bit">The index of the bit.</param>
		public void Set(uint bit)
		{
			if (bit >= length) throw new ArgumentOutOfRangeException(nameof(bit), "Must be less than Length.");
			bitfield[(int)(bit / 32)] |= 1u << ((int)(bit % 32));
		}

		/// <summary>
		/// Sets the specified bits.
		/// </summary>
		/// <param name="from">The index of the first bit.</param>
		/// <param name="to">The index of the last bit.</param>
		public void Set(int from, int to)
		{
			if (from < 0 || from >= length) throw new ArgumentOutOfRangeException(nameof(from), "Must be equal or greater than 0 and less than Length.");
			if (to < 0 || to >= length) throw new ArgumentOutOfRangeException(nameof(to), "Must be equal or greater than 0 and less than Length.");
			Set((uint)from, (uint)to);
		}

		/// <summary>
		/// Sets the specified bits.
		/// </summary>
		/// <param name="from">The index of the first bit.</param>
		/// <param name="to">The index of the last bit.</param>
		public void Set(uint from, uint to)
		{
			if (from >= length) throw new ArgumentOutOfRangeException(nameof(from), "Must be less than Length.");
			if (to >= length) throw new ArgumentOutOfRangeException(nameof(to), "Must be less than Length.");

			if (from == to) { Set(from); return; }

			int uintPos = (int)(from / 32);
			int bit = (int)(from % 32);
			uint count = to - from + 1;

			if (bit != 0)
			{
				uint update = 0;
				for (int i = bit; i < 32 && count > 0; i++, count--) update |= 1u << i;
				bitfield[uintPos] |= update;
				uintPos++;
				bit = 0;
			}

			while (count >= 32)
			{
				bitfield[uintPos++] = uint.MaxValue;
				count -= 32;
			}

			if (count != 0)
			{
				uint update = 0;
				for (int i = 0; i < 32 && count > 0; i++, count--) update |= 1u << i;
				bitfield[uintPos] |= update;
			}
		}

		/// <summary>
		/// Sets all bits in the <see cref="Bitfield"/>.
		/// </summary>
		public void SetAll()
		{
			for (int i = bitfield.Length - 1; i >= 0; i--) bitfield[i] = uint.MaxValue;
		}

		/// <summary>
		/// Resets the specified bit.
		/// </summary>
		/// <param name="bit">The index of the bit.</param>
		public void Reset(int bit)
		{
			if (bit < 0 || bit >= length) throw new ArgumentOutOfRangeException(nameof(bit), "Must be equal or greater than 0 and less than Length.");
			bitfield[bit / 32] &= ~(1u << (bit % 32));
		}

		/// <summary>
		/// Resets the specified bit.
		/// </summary>
		/// <param name="bit">The index of the bit.</param>
		public void Reset(uint bit)
		{
			if (bit >= length) throw new ArgumentOutOfRangeException(nameof(bit), "Must be less than Length.");
			bitfield[(int)(bit / 32)] &= ~(1u << ((int)(bit % 32)));
		}

		/// <summary>
		/// Resets the specified bits.
		/// </summary>
		/// <param name="from">The index of the first bit.</param>
		/// <param name="to">The index of the last bit.</param>
		public void Reset(int from, int to)
		{
			if (from < 0 || from >= length) throw new ArgumentOutOfRangeException(nameof(from), "Must be equal or greater than 0 and less than Length.");
			if (to < 0 || to >= length) throw new ArgumentOutOfRangeException(nameof(to), "Must be equal or greater than 0 and less than Length.");
			Reset((uint)from, (uint)to);
		}

		/// <summary>
		/// Resets the specified bits.
		/// </summary>
		/// <param name="from">The index of the first bit.</param>
		/// <param name="to">The index of the last bit.</param>
		public void Reset(uint from, uint to)
		{
			if (from >= length) throw new ArgumentOutOfRangeException(nameof(from), "Must be less than Length.");
			if (to >= length) throw new ArgumentOutOfRangeException(nameof(to), "Must be less than Length.");

			if (from == to) { Reset(from); return; }

			int uintPos = (int)(from / 32);
			int bit = (int)(from % 32);
			uint count = to - from + 1;

			if (bit != 0)
			{
				uint update = 0;
				for (int i = bit; i < 32 && count > 0; i++, count--) update |= 1u << i;
				bitfield[uintPos] &= ~update;
				uintPos++;
				bit = 0;
			}

			while (count >= 32)
			{
				bitfield[uintPos++] = 0;
				count -= 32;
			}

			if (count != 0)
			{
				uint update = 0;
				for (int i = 0; i < 32 && count > 0; i++, count--) update |= 1u << i;
				bitfield[uintPos] &= ~update;
			}
		}

		/// <summary>
		/// Resets all bits in the <see cref="Bitfield"/>.
		/// </summary>
		public void ResetAll()
		{
			for (int i = bitfield.Length - 1; i >= 0; i--) bitfield[i] = 0;
		}

		/// <summary>
		/// Get and set the value of the bit at the specified position.
		/// </summary>
		/// <param name="index">The index of the bit.</param>
		/// <returns>The value of the bit at the specified position.</returns>
		public bool this[int index]
		{
			get { return Get(index); }
			set { if (value) Set(index); else Reset(index); }
		}

		/// <summary>
		/// Get and set the value of the bit at the specified position.
		/// </summary>
		/// <param name="index">The index of the bit.</param>
		/// <returns>The value of the bit at the specified position.</returns>
		public bool this[uint index]
		{
			get { return Get(index); }
			set { if (value) Set(index); else Reset(index); }
		}

		/// <summary>
		/// Copies the entire <see cref="Bitfield"/> to a one-dimensional array of <b>bool</b>, starting at the specified index of the target array.
		/// </summary>
		/// <param name="destination">The array to copy to.</param>
		/// <param name="index">The index in the <paramref name="destination"/> array at which copying begins.</param>
		public void CopyTo(Array destination, int index)
		{
			if (destination == null) throw new ArgumentNullException(nameof(destination));
			if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "Must not be less than zero (0).");
			if (destination.Rank != 1) throw new ArgumentException("Must be a 1-dimensional array.", nameof(destination));

			bool[] dst = destination as bool[];
			if (dst == null) throw new NotSupportedException("Only bool[] supported.");

			if ((long)dst.Length - index < length) throw new ArgumentException("Array destination (starting at index) is not long enough.");

			for (int i = 0; i < length; i++) dst[index + i] = (bitfield[i / 32] & (1u << (i % 32))) != 0;
		}

		/// <summary>
		/// Gets the number of bits in the <see cref="Bitfield"/>.
		/// </summary>
		public int Count
		{
			get { return (int)length; }
		}

		/// <summary>
		/// Gets a value indicating whether access to the <see cref="Bitfield"/> is synchronized (thread safe).
		/// </summary>
		public bool IsSynchronized { get { return false; } }

		object syncRoot;

		/// <summary>
		/// Gets an object that can be used to synchronize access to the <see cref="Bitfield"/>.
		/// </summary>
		public object SyncRoot
		{
			get
			{
				if (syncRoot == null) Interlocked.CompareExchange(ref syncRoot, new object(), null);
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
		/// Gets a list of all bits, that are set.
		/// </summary>
		public IEnumerable<uint> SetBits { get { return new BitCollection(this, true); } }

		/// <summary>
		/// Gets a list of all bits, that are not set.
		/// </summary>
		public IEnumerable<uint> NotSetBits { get { return new BitCollection(this, false); } }

		class BitfieldEnumerator : IEnumerator, IEnumerator<bool>
		{
			Bitfield bits;
			bool cur;
			long index;

			public BitfieldEnumerator(Bitfield bits)
			{
				this.bits = bits;
				index = -1;
			}

			public virtual bool MoveNext()
			{
				if (index < bits.length - 1)
				{
					index++;
					cur = bits.Get((uint)index);
					return true;
				}

				index = bits.Count;
				return false;
			}

			public void Reset()
			{
				index = -1;
			}

			object IEnumerator.Current
			{
				get
				{
					if (index == -1) throw new InvalidOperationException("Enumeration not started yet.");
					if (index >= bits.Count) throw new InvalidOperationException("Enumeration has ended");
					return cur;
				}
			}

			bool IEnumerator<bool>.Current
			{
				get
				{
					if (index == -1) throw new InvalidOperationException("Enumeration not started yet.");
					if (index >= bits.Count) throw new InvalidOperationException("Enumeration has ended");
					return cur;
				}
			}

			public void Dispose() { }
		}

		class BitCollection : IEnumerable<uint>
		{
			Bitfield bits;
			bool value;

			public BitCollection(Bitfield bits, bool value)
			{
				this.bits = bits;
				this.value = value;
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
			bool value = true;

			public BitfieldSetEnumerator(Bitfield bits, bool value = true)
			{
				this.bits = bits;
				index = -1;
				this.value = value;
			}

			public virtual bool MoveNext()
			{
				while (index < bits.length - 1)
				{
					index++;
					if (bits.Get((uint)index) == value) return true;
				}

				index = bits.Count;
				return false;
			}

			public void Reset()
			{
				index = -1;
			}

			public virtual uint Current
			{
				get
				{
					if (index == -1) throw new InvalidOperationException("Enumeration not started yet.");
					if (index >= bits.Count) throw new InvalidOperationException("Enumeration has ended");
					return (uint)index;
				}
			}

			public void Dispose() { }

			object IEnumerator.Current
			{
				get
				{
					if (index == -1) throw new InvalidOperationException("Enumeration not started yet.");
					if (index >= bits.Count) throw new InvalidOperationException("Enumeration has ended");
					return (uint)index;
				}
			}
		}
	}
}
