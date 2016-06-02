using System;
using System.Collections;
using System.Collections.Generic;

namespace Free.Core.Collections.Generic
{
	/// <summary>
	/// Represents a set of values, with predictable iteration order.
	/// </summary>
	/// <typeparam name="T">The type of elements in the hash set.</typeparam>
	public class LinkedHashSet<T> : ISet<T>
	{
		const string ExpectionMessageCollectionIsReadOnly = "Collection is read only.";
		const string ExpectionMessageWrongType = "Wrong type.";
		const string ExpectionMessageInvalidArrayType = "Invalid array type.";
		const string ExpectionMessageArrayPlusOffsetTooSmall = "Array (plus offset) too small.";
		const string ExpectionMessageArrayRankNotOne = "Array with rank not equal to one (1) is not supported.";
		const string ExpectionMessageArrayLowerBoundNotZero = "Array with lower bound not equal to zero (0) is not supported.";

		#region Variables
		Dictionary<T, LinkedListNode<T>> dict;
		LinkedList<T> list;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the <see cref="IEqualityComparer{T}"/> that is used to determine equality of elements for the hashset.
		/// </summary>
		public IEqualityComparer<T> Comparer { get { return dict.Comparer; } }

		/// <summary>
		/// Gets the number of elements contained in the <see cref="LinkedHashSet{T}"/>.
		/// </summary>
		public int Count { get { return dict.Count; } }
		#endregion

		#region Ctors
		/// <summary>
		/// Initializes a new instance of the <see cref="LinkedHashSet{T}"/> class that is empty.
		/// An <see cref="IEqualityComparer{T}"/> can be specified.
		/// </summary>
		/// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing elements,
		/// or <b>null</b> to use the default <see cref="EqualityComparer{T}"/> for the type of the element.</param>
		public LinkedHashSet(IEqualityComparer<T> comparer = null)
		{
			dict = new Dictionary<T, LinkedListNode<T>>(comparer);
			list = new LinkedList<T>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkedHashSet{T}"/> class that contains
		/// elements copied from the specified <see cref="IEnumerable{T}"/>.
		/// An <see cref="IEqualityComparer{T}"/> can be specified.
		/// </summary>
		/// <param name="collection">The <see cref="IEnumerable{T}"/> whose elements are copied to the new <see cref="LinkedHashSet{T}"/>.</param>
		/// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing elements,
		/// or <b>null</b> to use the default <see cref="EqualityComparer{T}"/> for the type of the element.</param>
		public LinkedHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer = null)
		{
			if (collection == null) throw new ArgumentNullException(nameof(collection));

			dict = new Dictionary<T, LinkedListNode<T>>(comparer);
			list = new LinkedList<T>();

			foreach (var item in collection) Add(item);
		}
		#endregion

		/// <summary>
		/// Adds the specified element to a <see cref="LinkedHashSet{T}"/>.
		/// </summary>
		/// <param name="item">The element to add to the <see cref="LinkedHashSet{T}"/> object.</param>
		/// <returns><b>true</b> if the element is added to the <see cref="LinkedHashSet{T}"/> object; <b>false</b> if the element is already present.</returns>
		public bool Add(T item)
		{
			if (dict.ContainsKey(item)) return false;

			LinkedListNode<T> node = list.AddLast(item);
			dict.Add(item, node);

			return true;
		}

		/// <summary>
		/// Removes all elements from a <see cref="LinkedHashSet{T}"/>.
		/// </summary>
		public void Clear()
		{
			dict.Clear();
			list.Clear();
		}

		/// <summary>
		/// Determines whether a <see cref="LinkedHashSet{T}"/> object contains the specified element.
		/// </summary>
		/// <param name="item">The element to locate in the <see cref="LinkedHashSet{T}"/> object.</param>
		/// <returns><b>true</b> if the <see cref="LinkedHashSet{T}"/> object contains the specified element; otherwise, <b>false</b>.</returns>
		public bool Contains(T item) { return dict.ContainsKey(item); }

		/// <summary>
		/// Removes the specified element from a <see cref="LinkedHashSet{T}"/>.
		/// </summary>
		/// <param name="item">The element to remove.</param>
		/// <returns><b>true</b> if the element is successfully found and removed; otherwise, <b>false</b>.
		/// This method returns <b>false</b> if element is not found in the <see cref="LinkedHashSet{T}"/>.</returns>
		public bool Remove(T item)
		{
			LinkedListNode<T> node;
			if (!dict.TryGetValue(item, out node)) return false;
			dict.Remove(item);
			list.Remove(node);
			return true;
		}

		#region Implements ISet<T>
		/// <summary>
		/// Modifies the current set so that it contains all elements that are present in
		/// the current set, in the specified collection, or in both.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		public void UnionWith(IEnumerable<T> other)
		{
			if (other == null) throw new ArgumentNullException(nameof(other));

			foreach (var item in other)
			{
				if (dict.ContainsKey(item)) continue;

				LinkedListNode<T> node = list.AddLast(item);
				dict.Add(item, node);
			}
		}

