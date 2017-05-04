namespace Free.Core.Drawing
{
	/// <summary>
	/// Describes the planar configuration of the image / raster storage.
	/// </summary>
	public enum PlanarConfiguration
	{
		/// <summary>
		/// The channels are stored interleaved (e.g. RGBRGBRGB...). If the data is also store <see cref="StorageLayout.Chunked"/> or <see cref="StorageLayout.Tiled"/>,
		/// there are multiple chunks/tiles with this planar configuration.
		/// </summary>
		Continuously,

		/// <summary>
		/// The channels are stored separated (i.e. a storage for each channel). If the data is also store <see cref="StorageLayout.Chunked"/> or <see cref="StorageLayout.Tiled"/>,
		/// there are multiple chunks/tiles for every channel.
		/// </summary>
		Separated,
	}
}
