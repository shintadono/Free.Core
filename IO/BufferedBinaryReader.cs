using System;
using System.IO;

namespace Free.Core.IO
{
	/// <summary>
	/// Provides a buffered binary reader.
	/// </summary>
	[CLSCompliant(false)]
	public class BufferedBinaryReader : IDisposable
	{
		Stream m_stream;
		byte[] m_buffer;
		int posInBuffer;
		int capInBuffer;
		byte[] i_buffer;
		int i_bufferSize;

		/// <summary>
		/// Creates an buffered binary reader for a specified filename.
		/// </summary>
		/// <param name="filename">The path to the file.</param>
		/// <param name="bufferSize">The size of the buffer.</param>
		public BufferedBinaryReader(string filename, int bufferSize = 0x10000) : this(new FileStream(filename, FileMode.Open, FileAccess.Read), bufferSize) { }

		/// <summary>
		/// Creates an buffered binary reader for a specified <see cref="Stream"/>.
		/// </summary>
		/// <param name="input">The <see cref="Stream"/>.</param>
		/// <param name="bufferSize">The size of the buffer.</param>
		public BufferedBinaryReader(Stream input, int bufferSize = 0x10000)
		{
			if (input == null) throw new ArgumentNullException("input");
			if (!input.CanRead) throw new ArgumentException("Stream not readable.", "input");
			if (bufferSize <= 0) throw new ArgumentOutOfRangeException("bufferSize", "Must be greater than zero.");

			i_buffer = new byte[8];

			m_stream = input;

			i_bufferSize = bufferSize;
			FillBuffer();
		}

		/// <summary>
		/// Gets the length of the stream/file in bytes.
		/// </summary>
		public long StreamLength { get { return m_stream.Length; } }

		/// <summary>
		/// Gets the current offset into the stream/file in bytes.
		/// </summary>
		public long StreamPosition { get { return m_stream.Position - (capInBuffer - posInBuffer); } }

		/// <summary>
		/// Closes the reader by disposing the allocated memory and releasing/closing the underlying stream.
		/// </summary>
		public virtual void Close()
		{
			Dispose(true);
		}

		/// <summary>
		/// Disposes the allocated memory and releases/closes the underlying stream.
		/// </summary>
		/// <param name="disposing">Indicates whether the the underlying stream shall be closed, or not.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && m_stream != null) m_stream.Close();
			m_stream = null;
			m_buffer = null;
			capInBuffer = posInBuffer = 0;
		}

		/// <summary>
		/// Fills to buffer from the underlying stream.
		/// </summary>
		protected virtual void FillBuffer()
		{
			if (m_buffer == null || m_buffer.Length < i_bufferSize)
				m_buffer = new byte[i_bufferSize];

			posInBuffer = 0;
			capInBuffer = m_stream.Read(m_buffer, 0, i_bufferSize);
		}

		/// <summary>
		/// Reads a sequence of bytes into a byte array.
		/// </summary>
		/// <param name="buffer">The array to store the read bytes.</param>
		/// <param name="index">The position in <paramref name="buffer"/> at which to begin storing.</param>
		/// <param name="count">The number of byte to read.</param>
		/// <returns>The number of bytes read into the <paramref name="buffer"/>.</returns>
		public virtual int Read(byte[] buffer, int index, int count)
		{
			if (buffer == null) throw new ArgumentNullException("buffer");
			if (index < 0) throw new ArgumentOutOfRangeException("index");
			if (count < 0) throw new ArgumentOutOfRangeException("count");
			if ((buffer.Length - index) < count) throw new ArgumentException();
			if (m_stream == null) throw new ObjectDisposedException(null, "File closed");

			int posInTarget = 0;
			do
			{
				if (capInBuffer <= posInBuffer) break;

				int len = Math.Min(capInBuffer - posInBuffer, count);
				Buffer.BlockCopy(m_buffer, posInBuffer, buffer, posInTarget, len);
				posInBuffer += len;
				posInTarget += len;
				count -= len;
				if (posInBuffer == capInBuffer) FillBuffer();
			}
			while (count > 0);

			return posInTarget;
		}

