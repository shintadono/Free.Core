using System;
using System.Collections;
using System.Collections.Generic;

namespace Free.Core.Collections.Generic
{
	/// <summary>
	/// Represents a set of values, with predictable iteration order.
	/// </summary>
	/// <typeparam name="T">The type of elements in the hash set.</typeparam>
	public class LinkedHashSet<T> : ISet<T>, IElementLookup<T, T>
	{
		const string ExpectionMessageCollectionIsEmpty = "Collection is empty.";
		const string ExpectionMessageNoElementFoundMatchingThePredicateOrCollectionIsEmpty = "No element found matching the perdicate, or collection is empty.";

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

		/// <summary>
		/// Retrieves a value from the <see cref="LinkedHashSet{T}"/>, or adds it to the <see cref="LinkedHashSet{T}"/>.
		/// </summary>
		/// <param name="item">The value for which to get the store element, or the value to add.</param>
		/// <returns>The store element if found; otherwise, <paramref name="item"/>.</returns>
		public T GetOrAdd(T item)
		{
			LinkedListNode<T> node;
			if (!dict.TryGetValue(item, out node))
			{
				node = list.AddLast(item);
				dict.Add(item, node);
				return item;
			}

			return node.Value;
		}

		/// <summary>
		/// Retrieves a value from the <see cref="LinkedHashSet{T}"/>, or adds it to the <see cref="LinkedHashSet{T}"/>.
		/// </summary>
		/// <param name="item">The value for which to get the store element, or the value to add.</param>
		/// <returns>The store element if found; otherwise, <paramref name="item"/>.</returns>
		public T this[T item] { get { return GetOrAdd(item); } }

		#region Implements ISet<T>
		/// <summary>
		/// Modifies the current set so that it contains all elements that are present in
		/// the current set, in the specified collection, or in both.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		public void UnionWith(IEnumerable<T> other)
		{
			if (other == null) throw new ArgumentNullException(nameof(other));

			if (ReferenceEquals(this, other)) return; // We union with ourself, so no element will added.

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
					while (c != null);

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
					while (c != null);

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
			while (c != null);
		}

