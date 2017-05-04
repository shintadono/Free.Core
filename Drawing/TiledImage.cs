using System;

namespace Free.Core.Drawing
{
	/// <summary>
	/// Describes a tiled image / raster.
	/// </summary>
	public interface TiledImage : ImageCommons
	{
		/// <summary>
		/// Gets the width of the tiles in the image / raster. All tiles have the same width (number of samples).
		/// </summary>
		int TileWidth { get; }

		/// <summary>
		/// Gets the height of the tiles in the image / raster. All tiles have the same height (number of lines).
		/// </summary>
		int TileHeight { get; }

		/// <summary>
		/// Gets the content of the specified tile (specified by tile index in <paramref name="x"/> and
		/// <paramref name="y"/> direction) as a new single-chunked image / raster, with the same
		/// <see cref="PlanarConfiguration"/> as the image / raster.
		/// </summary>
		/// <param name="x">The index of the tile in x-direction.</param>
		/// <param name="y">The index of the tile in y-direction.</param>
		/// <returns>A single-chunked image / raster.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If at least one of <paramref name="x"/> and
		/// <paramref name="y"/> is negative, or refers to a tile outside the image / raster.</exception>
		TiledImage GetTile(int x, int y);

		/// <summary>
		/// Gets the content of the specified tile (specified by tile index in <paramref name="x"/> and
		/// <paramref name="y"/> direction) as a new single-chunked image / raster.
		/// </summary>
		/// <param name="x">The index of the tile in x-direction.</param>
		/// <param name="y">The index of the tile in y-direction.</param>
		/// <param name="channel">The index of the channel.</param>
		/// <returns>A single-chunked image / raster.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If at least one of <paramref name="x"/> and
		/// <paramref name="y"/> is negative, or refers to a tile outside the image / raster.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="channel"/> is negative, or
		/// refers to a channel outside the image / raster.</exception>
		TiledImage GetTile(int x, int y, int channel);
	}
}