		/// <summary>
		/// Reads a <b>bool</b> and returns it.
		/// </summary>
		/// <returns>The <b>bool</b>.</returns>
		public virtual bool ReadBoolean()
		{
			if (m_stream == null) throw new ObjectDisposedException(null, "File closed");
			if (capInBuffer <= posInBuffer) throw new EndOfStreamException();

			byte val = m_buffer[posInBuffer];
			posInBuffer++;
			if (posInBuffer == capInBuffer) FillBuffer();

			return val != 0;
		}

		/// <summary>
		/// Reads a <b>byte</b> and returns it.
		/// </summary>
		/// <returns>The <b>byte</b>.</returns>
		public virtual byte ReadByte()
		{
			if (m_stream == null) throw new ObjectDisposedException(null, "File closed");
			if (capInBuffer <= posInBuffer) throw new EndOfStreamException();

			byte val = m_buffer[posInBuffer];
			posInBuffer++;
			if (posInBuffer == capInBuffer) FillBuffer();

			return val;
		}

		/// <summary>
		/// Reads a specified number of bytes.
		/// </summary>
		/// <param name="count">The number of bytes to read.</param>
		/// <returns>The read byte as array of <b>byte</b>. The number of read bytes may be smaller
		/// than <paramref name="count"/>, if the end of the stream/file is reached.</returns>
		public virtual byte[] ReadBytes(int count)
		{
			if (count < 0) throw new ArgumentOutOfRangeException("count", "Must not by smaller than zero (0).");
			if (m_stream == null) throw new ObjectDisposedException(null, "File closed");
			byte[] buffer = new byte[count];

			int offset = 0;
			do
			{
				int num2 = Read(buffer, offset, count);
				if (num2 == 0) break;
				offset += num2;
				count -= num2;
			}
			while (count > 0);

			if (offset != buffer.Length)
			{
				byte[] dst = new byte[offset];
				Buffer.BlockCopy(buffer, 0, dst, 0, offset);
				buffer = dst;
			}

			return buffer;
		}

		/// <summary>
		/// Reads a <b>double</b> and returns it.
		/// </summary>
		/// <returns>The <b>double</b>.</returns>
		public virtual unsafe double ReadDouble()
		{
			if (m_stream == null) throw new ObjectDisposedException(null, "File closed");
			if (capInBuffer <= posInBuffer) throw new EndOfStreamException();

			int fillsize = 8;

			int len = Math.Min(capInBuffer - posInBuffer, fillsize);
			Buffer.BlockCopy(m_buffer, posInBuffer, i_buffer, 0, len);
			posInBuffer += len;

			if (posInBuffer == capInBuffer) FillBuffer();
			if (len < fillsize) Buffer.BlockCopy(m_buffer, 0, i_buffer, len, posInBuffer = fillsize - len);

			//uint num=(uint)(((i_buffer[0]|(i_buffer[1]<<8))|(i_buffer[2]<<0x10))|(i_buffer[3]<<0x18));
			//uint num2=(uint)(((i_buffer[4]|(i_buffer[5]<<8))|(i_buffer[6]<<0x10))|(i_buffer[7]<<0x18));
			//ulong num3=((ulong)num2<<0x20)|num;
			//return *(((double*)&num3));
			fixed (byte* ret = i_buffer) return *(double*)ret;
		}

		/// <summary>
		/// Reads a <b>short</b> and returns it.
		/// </summary>
		/// <returns>The <b>short</b>.</returns>
		public virtual unsafe short ReadInt16()
		{
			if (m_stream == null) throw new ObjectDisposedException(null, "File closed");
			if (capInBuffer <= posInBuffer) throw new EndOfStreamException();

			int fillsize = 2;

			int len = Math.Min(capInBuffer - posInBuffer, fillsize);
			Buffer.BlockCopy(m_buffer, posInBuffer, i_buffer, 0, len);
			posInBuffer += len;

			if (posInBuffer == capInBuffer) FillBuffer();
			if (len < fillsize) Buffer.BlockCopy(m_buffer, 0, i_buffer, len, posInBuffer = fillsize - len);

			//return (short)(i_buffer[0]|(i_buffer[1]<<8));
			fixed (byte* ret = i_buffer) return *(short*)ret;
		}

