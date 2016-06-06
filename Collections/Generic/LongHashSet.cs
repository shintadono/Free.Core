using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Free.Core.Collections.Generic
{
	/// <summary>
	/// Represents a set of values, that isn't limited by the 2 GigaByte array limit.
	/// </summary>
	/// <typeparam name="T">The type of elements in the hash set.</typeparam>
	[DebuggerDisplay("Count = {Count}")]
	[CLSCompliant(false)]
	public class LongHashSet<T> : IEnumerable<T>
	{
		List<HashSet<T>> blocks;
		static ulong maximumBlockLength=1<<26;

		/// <summary>
		/// Initializes a new instance of the <see cref="LongHashSet{T}"/> class that is empty.
		/// </summary>
		public LongHashSet()
		{
			blocks=new List<HashSet<T>>();
			ExtendBlocks();
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="LongHashSet{T}"/>.
		/// </summary>
		public ulong Count
		{
			get
			{
				ulong ret = 0;
				foreach (HashSet<T> list in blocks) ret += (ulong)list.Count;
				return ret;
			}
		}

		/// <summary>
		/// Determines whether a <see cref="LongHashSet{T}"/> object contains the specified element.
		/// </summary>
		/// <param name="item">The element to locate in the <see cref="LongHashSet{T}"/> object.</param>
		/// <returns><b>true</b> if the <see cref="LongHashSet{T}"/> object contains the specified element; otherwise, <b>false</b>.</returns>
		public bool Contains(T item)
		{
			for(int i=0; i<blocks.Count; i++)
			{
				if(blocks[i].Contains(item)) return true;
			}

			return false;
		}

		HashSet<T> LastBlock { get { return blocks[blocks.Count - 1]; } }

		ulong FreeSpaceInLastBlock() { return maximumBlockLength - (ulong)LastBlock.Count; }

		void ExtendBlocks() { blocks.Add(new HashSet<T>()); }

		/// <summary>
		/// Adds the specified element to a <see cref="LongHashSet{T}"/>.
		/// </summary>
		/// <param name="item">The element to add to the <see cref="LongHashSet{T}"/> object.</param>
		/// <returns><b>true</b> if the element is added to the <see cref="LongHashSet{T}"/> object; <b>false</b> if the element is already present.</returns>
		public bool Add(T item)
		{
			for (int i = 0; i < blocks.Count - 1; i++)
			{
				if (blocks[i].Contains(item)) return true;
			}

			if (FreeSpaceInLastBlock() == 0)
			{
				if (blocks[blocks.Count - 1].Contains(item)) return true;
				ExtendBlocks();
			}

			return LastBlock.Add(item);
		}

		/// <summary>
		/// Removes all elements from a <see cref="LongHashSet{T}"/>.
		/// </summary>
		public void Clear()
		{
			blocks.Clear();
			ExtendBlocks();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			foreach(HashSet<T> block in blocks)
			{
				foreach(T x in block) yield return x;
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through a <see cref="LongHashSet{T}"/> object.
		/// </summary>
		/// <returns>A <see cref="IEnumerator{T}"/> for the <see cref="LongHashSet{T}"/> object.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			foreach(HashSet<T> block in blocks)
			{
				foreach(T x in block) yield return x;
			}
		}
	}
}
