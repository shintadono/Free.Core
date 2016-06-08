using System;
using System.Collections;
using System.Collections.Generic;

namespace Free.Core.Collections.Generic
{
	/// <summary>
	/// Represents a collection of keys and values, with predictable iteration order.
	/// </summary>
	/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of the keys in the dictionary.</typeparam>
	public class LinkedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IDictionary
	{
		const string ExpectionMessageCollectionIsReadOnly = "Collection is read only.";
		const string ExpectionMessageWrongType = "Wrong type.";
		const string ExpectionMessageInvalidArrayType = "Invalid array type.";
		const string ExpectionMessageArrayPlusOffsetTooSmall = "Array (plus offset) too small.";
		const string ExpectionMessageArrayRankNotOne = "Array with rank not equal to one (1) is not supported.";
		const string ExpectionMessageArrayLowerBoundNotZero = "Array with lower bound not equal to zero (0) is not supported.";
		const string ExpectionMessageCollectionIsEmpty = "Collection is empty.";
		const string ExpectionMessageNoElementFoundMatchingThePredicateOrCollectionIsEmpty = "No element found matching the perdicate, or collection is empty.";

		#region Variables
		Dictionary<TKey, LinkedListNode<Tuple<TKey, TValue>>> dict;
		LinkedList<Tuple<TKey, TValue>> list;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key of the value to get or set.</param>
		/// <returns>The value associated with the specified key. If the specified key is not found,
		/// a get operation throws a <see cref="KeyNotFoundException"/>, and a set operation creates a new element with the specified key.</returns>
		public TValue this[TKey key]
		{
			get
			{
				return dict[key].Value.Item2;
			}
			set
			{
				if (!dict.ContainsKey(key))
				{
					LinkedListNode<Tuple<TKey, TValue>> node = new LinkedListNode<Tuple<TKey, TValue>>(new Tuple<TKey, TValue>(key, value));
					dict[key] = node;
					list.AddLast(node);
				}
				else dict[key].Value = new Tuple<TKey, TValue>(key, value);
			}
		}

		/// <summary>
		/// Gets the <see cref="IEqualityComparer{T}"/> that is used to determine equality of keys for the dictionary.
		/// </summary>
		public IEqualityComparer<TKey> Comparer { get { return dict.Comparer; } }

		/// <summary>
		/// Gets the number of key/value pairs contained in the <see cref="LinkedDictionary{TKey, TValue}"/>.
		/// </summary>
		public int Count { get { return dict.Count; } }

		/// <summary>
		/// Gets a collection containing the keys in the <see cref="LinkedDictionary{TKey, TValue}"/>.
		/// </summary>
		public ICollection<TKey> Keys { get { return new KeyCollection(this); } }

		/// <summary>
		/// Gets a collection containing the values in the <see cref="LinkedDictionary{TKey, TValue}"/>.
		/// </summary>
		public ICollection<TValue> Values { get { return new ValueCollection(this); } }
		#endregion