		/// <summary>
		/// Reads an <b>int</b> and returns it.
		/// </summary>
		/// <returns>The <b>int</b>.</returns>
		public virtual unsafe int ReadInt32()
		{
			if (m_stream == null) throw new ObjectDisposedException(null, "File closed");
			if (capInBuffer <= posInBuffer) throw new EndOfStreamException();

			int fillsize = 4;

			int len = Math.Min(capInBuffer - posInBuffer, fillsize);
			Buffer.BlockCopy(m_buffer, posInBuffer, i_buffer, 0, len);
			posInBuffer += len;

			if (posInBuffer == capInBuffer) FillBuffer();
			if (len < fillsize) Buffer.BlockCopy(m_buffer, 0, i_buffer, len, posInBuffer = fillsize - len);

			//return (((i_buffer[0]|(i_buffer[1]<<8))|(i_buffer[2]<<0x10))|(i_buffer[3]<<0x18));
			fixed (byte* ret = i_buffer) return *(int*)ret;
		}

		/// <summary>
		/// Reads a 3-byte <b>int</b> and returns it.
		/// </summary>
		/// <returns>The 3-byte <b>int</b>.</returns>
		public virtual int ReadInt24()
		{
			if (m_stream == null) throw new ObjectDisposedException(null, "File closed");
			if (capInBuffer <= posInBuffer) throw new EndOfStreamException();

			int fillsize = 3;

			int len = Math.Min(capInBuffer - posInBuffer, fillsize);
			Buffer.BlockCopy(m_buffer, posInBuffer, i_buffer, 0, len);
			posInBuffer += len;

			if (posInBuffer == capInBuffer) FillBuffer();
			if (len < fillsize) Buffer.BlockCopy(m_buffer, 0, i_buffer, len, posInBuffer = fillsize - len);

			return (int)((i_buffer[0] | (i_buffer[1] << 8)) | (i_buffer[2] << 0x10));
		}

		/// <summary>
		/// Reads a <b>long</b> and returns it.
		/// </summary>
		/// <returns>The <b>long</b>.</returns>
		public virtual unsafe long ReadInt64()
		{
			if (m_stream == null) throw new ObjectDisposedException(null, "File closed");
			if (capInBuffer <= posInBuffer) throw new EndOfStreamException();

			int fillsize = 8;

			int len = Math.Min(capInBuffer - posInBuffer, fillsize);
			Buffer.BlockCopy(m_buffer, posInBuffer, i_buffer, 0, len);
			posInBuffer += len;

			if (posInBuffer == capInBuffer) FillBuffer();
			if (len < fillsize) Buffer.BlockCopy(m_buffer, 0, i_buffer, len, posInBuffer = fillsize - len);

			//uint num=(uint)(((i_buffer[0]|(i_buffer[1]<<8))|(i_buffer[2]<<0x10))|(i_buffer[3]<<0x18));
			//uint num2=(uint)(((i_buffer[4]|(i_buffer[5]<<8))|(i_buffer[6]<<0x10))|(i_buffer[7]<<0x18));
			//return (long)(((ulong)num2<<0x20)|num);
			fixed (byte* ret = i_buffer) return *(long*)ret;
		}

		/// <summary>
		/// Reads a <b>sbyte</b> and returns it.
		/// </summary>
		/// <returns>The <b>sbyte</b>.</returns>
		public virtual sbyte ReadSByte()
		{
			if (m_stream == null) throw new ObjectDisposedException(null, "File closed");
			if (capInBuffer <= posInBuffer) throw new EndOfStreamException();

			byte val = m_buffer[posInBuffer];
			posInBuffer++;
			if (posInBuffer == capInBuffer) FillBuffer();

			return (sbyte)val;
		}

