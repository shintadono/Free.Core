using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Free.Core.Collections.Generic
{
	/// <summary>
	/// TODO
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[DebuggerDisplay("Count = {Count}")]
	[CLSCompliant(false)]
	public class LongList<T> : IEnumerable<T>
	{
		private List<List<T>> blocks;
		private static ulong maximumBlockLength=1<<27;

		/// <summary>
		/// TODO
		/// </summary>
		public LongList()
		{
			blocks=new List<List<T>>();
			ExtendBlocks();
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
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
		/// TODO
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
		/// TODO
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public bool Contains(T val)
		{
			foreach(List<T> block in blocks)
			{
				if(block.Contains(val)) return true;
			}

			return false;
		}

		private List<T> LastBlock
		{
			get { return blocks[blocks.Count-1]; }
		}

		private ulong FreeSpaceInLastBlock()
		{
			return maximumBlockLength-(ulong)LastBlock.Count;
		}

		private void ExtendBlocks()
		{
			blocks.Add(new List<T>());
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="x"></param>
		public void Add(T x)
		{
			if(FreeSpaceInLastBlock()==0) ExtendBlocks();
			LastBlock.Add(x);
		}

		/// <summary>
		/// TODO
		/// </summary>
		public void Clear()
		{
			blocks.Clear();
			ExtendBlocks();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			foreach(List<T> block in blocks)
			{
				foreach(T x in block)
				{
					yield return x;
				}
			}
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <returns></returns>
		public IEnumerator<T> GetEnumerator()
		{
			foreach(List<T> block in blocks)
			{
				foreach(T x in block)
				{
					yield return x;
				}
			}
		}
	}
}
