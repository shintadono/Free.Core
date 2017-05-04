namespace Free.Core.Drawing
{
	/// <summary>
	/// Describes the layout of the storage of an image / raster.
	/// </summary>
	public enum StorageLayout
	{
		/// <summary>
		/// The data is stored as a single chunk. If the data is also store <see cref="PlanarConfiguration.Separated"/>, there is a chunk for every channel.
		/// </summary>
		SingleChunked,

		/// <summary>
		/// The data is stored as multiple chunks. All chunks must have the same number of lines (even the last one, and even if the image / raster
		/// has a height that can't be divide (w/o remainder) by the number of lines of the chunks). Single-line-chunks are allowed.
		/// If the data is also store <see cref="PlanarConfiguration.Separated"/>, there are multiple chunks for every channel.
		/// </summary>
		Chunked,

		/// <summary>
		/// The data is stored tiled. All tiles must have the same dimensions. Single-pixel-tiles are allowed, but highly discouraged.
		/// If the data is also store <see cref="PlanarConfiguration.Separated"/>, there are multiple tiled for every channel.
		/// </summary>
		Tiled,
	}
}