		/// <summary>
		/// Reads a <b>float</b> and returns it.
		/// </summary>
		/// <returns>The <b>float</b>.</returns>
		public virtual unsafe float ReadSingle()
		{
			if (m_stream == null) throw new ObjectDisposedException(null, "File closed");
			if (capInBuffer <= posInBuffer) throw new EndOfStreamException();

			int fillsize = 4;

			int len = Math.Min(capInBuffer - posInBuffer, fillsize);
			Buffer.BlockCopy(m_buffer, posInBuffer, i_buffer, 0, len);
			posInBuffer += len;

			if (posInBuffer == capInBuffer) FillBuffer();
			if (len < fillsize) Buffer.BlockCopy(m_buffer, 0, i_buffer, len, posInBuffer = fillsize - len);

			//uint num=(uint)(((i_buffer[0]|(i_buffer[1]<<8))|(i_buffer[2]<<0x10))|(i_buffer[3]<<0x18));
			//return *(((float*)&num));
			fixed (byte* ret = i_buffer) return *(float*)ret;
		}

		/// <summary>
		/// Reads a <b>ushort</b> and returns it.
		/// </summary>
		/// <returns>The <b>ushort</b>.</returns>
		public virtual unsafe ushort ReadUInt16()
		{
			if (m_stream == null) throw new ObjectDisposedException(null, "File closed");
			if (capInBuffer <= posInBuffer) throw new EndOfStreamException();

			int fillsize = 2;

			int len = Math.Min(capInBuffer - posInBuffer, fillsize);
			Buffer.BlockCopy(m_buffer, posInBuffer, i_buffer, 0, len);
			posInBuffer += len;

			if (posInBuffer == capInBuffer) FillBuffer();
			if (len < fillsize) Buffer.BlockCopy(m_buffer, 0, i_buffer, len, posInBuffer = fillsize - len);

			//return (ushort)(i_buffer[0]|(i_buffer[1]<<8));
			fixed (byte* ret = i_buffer) return *(ushort*)ret;
		}

		/// <summary>
		/// Reads 3-byte <b>uint</b> and returns it.
		/// </summary>
		/// <returns>The 3-byte <b>uint</b>.</returns>
		public virtual uint ReadUInt24()
		{
			if (m_stream == null) throw new ObjectDisposedException(null, "File closed");
			if (capInBuffer <= posInBuffer) throw new EndOfStreamException();

			int fillsize = 3;

			int len = Math.Min(capInBuffer - posInBuffer, fillsize);
			Buffer.BlockCopy(m_buffer, posInBuffer, i_buffer, 0, len);
			posInBuffer += len;

			if (posInBuffer == capInBuffer) FillBuffer();
			if (len < fillsize) Buffer.BlockCopy(m_buffer, 0, i_buffer, len, posInBuffer = fillsize - len);

			return (uint)((i_buffer[0] | (i_buffer[1] << 8)) | (i_buffer[2] << 0x10));
		}

		/// <summary>
		/// Reads an <b>uint</b> and returns it.
		/// </summary>
		/// <returns>The <b>uint</b>.</returns>
		public virtual unsafe uint ReadUInt32()
		{
			if (m_stream == null) throw new ObjectDisposedException(null, "File closed");
			if (capInBuffer <= posInBuffer) throw new EndOfStreamException();

			int fillsize = 4;

			int len = Math.Min(capInBuffer - posInBuffer, fillsize);
			Buffer.BlockCopy(m_buffer, posInBuffer, i_buffer, 0, len);
			posInBuffer += len;

			if (posInBuffer == capInBuffer) FillBuffer();
			if (len < fillsize) Buffer.BlockCopy(m_buffer, 0, i_buffer, len, posInBuffer = fillsize - len);

			//return (uint)(((i_buffer[0]|(i_buffer[1]<<8))|(i_buffer[2]<<0x10))|(i_buffer[3]<<0x18));
			fixed (byte* ret = i_buffer) return *(uint*)ret;
		}

