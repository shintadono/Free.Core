using System;

namespace Free.Core.Generic
{
	/// <summary>
	/// Similar in purpose as <see cref="ICloneable"/> but generic. Shall return a deep clone!
	/// </summary>
	/// <typeparam name="T">Type of the object implementing <see cref="ICopy{T}"/>.</typeparam>
	public interface ICopy<T>
	{
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		T Copy();
	}
}
