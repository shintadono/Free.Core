using System;
using System.IO;
using System.Runtime.Remoting;

namespace Free.Core.IO
{
	/// <summary>
	/// Adds a layer on another stream, that protects that stream from being closed (by disposing or closing of this stream). This class cannot be inherited.
	/// </summary>
	/// <threadsafety static="true" instance="true"/>
	public sealed class NonClosingStream : Stream
	{
		static string closedError="Stream has been closed.";

		Stream stream;
		bool closed=false;

		/// <summary>
		/// Initializes a new instance of the <see cref="NonClosingStream"/> class.
		/// </summary>
		/// <param name="stream">An instance of the <see cref="Stream"/> class. Must not be <c>null</c>.</param>
		/// <exception cref="ArgumentNullException">stream is <c>null</c>.</exception>
		public NonClosingStream(Stream stream)
		{
			if(stream==null) throw new ArgumentNullException("stream");
			this.stream=stream;
		}

		#region Properties
		/// <summary>
		/// Gets the base stream of this stream.
		/// </summary>
		/// <value>The base stream of this stream.</value>
		public Stream BaseStream { get { return stream; } }

		/// <summary>
		/// Gets a value indicating whether the base stream supports reading.
		/// </summary>
		/// <value><c>true</c> if the base stream supports reading; otherwise, <c>false</c>.</value>
		public override bool CanRead { get { return closed?false:stream.CanRead; } }

		/// <summary>
		/// Gets a value indicating whether the base stream supports seeking.
		/// </summary>
		/// <value><c>true</c> if the base stream supports seeking; otherwise, <c>false</c>.</value>
		public override bool CanSeek { get { return closed?false:stream.CanSeek; } }

		/// <summary>
		/// Gets a value indicating whether the base stream can time out.
		/// </summary>
		/// <value><c>true</c> if the base stream can time out; otherwise, <c>false</c>.</value>
		public override bool CanTimeout { get { return closed?false:stream.CanTimeout; } }

		/// <summary>
		/// Gets a value indicating whether the base stream supports writing.
		/// </summary>
		/// <value><c>true</c> if the base stream supports writing; otherwise, <c>false</c>.</value>
		public override bool CanWrite { get { return closed?false:stream.CanWrite; } }

		/// <summary>
		/// Gets the length of the base stream in bytes.
		/// </summary>
		/// <value>The length of the base stream in bytes as a <see cref="Int64"/> value.</value>
		/// <exception cref="ObjectDisposedException">If stream has been closed.</exception>
		public override long Length { get { if(closed) throw new ObjectDisposedException(null, closedError); return stream.Length; } }

		/// <summary>
		/// Gets or sets the position within the base stream.
		/// </summary>
		/// <value>The position within the base stream in bytes as a <see cref="Int64"/> value.</value>
		/// <exception cref="ObjectDisposedException">If stream has been closed.</exception>
		public override long Position
		{
			get { if(closed) throw new ObjectDisposedException(null, closedError); return stream.Position; }
			set { if(closed) throw new ObjectDisposedException(null, closedError); stream.Position=value; }
		}

		/// <summary>
		/// Gets or sets the time how long the base stream will try to read before timing out.
		/// </summary>
		/// <value>The time (in milliseconds) how long the base stream will try to read before timing out as a <see cref="Int32"/> value.</value>
		/// <exception cref="ObjectDisposedException">If stream has been closed.</exception>
		public override int ReadTimeout
		{
			get { if(closed) throw new ObjectDisposedException(null, closedError); return stream.ReadTimeout; }
			set { if(closed) throw new ObjectDisposedException(null, closedError); stream.ReadTimeout=value; }
		}

		/// <summary>
		/// Gets or sets the time how long the base stream will try to write before timing out.
		/// </summary>
		/// <value>The time (in milliseconds) how long the base stream will try to write before timing out as a <see cref="Int32"/> value.</value>
		/// <exception cref="ObjectDisposedException">If stream has been closed.</exception>
		public override int WriteTimeout
		{
			get { if(closed) throw new ObjectDisposedException(null, closedError); return stream.WriteTimeout; }
			set { if(closed) throw new ObjectDisposedException(null, closedError); stream.WriteTimeout=value; }
		}
		#endregion