		/// <summary>
		/// Removes all elements in the specified collection from the current set.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		public void ExceptWith(IEnumerable<T> other)
		{
			if (other == null) throw new ArgumentNullException(nameof(other));

			if (ReferenceEquals(this, other))
			{
				Clear();
				return;
			}

			foreach (var item in other) Remove(item);
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

			ICollection<T> collection = other as ICollection<T>;
			if (collection != null)
			{
				if (collection.Count == 0) return; // x XOR 0 == x

				LinkedHashSet<T> lhs = other as LinkedHashSet<T>;
				HashSet<T> hs = other as HashSet<T>;
				if ((lhs != null && Comparer.Equals(lhs.Comparer)) || (hs != null && Comparer.Equals(hs.Comparer))) // No duplicates in other.
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

			foreach (var node in remove)
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

		/// <summary>
		/// Determines whether a set is a subset of a specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns><b>true</b> if the current set is a subset of other; otherwise, <b>false</b>.</returns>
		public bool IsSubsetOf(IEnumerable<T> other)
		{
			if (other == null) throw new ArgumentNullException(nameof(other));

			// Empty set is subset of any set and set can be a subset of itself.
			if (dict.Count == 0 || ReferenceEquals(this, other)) return true;

			ICollection<T> collection = other as ICollection<T>;
			if (collection != null)
			{
				// If this set has more elements than the other set, it can't be a subset.
				if (dict.Count > collection.Count) return false;

				LinkedHashSet<T> lhs = other as LinkedHashSet<T>;
				if (lhs != null && Comparer.Equals(lhs.Comparer))
				{
					foreach (var item in this)
					{
						if (!lhs.dict.ContainsKey(item)) return false;
					}

					return true;
				}

				HashSet<T> hs = other as HashSet<T>;
				if (hs != null && Comparer.Equals(hs.Comparer))
				{
					foreach (var item in this)
					{
						if (!hs.Contains(item)) return false;
					}

					return true;
				}
			}

			int notFound = 0;
			HashSet<LinkedListNode<T>> uniqueFound = new HashSet<LinkedListNode<T>>();
			foreach (var item in other)
			{
				LinkedListNode<T> node;
				if (dict.TryGetValue(item, out node)) uniqueFound.Add(node);
				else notFound++;
			}

			return uniqueFound.Count == Count && notFound >= 0;
		}

		/// <summary>
		/// Determines whether a set is a proper subset of a specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns><b>true</b> if the current set is a proper subset of other; otherwise, <b>false</b>.</returns>
		public bool IsProperSubsetOf(IEnumerable<T> other)
		{
			if (other == null) throw new ArgumentNullException(nameof(other));

			if (ReferenceEquals(this, other)) return false; // Set can not be a proper subset of itself.

			ICollection<T> collection = other as ICollection<T>;
			if (collection != null)
			{
				// The empty set is a proper subset of anything but the empty set.
				if (dict.Count == 0) return collection.Count > 0;

				// If this set has equal number or more elements than the other set, it can't be a proper subset.
				if (dict.Count >= collection.Count) return false;

				LinkedHashSet<T> lhs = other as LinkedHashSet<T>;
				if (lhs != null && Comparer.Equals(lhs.Comparer))
				{
					foreach (var item in this)
					{
						if (!lhs.dict.ContainsKey(item)) return false;
					}

					return true;
				}

				HashSet<T> hs = other as HashSet<T>;
				if (hs != null && Comparer.Equals(hs.Comparer))
				{
					foreach (var item in this)
					{
						if (!hs.Contains(item)) return false;
					}

					return true;
				}
			}

			if (dict.Count == 0)
			{
				// The empty set is a proper subset of anything but the empty set.
				foreach (var item in other) return true;
				return false;
			}

			int notFound = 0;
			HashSet<LinkedListNode<T>> uniqueFound = new HashSet<LinkedListNode<T>>();
			foreach (var item in other)
			{
				LinkedListNode<T> node;
				if (dict.TryGetValue(item, out node)) uniqueFound.Add(node);
				else notFound++;
			}

			return uniqueFound.Count == Count && notFound > 0; // At least one element of the other set, should not be in this set, but all of this set must be in other.
		}

		/// <summary>
		/// Determines whether a set is a superset of a specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns><b>true</b> if the current set is a superset of other; otherwise, <b>false</b>.</returns>
		public bool IsSupersetOf(IEnumerable<T> other)
		{
			if (other == null) throw new ArgumentNullException(nameof(other));

			ICollection<T> collection = other as ICollection<T>;
			if (collection != null)
			{
				// If the other set is empty, it is a superset of this.
				if (collection.Count == 0) return true;

				LinkedHashSet<T> lhs = other as LinkedHashSet<T>;
				HashSet<T> hs = other as HashSet<T>;
				if ((lhs != null && Comparer.Equals(lhs.Comparer)) || (hs != null && Comparer.Equals(hs.Comparer)))
				{
					if (dict.Count < collection.Count) return false;
				}
			}

			foreach (var item in other)
			{
				if (!dict.ContainsKey(item)) return false;
			}

			return true;
		}

		/// <summary>
		/// Determines whether a set is a proper superset of a specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns><b>true</b> if the current set is a proper superset of other; otherwise, <b>false</b>.</returns>
		public bool IsProperSupersetOf(IEnumerable<T> other)
		{
			if (other == null) throw new ArgumentNullException(nameof(other));

			// Empty set is never a proper superset of any set and set can not be a proper superset of itself.
			if (dict.Count == 0 || ReferenceEquals(this, other)) return false;

			ICollection<T> collection = other as ICollection<T>;
			if (collection != null)
			{
				// If the other set is empty (and since we checked that this set has at least one element above), it is a proper superset of this.
				if (collection.Count == 0) return true;

				LinkedHashSet<T> lhs = other as LinkedHashSet<T>;
				HashSet<T> hs = other as HashSet<T>;
				if ((lhs != null && Comparer.Equals(lhs.Comparer)) || (hs != null && Comparer.Equals(hs.Comparer)))
				{
					if (dict.Count <= collection.Count) return false;

					foreach (var item in other)
					{
						if (!dict.ContainsKey(item)) return false;
					}

					return true;
				}
			}

			HashSet<LinkedListNode<T>> uniqueFound = new HashSet<LinkedListNode<T>>();
			foreach (var item in other)
			{
				LinkedListNode<T> node;
				if (dict.TryGetValue(item, out node)) uniqueFound.Add(node);
				else return false;
			}

			return uniqueFound.Count < dict.Count; // At least one element of this set, should not be in the other set.
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

			foreach (var item in other) if (dict.ContainsKey(item)) return true;

			return false;
		}

		/// <summary>
		/// Determines whether the current set and the specified collection contain the same elements.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns><b>true</b> if the current set is equal to other; otherwise, <b>false</b>.</returns>
		public bool SetEquals(IEnumerable<T> other)
		{
			if (other == null) throw new ArgumentNullException(nameof(other));

			if (ReferenceEquals(this, other)) return true;

			ICollection<T> collection = other as ICollection<T>;
			if (collection != null)
			{
				// If this set is empty but other contains at least one element, they can't be equal.
				if (dict.Count == 0 && collection.Count > 0) return false;

				LinkedHashSet<T> lhs = other as LinkedHashSet<T>;
				HashSet<T> hs = other as HashSet<T>;
				if ((lhs != null && Comparer.Equals(lhs.Comparer)) || (hs != null && Comparer.Equals(hs.Comparer)))
				{
					if (dict.Count != collection.Count) return false;

					foreach (var item in other)
					{
						if (!dict.ContainsKey(item)) return false;
					}

					return true;
				}
			}

			if (dict.Count == 0)
			{
				// If this set is empty but other contains at least one element, they can't be equal.
				foreach (var item in other) return false;
				return true;
			}

			HashSet<LinkedListNode<T>> uniqueFound = new HashSet<LinkedListNode<T>>();
			foreach (var item in other)
			{
				LinkedListNode<T> node;
				if (dict.TryGetValue(item, out node)) uniqueFound.Add(node);
				else return false;
			}

			return uniqueFound.Count == Count;
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

		#region Some LINQ operations, that should be faster than the LINQ extensions
		/// <summary>
		/// Returns the first element in a <see cref="LinkedHashSet{T}"/>.
		/// </summary>
		/// <returns>The first element in the <see cref="LinkedHashSet{T}"/>.</returns>
		public T First()
		{
			var first = list.First;
			if (first == null) throw new InvalidOperationException(ExpectionMessageCollectionIsEmpty);
			return first.Value;
		}

		/// <summary>
		/// Returns the first element in a <see cref="LinkedHashSet{T}"/> that satisfies a specified condition.
		/// </summary>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The first element in the <see cref="LinkedHashSet{T}"/> that passes the test in the specified <paramref name="predicate"/> function.</returns>
		public T First(Func<T, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));

			foreach (var item in list)
			{
				if (predicate(item)) return item;
			}

			throw new InvalidOperationException(ExpectionMessageNoElementFoundMatchingThePredicateOrCollectionIsEmpty);
		}

		/// <summary>
		/// Returns the first element of a <see cref="LinkedHashSet{T}"/>, or a default value if the <see cref="LinkedHashSet{T}"/> contains no elements.
		/// </summary>
		/// <returns><b>default(T)</b> if the <see cref="LinkedHashSet{T}"/> is empty; otherwise, the first element in the <see cref="LinkedHashSet{T}"/>.</returns>
		public T FirstOrDefault()
		{
			var first = list.First;
			if (first == null) return default(T);
			return first.Value;
		}

		/// <summary>
		/// Returns the first element in a <see cref="LinkedHashSet{T}"/> that satisfies a specified condition or a default value if no such element is found.
		/// </summary>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns><b>default(T)</b> if the <see cref="LinkedHashSet{T}"/> is empty or if no element passes the test specified by <paramref name="predicate"/>; otherwise, the
		/// first element in the <see cref="LinkedHashSet{T}"/> that passes the test specified by <paramref name="predicate"/>.</returns>
		public T FirstOrDefault(Func<T, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));

