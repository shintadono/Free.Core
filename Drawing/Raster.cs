using System;

namespace Free.Core.Drawing
{
	/// <summary>
	/// Describes a generic single-channel/band raster (<see cref="PlanarConfiguration"/> is always <see cref="PlanarConfiguration.Continuously"/>).
	/// </summary>
	/// <typeparam name="T">The type of the raster.</typeparam>
	public class Raster<T> : TiledImage where T: struct
	{
		#region Const
		/// <summary>
		/// Return value for <see cref="ChannelTypes"/>, for <see cref="ChannelType.Byte"/>.
		/// </summary>
		protected static readonly ChannelType[] ChannelTypeByte = new ChannelType[] { ChannelType.Byte };

		/// <summary>
		/// Return value for <see cref="ChannelTypes"/>, for <see cref="ChannelType.SByte"/>.
		/// </summary>
		protected static readonly ChannelType[] ChannelTypeSByte = new ChannelType[] { ChannelType.SByte };

		/// <summary>
		/// Return value for <see cref="ChannelTypes"/>, for <see cref="ChannelType.SShort"/>.
		/// </summary>
		protected static readonly ChannelType[] ChannelTypeSShort = new ChannelType[] { ChannelType.SShort };

		/// <summary>
		/// Return value for <see cref="ChannelTypes"/>, for <see cref="ChannelType.UShort"/>.
		/// </summary>
		protected static readonly ChannelType[] ChannelTypeUShort = new ChannelType[] { ChannelType.UShort };

		/// <summary>
		/// Return value for <see cref="ChannelTypes"/>, for <see cref="ChannelType.SInt"/>.
		/// </summary>
		protected static readonly ChannelType[] ChannelTypeSInt = new ChannelType[] { ChannelType.SInt };

		/// <summary>
		/// Return value for <see cref="ChannelTypes"/>, for <see cref="ChannelType.UInt"/>.
		/// </summary>
		protected static readonly ChannelType[] ChannelTypeUInt = new ChannelType[] { ChannelType.UInt };

		/// <summary>
		/// Return value for <see cref="ChannelTypes"/>, for <see cref="ChannelType.SLong"/>.
		/// </summary>
		protected static readonly ChannelType[] ChannelTypeSLong = new ChannelType[] { ChannelType.SLong };

		/// <summary>
		/// Return value for <see cref="ChannelTypes"/>, for <see cref="ChannelType.ULong"/>.
		/// </summary>
		protected static readonly ChannelType[] ChannelTypeULong = new ChannelType[] { ChannelType.ULong };

		/// <summary>
		/// Return value for <see cref="ChannelTypes"/>, for <see cref="ChannelType.Single"/>.
		/// </summary>
		protected static readonly ChannelType[] ChannelTypeSingle = new ChannelType[] { ChannelType.Single };

		/// <summary>
		/// Return value for <see cref="ChannelTypes"/>, for <see cref="ChannelType.Double"/>.
		/// </summary>
		protected static readonly ChannelType[] ChannelTypeDouble = new ChannelType[] { ChannelType.Double };

		/// <summary>
		/// Return value for <see cref="ChannelTypes"/>, for <see cref="ChannelType.Structure"/>.
		/// </summary>
		protected static readonly ChannelType[] ChannelTypeStructure = new ChannelType[] { ChannelType.Structure };
		#endregion

		#region Variables
		/// <summary>
		/// Contains the elements of the raster. Only change access this directly, if you know what you are doing.
		/// </summary>
		/// <remarks>
		/// <para><b>Layout:</b> An array of <b>TileCountX*TileCountY</b> arrays of <see cref="TileWidth"/>*<see cref="TileHeight"/> elements each.
		/// </para>
		/// <para><b>ATTENTION: Do not reference the same tile more than once, in the main array. Unless you know what you are doing.</b></para>
		/// <para><b>ATTENTION: Do not share the same tile with other raster.</b></para>
		/// </remarks>
		public T[][] Data; // Tiled
		#endregion

		#region Properties

