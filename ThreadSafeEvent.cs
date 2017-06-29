using System;

namespace Free.Core
{
	/// <summary>
	/// Describes a thread safe event.
	/// </summary>
	public class ThreadSafeEvent : IDisposable
	{
		EventHandler internalEventHandler;
		readonly object internalEventHandlerLock = new object();

		/// <summary>
		/// Encapsulates the event.
		/// </summary>
		public event EventHandler Event
		{
			add
			{
				lock (internalEventHandlerLock)
				{
					internalEventHandler += value;
				}
			}
			remove
			{
				lock (internalEventHandlerLock)
				{
					internalEventHandler -= value;
				}
			}
		}

		/// <summary>
		/// Fires the event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event arguments.</param>
		public virtual void Fire(object sender, EventArgs e)
		{
			EventHandler handler;
			lock (internalEventHandlerLock)
			{
				handler = internalEventHandler;
			}
			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/// <summary>
		/// Removes all event handler from the event.
		/// </summary>
		public void RemoveAll()
		{
			lock (internalEventHandlerLock)
			{
				internalEventHandler = null;
			}
		}

		void IDisposable.Dispose()
		{
			RemoveAll();
		}
	}

	/// <summary>
	/// Describes a generic thread safe event.
	/// </summary>
	/// <typeparam name="T">Must be <see cref="EventArgs"/> or based on it.</typeparam>
	public sealed class ThreadSafeEvent<T> : IDisposable where T : EventArgs
	{
		EventHandler<T> internalEventHandler;
		readonly object internalEventHandlerLock = new object();

		/// <summary>
		/// Encapsulates the event.
		/// </summary>
		public event EventHandler<T> Event
		{
			add
			{
				lock (internalEventHandlerLock)
				{
					internalEventHandler += value;
				}
			}
			remove
			{
				lock (internalEventHandlerLock)
				{
					internalEventHandler -= value;
				}
			}
		}

		/// <summary>
		/// Fires the event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event arguments.</param>
		public void Fire(object sender, T e)
		{
			EventHandler<T> handler;
			lock (internalEventHandlerLock)
			{
				handler = internalEventHandler;
			}

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/// <summary>
		/// Removes all event handler from the event.
		/// </summary>
		public void RemoveAll()
		{
			lock (internalEventHandlerLock)
			{
				internalEventHandler = null;
			}
		}

		void IDisposable.Dispose()
		{
			RemoveAll();
		}
	}
}
