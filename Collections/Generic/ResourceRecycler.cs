using System;
using System.Collections.Generic;

namespace Free.Core.Collections.Generic
{
	/// <summary>
	/// Manages, caches and recycles resources that are heavy on "Create&amp;Release"-style resource management.
	/// </summary>
	/// <typeparam name="K">The type for the key. Must be a class! (<b>null</b> signals unsed resources.)</typeparam>
	/// <typeparam name="T">The type for the resources.</typeparam>
	public class ResourceRecycler<K, T> where K : class
	{
		/// <summary>
		/// The cache tile informations.
		/// </summary>
		class Entry
		{
			/// <summary>
			/// For double-linked list.
			/// </summary>
			public Entry Previous = null, Next = null;

			/// <summary>
			/// Stored key.
			/// </summary>
			public K Key = null;

			/// <summary>
			/// Stored resource.
			/// </summary>
			public T Resource;

			/// <summary>
			/// Creates an instance with a given resource.
			/// </summary>
			/// <param name="resource">The resource to store.</param>
			public Entry(T resource)
			{
				Resource = resource;
			}
		}

		/// <summary>
		/// Dictionary for O(log n) search operations.
		/// </summary>
		Dictionary<K, Entry> dict = new Dictionary<K, Entry>();

		/// <summary>
		/// Double-linked list for fast re-arrange operations.
		/// </summary>
		Entry Head = null, Tail = null;

		/// <summary>
		/// Creates an empty instance.
		/// </summary>
		public ResourceRecycler() { }

		/// <summary>
		/// Creates an instance filled with entries initialised from given resources.
		/// </summary>
		/// <param name="resources">The resources to initialise the cache with.</param>
		public ResourceRecycler(T[] resources)
		{
			Init(resources);
		}

		/// <summary>
		/// Clears the cache, removes all resources.
		/// </summary>
		public void Clear()
		{
			dict.Clear();

			Entry h = Head;
			while (h != null)
			{
				Entry hNext = h.Next;
				h.Previous = h.Next = null;
				h = hNext;
			}

			Head = Tail = null;
		}

		/// <summary>
		/// Fills the instance with entries from given resources. All previous managed resources will be removed.
		/// </summary>
		/// <param name="resources">The resources to initialise the cache with.</param>
		public void Init(T[] resources)
		{
			if (resources == null) throw new ArgumentNullException("resources");
			if (resources.Length == 0) throw new ArgumentException("Must not be empty.", "resources");

			Clear();

			Tail = Head = new Entry(resources[0]);

			for (int i = 1; i < resources.Length; i++)
			{
				Tail.Next = new Entry(resources[i]);
				Tail.Next.Previous = Tail;
				Tail = Tail.Next;
			}
		}

		/// <summary>
		/// Determines whether the cache contains the specified key.
		/// </summary>
		/// <param name="key">The key to locate in the cache.</param>
		/// <returns><b>true</b> if the cache contains an element with the specified <paramref name="key"/>; otherwise, <b>false</b>.</returns>
		public bool ContainsKey(K key)
		{
			if (key == null) throw new ArgumentNullException("key");

			return dict.ContainsKey(key);
		}

		/// <summary>
		/// Gets the resource associated with the specified key.
		/// </summary>
		/// <param name="key">The key of the resource to get.</param>
		/// <returns>The resource for the specified <paramref name="key"/>.</returns>
		public T GetResource(K key)
		{
			if (key == null) throw new ArgumentNullException("key");
			return dict[key].Resource;
		}

		/// <summary>
		/// Gets the resource associated with the specified key.
		/// </summary>
		/// <param name="key">The key of the resource to get.</param>
		/// <param name="resource">Returns the resource for the specified <paramref name="key"/>.</param>
		/// <returns><b>true</b> if the cache contains the resource for the specified <paramref name="key"/>; otherwise, <b>false</b>.</returns>
		public bool TryGetResource(K key, out T resource)
		{
			if (key == null) throw new ArgumentNullException("key");

			Entry entry = null;
			if (!dict.TryGetValue(key, out entry))
			{
				resource = default(T);
				return false;
			}

			resource = entry.Resource;
			return true;
		}

		/// <summary>
		/// Moves a key to the top of the cache. All usages of an element of should move them to the top to signal recent usage.
		/// Least recent objects will get collected at the bottom of the cache and can be recycled using <see cref="RecycleToTop"/>.
		/// </summary>
		/// <param name="key">The key to move.</param>
		/// <returns><b>true</b> if the cache contains an element with the specified <paramref name="key"/>; otherwise, <b>false</b>.</returns>
		public bool MoveToTop(K key)
		{
			if (key == null) throw new ArgumentNullException("key");

			Entry entry = null;
			if (!dict.TryGetValue(key, out entry)) return false;

			// Check, whether we're already at the top.
			if (entry.Previous == null) return true;

			// Remove entry.
			entry.Previous.Next = entry.Next;
			if (entry.Next != null) entry.Next.Previous = entry.Previous;

			// Prepare new head.
			entry.Next = Head;
			entry.Previous = null;

			// Prepare old head.
			Head.Previous = entry;

			// Switch head.
			Head = entry;

			return true;
		}

		/// <summary>
		/// Recycles an resource from the bottom of the cache, giving it a new perpose (<paramref name="newKey"/>) and sending it to the top of the cache.
		/// </summary>
		/// <param name="newKey">The new key for the resource.</param>
		/// <returns>The recylcled resource.</returns>
		public T RecycleToTop(K newKey)
		{
			if (newKey == null) throw new ArgumentNullException("newKey");

			if (Tail == null || Head == null) throw new InvalidOperationException("Cache is empty. Recycling not possible.");

			if (Tail != Head)
			{
				// Move Tail to Head
				Entry entry = Tail;

				Tail = Tail.Previous;
				Tail.Next = null;

				entry.Previous = null;
				entry.Next = Head;

				Head.Previous = entry;
				Head = entry;
			}

			dict.Remove(Head.Key);

			Head.Key = newKey;

			dict.Add(newKey, Head);

			return Head.Resource;
		}
	}
}