			foreach (var item in list)
			{
				if (predicate(item)) return item;
			}

			return default(T);
		}

		/// <summary>
		/// Returns the last element of a <see cref="LinkedHashSet{T}"/>.
		/// </summary>
		/// <returns>The last element in the <see cref="LinkedHashSet{T}"/>.</returns>
		public T Last()
		{
			var last = list.Last;
			if (last == null) throw new InvalidOperationException(ExpectionMessageCollectionIsEmpty);
			return last.Value;
		}

		/// <summary>
		/// Returns the last element in a <see cref="LinkedHashSet{T}"/> that satisfies a specified condition.
		/// </summary>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The last element in the <see cref="LinkedHashSet{T}"/> that passes the test in the specified <paramref name="predicate"/> function.</returns>
		public T Last(Func<T, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));

			var current = list.Last;
			while (current != null)
			{
				if (predicate(current.Value)) return current.Value;
				current = current.Previous;
			}

			throw new InvalidOperationException(ExpectionMessageNoElementFoundMatchingThePredicateOrCollectionIsEmpty);
		}

		/// <summary>
		/// Returns the last element of a <see cref="LinkedHashSet{T}"/>, or a default value if the <see cref="LinkedHashSet{T}"/> contains no elements.
		/// </summary>
		/// <returns><b>default(T)</b> if the <see cref="LinkedHashSet{T}"/> is empty; otherwise, the last element in the <see cref="LinkedHashSet{T}"/>.</returns>
		public T LastOrDefault()
		{
			var last = list.Last;
			if (last == null) return default(T);
			return last.Value;
		}

		/// <summary>
		/// Returns the last element in a <see cref="LinkedHashSet{T}"/> that satisfies a specified condition or a default value if no such element is found.
		/// </summary>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns><b>default(T)</b> if the <see cref="LinkedHashSet{T}"/> is empty or if no element passes the test specified by <paramref name="predicate"/>; otherwise, the
		/// last element in the <see cref="LinkedHashSet{T}"/> that passes the test specified by <paramref name="predicate"/>.</returns>
		public T LastOrDefault(Func<T, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));

			var current = list.Last;
			while (current != null)
			{
				if (predicate(current.Value)) return current.Value;
				current = current.Previous;
			}

			return default(T);
		}

		/// <summary>
		/// Returns the element of a <see cref="LinkedHashSet{T}"/> in reverse order.
		/// </summary>
		/// <returns>A sequence whose elements correspond to those of the <see cref="LinkedHashSet{T}"/> in reverse order.</returns>
		public IEnumerable<T> Reverse()
		{
			var current = list.Last;
			while (current != null)
			{
				yield return current.Value;
				current = current.Previous;
			}
		}
		#endregion
	}
}
