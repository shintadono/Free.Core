using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Free.Core.Collections.Generic
{
	/// <summary>
	/// Represents a list of values, that is a <b>struct</b> itself. All but the first 4 values are stored in a reference type list
	/// that is shared between all copies. So caution is adviced when manipulating copies of a list.
	/// </summary>
	/// <typeparam name="T">The type of elements in the list.</typeparam>
	[DebuggerDisplay("Count = {count}")]
	public struct StructList4<T> : IList<T>, IReadOnlyList<T>, IEquatable<StructList4<T>>
	{
		#region Constants
		const string EnumerationFailedExceptionMessage = "Enumeration failed due to changes made to the collection while enumerating.";
		const string MustBePositiveAndSmallerNumberOfElementsExceptionMessage = "Must be positive and smaller than the number of elements.";
		const string MustBePositiveAndSmallerOrEqualNumberOfElementsExceptionMessage = "Must be positive and smaller than or equal to the number of elements.";
		const string MustBePositiveAndMustNotExceedNumberOfElementsLeftExceptionMessage = "Must be positive and must not exceed the number of element from starting index to the end of the list.";
		const string MustBePositiveAndSmallerTargetSizeExceptionMessage = "Must be positive and smaller than the target size.";
		const string MustHaveSpaceToHoldContentIndexExceptionMessage = "Must have enough space (after the starting index) to hold the content of the list.";
		#endregion

		#region Fields
		/// <summary>
		/// Holds the number of elements currently stored in the <see cref="StructList4{T}"/>.
		/// </summary>
		int count;

		/// <summary>
		/// The values.
		/// </summary>
		T v0, v1, v2, v3;

		/// <summary>
		/// The values.
		/// </summary>
		List<T> values;

		/// <summary>
		/// Version to keep track of changes to the enumerator.
		/// </summary>
		int version;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the element at the specified <paramref name="index"/>.
		/// </summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <returns>The element at the specified <paramref name="index"/>.</returns>
		public T this[int index]
		{
			get
			{
				if (index < 0 || index >= count) throw new ArgumentOutOfRangeException(nameof(index), MustBePositiveAndSmallerNumberOfElementsExceptionMessage);

				switch (index)
				{
					case 0: return v0;
					case 1: return v1;
					case 2: return v2;
					case 3: return v3;
					default: return values[index - 4];
				}
			}
			set
			{
				if (index < 0 || index >= count) throw new ArgumentOutOfRangeException(nameof(index), MustBePositiveAndSmallerNumberOfElementsExceptionMessage);

				version++;

				switch (index)
				{
					case 0: v0 = value; break;
					case 1: v1 = value; break;
					case 2: v2 = value; break;
					case 3: v3 = value; break;
					default: values[index - 4] = value; break;
				}
			}
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="StructList4{T}"/>.
		/// </summary>
		public int Count { get { return count; } }

		bool ICollection<T>.IsReadOnly { get { return false; } }
		#endregion

		#region Methods
		/// <summary>
		/// Adds the specified <paramref name="item"/> to the <see cref="StructList4{T}"/>.
		/// </summary>
		/// <param name="item">The item to add.</param>
		public void Add(T item)
		{
			if (values == null) values = new List<T>();

			version++;
			count++;

			switch (count)
			{
				case 1: v0 = item; return;
				case 2: v1 = item; return;
				case 3: v2 = item; return;
				case 4: v3 = item; return;
				default: values.Add(item); return;
			}
		}

		/// <summary>
		/// Adds the specified <paramref name="items"/> to the <see cref="StructList4{T}"/>.
		/// </summary>
		/// <param name="items">The items to add.</param>
		public void AddRange(IEnumerable<T> items)
		{
			if (values == null) values = new List<T>();

			version++;

			if (count >= 4)
			{
				values.AddRange(items);
				count = values.Count + 4;
				return;
			}

			foreach (var i in items) Add(i);
		}

		/// <summary>
		/// Removes all elements from a <see cref="StructList4{T}"/>.
		/// </summary>
		public void Clear()
		{
			if (values == null) values = new List<T>();

			version++;
			count = 0;
			values.Clear();
		}

		/// <summary>
		/// Determines whether a specific <paramref name="item"/> is in the <see cref="StructList4{T}"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="StructList4{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <returns><b>true</b> if item is found in the <see cref="StructList4{T}"/>; otherwise, <b>false</b>.</returns>
		public bool Contains(T item)
		{
			if (item == null)
			{
				if (count <= 0) return false;
				if (v0 == null) return true;
				if (count <= 1) return false;
				if (v1 == null) return true;
				if (count <= 2) return false;
				if (v2 == null) return true;
				if (count <= 3) return false;
				if (v3 == null) return true;
			}
			else
			{
				if (count <= 0) return false;
				if (item.Equals(v0)) return true;
				if (count <= 1) return false;
				if (item.Equals(v1)) return true;
				if (count <= 2) return false;
				if (item.Equals(v2)) return true;
				if (count <= 3) return false;
				if (item.Equals(v3)) return true;
			}

			if (count <= 4) return false;
			return values.Contains(item);
		}

		/// <summary>
		/// Copies the entire <see cref="StructList4{T}"/> to a <paramref name="array"/>, starting at the specified
		/// <paramref name="arrayIndex">index</paramref> of the target <paramref name="array"/>.
		/// </summary>
		/// <param name="array">The array that is the destination of the elements to be copied.</param>
		/// <param name="arrayIndex">The index in <paramref name="array"/> at which to start copying.</param>
		public void CopyTo(T[] array, int arrayIndex = 0)
		{
			if (array == null) throw new ArgumentNullException(nameof(array));
			if (arrayIndex < 0 || arrayIndex >= array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex), MustBePositiveAndSmallerTargetSizeExceptionMessage);
			if (count > array.Length - arrayIndex) throw new ArgumentException(MustHaveSpaceToHoldContentIndexExceptionMessage, nameof(array));

			if (count == 0) return;
			array[0] = v0;
			if (count == 1) return;
			array[1] = v1;
			if (count == 2) return;
			array[2] = v2;
			if (count == 3) return;
			array[3] = v3;
			if (count == 4) return;

			values.CopyTo(array, arrayIndex + 4);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object.
		/// </summary>
		/// <param name="other">The other object.</param>
		/// <returns><b>true</b> if the current object is equal to the other parameter; otherwise, <b>false</b>.</returns>
		public bool Equals(StructList4<T> other)
		{
			if (count != other.count) return false;
			if (!Equals(v0, other.v0)) return false;
			if (!Equals(v1, other.v1)) return false;
			if (!Equals(v2, other.v2)) return false;
			if (!Equals(v3, other.v3)) return false;
			return ReferenceEquals(values, other.values);
		}

		/// <summary>
		/// Determines the position of first occurrence of a specific <paramref name="item"/> in the <see cref="StructList4{T}"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="StructList4{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <returns>If found, the index where the item was found; otherwise, minus one (-1).</returns>
		/// <overloads>Returns the index of the first occurrence of an element in the <see cref="StructList4{T}"/>.</overloads>
		public int IndexOf(T item)
		{
			return IndexOf(item, 0, count);
		}

		/// <summary>
		/// Determines the position of first occurrence (starting at a specified <paramref name="index"/>) of a specific <paramref name="item"/> in the <see cref="StructList4{T}"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="StructList4{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <param name="index">The starting index of the search.</param>
		/// <returns>If found, the index where the item was found; otherwise, minus one (-1).</returns>
		public int IndexOf(T item, int index)
		{
			if (index < 0 || index >= Math.Min(1, count)) throw new ArgumentOutOfRangeException(nameof(index), MustBePositiveAndSmallerNumberOfElementsExceptionMessage);

			return IndexOf(item, index, count - index);
		}

		/// <summary>
		/// Determines the position of first occurrence (starting at a specified <paramref name="index"/>) of a specific <paramref name="item"/> in the <see cref="StructList4{T}"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="StructList4{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <param name="index">The starting index of the search.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <returns>If found, the index where the item was found; otherwise, minus one (-1).</returns>
		public int IndexOf(T item, int index, int count)
		{
			if (index < 0 || index >= Math.Min(1, this.count)) throw new ArgumentOutOfRangeException(nameof(index), MustBePositiveAndSmallerNumberOfElementsExceptionMessage);
			if (count < 0 || index > this.count - count) throw new ArgumentOutOfRangeException(nameof(count), MustBePositiveAndMustNotExceedNumberOfElementsLeftExceptionMessage);

			if (item == null)
			{
				if (this.count <= 0 || count <= 0) return -1;
				if (index == 0)
				{
					if (v0 == null) return 0;
					count--;
				}
				if (this.count <= 1 || count <= 0) return -1;
				if (index <= 1)
				{
					if (v1 == null) return 1;
					count--;
				}
				if (this.count <= 2 || count <= 0) return -1;
				if (index <= 2)
				{
					if (v2 == null) return 2;
					count--;
				}
				if (this.count <= 3 || count <= 0) return -1;
				if (index <= 3)
				{
					if (v3 == null) return 3;
					count--;
				}
			}
			else
			{
				if (this.count <= 0 || count <= 0) return -1;
				if (index == 0)
				{
					if (item.Equals(v0)) return 0;
					count--;
				}
				if (this.count <= 1 || count <= 0) return -1;
				if (index <= 1)
				{
					if (item.Equals(v1)) return 1;
					count--;
				}
				if (this.count <= 2 || count <= 0) return -1;
				if (index <= 2)
				{
					if (item.Equals(v2)) return 2;
					count--;
				}
				if (this.count <= 3 || count <= 0) return -1;
				if (index <= 3)
				{
					if (item.Equals(v3)) return 3;
					count--;
				}
			}

			if (this.count <= 4 || count <= 0) return -1;
			return values.IndexOf(item, Math.Max(0, index - 4), count) + 4;
		}

		/// <summary>
		/// Inserts a specific <paramref name="item"/> into the <see cref="StructList4{T}"/> at the specified index.
		/// </summary>
		/// <param name="index">The index at which <paramref name="item"/> should be inserted.</param>
		/// <param name="item">The object to insert. The value can be <b>null</b> for reference types.</param>
		public void Insert(int index, T item)
		{
			if (index < 0 || index > count) throw new ArgumentOutOfRangeException(nameof(index), MustBePositiveAndSmallerOrEqualNumberOfElementsExceptionMessage);

			if (values == null) values = new List<T>();

			version++;
			count++;

			if (index >= 4)
			{
				values.Insert(index - 4, item);
				return;
			}

			if (count > 4) values.Insert(0, v3);
			if (index == 3) { v3 = item; return; }

			if (count > 3) v3 = v2;
			if (index == 2) { v2 = item; return; }

			if (count > 2) v2 = v1;
			if (index == 1) { v1 = item; return; }

			if (count > 1) v1 = v0;
			v0 = item;
		}

		/// <summary>
		/// Determines the position of last occurrence of a specific <paramref name="item"/> in the <see cref="StructList4{T}"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="StructList4{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <returns>If found, the index where the item was found; otherwise, minus one (-1).</returns>
		/// <overloads>Returns the index of the last occurrence of an element in the <see cref="StructList4{T}"/>.</overloads>
		public int LastIndexOf(T item)
		{
			if (count == 0) return -1;

			return LastIndexOf(item, count-1, count);
		}

		/// <summary>
		/// Determines the position of last occurrence (starting at a specified <paramref name="index"/>) of a specific <paramref name="item"/> in the <see cref="StructList4{T}"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="StructList4{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <param name="index">The starting index of the search.</param>
		/// <returns>If found, the index where the item was found; otherwise, minus one (-1).</returns>
		public int LastIndexOf(T item, int index)
		{
			if (count == 0) return -1;

			if (index < 0 || index >= count) throw new ArgumentOutOfRangeException(nameof(index), MustBePositiveAndSmallerNumberOfElementsExceptionMessage);

			return LastIndexOf(item, index, index + 1);
		}

		/// <summary>
		/// Determines the position of last occurrence (starting at a specified <paramref name="index"/>) of a specific <paramref name="item"/> in the <see cref="StructList4{T}"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="StructList4{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <param name="index">The starting index of the search.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <returns>If found, the index where the item was found; otherwise, minus one (-1).</returns>
		public int LastIndexOf(T item, int index, int count)
		{
			if (this.count == 0) return -1;

			if (index < 0 || index >= this.count) throw new ArgumentOutOfRangeException(nameof(index), MustBePositiveAndSmallerNumberOfElementsExceptionMessage);
			if (count < 0 || count > index + 1) throw new ArgumentOutOfRangeException(nameof(count), MustBePositiveAndMustNotExceedNumberOfElementsLeftExceptionMessage);

			if (index >= 4)
			{
				int countInValues = Math.Min(count, index - 3);
				count -= countInValues;

				int ret = values.LastIndexOf(item, index - 4, countInValues);
				if (ret != -1) return ret;

				if (count <= 0) return -1;

				index = 3;
			}

			if (item == null)
			{
				switch (index)
				{
					case 3: if (v3 == null) return 3; count--; if (count <= 0) return -1; goto case 2;
					case 2: if (v2 == null) return 2; count--; if (count <= 0) return -1; goto case 1;
					case 1: if (v1 == null) return 1; count--; if (count <= 0) return -1; goto case 0;
					case 0: if (v0 == null) return 0; return -1;
				}
			}
			else
			{
				switch (index)
				{
					case 3: if (item.Equals(v3)) return 3; count--; if (count <= 0) return -1; goto case 2;
					case 2: if (item.Equals(v2)) return 2; count--; if (count <= 0) return -1; goto case 1;
					case 1: if (item.Equals(v1)) return 1; count--; if (count <= 0) return -1; goto case 0;
					case 0: if (item.Equals(v0)) return 0; return -1;
				}
			}

			return -1;
		}

		/// <summary>
		/// Removes the first occurrence of a specific <paramref name="item"/> from the <see cref="StructList4{T}"/>.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="StructList4{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <returns><b>true</b> if <paramref name="item"/> is successfully removed; otherwise, <b>false</b>.</returns>
		public bool Remove(T item)
		{
			int index = IndexOf(item);
			if (index < 0) return false;

			RemoveAt(index);

			return true;
		}

		/// <summary>
		/// Removes the element at the specified <paramref name="index"/> of the <see cref="StructList4{T}"/>.
		/// </summary>
		/// <param name="index">The index of the element to remove.</param>
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= count) throw new ArgumentOutOfRangeException(nameof(index), MustBePositiveAndSmallerNumberOfElementsExceptionMessage);

			if (values == null) values = new List<T>();

			version++;
			count--;

			if (index >= 4)
			{
				values.RemoveAt(index - 4);
				return;
			}

			if (index == count) return;

			switch (index)
			{
				case 0: v0 = v1; if (count == 1) break; else goto case 1;
				case 1: v1 = v2; if (count == 2) break; else goto case 2;
				case 2: v2 = v3; if (count == 3) break; else goto case 3;
				case 3: v3 = values[0]; values.RemoveAt(0); break;
			}
		}

		/// <summary>
		/// Removes a range of elements from the <see cref="StructList4{T}"/>.
		/// </summary>
		/// <param name="index">The starting index of the range of elements to remove.</param>
		/// <param name="count">The number of elements to remove.</param>
		public void RemoveRange(int index, int count)
		{
			if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), MustBePositiveAndSmallerNumberOfElementsExceptionMessage);
			if (count < 0 || this.count - index < count) throw new ArgumentOutOfRangeException(nameof(count), MustBePositiveAndMustNotExceedNumberOfElementsLeftExceptionMessage);

			if (count == 0) return;

			for (int i = index + count - 1; i >= index; i--) RemoveAt(i);
		}

		/// <summary>
		/// Copies the elements of the <see cref="StructList4{T}"/> to a new array.
		/// </summary>
		/// <returns></returns>
		public T[] ToArray()
		{
			T[] ret = new T[count];
			CopyTo(ret);
			return ret;
		}
		#endregion

		#region Implements IEnumerator<T>/IEnumerator members.
		IEnumerator<T> GetEnumerator()
		{
			int ver = version;

			if (count == 0) yield break;
			if (ver != version) throw new InvalidOperationException(EnumerationFailedExceptionMessage);
			yield return v0;

			if (count == 1) yield break;
			if (ver != version) throw new InvalidOperationException(EnumerationFailedExceptionMessage);
			yield return v1;

			if (count == 2) yield break;
			if (ver != version) throw new InvalidOperationException(EnumerationFailedExceptionMessage);
			yield return v2;

			if (count == 3) yield break;
			if (ver != version) throw new InvalidOperationException(EnumerationFailedExceptionMessage);
			yield return v3;

			if (count == 4) yield break;

			for (int i = 0; i < values.Count; i++)
			{
				if (ver != version) throw new InvalidOperationException(EnumerationFailedExceptionMessage);
				yield return values[i];
			}
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator() { return GetEnumerator(); }

		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
		#endregion
	}
}
