using System.Collections.Generic;

namespace StardewValley
{
	/// <summary>
	/// JCF: I added this type from my own codebase.
	///
	///      This is NOT threadsafe. If more than one thread could potentially call Get/Return 
	///      then put a lock around those calls (just lock around Get/Return, you do not need to
	///      lock around whatever work you do).
	///
	///      Protip: Use a finally block to ensure the list gets returned even if an exception occurs
	///              and is handled somewhere higher in the callstack.
	/// </summary>
	public class ListPool<T>
	{
		private readonly Stack<List<T>> _in;

		public ListPool()
		{
			_in = new Stack<List<T>>();
			_in.Push(new List<T>());
		}

		public List<T> Get()
		{
			if (_in.Count == 0)
			{
				_in.Push(new List<T>());
			}
			return _in.Pop();
		}

		public void Return(List<T> list)
		{
			list.Clear();
			_in.Push(list);
		}
	}
}
