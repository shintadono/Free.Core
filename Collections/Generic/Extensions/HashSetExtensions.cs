using System;
using System.Collections.Generic;

namespace Free.Core.Collections.Generic.Extensions
{
	/// <summary>
	/// Adds methods to the <see cref="HashSet{T}"/> class.
	/// </summary>
	public static class HashSetExtensions
	{
		/// <summary>
		/// Returns all combinations of elements in the specified <paramref name="hashSet"/> as <see cref="Tuple{T1, T2}"/>.
		/// The order of the elements in the <see cref="Tuple{T1, T2}"/> is undefinied. Each combination of elements is only returned once.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the <paramref name="hashSet"/> and the returned <see cref="Tuple{T1, T2}"/>.</typeparam>
		/// <param name="hashSet">The hash set with the elements.</param>
		/// <returns>An enumeration of <see cref="Tuple{T1, T2}"/>, providing all combininations of elements.</returns>
		public static IEnumerable<Tuple<T, T>> ForAllCombinations<T>(this HashSet<T> hashSet)
		{
			if (null == hashSet) throw new ArgumentNullException(nameof(hashSet));

			return ForAllCombinationsEnumerable(hashSet);
		}

		static IEnumerable<Tuple<T, T>> ForAllCombinationsEnumerable<T>(this HashSet<T> hashSet)
		{
			if (hashSet.Count <= 1) yield break;

			//if (typeof(HashSet<T>.Enumerator).IsValueType)
			{
				HashSet<T>.Enumerator iter = hashSet.GetEnumerator();

				while (iter.MoveNext())
				{
					T a = iter.Current;
					HashSet<T>.Enumerator secondIter = iter; // Since the enumerator is a struct, we can copy the current content and don't need to fast forward the the right location.
					while (secondIter.MoveNext()) yield return new Tuple<T, T>(a, secondIter.Current);
				}
			}
			//else
			//{
			//	IEnumerator<T> secondIter = hashSet.GetEnumerator(); // We use a second iterator that we can reset to avoid creating instances over and over again.

			//	int count = 0;
			//	foreach (var a in hashSet)
			//	{
			//		count++;
			//		if (count >= hashSet.Count) yield break;

			//		secondIter.Reset();
			//		for (int i = 0; i < count; i++) secondIter.MoveNext(); // Fast forward to the right location in secondIter.

			//		while (secondIter.MoveNext()) yield return new Tuple<T, T>(a, secondIter.Current);
			//	}
			//}
		}

		/// <summary>
		/// For all combinations of elements in the specified <paramref name="hashSet"/> a specified <paramref name="action"/> is performed.
		/// The order of the elements in the <paramref name="action"/> is undefinied. Each combination of elements is only invoked once.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the <paramref name="hashSet"/> and <paramref name="action"/>.</typeparam>
		/// <param name="hashSet">The hash set with the elements.</param>
		/// <param name="action">The action to perform with each combination of elements.</param>
		public static void ForAllCombinations<T>(this HashSet<T> hashSet, Action<T, T> action)
		{
			if (null == hashSet) throw new ArgumentNullException(nameof(hashSet));
			if (null == action) throw new ArgumentNullException(nameof(action));
			if (hashSet.Count <= 1) return;

			//if (typeof(HashSet<T>.Enumerator).IsValueType)
			{
				HashSet<T>.Enumerator iter = hashSet.GetEnumerator();

				while (iter.MoveNext())
				{
					T a = iter.Current;
					HashSet<T>.Enumerator secondIter = iter; // Since the enumerator is a struct, we can copy the current content and don't need to fast forward the the right location.
					while (secondIter.MoveNext()) action(a, secondIter.Current);
				}
			}
			//else
			//{
			//	IEnumerator<T> secondIter = hashSet.GetEnumerator(); // We use a second iterator that we can reset to avoid creating instances over and over again.

			//	int count = 0;
			//	foreach (var a in hashSet)
			//	{
			//		count++;
			//		if (count >= hashSet.Count) return;

			//		secondIter.Reset();
			//		for (int i = 0; i < count; i++) secondIter.MoveNext(); // Fast forward to the right location in secondIter.

			//		while (secondIter.MoveNext()) action(a, secondIter.Current);
			//	}
			//}
		}
	}
}