		/// <summary>
		/// Modifies the current set so that it contains only elements that are also in a
		/// specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		public void IntersectWith(IEnumerable<T> other)
		{
			if (other == null) throw new ArgumentNullException(nameof(other));

			if (dict.Count == 0) return; // No elements to remove.
			if (ReferenceEquals(this, other)) return; // We intersect with ourself, so all elements will stay.

			var c = list.First;
			if (c == null) return;

			ICollection<T> collection = other as ICollection<T>;
			if (collection != null)
			{
				if (collection.Count == 0)
				{
					Clear();
					return;
				}

				LinkedHashSet<T> lhs = other as LinkedHashSet<T>;
				if (lhs != null && Comparer.Equals(lhs.Comparer))
				{
					do
					{
						var next = c.Next;

						if (!lhs.Contains(c.Value))
						{
							dict.Remove(c.Value);
							list.Remove(c);
						}

						c = next;
					}
					while (list.Count > 0 && c != list.First);

					return;
				}

				HashSet<T> hs = other as HashSet<T>;
				if (hs != null && Comparer.Equals(hs.Comparer))
				{
					do
					{
						var next = c.Next;

						if (!hs.Contains(c.Value))
						{
							dict.Remove(c.Value);
							list.Remove(c);
						}

						c = next;
					}
					while (list.Count > 0 && c != list.First);

					return;
				}
			}

			HashSet<LinkedListNode<T>> keep = new HashSet<LinkedListNode<T>>();
			foreach (var item in other)
			{
				LinkedListNode<T> node;
				if (dict.TryGetValue(item, out node)) keep.Add(node);
			}

			do
			{
				var next = c.Next;

				if (!keep.Contains(c))
				{
					dict.Remove(c.Value);
					list.Remove(c);
				}

				c = next;
			}
			while (list.Count > 0 && c != list.First);
		}

		/// <summary>
		/// Removes all elements in the specified collection from the current set.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		public void ExceptWith(IEnumerable<T> other)
		{
			if (other == null) throw new ArgumentNullException(nameof(other));

			foreach (T item in other) Remove(item);
		}

		/// <summary>
		/// Modifies the current set so that it contains only elements that are present either
		/// in the current set or in the specified collection, but not both.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		public void SymmetricExceptWith(IEnumerable<T> other)
		{
			if (other == null) throw new ArgumentNullException(nameof(other));

			if (dict.Count == 0) // 0 XOR x == x
			{
				UnionWith(other);
				return;
			}

			if (ReferenceEquals(this, other)) // x XOR x == 0
			{
				Clear();
				return;
			}

			var c = list.First;
			if (c == null) return;

			ICollection<T> collection = other as ICollection<T>;
			if (collection != null)
			{
				if (collection.Count == 0) return; // x XOR 0 == x

				LinkedHashSet<T> lhs = other as LinkedHashSet<T>;
				HashSet<T> hs = other as HashSet<T>;
				if ((lhs != null && Comparer.Equals(lhs.Comparer))|| (hs != null && Comparer.Equals(hs.Comparer))) // No dupicates in other.
				{
					foreach (var item in other)
					{
						LinkedListNode<T> node;
						if (dict.TryGetValue(item, out node))
						{
							dict.Remove(item);
							list.Remove(node);
						}
						else
						{
							node = list.AddLast(item);
							dict.Add(item, node);
						}
					}

					return;
				}
			}

			LinkedHashSet<T> add = new LinkedHashSet<T>(dict.Comparer);
			HashSet<LinkedListNode<T>> remove = new HashSet<LinkedListNode<T>>();
			foreach (var item in other)
			{
				LinkedListNode<T> node;
				if (dict.TryGetValue(item, out node)) remove.Add(node);
				else add.Add(item);
			}

			foreach(var node in remove)
			{
				dict.Remove(node.Value);
				list.Remove(node);
			}

			foreach (var item in add)
			{
				LinkedListNode<T> node = list.AddLast(item);
				dict.Add(item, node);
			}
		}

		public bool IsSubsetOf(IEnumerable<T> other)
		{
			throw new NotImplementedException();
		}

		public bool IsSupersetOf(IEnumerable<T> other)
		{
			throw new NotImplementedException();
		}

		public bool IsProperSupersetOf(IEnumerable<T> other)
		{
			throw new NotImplementedException();
		}

		public bool IsProperSubsetOf(IEnumerable<T> other)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Determines whether the current set overlaps with the specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns><b>true</b> if the current set and other share at least one common element; otherwise, <b>false</b>.</returns>
		public bool Overlaps(IEnumerable<T> other)
		{
			if (other == null) throw new ArgumentNullException(nameof(other));

			if (dict.Count == 0) return false;

			foreach (T item in other) if (dict.ContainsKey(item)) return true;

			return false;
		}

		public bool SetEquals(IEnumerable<T> other)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Implements ICollection<T> explicit
		void ICollection<T>.CopyTo(T[] array, int arrayIndex) { list.CopyTo(array, arrayIndex); }

		bool ICollection<T>.IsReadOnly { get { return false; } }

		void ICollection<T>.Add(T item) { Add(item); }
		#endregion

		#region Implements IEnumerable/IEnumerable<T>
		/// <summary>
		/// Returns an enumerator that iterates through a <see cref="LinkedHashSet{T}"/> object.
		/// </summary>
		/// <returns>A <see cref="IEnumerator{T}"/> for the <see cref="LinkedHashSet{T}"/> object.</returns>
		public IEnumerator<T> GetEnumerator() { return list.GetEnumerator(); }

		IEnumerator IEnumerable.GetEnumerator() { return list.GetEnumerator(); }
		#endregion
	}
}
