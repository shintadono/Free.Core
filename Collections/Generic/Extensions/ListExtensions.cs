using System;
using System.Collections.Generic;

namespace Free.Core.Collections.Generic.Extensions
{
	/// <summary>
	/// Added methods to the <see cref="List{T}"/> class.
	/// </summary>
	public static class ListExtensions
	{
		/// <summary>
		/// Re-arrange (in-place) the elements of a list.
		/// </summary>
		/// <typeparam name="T">Type of the <see cref="List{T}"/>.</typeparam>
		/// <param name="list">The list to be re-arranged.</param>
		/// <param name="offset">If positive, the index that will become the new start of the list. If negative list.Count-offset will become the new start.</param>
		public static void Rotate<T>(this List<T> list, int offset)
		{
			if (list == null) throw new ArgumentNullException(nameof(list));
			if (list.Count == 0) return;

			offset %= list.Count;
			if (offset == 0) return;

			if (offset < 0) // Add to front, remove from back.
			{
				list.InsertRange(0, list.GetRange(list.Count + offset, -offset));
				list.RemoveRange(list.Count + offset, -offset);
			}
			else // Remove from front, add to back.
			{
				list.InsertRange(list.Count, list.GetRange(0, offset));
				list.RemoveRange(0, offset);
			}
		}

		/// <summary>
		/// Re-arranges the elements of a list.
		/// </summary>
		/// <typeparam name="T">Type of the <see cref="List{T}"/>.</typeparam>
		/// <param name="list">The list.</param>
		/// <param name="offset">If positive, the index that will become the new start of the list. If negative list.Count-offset will become the new start.</param>
		/// <returns>A <see cref="List{T}"/> if the element re-arranged.</returns>
		public static List<T> Rotated<T>(this List<T> list, int offset)
		{
			if (list == null) throw new ArgumentNullException(nameof(list));
			if (list.Count == 0) return new List<T>();

			offset %= list.Count;
			if (offset == 0) return new List<T>(list);

			List<T> ret = new List<T>();

			if (offset < 0)
			{
				ret.AddRange(list.GetRange(list.Count + offset, -offset));
				ret.AddRange(list.GetRange(0, list.Count + offset));
			}
			else
			{
				ret.AddRange(list.GetRange(offset, list.Count - offset));
				ret.AddRange(list.GetRange(0, offset));
			}

			return ret;
		}
	}
}