		/// <summary>
		/// Reads an <b>ulong</b> and returns it.
		/// </summary>
		/// <returns>The <b>ulong</b>.</returns>
		public virtual unsafe ulong ReadUInt64()
		{
			if (m_stream == null) throw new ObjectDisposedException(null, "File closed");
			if (capInBuffer <= posInBuffer) throw new EndOfStreamException();

			int fillsize = 8;

			int len = Math.Min(capInBuffer - posInBuffer, fillsize);
			Buffer.BlockCopy(m_buffer, posInBuffer, i_buffer, 0, len);
			posInBuffer += len;

			if (posInBuffer == capInBuffer) FillBuffer();
			if (len < fillsize) Buffer.BlockCopy(m_buffer, 0, i_buffer, len, posInBuffer = fillsize - len);

			//uint num=(uint)(((i_buffer[0]|(i_buffer[1]<<8))|(i_buffer[2]<<0x10))|(i_buffer[3]<<0x18));
			//uint num2=(uint)(((i_buffer[4]|(i_buffer[5]<<8))|(i_buffer[6]<<0x10))|(i_buffer[7]<<0x18));
			//return (((ulong)num2<<0x20)|num);
			fixed (byte* ret = i_buffer) return *(ulong*)ret;
		}

		void IDisposable.Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		/// Indicates whether the underlying stream and thus the reader can seek.
		/// </summary>
		public bool CanSeek { get { return m_stream == null ? false : m_stream.CanSeek; } }

		/// <summary>
		/// Seeks to the specified position in the reader.
		/// </summary>
		/// <param name="offset">The offset in regards to <paramref name="origin"/>.</param>
		/// <param name="origin">Indicates the reference point used to obtain the new position.</param>
		/// <returns>The new position in the reader.</returns>
		public long Seek(long offset, SeekOrigin origin)
		{
			if (m_stream == null) throw new ObjectDisposedException(null, "File closed");
			if (!m_stream.CanSeek) throw new NotSupportedException("Stream not seekable.");

			long currentpos = m_stream.Position - (capInBuffer - posInBuffer);
			long pos = 0;
			switch (origin)
			{
				case SeekOrigin.Begin: break;
				case SeekOrigin.Current: pos = currentpos; break;
				case SeekOrigin.End: pos = m_stream.Length; break;
			}

			m_stream.Seek(pos + offset, SeekOrigin.Begin);
			FillBuffer();

			return pos + offset;
		}

		/// <summary>
		/// Skips the specified amount of bytes.
		/// </summary>
		/// <param name="offset">The number of bytes to skip.</param>
		public void Skip(long offset)
		{
			if (m_stream == null) throw new ObjectDisposedException(null, "File closed");
			if (capInBuffer <= posInBuffer) throw new EndOfStreamException();

			if (offset > capInBuffer - posInBuffer)
			{
				if (!m_stream.CanSeek)
				{
					offset -= capInBuffer - posInBuffer;

					if (m_buffer == null || m_buffer.Length < i_bufferSize)
						m_buffer = new byte[i_bufferSize];

					do
					{
						int len = (int)Math.Min(offset, i_bufferSize);
						int read = m_stream.Read(m_buffer, 0, len);
						if (read == 0) throw new EndOfStreamException();

						offset -= read;
					}
					while (offset > 0);

					FillBuffer();
				}
				else Seek(offset, SeekOrigin.Current);
			}
			else
			{
				posInBuffer += (int)offset;

				if (posInBuffer == capInBuffer) FillBuffer();
			}
		}

		/// <summary>
		/// Indicates whether the reader has reached the end of the stream/file.
		/// </summary>
		public bool EOF
		{
			get { return capInBuffer <= posInBuffer; }
		}

		/// <summary>
		/// Returns the underlying stream. This results in closing the reader. The reader will be unusable after this.
		/// </summary>
		/// <returns>The underlying stream</returns>
		public Stream ReleaseInnerStream()
		{
			if (m_stream == null) throw new ObjectDisposedException(null, "File closed");
			if (!m_stream.CanSeek) throw new NotSupportedException("Stream not seekable.");

			m_stream.Seek(m_stream.Position - (capInBuffer - posInBuffer), SeekOrigin.Begin);
			Stream ret = m_stream;
			m_stream = null;

			Dispose(true);

			return ret;
		}
	}
}
