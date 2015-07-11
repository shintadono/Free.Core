using System.Diagnostics;

namespace Free.Core.Collections.Generic
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
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if(obj==null) return false;
			Pair<T> o=(Pair<T>)obj;
			return First.Equals(o.First)&&Second.Equals(o.Second);
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

	/// <summary>
	/// TODO
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class StructPair<T> where T : struct
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
		public StructPair(T first, T second)
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
			if(obj==null) return false;
			StructPair<T> o=(StructPair<T>)obj;
			return First.Equals(o.First)&&Second.Equals(o.Second);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return First.GetHashCode()^Second.GetHashCode();
		}
	}
}
