using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Free.Core.Collections.Generic
{
	/// <summary>
	/// Represents a unchangable list of values.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[ComVisible(false)]
	[DebuggerDisplay("Count = {Count}")]
	public class ReadOnlyList<T> : IList<T>, IList, IReadOnlyList<T>
	{
		/// <summary>
		/// The base list.
		/// </summary>
		protected IList<T> list;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadOnlyList{T}"/> class that is empty.
		/// </summary>
		public ReadOnlyList() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadOnlyList{T}"/> class that contains
		/// elements copied from the specified <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <param name="collection">The <see cref="IEnumerable{T}"/> whose elements are copied to the new <see cref="ReadOnlyList{T}"/>.</param>
		public ReadOnlyList(IEnumerable<T> collection) { list = new List<T>(collection); }

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadOnlyList{T}"/> class that contains
		/// elements copied from the specified arguments.
		/// </summary>
		/// <param name="collection">The elements for the new <see cref="ReadOnlyList{T}"/>.</param>
		public ReadOnlyList(params T[] collection) { list = new List<T>(collection); }

		void ThrowReadOnly() { throw new NotSupportedException("Collection is read-only."); }

		#region Implements IList<T>
		/// <summary>
		/// Determines the index of a specific item in the <see cref="ReadOnlyList{T}"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="ReadOnlyList{T}"/>.</param>
		/// <returns>The index of <paramref name="item"/> if found in the list; otherwise, -1.</returns>
		public int IndexOf(T item) { return list.IndexOf(item); }

		void IList<T>.Insert(int index, T item) { ThrowReadOnly(); }

		void IList<T>.RemoveAt(int index) { ThrowReadOnly(); }

		/// <summary>
		/// Gets the element at the specified index. Throws exception when used to set a value.
		/// </summary>
		/// <param name="index">The zero-based index of the element to get.</param>
		/// <returns>The element at the specified index.</returns>
		public T this[int index]
		{
			get { return list[index]; }
			set { ThrowReadOnly(); }
		}
		#endregion

		#region Implements ICollection<T>
		void ICollection<T>.Add(T item) { ThrowReadOnly(); }

		void ICollection<T>.Clear() { ThrowReadOnly(); }

		/// <summary>
		/// Determines whether the <see cref="ReadOnlyList{T}"/> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="ReadOnlyList{T}"/>.</param>
		/// <returns><b>true</b> if item is found in the <see cref="ReadOnlyList{T}"/>; otherwise, <b>false</b>.</returns>
		public bool Contains(T item) { return list.Contains(item); }

		void ICollection<T>.CopyTo(T[] array, int arrayIndex) { list.CopyTo(array, arrayIndex); }

		/// <summary>
		/// Gets the number of elements contained in the <see cref="ReadOnlyList{T}"/>.
		/// </summary>
		public int Count { get { return list.Count; } }

		bool ICollection<T>.IsReadOnly { get { return true; } }

		bool ICollection<T>.Remove(T item)
		{
			ThrowReadOnly();
			return false; // To silence the compiler.
		}
		#endregion

		#region IEnumerable/<T>
		IEnumerator<T> IEnumerable<T>.GetEnumerator() { return list.GetEnumerator(); }

		IEnumerator IEnumerable.GetEnumerator() { return list.GetEnumerator(); }
		#endregion

		#region IList
		int IList.Add(object value)
		{
			ThrowReadOnly();
			return -1; // To silence the compiler.
		}

		void IList.Clear() { ThrowReadOnly(); }

		bool IList.Contains(object value)
		{
			if (value is T) return list.Contains((T)value);
			if (value == null && default(T) == null) return list.Contains((T)value);
			return false;
		}

		int IList.IndexOf(object value)
		{
			if (value is T) return list.IndexOf((T)value);
			if (value == null && default(T) == null) return list.IndexOf((T)value);
			return -1;
		}

		void IList.Insert(int index, object value) { ThrowReadOnly(); }

		bool IList.IsFixedSize { get { return true; } }

		bool IList.IsReadOnly { get { return true; } }

		void IList.Remove(object value) { ThrowReadOnly(); }

		void IList.RemoveAt(int index) { ThrowReadOnly(); }

		object IList.this[int index]
		{
			get { return list[index]; }
			set { ThrowReadOnly(); }
		}
		#endregion

		#region ICollection
		void ICollection.CopyTo(Array array, int index) { ((ICollection)list).CopyTo(array, index); }

		bool ICollection.IsSynchronized { get { return false; } }

		object ICollection.SyncRoot { get { return ((ICollection)list).SyncRoot; } }
		#endregion
	}
}