		#region Ctors
		/// <summary>
		/// Initializes a new instance of the <see cref="LinkedDictionary{TKey, TValue}"/> class that is empty.
		/// The initial capacity and an <see cref="IEqualityComparer{T}"/> can be specified.
		/// </summary>
		/// <param name="capacity">The initial number of elements that the <see cref="LinkedDictionary{TKey, TValue}"/> can contain.</param>
		/// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing keys,
		/// or <b>null</b> to use the default <see cref="EqualityComparer{T}"/> for the type of the key.</param>
		public LinkedDictionary(int capacity = 0, IEqualityComparer<TKey> comparer = null)
		{
			dict = new Dictionary<TKey, LinkedListNode<Tuple<TKey, TValue>>>(capacity, comparer);
			list = new LinkedList<Tuple<TKey, TValue>>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkedDictionary{TKey, TValue}"/> class that contains
		/// elements copied from the specified <see cref="IDictionary{TKey, TValue}"/>.
		/// An <see cref="IEqualityComparer{T}"/> can be specified.
		/// </summary>
		/// <param name="dictionary">The <see cref="IDictionary{TKey, TValue}"/> whose elements are copied to the new <see cref="LinkedDictionary{TKey, TValue}"/>.</param>
		/// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing keys,
		/// or <b>null</b> to use the default <see cref="EqualityComparer{T}"/> for the type of the key.</param>
		public LinkedDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer = null)
		{
			if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

			dict = new Dictionary<TKey, LinkedListNode<Tuple<TKey, TValue>>>(comparer);
			list = new LinkedList<Tuple<TKey, TValue>>();

			foreach (var pair in dictionary) Add(pair.Key, pair.Value);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkedDictionary{TKey, TValue}"/> class that is empty.
		/// An <see cref="IEqualityComparer{T}"/> can be specified.
		/// </summary>
		/// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing keys,
		/// or <b>null</b> to use the default <see cref="EqualityComparer{T}"/> for the type of the key.</param>
		public LinkedDictionary(IEqualityComparer<TKey> comparer)
		{
			dict = new Dictionary<TKey, LinkedListNode<Tuple<TKey, TValue>>>(comparer);
			list = new LinkedList<Tuple<TKey, TValue>>();
		}
		#endregion

		/// <summary>
		/// Adds the specified key and value to the dictionary.
		/// </summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="value">The value of the element to add. The value can be <b>null</b> for reference types.</param>
		public void Add(TKey key, TValue value)
		{
			LinkedListNode<Tuple<TKey, TValue>> node = new LinkedListNode<Tuple<TKey, TValue>>(new Tuple<TKey, TValue>(key, value));
			dict.Add(key, node);
			list.AddLast(node);
		}

		/// <summary>
		/// Removes all keys and values from a <see cref="LinkedDictionary{TKey, TValue}"/>.
		/// </summary>
		public void Clear()
		{
			dict.Clear();
			list.Clear();
		}

		/// <summary>
		/// Determines whether the <see cref="LinkedDictionary{TKey, TValue}"/> contains the specified key.
		/// </summary>
		/// <param name="key">The key to locate in the <see cref="LinkedDictionary{TKey, TValue}"/>.</param>
		/// <returns><b>true</b> if the <see cref="LinkedDictionary{TKey, TValue}"/> contains an element with the specified key; otherwise, <b>false</b>.</returns>
		public bool ContainsKey(TKey key)
		{
			return dict.ContainsKey(key);
		}

		/// <summary>
		/// Removes the value with the specified key from a <see cref="LinkedDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <returns><b>true</b> if the element is successfully found and removed; otherwise, <b>false</b>.
		/// This method returns <b>false</b> if key is not found in the <see cref="LinkedDictionary{TKey, TValue}"/>.</returns>
		public bool Remove(TKey key)
		{
			LinkedListNode<Tuple<TKey, TValue>> node;
			if (dict.TryGetValue(key, out node))
			{
				list.Remove(node);
				return dict.Remove(key);
			}

			return false;
		}

		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key of the value to get.</param>
		/// <param name="value">When this method returns, contains the value associated with the specified key,
		/// if the <paramref name="key"/> is found; otherwise, the default value for the type of the <paramref name="value"/> parameter.</param>
		/// <returns><b>true</b> if the <see cref="LinkedDictionary{TKey, TValue}"/> contains an element with the specified key; otherwise, <b>false</b>.</returns>
		public bool TryGetValue(TKey key, out TValue value)
		{
			LinkedListNode<Tuple<TKey, TValue>> node;
			if (dict.TryGetValue(key, out node))
			{
				value = node.Value.Item2;
				return true;
			}

			value = default(TValue);
			return false;
		}

		#region Implements IReadOnlyDictionary<TKey, TValue> explicit
		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys { get { return new KeyCollection(this); } }

		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values { get { return new ValueCollection(this); } }
		#endregion

		#region Implements ICollection<KeyValuePair<TKey, TValue>> explicit
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly { get { return false; } }

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) { Add(item.Key, item.Value); }

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			LinkedListNode<Tuple<TKey, TValue>> node;
			if (dict.TryGetValue(item.Key, out node)) return EqualityComparer<TValue>.Default.Equals(node.Value.Item2, item.Value);
			return false;
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (array == null) throw new ArgumentNullException(nameof(array));
			if (arrayIndex < 0 || arrayIndex >= array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
			if (array.Length - arrayIndex < Count) throw new ArgumentException(nameof(array), ExpectionMessageArrayPlusOffsetTooSmall);

			int offset = arrayIndex;
			foreach (var keyValue in list) array[offset++] = new KeyValuePair<TKey, TValue>(keyValue.Item1, keyValue.Item2);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			LinkedListNode<Tuple<TKey, TValue>> node;
			if (dict.TryGetValue(item.Key, out node))
			{
				if (EqualityComparer<TValue>.Default.Equals(node.Value.Item2, item.Value))
				{
					list.Remove(node);
					return dict.Remove(item.Key);
				}
			}

			return false;
		}
		#endregion

