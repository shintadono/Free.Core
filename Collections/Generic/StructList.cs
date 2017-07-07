using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Free.Core.Collections.Generic
{
	/// <summary>
	/// Represents a list of values, that is a <b>struct</b> itself. All values are stored in a reference type list
	/// that is shared between all copies.
	/// </summary>
	/// <typeparam name="T">The type of elements in the list.</typeparam>
	[DebuggerDisplay("Count = {Count}")]
	public struct StructList<T> : IList<T>, IReadOnlyList<T>, IEquatable<StructList<T>>
	{
		#region Fields
		/// <summary>
		/// The values.
		/// </summary>
		public List<T> Values;
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
				if (Values == null) Values = new List<T>();
				return Values[index];
			}
			set
			{
				if (Values == null) Values = new List<T>();
				Values[index] = value;
			}
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="StructList{T}"/>.
		/// </summary>
		public int Count
		{
			get
			{
				if (Values == null) Values = new List<T>();
				return Values.Count;
			}
		}

		bool ICollection<T>.IsReadOnly
		{
			get
			{
				if (Values == null) Values = new List<T>();
				return ((IList<T>)Values).IsReadOnly;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Adds the specified <paramref name="item"/> to the <see cref="StructList{T}"/>.
		/// </summary>
		/// <param name="item">The item to add.</param>
		public void Add(T item)
		{
			if (Values == null) Values = new List<T>();
			Values.Add(item);
		}

		/// <summary>
		/// Adds the specified <paramref name="items"/> to the <see cref="StructList{T}"/>.
		/// </summary>
		/// <param name="items">The items to add.</param>
		public void AddRange(IEnumerable<T> items)
		{
			if (Values == null) Values = new List<T>();
			Values.AddRange(items);
		}

		/// <summary>
		/// Removes all elements from a <see cref="StructList{T}"/>.
		/// </summary>
		public void Clear()
		{
			if (Values == null) Values = new List<T>();
			Values.Clear();
		}

		/// <summary>
		/// Determines whether a specific <paramref name="item"/> is in the <see cref="StructList{T}"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="StructList{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <returns><b>true</b> if item is found in the <see cref="StructList{T}"/>; otherwise, <b>false</b>.</returns>
		public bool Contains(T item)
		{
			if (Values == null) Values = new List<T>();
			return Values.Contains(item);
		}

		/// <summary>
		/// Copies the entire <see cref="StructList{T}"/> to a <paramref name="array"/>, starting at the specified
		/// <paramref name="arrayIndex">index</paramref> of the target <paramref name="array"/>.
		/// </summary>
		/// <param name="array">The array that is the destination of the elements to be copied.</param>
		/// <param name="arrayIndex">The index in <paramref name="array"/> at which to start copying.</param>
		/// <overloads>Copies the <see cref="StructList{T}"/> or a portion of it to an array.</overloads>
		public void CopyTo(T[] array, int arrayIndex = 0)
		{
			if (Values == null) Values = new List<T>();
			Values.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Copies a range of elements from the <see cref="StructList{T}"/> to a <paramref name="array"/>, starting at the specified
		/// <paramref name="arrayIndex">index</paramref> of the target <paramref name="array"/>.
		/// </summary>
		/// <param name="index">The index in the source <see cref="StructList{T}"/> at which copying begins.</param>
		/// <param name="array">The array that is the destination of the elements to be copied.</param>
		/// <param name="arrayIndex">The index in <paramref name="array"/> at which to start copying.</param>
		/// <param name="count">The number of elements to copy.</param>
		public void CopyTo(int index, T[] array, int arrayIndex, int count)
		{
			if (Values == null) Values = new List<T>();
			Values.CopyTo(index, array, arrayIndex, count);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object.
		/// </summary>
		/// <param name="other">The other object.</param>
		/// <returns><b>true</b> if the current object is equal to the other parameter; otherwise, <b>false</b>.</returns>
		public bool Equals(StructList<T> other)
		{
			return ReferenceEquals(Values, other.Values);
		}

		/// <summary>
		/// Determines the position of first occurrence of a specific <paramref name="item"/> in the <see cref="StructList{T}"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="StructList{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <returns>If found, the index where the item was found; otherwise, minus one (-1).</returns>
		/// <overloads>Returns the index of the first occurrence of an element in the <see cref="StructList{T}"/>.</overloads>
		public int IndexOf(T item)
		{
			if (Values == null) Values = new List<T>();
			return Values.IndexOf(item);
		}

		/// <summary>
		/// Determines the position of first occurrence (starting at a specified <paramref name="index"/>) of a specific <paramref name="item"/> in the <see cref="StructList{T}"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="StructList{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <param name="index">The starting index of the search.</param>
		/// <returns>If found, the index where the item was found; otherwise, minus one (-1).</returns>
		public int IndexOf(T item, int index)
		{
			if (Values == null) Values = new List<T>();
			return Values.IndexOf(item, index);
		}

		/// <summary>
		/// Determines the position of first occurrence (starting at a specified <paramref name="index"/>) of a specific <paramref name="item"/> in the <see cref="StructList{T}"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="StructList{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <param name="index">The starting index of the search.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <returns>If found, the index where the item was found; otherwise, minus one (-1).</returns>
		public int IndexOf(T item, int index, int count)
		{
			if (Values == null) Values = new List<T>();
			return Values.IndexOf(item, index, count);
		}

		/// <summary>
		/// Inserts a specific <paramref name="item"/> into the <see cref="StructList{T}"/> at the specified index.
		/// </summary>
		/// <param name="index">The index at which <paramref name="item"/> should be inserted.</param>
		/// <param name="item">The object to insert. The value can be <b>null</b> for reference types.</param>
		public void Insert(int index, T item)
		{
			if (Values == null) Values = new List<T>();
			Values.Insert(index, item);
		}

		/// <summary>
		/// Inserts a specific <paramref name="items"/> into the <see cref="StructList{T}"/> at the specified index.
		/// </summary>
		/// <param name="index">The index at which <paramref name="items"/> should be inserted.</param>
		/// <param name="items">The objects to insert. The collection itself must not be <b>null</b>, but the items can be <b>null</b> for reference types.</param>
		public void InsertRange(int index, IEnumerable<T> items)
		{
			if (Values == null) Values = new List<T>();
			Values.InsertRange(index, items);
		}

		/// <summary>
		/// Determines the position of last occurrence of a specific <paramref name="item"/> in the <see cref="StructList{T}"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="StructList{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <returns>If found, the index where the item was found; otherwise, minus one (-1).</returns>
		/// <overloads>Returns the index of the last occurrence of an element in the <see cref="StructList{T}"/>.</overloads>
		public int LastIndexOf(T item)
		{
			if (Values == null) Values = new List<T>();
			return Values.LastIndexOf(item);
		}

		/// <summary>
		/// Determines the position of last occurrence (starting at a specified <paramref name="index"/>) of a specific <paramref name="item"/> in the <see cref="StructList{T}"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="StructList{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <param name="index">The starting index of the search.</param>
		/// <returns>If found, the index where the item was found; otherwise, minus one (-1).</returns>
		public int LastIndexOf(T item, int index)
		{
			if (Values == null) Values = new List<T>();
			return Values.LastIndexOf(item, index);
		}

		/// <summary>
		/// Determines the position of last occurrence (starting at a specified <paramref name="index"/>) of a specific <paramref name="item"/> in the <see cref="StructList{T}"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="StructList{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <param name="index">The starting index of the search.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <returns>If found, the index where the item was found; otherwise, minus one (-1).</returns>
		public int LastIndexOf(T item, int index, int count)
		{
			if (Values == null) Values = new List<T>();
			return Values.LastIndexOf(item, index, count);
		}

		/// <summary>
		/// Removes the first occurrence of a specific <paramref name="item"/> from the <see cref="StructList{T}"/>.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="StructList{T}"/>. The value can be <b>null</b> for reference types.</param>
		/// <returns><b>true</b> if <paramref name="item"/> is successfully removed; otherwise, <b>false</b>.</returns>
		public bool Remove(T item)
		{
			if (Values == null) Values = new List<T>();
			return Values.Remove(item);
		}

		/// <summary>
		/// Removes the element at the specified <paramref name="index"/> of the <see cref="StructList{T}"/>.
		/// </summary>
		/// <param name="index">The index of the element to remove.</param>
		public void RemoveAt(int index)
		{
			if (Values == null) Values = new List<T>();
			Values.RemoveAt(index);
		}

		/// <summary>
		/// Removes a range of elements from the <see cref="StructList{T}"/>.
		/// </summary>
		/// <param name="index">The starting index of the range of elements to remove.</param>
		/// <param name="count">The number of elements to remove.</param>
		public void RemoveRange(int index, int count)
		{
			if (Values == null) Values = new List<T>();
			Values.RemoveRange(index, count);
		}

		/// <summary>
		/// Reverses the order of the elements in the entire <see cref="StructList{T}"/>.
		/// </summary>
		/// <overloads>Reverses the order of the elements in the <see cref="StructList{T}"/> or a portion of it.</overloads>
		public void Reverse()
		{
			if (Values == null) Values = new List<T>();
			Values.Reverse();
		}

		/// <summary>
		/// Reverses the order of the elements in the specified range.
		/// </summary>
		/// <param name="index">The starting index of the range to reverse.</param>
		/// <param name="count">The number of elements to reverse.</param>
		public void Reverse(int index, int count)
		{
			if (Values == null) Values = new List<T>();
			Values.Reverse(index, count);
		}

		/// <summary>
		/// Copies the elements of the <see cref="StructList{T}"/> to a new array.
		/// </summary>
		/// <returns></returns>
		public T[] ToArray()
		{
			if (Values == null) Values = new List<T>();
			return Values.ToArray();
		}
		#endregion

		#region Implements IEnumerator<T>/IEnumerator members.
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			if (Values == null) Values = new List<T>();
			return Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			if (Values == null) Values = new List<T>();
			return Values.GetEnumerator();
		}
		#endregion
	}
}
