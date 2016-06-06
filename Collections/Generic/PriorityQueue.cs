using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Free.Core.Collections.Generic
{
	/// <summary>
	/// Represents a list of values, with an order specified by an implementation of <see cref="IComparer{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of elements in the priority queue.</typeparam>
	[DebuggerDisplay("Count = {Count}")]
	public class PriorityQueue<T>
	{
		List<T> entries;
		IComparer<T> comparer;

		/// <summary>
		/// Initializes a new instance of the <see cref="PriorityQueue{T}"/> class that is empty.
		/// </summary>
		/// <param name="comparer">The implementation of <see cref="IComparer{T}"/> to use for the sorting of the <see cref="PriorityQueue{T}"/>.
		/// Can be <b>null</b>; in this case the default comparer of <typeparamref name="T"/> is used (see <see cref="Comparer{T}.Default"/>).</param>
		public PriorityQueue(IComparer<T> comparer = null)
		{
			entries = new List<T>();

			if (comparer == null) comparer = Comparer<T>.Default;
			this.comparer = comparer;
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="PriorityQueue{T}"/>.
		/// </summary>
		public int Count { get { return entries.Count; } }

		/// <summary>
		/// Indicates whether the <see cref="PriorityQueue{T}"/> is empty.
		/// </summary>
		public bool IsEmpty { get { return entries.Count == 0; } }

		/// <summary>
		/// Removes all elements from a <see cref="PriorityQueue{T}"/>.
		/// </summary>
		public void Clear() { entries.Clear(); }

		/// <summary>
		/// Adds an element to an instance of <see cref="PriorityQueue{T}"/>.
		/// </summary>
		/// <param name="item">The element to be added to the <see cref="PriorityQueue{T}"/>.</param>
		public void Add(T item)
		{
			if (item == null) throw new ArgumentNullException(nameof(item));

			entries.Add(item);
			if (entries.Count <= 1) return;

			int emptyslot = entries.Count - 1;
			for (int i = (emptyslot - 1) / 2; emptyslot > 0 && comparer.Compare(entries[i], item) == 1; i = (emptyslot - 1) / 2)
			{
				entries[emptyslot] = entries[i];
				emptyslot = i;
			}

			entries[emptyslot] = item;
		}

		/// <summary>
		/// Gets the top element of the <see cref="PriorityQueue{T}"/>.
		/// </summary>
		/// <returns>The top element of the queue, or if queue is empty the default of the type <typeparamref name="T"/>.</returns>
		public T Top
		{
			get
			{
				if (entries.Count == 0) return default(T);
				return entries[0];
			}
		}

		/// <summary>
		/// Gets and removes the top element of the <see cref="PriorityQueue{T}"/>.
		/// </summary>
		/// <returns>The top element of the queue, or if queue is empty the default of the type <typeparamref name="T"/>.</returns>
		public T Pop()
		{
			T top = Top;

			if (entries.Count > 1)
			{
				// Save last slot's entry.
				int lastslot = entries.Count - 1;
				T entry = entries[lastslot];

				// Move top entry to last slot.
				entries[lastslot] = entries[0];

				int emptyslot = 0;
				int i = 2 * emptyslot + 2;

				// Move empty slot down the list.
				for (; i < lastslot; i = 2 * i + 2)
				{
					if (comparer.Compare(entries[i], entries[i - 1]) == 1) i--;
					entries[emptyslot] = entries[i];
					emptyslot = i;
				}

				if (i == lastslot)
				{
					entries[emptyslot] = entries[lastslot - 1];
					emptyslot = lastslot - 1;
				}

				// Re-add saved entry.
				for (i = (emptyslot - 1) / 2; emptyslot > 0 && comparer.Compare(entries[i], entry) == 1; i = (emptyslot - 1) / 2)
				{
					entries[emptyslot] = entries[i];
					emptyslot = i;
				}

				entries[emptyslot] = entry;
			}

			if (entries.Count != 0) entries.RemoveAt(entries.Count - 1);

			return top;
		}
	}
}