		#region Implements IDictionary explicit
		object IDictionary.this[object key]
		{
			get
			{
				return ((IDictionary)dict)[key];
			}
			set
			{
				if (key == null) throw new ArgumentNullException(nameof(key));
				if (value == null && !(default(TValue) == null)) throw new ArgumentNullException(nameof(value));

				try
				{
					TKey tempKey = (TKey)key;

					try
					{
						this[tempKey] = (TValue)value;
					}
					catch (InvalidCastException)
					{
						throw new ArgumentException(ExpectionMessageWrongType, nameof(value));
					}
				}
				catch (InvalidCastException)
				{
					throw new ArgumentException(ExpectionMessageWrongType, nameof(key));
				}
			}
		}

		bool IDictionary.IsFixedSize { get { return false; } }

		bool IDictionary.IsReadOnly { get { return false; } }

		ICollection IDictionary.Keys { get { return new KeyCollection(this); } }

		ICollection IDictionary.Values { get { return new ValueCollection(this); } }

		void IDictionary.Add(object key, object value)
		{
			if (key == null) throw new ArgumentNullException(nameof(key));
			if (value == null && !(default(TValue) == null)) throw new ArgumentNullException(nameof(value));

			try
			{
				TKey tempKey = (TKey)key;

				try
				{
					Add(tempKey, (TValue)value);
				}
				catch (InvalidCastException)
				{
					throw new ArgumentException(ExpectionMessageWrongType, nameof(value));
				}
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(ExpectionMessageWrongType, nameof(key));
			}
		}

		bool IDictionary.Contains(object key)
		{
			if (key == null) throw new ArgumentNullException(nameof(key));

			if (key is TKey) return ContainsKey((TKey)key);
			return false;
		}

		IDictionaryEnumerator IDictionary.GetEnumerator() { return new Enumerator(this, false); }

		void IDictionary.Remove(object key)
		{
			if (key == null) throw new ArgumentNullException(nameof(key));

			if (key is TKey) Remove((TKey)key);
		}
		#endregion

		#region Implements ICollection explicit
		bool ICollection.IsSynchronized { get { return false; } }

		object ICollection.SyncRoot { get { return ((ICollection)dict).SyncRoot; } }

		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			if (array == null) throw new ArgumentNullException(nameof(array));
			if (array.Rank != 1) throw new ArgumentException(nameof(array), ExpectionMessageArrayRankNotOne);
			if (array.GetLowerBound(0) != 0) throw new ArgumentException(nameof(array), ExpectionMessageArrayLowerBoundNotZero);
			if (arrayIndex < 0 || arrayIndex >= array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
			if (array.Length - arrayIndex < Count) throw new ArgumentException(nameof(array), ExpectionMessageArrayPlusOffsetTooSmall);

			KeyValuePair<TKey, TValue>[] values = array as KeyValuePair<TKey, TValue>[];
			if (values != null)
			{
				((ICollection<KeyValuePair<TKey, TValue>>)this).CopyTo(values, arrayIndex);
				return;
			}

			object[] objects = array as object[];
			if (objects == null) throw new ArgumentException(nameof(array), ExpectionMessageInvalidArrayType);

			int offset = arrayIndex;
			try
			{
				foreach (var keyValue in list) objects[offset++] = new KeyValuePair<TKey, TValue>(keyValue.Item1, keyValue.Item2);
			}
			catch (ArrayTypeMismatchException)
			{
				throw new ArgumentException(nameof(array), ExpectionMessageInvalidArrayType);
			}
		}
		#endregion

