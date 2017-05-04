using System;

namespace Free.Core.Drawing
{
	/// <summary>
	/// Describes a chunked image of 1, 3 (RGB) or 4 (RGBX) 32-bit floating-point numbers per pixel.
	/// </summary>
	public class ChunkedFloatImage : TiledFloatImage, ChunkedImage
	{
		#region Properties

		/// <summary>
		/// Gets the number of chunks.
		/// </summary>
		public int NumberOfChunks { get { return NumberOfTileY; } }

		#region Implements ChunkedImage
		/// <summary>
		/// Gets the height of the chunks in the image. All chunks have the same height (number of lines).
		/// </summary>
		public int ChunkHeight
		{
			get
			{
				return TileHeight;
			}
			protected set
			{
				TileHeight = value;
			}
		}
		#endregion

		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="width">The width of the image in pixels. Must be greater zero (0)</param>
		/// <param name="height">The height of the image in pixels. Must be greater zero (0)</param>
		/// <param name="RGB">Indicates whether the image shall be RGB or single-channeled.</param>
		/// <param name="extraChannel">Indicates whether a RGB image shall contain an extra channel, e.g. for alpha. Must be <b>false</b> if <paramref name="RGB"/> is <b>false</b>.</param>
		/// <param name="init">Indicates whether the <see cref="Bits"/> array shall be created and initialize with zeros (0).</param>
		/// <param name="chunkHeight">The height of the chunks in pixels. Must be greater zero (0).</param>
		/// <param name="planarConfig">The planar configuration to use. Single-channel images must be continuously.</param>
		public ChunkedFloatImage(int width, int height, bool RGB, bool extraChannel, bool init, int chunkHeight = 256, PlanarConfiguration planarConfig = PlanarConfiguration.Continuously):
			base(width, height, RGB, extraChannel, init, width, chunkHeight, planarConfig)
		{
			// We just need to correct the StorageLayout value, every thing else is set up correctly in the TiledImage16 constructor.
			StorageLayout = NumberOfChunks > 1 ? StorageLayout.Chunked : StorageLayout.SingleChunked;
		}

		/// <summary>
		/// Creates a new instance with a specified color.
		/// </summary>
		/// <param name="width">The width of the image in pixels. Must be greater zero (0)</param>
		/// <param name="height">The height of the image in pixels. Must be greater zero (0)</param>
		/// <param name="RGB">Indicates whether the image shall be RGB or single-channeled.</param>
		/// <param name="extraChannel">Indicates whether a RGB image shall contain an extra channel, e.g. for alpha. Must be <b>false</b> if <paramref name="RGB"/> is <b>false</b>.</param>
		/// <param name="initColor">The initial color. ( Layout: [I] or [R] [G] [B] (/[X]) )</param>
		/// <param name="chunkHeight">The height of the chunks in pixels. Must be greater zero (0).</param>
		/// <param name="planarConfig">The planar configuration to use. Single-channel images must be continuously.</param>
		public ChunkedFloatImage(int width, int height, bool RGB, bool extraChannel, float[] initColor, int chunkHeight = 256, PlanarConfiguration planarConfig = PlanarConfiguration.Continuously) :
			base(width, height, RGB, extraChannel, initColor, width, chunkHeight, planarConfig)
		{
			// We just need to correct the StorageLayout value, every thing else is set up correctly in the TiledImage16 constructor.
			StorageLayout = NumberOfChunks > 1 ? StorageLayout.Chunked : StorageLayout.SingleChunked;
		}
		#endregion

		#region Methods

		/// <summary>
		/// Gets the content of the specified <paramref name="chunk"/> as a new single-chunked image, with the same <see cref="PlanarConfiguration"/> as the image.
		/// </summary>
		/// <param name="chunk">The index of the chunk.</param>
		/// <returns>A single-chunked image.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="chunk"/> is negative, or refers to a chunk outside the image.</exception>
		public FloatImage GetChunk(int chunk)
		{
			return GetTile(0, chunk);
		}

		/// <summary>
		/// Gets the content of the specified <paramref name="chunk"/> as a new single-chunked image.
		/// </summary>
		/// <param name="chunk">The index of the chunk.</param>
		/// <param name="channel">The index of the channel.</param>
		/// <returns>A single-chunked image.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="chunk"/> is negative, or refers to a chunk outside the image.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="channel"/> is negative, or refers to a channel outside the image.</exception>
		public FloatImage GetChunk(int chunk, int channel)
		{
			return GetTile(0, chunk, channel);
		}

		#region Implements ChunkedImage
		ChunkedImage ChunkedImage.GetChunk(int chunk)
		{
			return GetChunk(chunk);
		}

		ChunkedImage ChunkedImage.GetChunk(int chunk, int channel)
		{
			return GetChunk(chunk, channel);
		}
		#endregion

		#endregion
	}
}
