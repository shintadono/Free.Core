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
		public static IEnumerable<Tuple<T, T>> GetAllCombinations<T>(this HashSet<T> hashSet)
		{
			if (null == hashSet) throw new ArgumentNullException(nameof(hashSet));

			return GetAllCombinationsEnumerable(hashSet);
		}

		static IEnumerable<Tuple<T, T>> GetAllCombinationsEnumerable<T>(this HashSet<T> hashSet)
		{
			if (hashSet.Count <= 1) yield break;

			IEnumerator<T> secondIter = hashSet.GetEnumerator(); // We use a second iterator that we can reset to avoid creating instances over and over again.

			int count = 0;
			foreach (var a in hashSet)
			{
				count++;
				secondIter.Reset();
				for (int i = 0; i < count; i++) secondIter.MoveNext(); // Skip count in secondIter.

				while (secondIter.MoveNext()) yield return new Tuple<T, T>(a, secondIter.Current);
			}
		}

		/// <summary>
		/// Visits all combination of elements in the specified <paramref name="hashSet"/> and performing a specified <paramref name="action"/>.
		/// The order of the elements in the <paramref name="action"/> is undefinied. Each combination of elements is only invoked once.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the <paramref name="hashSet"/> and <paramref name="action"/>.</typeparam>
		/// <param name="hashSet">The hash set with the elements.</param>
		/// <param name="action">The action to perform with each combination of elements.</param>
		public static void VisitAllCombinations<T>(this HashSet<T> hashSet, Action<T, T> action)
		{
			if (null == hashSet) throw new ArgumentNullException(nameof(hashSet));
			if (null == action) throw new ArgumentNullException(nameof(action));
			if (hashSet.Count <= 1) return;

			IEnumerator<T> secondIter = hashSet.GetEnumerator(); // We use a second iterator that we can reset to avoid creating instances over and over again.

			int count = 0;
			foreach (var a in hashSet)
			{
				count++;
				secondIter.Reset();
				for (int i = 0; i < count; i++) secondIter.MoveNext(); // Skip count in secondIter.

				while (secondIter.MoveNext()) action(a, secondIter.Current);
			}
		}
	}
}