		/// <summary>
		/// Gets/Sets the value of the specified cell.
		/// </summary>
		/// <param name="x">The column of the cell.</param>
		/// <param name="y">The row of the cell.</param>
		/// <returns>The value of the cell.</returns>
		public T this[int x, int y]
		{
			get
			{
				if (x < 0 || x >= Width) throw new IndexOutOfRangeException(nameof(x) + " index must be greater or equal to zero (0), and smaller than the " + nameof(Width) + ".");
				if (y < 0 || y >= Height) throw new IndexOutOfRangeException(nameof(y) + " index must be greater or equal to zero (0), and smaller than the " + nameof(Height) + ".");

				int xTile = x / TileWidth;
				int yTile = y / TileHeight;

				int xInTile = x % TileWidth;
				int yInTile = y % TileHeight;

				return Data[yTile * NumberOfTileX + xTile][yInTile * TileWidth + xInTile];
			}
			set
			{
				if (x < 0 || x >= Width) throw new IndexOutOfRangeException(nameof(x) + " index must be greater or equal to zero (0), and smaller than the " + nameof(Width) + ".");
				if (y < 0 || y >= Height) throw new IndexOutOfRangeException(nameof(y) + " index must be greater or equal to zero (0), and smaller than the " + nameof(Height) + ".");

				int xTile = x / TileWidth;
				int yTile = y / TileHeight;

				int xInTile = x % TileWidth;
				int yInTile = y % TileHeight;

				Data[yTile * NumberOfTileX + xTile][yInTile * TileWidth + xInTile] = value;
			}
		}

		/// <summary>
		/// Gets the number of tiles in x-direction.
		/// </summary>
		public int NumberOfTileX { get; }

		/// <summary>
		/// Gets the number of tiles in y-direction.
		/// </summary>
		public int NumberOfTileY { get; }

		#region Implements ImageCommons
		/// <summary>
		/// Gets the width of the raster.
		/// </summary>
		public int Width { get; protected set; }

		/// <summary>
		/// Gets the height of the raster.
		/// </summary>
		public int Height { get; protected set; }

		/// <summary>
		/// Gets the number of channels in the raster. This is always one (1).
		/// </summary>
		public int NumberOfChannels { get { return 1; } }

		/// <summary>
		/// Gets the types of the raster.
		/// </summary>
		public ChannelType[] ChannelTypes { get; protected set; }

		/// <summary>
		/// Gets the planar configuration of the raster storage. This is always <see cref="PlanarConfiguration.Continuously"/>,
		/// since only one (1) channel is support by this object.
		/// </summary>
		public PlanarConfiguration PlanarConfiguration { get { return PlanarConfiguration.Continuously; } }

		/// <summary>
		/// Gets the layout of the raster storage.
		/// </summary>
		public StorageLayout StorageLayout { get { return Data != null ? (Data.Length == 1 ? StorageLayout.SingleChunked : StorageLayout.Tiled) : StorageLayout.Tiled; } }
		#endregion

		#region Implements TiledImage
		/// <summary>
		/// Gets the width of the tiles in the raster. All tiles have the same width (number of samples).
		/// </summary>
		public int TileWidth { get; protected set; }

