using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Free.Core.Collections.Generic
{
	/// <summary>
	/// TODO
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[ComVisible(false)]
	[DebuggerDisplay("Count = {Count}")]
	public class ReadOnlyList<T> : IList<T>, IList, IReadOnlyList<T>
	{
		/// <summary>
		/// TODO
		/// </summary>
		protected IList<T> list;

		/// <summary>
		/// TODO
		/// </summary>
		public ReadOnlyList() { }

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="collection"></param>
		public ReadOnlyList(IEnumerable<T> collection) { list=new List<T>(collection); }

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="collection"></param>
		public ReadOnlyList(params T[] collection) { list=new List<T>(collection); }

		void ThrowReadOnly() { throw new NotSupportedException("Collection is read-only."); }

		#region Implements IList<T>
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(T item) { return list.IndexOf(item); }

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, T item) { ThrowReadOnly(); }

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index) { ThrowReadOnly(); }

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public T this[int index]
		{
			get { return list[index]; }
			set { ThrowReadOnly(); }
		}
		#endregion

		#region Implements ICollection<T>
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="item"></param>
		public void Add(T item) { ThrowReadOnly(); }

		/// <summary>
		/// TODO
		/// </summary>
		public void Clear() { ThrowReadOnly(); }

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(T item) { return list.Contains(item); }

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(T[] array, int arrayIndex) { list.CopyTo(array, arrayIndex); }

		/// <summary>
		/// TODO
		/// </summary>
		public int Count { get { return list.Count; } }

		/// <summary>
		/// TODO
		/// </summary>
		public bool IsReadOnly { get { return true; } }

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(T item)
		{
			ThrowReadOnly();
			return false; // to silence the compiler
		}
		#endregion

		#region IEnumerable/<T>
		/// <summary>
		/// TODO
		/// </summary>
		/// <returns></returns>
		public IEnumerator<T> GetEnumerator() { return list.GetEnumerator(); }

		IEnumerator IEnumerable.GetEnumerator() { return list.GetEnumerator(); }
		#endregion

		#region IList
		int IList.Add(object value)
		{
			ThrowReadOnly();
			return -1; // to silence the compiler
		}

		bool IList.Contains(object value)
		{
			if(value is T) return list.Contains((T)value);
			if(value==null&&default(T)==null) return list.Contains((T)value);
			return false;
		}

		int IList.IndexOf(object value)
		{
			if(value is T) return list.IndexOf((T)value);
			if(value==null&&default(T)==null) return list.IndexOf((T)value);
			return -1;
		}

		void IList.Insert(int index, object value) { ThrowReadOnly(); }

		/// <summary>
		/// TODO
		/// </summary>
		public bool IsFixedSize { get { return true; } }

		void IList.Remove(object value) { ThrowReadOnly(); }

		object IList.this[int index]
		{
			get { return list[index]; }
			set { ThrowReadOnly(); }
		}
		#endregion

		#region ICollection
		void ICollection.CopyTo(Array array, int index) { ((ICollection)list).CopyTo(array, index); }

		/// <summary>
		/// TODO
		/// </summary>
		public bool IsSynchronized { get { return false; } }

		/// <summary>
		/// TODO
		/// </summary>
		public object SyncRoot { get { return ((ICollection)list).SyncRoot; } }
		#endregion
	}
}
