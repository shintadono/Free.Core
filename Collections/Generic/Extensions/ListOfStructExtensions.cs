// This causes ambiguities with ListOfICopyExtensions, even though there is proper distinction provided by the where clause,
// but the compiler somehow does not recognizing it. The only feature this extension provides above the List<T> constructor is,
// that it also handles classes derived from List<>. Can be re-enabled, when the compiler is able to handle this.

//using System;
//using System.Collections.Generic;

//namespace Free.Core.Collections.Generic.Extensions
//{
//	/// <summary>
//	/// Added methods to the <see cref="List{T}"/> class. When the element type is a value type (<b>int</b>, <b>struct</b>, etc.).
//	/// </summary>
//	public static class ListOfStructExtensions
//	{
//		/// <summary>
//		/// Creates a 'deep' clone of a <see cref="List{T}"/>.
//		/// </summary>
//		/// <typeparam name="T">The type of the elements. Must be a value type (<b>int</b>, <b>struct</b>, etc.).</typeparam>
//		/// <param name="list">The list to copy.</param>
//		/// <returns>The copy of the <paramref name="list"/>.</returns>
//		public static List<T> Copy<T>(this List<T> list) where T : struct
//		{
//			if (list == null) throw new ArgumentNullException(nameof(list));

//			List<T> ret = (List<T>)Activator.CreateInstance(list.GetType());
//			foreach (T item in list) ret.Add(item);
//			return ret;
//		}
//	}
//}
