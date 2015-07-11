using System;

namespace Free.Core
{
	/// <summary>
	/// TODO
	/// </summary>
	public class ThreadSafeEvent
	{
		EventHandler internalEventHandler;
		readonly object internalEventHandlerLock=new object();

		/// <summary>
		/// Encapsulates the event.
		/// </summary>
		public event EventHandler Event
		{
			add
			{
				lock(internalEventHandlerLock)
				{
					internalEventHandler+=value;
				}
			}
			remove
			{
				lock(internalEventHandlerLock)
				{
					internalEventHandler-=value;
				}
			}
		}

		/// <summary>
		/// Fires the event.
		/// </summary>
		public virtual void Fire(object sender, EventArgs e)
		{
			EventHandler handler;
			lock(internalEventHandlerLock)
			{
				handler=internalEventHandler;
			}
			if(handler!=null)
			{
				handler(sender, e);
			}
		}
	}

	/// <summary>
	/// TODO
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class ThreadSafeEvent<T> : IDisposable where T : EventArgs
	{
		EventHandler<T> internalEventHandler;
		readonly object internalEventHandlerLock=new object();

		/// <summary>
		/// Encapsulates the event.
		/// </summary>
		public event EventHandler<T> Event
		{
			add
			{
				lock(internalEventHandlerLock)
				{
					internalEventHandler+=value;
				}
			}
			remove
			{
				lock(internalEventHandlerLock)
				{
					internalEventHandler-=value;
				}
			}
		}

		/// <summary>
		/// Fires the event.
		/// </summary>
		public void Fire(object sender, T e)
		{
			EventHandler<T> handler;
			lock(internalEventHandlerLock)
			{
				handler=internalEventHandler;
			}

			if(handler!=null)
			{
				handler(sender, e);
			}
		}

		/// <summary>
		/// TODO
		/// </summary>
		public void RemoveAll()
		{
			lock(internalEventHandlerLock)
			{
				internalEventHandler=null;
			}
		}

		/// <summary>
		/// TODO
		/// </summary>
		public void Dispose()
		{
			RemoveAll();
		}
	}
}
