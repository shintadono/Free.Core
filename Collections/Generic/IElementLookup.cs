using System.Collections.Generic;

namespace Free.Core.Collections.Generic
{
	/// <summary>
	/// Describes an interface for looking up elements using the item/index property.
	/// </summary>
	/// <typeparam name="TKey">The type of the key to look up an element for.</typeparam>
	/// <typeparam name="TValue">The type of the element to look up.</typeparam>
	public interface IElementLookup<TKey, TValue>
	{
		/// <summary>
		/// Gets the element for the specified <paramref name="key"/>.
		/// </summary>
		/// <param name="key">The key of the element to look up.</param>
		/// <returns>The element found for the specified <paramref name="key"/>.
		/// No behaviour is specified by this interface regard not found element;
		/// the implementation could return <b>default</b> of <typeparamref name="TValue"/>
		/// or <b>throw</b> an <see cref="KeyNotFoundException"/>.</returns>
		TValue this[TKey key] { get; }
	}
}