		#region Implements IEnumerable<KeyValuePair<TKey, TValue>>/IEnumerable explicit
		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() { return new Enumerator(this, true); }

		IEnumerator IEnumerable.GetEnumerator() { return new Enumerator(this, true); }
		#endregion

		#region Some LINQ operations, that should be faster than the LINQ extensions
		/// <summary>
		/// Returns the first element in a <see cref="LinkedDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <returns>The first element in the <see cref="LinkedDictionary{TKey, TValue}"/>.</returns>
		public KeyValuePair<TKey, TValue> First()
		{
			var first = list.First;
			if (first == null) throw new InvalidOperationException(ExpectionMessageCollectionIsEmpty);
			return new KeyValuePair<TKey, TValue>(first.Value.Item1, first.Value.Item2);
		}

		/// <summary>
		/// Returns the first element in a <see cref="LinkedDictionary{TKey, TValue}"/> that satisfies a specified condition.
		/// </summary>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The first element in the <see cref="LinkedDictionary{TKey, TValue}"/> that passes the test in the specified <paramref name="predicate"/> function.</returns>
		public KeyValuePair<TKey, TValue> First(Func<KeyValuePair<TKey, TValue>, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));

			foreach (var item in list)
			{
				KeyValuePair<TKey, TValue> kv = new KeyValuePair<TKey, TValue>(item.Item1, item.Item2);
				if (predicate(kv)) return kv;
			}

