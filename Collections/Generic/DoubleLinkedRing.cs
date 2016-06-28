using System;
using System.Collections;
using System.Collections.Generic;

namespace Free.Core.Collections.Generic
{
	/// <summary>
	/// Represents a double-linked-ring of values.
	/// </summary>
	/// <typeparam name="T">The type of the values of the ring.</typeparam>
	public class DoubleLinkedRing<T> : ICollection<T>, IReadOnlyCollection<T>
	{
		const string ExpectionMessageArrayPlusOffsetTooSmall = "Array (plus offset) too small.";

		/// <summary>
		/// The start of the ring.
		/// </summary>
		public DoubleLinkedRingNode<T> Start;

		/// <summary>
		/// Returns the number of elements in the <see cref="DoubleLinkedRing{T}"/>.
		/// </summary>
		public int Count
		{
			get
			{
				DoubleLinkedRingNode<T> cur = Start;
				if (cur == null) return 0;

				int ret = 0;
				do
				{
					ret++;
					cur = cur.Next;
				}
				while (cur != null && cur != Start);

				return ret;
			}
		}

		/// <summary>
		/// Adds a value to a <see cref="DoubleLinkedRing{T}"/> after the specified node.
		/// </summary>
		/// <param name="after">The node after which to add the value.</param>
		/// <param name="item">The value to add.</param>
		/// <returns>The node in the double-linked-ring of the value.</returns>
		public DoubleLinkedRingNode<T> AddAfter(DoubleLinkedRingNode<T> after, T item)
		{
			DoubleLinkedRingNode<T> node = new DoubleLinkedRingNode<T>(item);
			node.Previous = after;
			node.Next = after.Next;
			node.Previous.Next = node;
			node.Next.Previous = node;
			return node;
		}

		/// <summary>
		/// Appends a value to a <see cref="DoubleLinkedRing{T}"/>.
		/// </summary>
		/// <param name="item">The value to append.</param>
		/// <returns>The node in the double-linked-ring of the value.</returns>
		public DoubleLinkedRingNode<T> Append(T item)
		{
			if (Start == null)
			{
				Start = new DoubleLinkedRingNode<T>(item);
				Start.Next = Start;
				Start.Previous = Start;

				return Start;
			}

			DoubleLinkedRingNode<T> toAdd = new DoubleLinkedRingNode<T>(item);

			toAdd.Previous = Start.Previous;
			toAdd.Next = Start;
			Start.Previous.Next = toAdd;
			Start.Previous = toAdd;

			return toAdd;
		}

		/// <summary>
		/// Removes all values from a <see cref="DoubleLinkedRing{T}"/>.
		/// </summary>
		public void Clear()
		{
			Start = null;
		}

		/// <summary>
		/// Determines whether a value is in the <see cref="DoubleLinkedRing{T}"/>.
		/// </summary>
		/// <param name="item">The value to locate in the <see cref="DoubleLinkedRing{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <returns><b>true</b> if <paramref name="item"/> is found in the <see cref="DoubleLinkedRing{T}"/>; otherwise, <b>false</b>.</returns>
		public bool Contains(T item)
		{
			DoubleLinkedRingNode<T> node = Find(item);
			return node != null;
		}

		/// <summary>
		/// Finds the first node that contains the specified value.
		/// </summary>
		/// <param name="item">The value to locate in the <see cref="DoubleLinkedRing{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <returns>The first <see cref="DoubleLinkedRingNode{T}"/> that contains the specified value, if found; otherwise, <b>null</b>.</returns>
		public DoubleLinkedRingNode<T> Find(T item)
		{
			DoubleLinkedRingNode<T> cur = Start;
			EqualityComparer<T> c = EqualityComparer<T>.Default;

			if (cur != null)
			{
				if (item != null)
				{
					do
					{
						if (c.Equals(cur.Value, item)) return cur;

						cur = cur.Next;
					}
					while (cur != null && cur != Start);
				}
				else
				{
					do
					{
						if (cur.Value == null) return cur;

						cur = cur.Next;
					}
					while (cur != null && cur != Start);
				}
			}

			return null;
		}

