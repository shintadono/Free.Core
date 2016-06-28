using System.Collections.Generic;

namespace Free.Core.Collections.Generic
{
	/// <summary>
	/// Represents a node in a double-linked-ring. (See <see cref="DoubleLinkedRing{T}"/>).
	/// </summary>
	/// <typeparam name="T">The type of the values in the double-linked-ring.</typeparam>
	public class DoubleLinkedRingNode<T>
	{
		/// <summary>
		/// The value of the node.
		/// </summary>
		public T Value;

		/// <summary>
		/// The next node in the double-linked-ring.
		/// </summary>
		public DoubleLinkedRingNode<T> Next;

		/// <summary>
		/// The previous node in the double-linked-ring.
		/// </summary>
		public DoubleLinkedRingNode<T> Previous;

		/// <summary>
		/// Initializes an instance of <see cref="DoubleLinkedRingNode{T}"/> with the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		public DoubleLinkedRingNode(T value)
		{
			Value = value;
		}

		/// <summary>
		/// Finds the next node that contains the specified value.
		/// </summary>
		/// <param name="item">The value to locate in the <see cref="DoubleLinkedRing{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <returns>The next <see cref="DoubleLinkedRingNode{T}"/> that contains the specified value, if found; otherwise, <b>null</b>.</returns>
		public DoubleLinkedRingNode<T> FindNext(T item)
		{
			DoubleLinkedRingNode<T> cur = this;
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
					while (cur != null && cur != this);
				}
				else
				{
					do
					{
						if (cur.Value == null) return cur;

						cur = cur.Next;
					}
					while (cur != null && cur != this);
				}
			}

			return null;
		}

		/// <summary>
		/// Finds the previous node that contains the specified value.
		/// </summary>
		/// <param name="item">The value to locate in the <see cref="DoubleLinkedRing{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <returns>The previous <see cref="DoubleLinkedRingNode{T}"/> that contains the specified value, if found; otherwise, <b>null</b>.</returns>
		public DoubleLinkedRingNode<T> FindPrevious(T item)
		{
			DoubleLinkedRingNode<T> last = Previous;
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
	}
}
