using System.Diagnostics;

namespace Free.Core.Generic
{
	/// <summary>
	/// TODO
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[DebuggerDisplay("Pair = {First} => {Second}")]
	public class Pair<T>
	{
		/// <summary>
		/// The first element of the pair.
		/// </summary>
		public T First {get; set;}

		/// <summary>
		/// The second element of the pair.
		/// </summary>
		public T Second {get; set;}

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
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if(obj is Pair<T>)
			{
				return First.Equals((obj as Pair<T>).First) && Second.Equals((obj as Pair<T>).Second);	
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
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
