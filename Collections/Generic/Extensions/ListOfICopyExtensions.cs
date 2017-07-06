using System;
using System.Collections.Generic;
using Free.Core.Generic;

namespace Free.Core.Collections.Generic.Extensions
{
	/// <summary>
	/// Adds methods to the <see cref="List{T}"/> class. When the element type is a <b>class</b> and derived from <see cref="ICopy{T}"/>.
	/// </summary>
	public static class ListOfICopyExtensions
	{
		/// <summary>
		/// Creates a deep clone of a <see cref="List{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of the elements. Must be a <b>class</b> and derived from <see cref="ICopy{T}"/>.</typeparam>
		/// <param name="list">The list to copy.</param>
		/// <returns>The copy of the <paramref name="list"/>.</returns>
		public static List<T> Copy<T>(this List<T> list) where T : class, ICopy<T>
		{
			if (list == null) throw new ArgumentNullException(nameof(list));

			List<T> ret = (List<T>)Activator.CreateInstance(list.GetType());
			foreach (T t in list) ret.Add(t.Copy());
			return ret;
		}
	}
}
