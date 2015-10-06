using System.Diagnostics;

namespace Free.Core.Generic
{
	/// <summary>
	/// A tuple of 2 elements of the same type.
	/// </summary>
	/// <typeparam name="T">Type of the elements.</typeparam>
	[DebuggerDisplay("Pair = {First} => {Second}")]
	public class Pair<T>
	{
		/// <summary>
		/// The first element of the pair.
		/// </summary>
		public T First;

		/// <summary>
		/// The second element of the pair.
		/// </summary>
		public T Second;

		/// <summary>
		/// Creates a pair.
		/// </summary>
		/// <param name="first">The first element for the pair.</param>
		/// <param name="second">The second element for the pair.</param>
		public Pair(T first, T second)
		{
			First=first;
			Second=second;
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current instance.
		/// </summary>
		/// <param name="obj">The object to compare with the current instance.</param>
		/// <returns><b>true</b> if the specified object is equal to the current instance; otherwise, <b>false</b>.</returns>
		public override bool Equals(object obj)
		{
			Pair<T> o = obj as Pair<T>;
			if (o == null) return false;
			return First.Equals(o.First) && Second.Equals(o.Second);
		}

		/// <summary>
		/// Get a hash code for the current instance.
		/// </summary>
		/// <returns>A hash code for the current instance.</returns>
		public override int GetHashCode()
		{
			if((object)First==null)
			{
				if((object)Second==null) return 0;
				return (-1)^Second.GetHashCode();
			}

			if((object)Second==null) return First.GetHashCode();
			return First.GetHashCode()^Second.GetHashCode();
		}
	}
}