		/// <summary>
		/// Gets the height of the tiles in the raster. All tiles have the same height (number of lines).
		/// </summary>
		public int TileHeight { get; protected set; }
		#endregion

		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="width">The width of the raster in cells. Must be greater zero (0)</param>
		/// <param name="height">The height of the raster in cells. Must be greater zero (0)</param>
		/// <param name="init">Indicates whether the <see cref="Data"/> array shall be created and initialized with default(T).</param>
		/// <param name="tileWidth">The width of the tiles in pixels. Must be greater zero (0)</param>
		/// <param name="tileHeight">The height of the tiles in pixels. Must be greater zero (0).</param>
		public Raster(int width, int height, bool init, int tileWidth = 256, int tileHeight = 256)
		{
			if (width < 1) throw new ArgumentOutOfRangeException(nameof(width), "Must be greater zero (0).");
			if (height < 1) throw new ArgumentOutOfRangeException(nameof(height), "Must be greater zero (0).");
			if (tileWidth < 1) throw new ArgumentOutOfRangeException(nameof(tileWidth), "Must be greater zero (0).");
			if (tileHeight < 1) throw new ArgumentOutOfRangeException(nameof(tileHeight), "Must be greater zero (0).");

			Width = width;
			Height = height;
			TileWidth = tileWidth;
			TileHeight = tileHeight;

			NumberOfTileX = (width + tileWidth - 1) / tileWidth;
			NumberOfTileY = (height + tileHeight - 1) / tileHeight;

			Type type = typeof(T);
			if (typeof(byte) == type) ChannelTypes = ChannelTypeByte;
			else if (typeof(ushort) == type) ChannelTypes = ChannelTypeUShort;
			else if (typeof(uint) == type) ChannelTypes = ChannelTypeUInt;
			else if (typeof(ulong) == type) ChannelTypes = ChannelTypeULong;
			else if (typeof(float) == type) ChannelTypes = ChannelTypeSingle;
			else if (typeof(double) == type) ChannelTypes = ChannelTypeDouble;
			else if (typeof(sbyte) == type) ChannelTypes = ChannelTypeSByte;
			else if (typeof(short) == type) ChannelTypes = ChannelTypeSShort;
			else if (typeof(int) == type) ChannelTypes = ChannelTypeSInt;
			else if (typeof(long) == type) ChannelTypes = ChannelTypeSLong;
			else ChannelTypes = ChannelTypeStructure;

			if (init)
			{
				int tiles = NumberOfTileX * NumberOfTileY;
				int tileSize = TileWidth * TileHeight;

				Data = new T[tiles][];
				for (int i = 0; i < tiles; i++) Data[i] = new T[tileSize];
			}
		}
		#endregion

		#region Methods

		/// <summary>
		/// Sets all cells with the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		public void Clear(T value)
		{
			int tiles = Data.Length;
			int tileSize = TileWidth * TileHeight;

			for (int i = 0; i < tiles;)
			{
				T[] tile = Data[i++];
				for (int j = 0; j < tileSize;) tile[j++] = value;
			}
		}

		/// <summary>
		/// Gets the content of the specified tile (specified by tile index in <paramref name="x"/> and
		/// <paramref name="y"/> direction) as a new raster.
		/// </summary>
		/// <param name="x">The index of the tile in x-direction.</param>
		/// <param name="y">The index of the tile in y-direction.</param>
		/// <returns>A raster.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If at least one of <paramref name="x"/> and
		/// <paramref name="y"/> is negative, or refers to a tile outside the raster.</exception>
		public Raster<T> GetTile(int x, int y)
		{
			if (x < 0 || x >= NumberOfTileX) throw new ArgumentOutOfRangeException(nameof(x));
			if (y < 0 || y >= NumberOfTileY) throw new ArgumentOutOfRangeException(nameof(y));

			var ret = new Raster<T>(TileWidth, TileHeight, true, TileWidth, TileHeight);

			Data[y * NumberOfTileX + x].CopyTo(ret.Data[0], 0);

			return ret;
		}

		/// <summary>
		/// Gets the content of the specified tile (specified by tile index in <paramref name="x"/> and
		/// <paramref name="y"/> direction) as a new raster.
		/// </summary>
		/// <param name="x">The index of the tile in x-direction.</param>
		/// <param name="y">The index of the tile in y-direction.</param>
		/// <param name="channel">The index of the channel. Must always be zero (0).</param>
		/// <returns>A raster.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If at least one of <paramref name="x"/> and
		/// <paramref name="y"/> is negative, or refers to a tile outside the raster.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="channel"/> is not zero(0).</exception>
		public Raster<T> GetTile(int x, int y, int channel)
		{
			if (channel == 0) throw new ArgumentOutOfRangeException(nameof(channel), "Must be zero (0).");

			return GetTile(x, y);
		}

		#region Implements TiledImage
		TiledImage TiledImage.GetTile(int x, int y)
		{
			return GetTile(x, y);
		}

		TiledImage TiledImage.GetTile(int x, int y, int channel)
		{
			return GetTile(x, y, channel);
		}
		#endregion

		#endregion
	}
}
