using System.Collections.Generic;

namespace Free.Core.Collections.Generic
{
	/// <summary>
	/// TODO
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class LongHashSet<T> : IEnumerable<T>
	{
		private List<HashSet<T>> blocks;
		private static ulong maximumBlockLength=1<<26;

		/// <summary>
		/// TODO
		/// </summary>
		public LongHashSet()
		{
			blocks=new List<HashSet<T>>();
			ExtendBlocks();
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public bool Contains(T val)
		{
			foreach(HashSet<T> block in blocks)
			{
				if(block.Contains(val)) return true;
			}

			return false;
		}

		private HashSet<T> LastBlock
		{
			get { return blocks[blocks.Count-1]; }
		}

		private ulong FreeSpaceInLastBlock()
		{
			return maximumBlockLength-(ulong)LastBlock.Count;
		}

		private void ExtendBlocks()
		{
			blocks.Add(new HashSet<T>());
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
			foreach(HashSet<T> block in blocks)
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
			foreach(HashSet<T> block in blocks)
			{
				foreach(T x in block)
				{
					yield return x;
				}
			}
		}
	}
}
