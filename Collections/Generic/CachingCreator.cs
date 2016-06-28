using System;
using System.Collections.Generic;

namespace Free.Core.Collections.Generic
{
	/// <summary>
	/// Represents a caching call to a object creation method.
	/// </summary>
	/// <typeparam name="TKey">The type of the key (input for creation method).</typeparam>
	/// <typeparam name="TValue">The type of the elements to be created.</typeparam>
	public sealed class CachingCreator<TKey, TValue> : IElementLookup<TKey, TValue>
	{
		/// <summary>
		/// The cache.
		/// </summary>
		Dictionary<TKey, TValue> cache = new Dictionary<TKey, TValue>();

		/// <summary>
		/// The create method.
		/// </summary>
		Func<TKey, TValue> createMethod;

		/// <summary>
		/// Initialized an instance of <see cref="CachingCreator{TKey, TValue}"/> with a <paramref name="create"/> method.
		/// </summary>
		/// <param name="create">The create method. Must not be <b>null</b>.</param>
		public CachingCreator(Func<TKey, TValue> create)
		{
			if (create == null) throw new ArgumentNullException(nameof(create));

			createMethod = create;
		}

		/// <summary>
		/// Gets, sets or creates elements for a specified key. (Creates, when the element was not created yet;
		/// gets, when the element was created or set, using the set property, prior to the get call.)
		/// </summary>
		/// <param name="key">The key of the element to get, set or create.</param>
		/// <returns>The created element or the element retrieved from the cache.</returns>
		public TValue this[TKey key]
		{
			get
			{
				TValue ret;
				if (cache.TryGetValue(key, out ret)) return ret;

				ret = createMethod(key);
				cache.Add(key, ret);
				return ret;
			}
			set
			{
				cache[key] = value;
			}
		}
	}
}
