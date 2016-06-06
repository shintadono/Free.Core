using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Free.Core.Collections.Generic
{
	/// <summary>
	/// Represents a list of values, that isn't limited by the 2 GigaByte array limit.
	/// </summary>
	/// <typeparam name="T">The type of elements in the list.</typeparam>
	[DebuggerDisplay("Count = {Count}")]
	[CLSCompliant(false)]
	public class LongList<T> : IEnumerable<T>
	{
		List<List<T>> blocks;
		static ulong maximumBlockLength=1<<27;

		/// <summary>
		/// Initializes a new instance of the <see cref="LongList{T}"/> class that is empty.
		/// </summary>
		public LongList()
		{
			blocks=new List<List<T>>();
			ExtendBlocks();
		}

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <returns>The element at the specified index.</returns>
		public T this[ulong index]
		{
			get
			{
				int blockNumber=(int)(index/maximumBlockLength);
				int offset=(int)(index%maximumBlockLength);
				return blocks[blockNumber][offset];
			}
			set
			{
				int blockNumber=(int)(index/maximumBlockLength);
				int offset=(int)(index%maximumBlockLength);
				blocks[blockNumber][offset]=value;
			}
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="LongList{T}"/>.
		/// </summary>
		public ulong Count
		{
			get
			{
				ulong ret=0;
				foreach(List<T> list in blocks) ret+=(ulong)list.Count;
				return ret;
			}
		}

		/// <summary>
		/// Determines whether an element is in the <see cref="LongList{T}"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="LongList{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <returns><b>true</b> if item is found in the <see cref="LongList{T}"/>; otherwise, <b>false</b>.</returns>
		public bool Contains(T item)
		{
			foreach(List<T> block in blocks)
			{
				if(block.Contains(item)) return true;
			}

			return false;
		}

		List<T> LastBlock { get { return blocks[blocks.Count - 1]; } }

		ulong FreeSpaceInLastBlock() { return maximumBlockLength - (ulong)LastBlock.Count; }

		void ExtendBlocks() { blocks.Add(new List<T>()); }

		/// <summary>
		/// Adds the specified element to a <see cref="LongList{T}"/>.
		/// </summary>
		/// <param name="item">The element to add to the <see cref="LongList{T}"/> object.</param>
		public void Add(T item)
		{
			if(FreeSpaceInLastBlock()==0) ExtendBlocks();
			LastBlock.Add(item);
		}

		/// <summary>
		/// Removes all elements from a <see cref="LongList{T}"/>.
		/// </summary>
		public void Clear()
		{
			blocks.Clear();
			ExtendBlocks();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			foreach (List<T> block in blocks)
			{
				foreach(T x in block) yield return x;
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through a <see cref="LongList{T}"/> object.
		/// </summary>
		/// <returns>A <see cref="IEnumerator{T}"/> for the <see cref="LongList{T}"/> object.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			foreach(List<T> block in blocks)
			{
				foreach(T x in block) yield return x;
			}
		}
	}
}
