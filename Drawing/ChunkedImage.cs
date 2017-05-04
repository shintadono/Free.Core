using System;

namespace Free.Core.Drawing
{
	/// <summary>
	/// Describes a chunked image / raster.
	/// </summary>
	public interface ChunkedImage : TiledImage
	{
		/// <summary>
		/// Gets the height of the chunks in the image / raster. All chunks have the same height (number of lines).
		/// </summary>
		int ChunkHeight { get; }

		/// <summary>
		/// Gets the content of the specified <paramref name="chunk"/> as a new single-chunked image / raster, with the same <see cref="PlanarConfiguration"/> as the image / raster.
		/// </summary>
		/// <param name="chunk">The index of the chunk.</param>
		/// <returns>A single-chunked image / raster.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="chunk"/> is negative, or refers to a chunk outside the image / raster.</exception>
		ChunkedImage GetChunk(int chunk);

		/// <summary>
		/// Gets the content of the specified <paramref name="chunk"/> as a new single-chunked image / raster.
		/// </summary>
		/// <param name="chunk">The index of the chunk.</param>
		/// <param name="channel">The index of the channel.</param>
		/// <returns>A single-chunked image / raster.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="chunk"/> is negative, or refers to a chunk outside the image / raster.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="channel"/> is negative, or refers to a channel outside the image / raster.</exception>
		ChunkedImage GetChunk(int chunk, int channel);
	}
}