			throw new InvalidOperationException(ExpectionMessageNoElementFoundMatchingThePredicateOrCollectionIsEmpty);
		}

		/// <summary>
		/// Returns the first element of a <see cref="LinkedDictionary{TKey, TValue}"/>, or a default value if the <see cref="LinkedDictionary{TKey, TValue}"/> contains no elements.
		/// </summary>
		/// <returns><b>default(KeyValuePair&lt;TKey, TValue&gt;)</b> if the <see cref="LinkedDictionary{TKey, TValue}"/> is empty; otherwise, the first element in the <see cref="LinkedDictionary{TKey, TValue}"/>.</returns>
		public KeyValuePair<TKey, TValue> FirstOrDefault()
		{
			var first = list.First;
			if (first == null) return default(KeyValuePair<TKey, TValue>);
			return new KeyValuePair<TKey, TValue>(first.Value.Item1, first.Value.Item2);
		}

		/// <summary>
		/// Returns the first element in a <see cref="LinkedDictionary{TKey, TValue}"/> that satisfies a specified condition or a default value if no such element is found.
		/// </summary>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns><b>default(KeyValuePair&lt;TKey, TValue&gt;)</b> if the <see cref="LinkedDictionary{TKey, TValue}"/> is empty or if no element passes the test specified by <paramref name="predicate"/>; otherwise, the
		/// first element in the <see cref="LinkedDictionary{TKey, TValue}"/> that passes the test specified by <paramref name="predicate"/>.</returns>
		public KeyValuePair<TKey, TValue> FirstOrDefault(Func<KeyValuePair<TKey, TValue>, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));

			foreach (var item in list)
			{
				KeyValuePair<TKey, TValue> kv = new KeyValuePair<TKey, TValue>(item.Item1, item.Item2);
				if (predicate(kv)) return kv;
			}

			return default(KeyValuePair<TKey, TValue>);
		}

		/// <summary>
		/// Returns the last element of a <see cref="LinkedDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <returns>The last element in the <see cref="LinkedDictionary{TKey, TValue}"/>.</returns>
		public KeyValuePair<TKey, TValue> Last()
		{
			var last = list.Last;
			if (last == null) throw new InvalidOperationException(ExpectionMessageCollectionIsEmpty);
			return new KeyValuePair<TKey, TValue>(last.Value.Item1, last.Value.Item2);
		}

		/// <summary>
		/// Returns the last element in a <see cref="LinkedDictionary{TKey, TValue}"/> that satisfies a specified condition.
		/// </summary>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>The last element in the <see cref="LinkedDictionary{TKey, TValue}"/> that passes the test in the specified <paramref name="predicate"/> function.</returns>
		public KeyValuePair<TKey, TValue> Last(Func<KeyValuePair<TKey, TValue>, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));

			var current = list.Last;
			while (current != null)
			{
				KeyValuePair<TKey, TValue> kv = new KeyValuePair<TKey, TValue>(current.Value.Item1, current.Value.Item2);
				if (predicate(kv)) return kv;
				current = current.Previous;
			}

			throw new InvalidOperationException(ExpectionMessageNoElementFoundMatchingThePredicateOrCollectionIsEmpty);
		}

		/// <summary>
		/// Returns the last element of a <see cref="LinkedDictionary{TKey, TValue}"/>, or a default value if the <see cref="LinkedDictionary{TKey, TValue}"/> contains no elements.
		/// </summary>
		/// <returns><b>default(KeyValuePair&lt;TKey, TValue&gt;)</b> if the <see cref="LinkedDictionary{TKey, TValue}"/> is empty; otherwise, the last element in the <see cref="LinkedDictionary{TKey, TValue}"/>.</returns>
		public KeyValuePair<TKey, TValue> LastOrDefault()
		{
			var last = list.Last;
			if (last == null) return default(KeyValuePair<TKey, TValue>);
			return new KeyValuePair<TKey, TValue>(last.Value.Item1, last.Value.Item2);
		}

		/// <summary>
		/// Returns the last element in a <see cref="LinkedDictionary{TKey, TValue}"/> that satisfies a specified condition or a default value if no such element is found.
		/// </summary>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns><b>default(KeyValuePair&lt;TKey, TValue&gt;)</b> if the <see cref="LinkedDictionary{TKey, TValue}"/> is empty or if no element passes the test specified by <paramref name="predicate"/>; otherwise, the
		/// last element in the <see cref="LinkedDictionary{TKey, TValue}"/> that passes the test specified by <paramref name="predicate"/>.</returns>
		public KeyValuePair<TKey, TValue> LastOrDefault(Func<KeyValuePair<TKey, TValue>, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));

			var current = list.Last;
			while (current != null)
			{
				KeyValuePair<TKey, TValue> kv = new KeyValuePair<TKey, TValue>(current.Value.Item1, current.Value.Item2);
				if (predicate(kv)) return kv;
				current = current.Previous;
			}

			return default(KeyValuePair<TKey, TValue>);
		}

		/// <summary>
		/// Returns the element of a <see cref="LinkedDictionary{TKey, TValue}"/> in reverse order.
		/// </summary>
		/// <returns>A sequence whose elements correspond to those of the <see cref="LinkedDictionary{TKey, TValue}"/> in reverse order.</returns>
		public IEnumerable<KeyValuePair<TKey, TValue>> Reverse()
		{
			var current = list.Last;
			while (current != null)
			{
				yield return new KeyValuePair<TKey, TValue>(current.Value.Item1, current.Value.Item2);
				current = current.Previous;
			}
		}
		#endregion

		struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator
		{
			LinkedDictionary<TKey, TValue> linkedDictionary;
			LinkedList<Tuple<TKey, TValue>>.Enumerator enumerator;
			KeyValuePair<TKey, TValue> current;
			bool keyValuePair;
			bool bad;

			internal Enumerator(LinkedDictionary<TKey, TValue> linkedDictionary, bool keyValuePair)
			{
				this.linkedDictionary = linkedDictionary;
				enumerator = linkedDictionary.list.GetEnumerator();
				this.keyValuePair = keyValuePair;
				current = new KeyValuePair<TKey, TValue>();
				bad = true;
			}

			public bool MoveNext()
			{
				if (!enumerator.MoveNext())
				{
					if (!bad)
					{
						bad = true;
						current = new KeyValuePair<TKey, TValue>();
					}
					return false;
				}

				Tuple<TKey, TValue> keyValue = enumerator.Current;
				current = new KeyValuePair<TKey, TValue>(keyValue.Item1, keyValue.Item2);
				bad = false;
				return true;
			}

			public KeyValuePair<TKey, TValue> Current { get { return current; } }

			public void Dispose() { }

			#region Implements IEnumerator explicit
			object IEnumerator.Current
			{
				get
				{
					if (bad) throw new InvalidOperationException();

					if (keyValuePair) return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
					return new DictionaryEntry(current.Key, current.Value);
				}
			}

			void IEnumerator.Reset()
			{
				((IEnumerator)enumerator).Reset();
				current = new KeyValuePair<TKey, TValue>();
				bad = true; // We are before the first element.
			}
			#endregion

			#region Implements IDictionaryEnumerator explicit
			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					if (bad) throw new InvalidOperationException();

					return new DictionaryEntry(current.Key, current.Value);
				}
			}

			object IDictionaryEnumerator.Key
			{
				get
				{
					if (bad) throw new InvalidOperationException();

					return current.Key;
				}
			}

			object IDictionaryEnumerator.Value
			{
				get
				{
					if (bad) throw new InvalidOperationException();

					return current.Value;
				}
			}
			#endregion
		}

		sealed class KeyCollection : ICollection<TKey>, ICollection, IReadOnlyCollection<TKey>
		{
			LinkedDictionary<TKey, TValue> linkedDictionary;

			public KeyCollection(LinkedDictionary<TKey, TValue> linkedDictionary)
			{
				if (linkedDictionary == null) throw new ArgumentNullException(nameof(linkedDictionary));
				this.linkedDictionary = linkedDictionary;
			}

			public int Count { get { return linkedDictionary.Count; } }

			bool ICollection<TKey>.IsReadOnly { get { return true; } }

			bool ICollection.IsSynchronized { get { return false; } }

			object ICollection.SyncRoot { get { return ((ICollection)linkedDictionary).SyncRoot; } }

			void ICollection<TKey>.Add(TKey item) { throw new NotSupportedException(ExpectionMessageCollectionIsReadOnly); }

			void ICollection<TKey>.Clear() { throw new NotSupportedException(ExpectionMessageCollectionIsReadOnly); }

			bool ICollection<TKey>.Contains(TKey item) { return linkedDictionary.ContainsKey(item); }

			void ICollection<TKey>.CopyTo(TKey[] array, int arrayIndex)
			{
				if (array == null) throw new ArgumentNullException(nameof(array));
				if (arrayIndex < 0 || arrayIndex >= array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
				if (array.Length - arrayIndex < linkedDictionary.Count) throw new ArgumentException(nameof(array), ExpectionMessageArrayPlusOffsetTooSmall);

				int offset = arrayIndex;
				foreach (var keyValue in linkedDictionary.list) array[offset++] = keyValue.Item1;
			}

			bool ICollection<TKey>.Remove(TKey item) { throw new NotSupportedException(ExpectionMessageCollectionIsReadOnly); }

			void ICollection.CopyTo(Array array, int arrayIndex)
			{
				if (array == null) throw new ArgumentNullException(nameof(array));
				if (array.Rank != 1) throw new ArgumentException(nameof(array), ExpectionMessageArrayRankNotOne);
				if (array.GetLowerBound(0) != 0) throw new ArgumentException(nameof(array), ExpectionMessageArrayLowerBoundNotZero);
				if (arrayIndex < 0 || arrayIndex >= array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
				if (array.Length - arrayIndex < linkedDictionary.Count) throw new ArgumentException(nameof(array), ExpectionMessageArrayPlusOffsetTooSmall);

				TKey[] values = array as TKey[];
				if (values != null)
				{
					((ICollection<TKey>)this).CopyTo(values, arrayIndex);
					return;
				}

				object[] objects = array as object[];
				if (objects == null) throw new ArgumentException(nameof(array), ExpectionMessageInvalidArrayType);

				int offset = arrayIndex;
				try
				{
					foreach (var keyValue in linkedDictionary.list) objects[offset++] = keyValue.Item1;
				}
				catch (ArrayTypeMismatchException)
				{
					throw new ArgumentException(nameof(array), ExpectionMessageInvalidArrayType);
				}
			}

			IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() { foreach (var keyValue in linkedDictionary.list) yield return keyValue.Item1; }

			IEnumerator IEnumerable.GetEnumerator() { foreach (var keyValue in linkedDictionary.list) yield return keyValue.Item1; }
		}

		sealed class ValueCollection : ICollection<TValue>, ICollection, IReadOnlyCollection<TValue>
		{
			LinkedDictionary<TKey, TValue> linkedDictionary;

			public ValueCollection(LinkedDictionary<TKey, TValue> linkedDictionary)
			{
				if (linkedDictionary == null) throw new ArgumentNullException(nameof(linkedDictionary));
				this.linkedDictionary = linkedDictionary;
			}

			public int Count { get { return linkedDictionary.Count; } }

			bool ICollection<TValue>.IsReadOnly { get { return true; } }

			bool ICollection.IsSynchronized { get { return false; } }

			object ICollection.SyncRoot { get { return ((ICollection)linkedDictionary).SyncRoot; } }

			void ICollection<TValue>.Add(TValue item) { throw new NotSupportedException(ExpectionMessageCollectionIsReadOnly); }

			void ICollection<TValue>.Clear() { throw new NotSupportedException(ExpectionMessageCollectionIsReadOnly); }

			bool ICollection<TValue>.Contains(TValue item)
			{
				if (item == null)
				{
					foreach (var keyValue in linkedDictionary.list)
						if (keyValue.Item2 == null) return true;
				}
				else
				{
					EqualityComparer<TValue> c = EqualityComparer<TValue>.Default;
					foreach (var keyValue in linkedDictionary.list)
						if (c.Equals(keyValue.Item2, item)) return true;
				}

				return false;
			}

			void ICollection<TValue>.CopyTo(TValue[] array, int arrayIndex)
			{
				if (array == null) throw new ArgumentNullException(nameof(array));
				if (arrayIndex < 0 || arrayIndex >= array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
				if (array.Length - arrayIndex < linkedDictionary.Count) throw new ArgumentException(nameof(array), ExpectionMessageArrayPlusOffsetTooSmall);

				int offset = arrayIndex;
				foreach (var keyValue in linkedDictionary.list) array[offset++] = keyValue.Item2;
			}

			bool ICollection<TValue>.Remove(TValue item) { throw new NotSupportedException(ExpectionMessageCollectionIsReadOnly); }

			void ICollection.CopyTo(Array array, int arrayIndex)
			{
				if (array == null) throw new ArgumentNullException(nameof(array));
				if (array.Rank != 1) throw new ArgumentException(nameof(array), ExpectionMessageArrayRankNotOne);
				if (array.GetLowerBound(0) != 0) throw new ArgumentException(nameof(array), ExpectionMessageArrayLowerBoundNotZero);
				if (arrayIndex < 0 || arrayIndex >= array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
				if (array.Length - arrayIndex < linkedDictionary.Count) throw new ArgumentException(nameof(array), ExpectionMessageArrayPlusOffsetTooSmall);

				TValue[] values = array as TValue[];
				if (values != null)
				{
					((ICollection<TValue>)this).CopyTo(values, arrayIndex);
					return;
				}

				object[] objects = array as object[];
				if (objects == null) throw new ArgumentException(nameof(array), ExpectionMessageInvalidArrayType);

				int offset = arrayIndex;
				try
				{
					foreach (var keyValue in linkedDictionary.list) objects[offset++] = keyValue.Item2;
				}
				catch (ArrayTypeMismatchException)
				{
					throw new ArgumentException(nameof(array), ExpectionMessageInvalidArrayType);
				}
			}

			IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() { foreach (var keyValue in linkedDictionary.list) yield return keyValue.Item2; }

			IEnumerator IEnumerable.GetEnumerator() { foreach (var keyValue in linkedDictionary.list) yield return keyValue.Item2; }
		}
	}
}