		/// <summary>
		/// Finds the last node that contains the specified value.
		/// </summary>
		/// <param name="item">The value to locate in the <see cref="DoubleLinkedRing{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <returns>The last <see cref="DoubleLinkedRingNode{T}"/> that contains the specified value, if found; otherwise, <b>null</b>.</returns>
		public DoubleLinkedRingNode<T> FindLast(T item)
		{
			DoubleLinkedRingNode<T> last = Start.Previous;
			DoubleLinkedRingNode<T> cur = last;
			EqualityComparer<T> c = EqualityComparer<T>.Default;

			if (cur != null)
			{
				if (item != null)
				{
					do
					{
						if (c.Equals(cur.Value, item)) return cur;

						cur = cur.Previous;
					}
					while (cur != null && cur != last);
				}
				else
				{
					do
					{
						if (cur.Value == null) return cur;

						cur = cur.Previous;
					}
					while (cur != null && cur != last);
				}
			}

			return null;
		}

		/// <summary>
		/// Removes the first occurrence of the specified value from the <see cref="DoubleLinkedRing{T}"/>.
		/// </summary>
		/// <param name="item">The value to remove.</param>
		/// <returns><b>true</b> if the element containing value is successfully removed; otherwise, <b>false</b>.
		/// This method also returns <b>false</b> if value was not found in the original <see cref="DoubleLinkedRing{T}"/>.</returns>
		public bool Remove(T item)
		{
			DoubleLinkedRingNode<T> node = Find(item);
			if (node == null) return false;

			Remove(node);
			return true;
		}

		/// <summary>
		/// Removes a node from a <see cref="DoubleLinkedRing{T}"/>.
		/// </summary>
		/// <param name="node">The node to remove.</param>
		public void Remove(DoubleLinkedRingNode<T> node)
		{
			if (node == Start)
			{
				if (node.Previous == node) Start = null;
				else Start = node.Previous;
			}

			node.Previous.Next = node.Next;
			node.Next.Previous = node.Previous;
		}

		/// <summary>
		/// Reverses the order of the values in the double-linked-ring. <see cref="Start"/> will remain the start of the ring.
		/// </summary>
		public void Reverse()
		{
			if (Start == null) return;

			DoubleLinkedRingNode<T> cur = Start;

			do
			{
				DoubleLinkedRingNode<T> tmp = cur.Next;
				cur.Next = cur.Previous;
				cur.Previous = tmp;

				cur = cur.Previous;
			}
			while (cur != Start);
		}

		#region Implements ICollection<T> explicit
		bool ICollection<T>.IsReadOnly { get { return false; } }

		void ICollection<T>.Add(T item)
		{
			Append(item);
		}

		void ICollection<T>.CopyTo(T[] array, int arrayIndex)
		{
			if (array == null) throw new ArgumentNullException(nameof(array));
			if (arrayIndex < 0 || arrayIndex > array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
			if (array.Length - arrayIndex < Count) throw new ArgumentException(ExpectionMessageArrayPlusOffsetTooSmall, nameof(array));

			DoubleLinkedRingNode<T> cur = Start;
			if (cur == null) return;
			do
			{
				array[arrayIndex++] = cur.Value;

				cur = cur.Next;
			}
			while (cur != null && cur != Start);
		}
		#endregion

		#region Implements IEnumerable<T>/IEnumerable explicit
		/// <summary>
		/// Returns a iterator of the nodes of the double-linked-ring in a <see cref="DoubleLinkedRing{T}"/>.
		/// </summary>
		/// <returns>The iterator.</returns>
		public IEnumerable<DoubleLinkedRingNode<T>> GetNodes()
		{
			if (Start == null) yield break;

			DoubleLinkedRingNode<T> cur = Start;

			do
			{
				yield return cur;

				cur = cur.Next;
			}
			while (cur != null && cur != Start);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach (var node in GetNodes()) yield return node.Value;
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			foreach (var node in GetNodes()) yield return node.Value;
		}
		#endregion
	}
}