		#region Methodes
		/// <summary>
		/// Begins an asynchronous read operation.
		/// </summary>
		/// <param name="buffer">The array of <see cref="Byte"/>s to read the data into.</param>
		/// <param name="offset">The offset in the array of <see cref="Byte"/>s at which to begin writing data read from the base stream.</param>
		/// <param name="count">The maximum number of bytes to read from the base stream.</param>
		/// <param name="callback">The callback to be called when the read is complete (optional).</param>
		/// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from another requests.</param>
		/// <returns>An <see cref="IAsyncResult"/> that represents the asynchronous read, which could still be pending.</returns>
		/// <exception cref="ObjectDisposedException">If stream has been closed.</exception>
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if(closed) throw new ObjectDisposedException(null, closedError);
			return stream.BeginRead(buffer, offset, count, callback, state);
		}

		/// <summary>
		/// Begins an asynchronous write operation.
		/// </summary>
		/// <param name="buffer">The array of <see cref="Byte"/>s to write the data into.</param>
		/// <param name="offset">The offset in the array of <see cref="Byte"/>s at which to begin writing data to the base stream.</param>
		/// <param name="count">The maximum number of bytes to write to the base stream.</param>
		/// <param name="callback">The callback to be called when the write is complete (optional).</param>
		/// <param name="state">A user-provided object that distinguishes this particular asynchronous write request from another requests.</param>
		/// <returns>An <see cref="IAsyncResult"/> that represents the asynchronous write, which could still be pending.</returns>
		/// <exception cref="ObjectDisposedException">If stream has been closed.</exception>
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if(closed) throw new ObjectDisposedException(null, closedError);
			return stream.BeginWrite(buffer, offset, count, callback, state);
		}

		/// <summary>
		/// Closes this stream, by flushing and disconnecting the base stream. The base stream won't be closed.
		/// </summary>
		public override void Close()
		{
			if(!closed) stream.Flush();
			stream=null;
			closed=true;
		}

		/// <summary>
		/// Throws a <see cref="NotSupportedException"/> instead of creating an object that contains all the relevant information required to generate a proxy used to communicate with a remote object.
		/// </summary>
		/// <param name="requestedType">The <see cref="Type"/> of the object that the new <see cref="ObjRef"/> will reference.</param>
		/// <returns>Nothing, especially not the information required to generate a proxy.</returns>
		/// <exception cref="NotSupportedException">Always.</exception>
		public override ObjRef CreateObjRef(Type requestedType) { throw new NotSupportedException(); }

		/// <summary>
		/// Waits for the pending asynchronous read to complete.
		/// </summary>
		/// <param name="asyncResult">The reference to the pending asynchronous request to finish.</param>
		/// <returns>The number of bytes read from the base stream as an <see cref="Int32"/> value, between zero (0) and the number of bytes you requested. Streams return zero (0) only at the end of the stream, otherwise, they should block until at least one byte is available.</returns>
		/// <exception cref="ObjectDisposedException">If stream has been closed.</exception>
		public override int EndRead(IAsyncResult asyncResult)
		{
			if(closed) throw new ObjectDisposedException(null, closedError);
			return stream.EndRead(asyncResult);
		}

		/// <summary>
		/// Ends an asynchronous write operation.
		/// </summary>
		/// <param name="asyncResult">The reference to the pending asynchronous request to finish.</param>
		/// <exception cref="ObjectDisposedException">If stream has been closed.</exception>
		public override void EndWrite(IAsyncResult asyncResult)
		{
			if(closed) throw new ObjectDisposedException(null, closedError);
			stream.EndWrite(asyncResult);
		}

		/// <summary>
		/// Flushes the base stream.
		/// </summary>
		/// <exception cref="ObjectDisposedException">If stream has been closed.</exception>
		public override void Flush()
		{
			if(closed) throw new ObjectDisposedException(null, closedError);
			stream.Flush();
		}

		/// <summary>
		/// Throws a <see cref="NotSupportedException"/> instead of obtaining a lifetime service object to control the lifetime policy for this instance.
		/// </summary>
		/// <returns>Nothing, especially not an object of type <see cref="System.Runtime.Remoting.Lifetime.ILease">ILease</see> used to control the lifetime policy for this instance.</returns>
		/// <exception cref="NotSupportedException">Always.</exception>
		public override object InitializeLifetimeService() { throw new NotSupportedException(); }

		/// <summary>
		/// Reads a sequence of bytes from the base stream and advances the position within the base stream by the number of bytes read.
		/// </summary>
		/// <param name="buffer">An array of <see cref="Byte"/>s. The bytes in this array will be replaced by bytes read from the base stream.</param>
		/// <param name="offset">The zero-based byte offset in the array of <see cref="Byte"/>s at which to begin storing the data read from the base stream.</param>
		/// <param name="count">The maximum number of bytes to be read from the base stream.</param>
		/// <returns>The total number of bytes read into the array as an <see cref="Int32"/> value. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
		/// <exception cref="ObjectDisposedException">If stream has been closed.</exception>
		public override int Read(byte[] buffer, int offset, int count)
		{
			if(closed) throw new ObjectDisposedException(null, closedError);
			return stream.Read(buffer, offset, count);
		}

		/// <summary>
		/// Reads a byte from the base stream and advances the position within the base stream by one byte, or returns <c>-1</c> if at the end of the base stream.
		/// </summary>
		/// <returns>The unsigned byte cast to an <see cref="Int32"/>, or <c>-1</c> if at the end of the base stream.</returns>
		/// <exception cref="ObjectDisposedException">If stream has been closed.</exception>
		public override int ReadByte()
		{
			if(closed) throw new ObjectDisposedException(null, closedError);
			return stream.ReadByte();
		}

		/// <summary>
		/// Sets the position within the base stream.
		/// </summary>
		/// <param name="offset">A byte offset relative to the origin parameter.</param>
		/// <param name="origin">A value of type <see cref="SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
		/// <returns>The new position within the base stream as a <see cref="Int64"/> value.</returns>
		/// <exception cref="ObjectDisposedException">If stream has been closed.</exception>
		public override long Seek(long offset, SeekOrigin origin)
		{
			if(closed) throw new ObjectDisposedException(null, closedError);
			return stream.Seek(offset, origin);
		}

		/// <summary>
		/// Sets the length of the base stream.
		/// </summary>
		/// <param name="value">The desired length of the base stream in bytes.</param>
		/// <exception cref="ObjectDisposedException">If stream has been closed.</exception>
		public override void SetLength(long value)
		{
			if(closed) throw new ObjectDisposedException(null, closedError);
			stream.SetLength(value);
		}

		/// <summary>
		/// Writes a sequence of bytes to the base stream and advances the position within the base stream by the number of bytes written.
		/// </summary>
		/// <param name="buffer">An array of <see cref="Byte"/>s. This method copies <paramref name="count"/> bytes from the array to the base stream.</param>
		/// <param name="offset">The zero-based byte offset in the array of <see cref="Byte"/>s at which to begin copying bytes to the base stream.</param>
		/// <param name="count">The number of bytes to be written to the base stream.</param>
		/// <exception cref="ObjectDisposedException">If stream has been closed.</exception>
		public override void Write(byte[] buffer, int offset, int count)
		{
			if(closed) throw new ObjectDisposedException(null, closedError);
			stream.Write(buffer, offset, count);
		}

		/// <summary>
		/// Writes a byte to the current position in the base stream and advances the position within the base stream by one byte.
		/// </summary>
		/// <param name="value">The byte to write to the base stream.</param>
		/// <exception cref="ObjectDisposedException">If stream has been closed.</exception>
		public override void WriteByte(byte value)
		{
			if(closed) throw new ObjectDisposedException(null, closedError);
			stream.WriteByte(value);
		}
		#endregion
	}
}
