using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Free.Core.Drawing
{
	/// <summary>
	/// Describes a generic single-channel/band raster (<see cref="PlanarConfiguration"/> is always <see cref="PlanarConfiguration.Continuously"/>).
	/// </summary>
	/// <typeparam name="T">The type of the raster.</typeparam>
	public class Raster<T> : TiledImage where T : struct, IEquatable<T>
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

		/// <summary>
		/// Default width for tiles.
		/// </summary>
		public const int DefaultTileWidth = 256;

		/// <summary>
		/// Default height for tiles.
		/// </summary>
		public const int DefaultTileHeight = 256;
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

		#region Statics
		/// <summary>
		/// Caches the neighbouresXYOffsets and maxDistance values for previous requested radii.
		/// </summary>
		public static Dictionary<double, Tuple<int, int[]>> NeighbouresXYOffsetsCache = new Dictionary<double, Tuple<int, int[]>>();

		/// <summary>
		/// Generates the relatives x- and y-coordinates of the elements at the lattice points with a maximum distance to the center of <paramref name="radius"/>.
		/// </summary>
		/// <param name="radius">The radius.</param>
		/// <param name="maxDistance">The maximum integral distance in x or y dimension of a lattice point to the center.</param>
		/// <returns>An array containing the relatives x- and y-coordinates of the elements at the lattice points with a maximum distance to the center of <paramref name="radius"/>.</returns>
		public static int[] GenerateNeighbouresXYOffsets(double radius, out int maxDistance)
		{
			if (double.IsInfinity(radius) || double.IsNaN(radius)) throw new ArgumentException("Must be a valid, non-infinte number.", nameof(radius));

			// Let's check the cache, whether we already calculated maxDistance and array for the given radius.
			Tuple<int, int[]> cached;
			if (NeighbouresXYOffsetsCache.TryGetValue(radius, out cached))
			{
				maxDistance = cached.Item1;
				return cached.Item2;
			}

			// We could throw here, but that seems to be overkill, since we can fix this.
			double radiusAbs = Math.Abs(radius);
			if (radiusAbs < 1) radiusAbs = 1;

			int rad = (int)Math.Floor(radiusAbs);

			var ret = new List<int>();
			for (var y = -rad; y <= rad; y++)
			{
				for (var x = -rad; x <= rad; x++)
				{
					if (y * y + x * x > radiusAbs * radiusAbs) continue;
					ret.Add(x);
					ret.Add(y);
				}
			}

			int[] retArray = ret.ToArray();
			lock (NeighbouresXYOffsetsCache)
			{
				NeighbouresXYOffsetsCache[radius] = new Tuple<int, int[]>(rad, retArray);
			}

			maxDistance = rad;
			return retArray;
		}

		/// <summary>
		/// Converts the alternating relative x- and y-coordinate values into index offsets for a specified <paramref name="tileWidth"/>.
		/// </summary>
		/// <param name="neighbouresXYOffsets">The array with alternating relative x- and y-coordinate values.</param>
		/// <param name="tileWidth">The tile width.</param>
		/// <returns>The index offsets as array.</returns>
		public static int[] ConvertToInTileIndexOffsets(int[] neighbouresXYOffsets, int tileWidth)
		{
			if (null == neighbouresXYOffsets) throw new ArgumentNullException(nameof(neighbouresXYOffsets));
			if (tileWidth < 1) throw new ArgumentOutOfRangeException(nameof(tileWidth), "Must be greater zero (0).");
			if (neighbouresXYOffsets.Length % 2 != 0) throw new ArgumentException("Must have an even number of elements.", nameof(neighbouresXYOffsets));

			var ret = new int[neighbouresXYOffsets.Length / 2];
			for (int i = 0, j = 0; j < ret.Length;)
			{
				int x = neighbouresXYOffsets[i++];
				int y = neighbouresXYOffsets[i++];
				ret[j++] = y * tileWidth + x;
			}
			return ret;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="width">The width of the raster in cells. Must be greater zero (0)</param>
		/// <param name="height">The height of the raster in cells. Must be greater zero (0)</param>
		/// <param name="init">Indicates whether the <see cref="Data"/> array shall be created and initialized with default(<typeparamref name="T"/>).</param>
		/// <param name="tileWidth">The width of the tiles in cells. Must be greater zero (0).</param>
		/// <param name="tileHeight">The height of the tiles in cells. Must be greater zero (0).</param>
		public Raster(int width, int height, bool init, int tileWidth = DefaultTileWidth, int tileHeight = DefaultTileHeight)
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
		public void Clear(T value = default(T))
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
		/// Draws the content of a specified <paramref name="source"/> raster to this raster at the specified position.
		/// </summary>
		/// <param name="x">The x position of the <paramref name="source"/> raster in this raster.</param>
		/// <param name="y">The y position of the <paramref name="source"/> raster in this raster.</param>
		/// <param name="source">The raster to draw.</param>
		public void Draw(int x, int y, Raster<T> source)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));

			// First let check whether the images overlap.
			if (x + source.Width <= 0 || x >= Width) return;
			if (y + source.Height <= 0 || y >= Height) return;

			int dstStartX = Math.Max(x, 0);
			int dstStartY = Math.Max(y, 0);
			int dstEndX = Math.Min(x + source.Width, Width);
			int dstEndY = Math.Min(y + source.Height, Height);

			int amountX = dstEndX - dstStartX;
			int amountY = dstEndY - dstStartY;

			int startTileXDest = dstStartX / TileWidth;
			int startTileYDest = dstStartY / TileHeight;
			int endTileXDest = (dstEndX - 1) / TileWidth;
			int endTileYDest = (dstEndY - 1) / TileHeight;

			int srcY = Math.Max(-y, 0);
			int tileStartYDest = dstStartY % TileHeight;
			int tileAmountY = Math.Min(TileHeight - tileStartYDest, amountY);

			// Go thru all target tiles.
			for (int dstTileY = startTileYDest; dstTileY <= endTileYDest; dstTileY++)
			{
				amountX = dstEndX - dstStartX; // Reset amount.

				int srcX = Math.Max(-x, 0);
				int tileStartXDest = dstStartX % TileWidth;
				int tileAmountX = Math.Min(TileWidth - tileStartXDest, amountX);

				for (int dstTileX = startTileXDest; dstTileX <= endTileXDest; dstTileX++)
				{
					// The tile index is assumed for PlanarConfiguration == Continuously, DrawIntoTile will handle Separated case inside.
					DrawIntoTile(dstTileY * NumberOfTileX + dstTileX, source, tileStartXDest, tileStartYDest, srcX, srcY, tileAmountX, tileAmountY);

					srcX += tileAmountX;
					tileStartXDest = 0;
					amountX -= tileAmountX;
					tileAmountX = Math.Min(amountX, TileWidth);
				}

				srcY += tileAmountY;
				tileStartYDest = 0;
				amountY -= tileAmountY;
				tileAmountY = Math.Min(amountY, TileHeight);
			}
		}

		void DrawIntoTile(int tileIndex, Raster<T> source, int dstStartX, int dstStartY, int srcStartX, int srcStartY, int amountX, int amountY)
		{
			int srcEndX = srcStartX + amountX;
			int srcEndY = srcStartY + amountY;

			int startTileXSource = srcStartX / source.TileWidth;
			int startTileYSource = srcStartY / source.TileHeight;
			int endTileXSource = (srcEndX - 1) / source.TileWidth;
			int endTileYSource = (srcEndY - 1) / source.TileHeight;

			int amountXBackup = amountX;

			int dstStartOfLineInDestTile = dstStartY * TileWidth + dstStartX;

			int tileStartYSrc = srcStartY % source.TileHeight;
			int tileAmountY = Math.Min(source.TileHeight - tileStartYSrc, amountY);

			for (int srcTileY = startTileYSource; srcTileY <= endTileYSource; srcTileY++)
			{
				amountX = amountXBackup; // Reset amount.

				int dstStartOfSourceTileInDestTile = dstStartOfLineInDestTile;

				int tileStartXSrc = srcStartX % source.TileWidth;
				int tileAmountX = Math.Min(source.TileWidth - tileStartXSrc, amountX);

				for (int srcTileX = startTileXSource; srcTileX <= endTileXSource; srcTileX++)
				{
					var data = Data[tileIndex];
					int dstStartOfSourceLineInDestTile = dstStartOfSourceTileInDestTile;

					int srcStartOfLine = tileStartYSrc * source.TileWidth + tileStartXSrc;

					var srcData = source.Data[srcTileY * source.NumberOfTileX + srcTileX];

					for (int y = 0; y < tileAmountY; y++)
					{
						int dst = dstStartOfSourceLineInDestTile;
						int src = srcStartOfLine * source.NumberOfChannels;

						for (int x = 0; x < tileAmountX; x++)
							data[dst++] = srcData[src++];

						srcStartOfLine += source.TileWidth;
						dstStartOfSourceLineInDestTile += TileWidth;
					}

					dstStartOfSourceTileInDestTile += tileAmountX;

					tileStartXSrc = 0;
					amountX -= tileAmountX;
					tileAmountX = Math.Min(amountX, source.TileWidth);
				}

				dstStartOfLineInDestTile += tileAmountY * TileWidth;

				tileStartYSrc = 0;
				amountY -= tileAmountY;
				tileAmountY = Math.Min(amountY, source.TileHeight);
			}
		}

		/// <summary>
		/// Gets a sub-raster at the specified position with the specified size. Requested areas outside the raster will be filled with the default(<typeparamref name="T"/>) values.
		/// </summary>
		/// <param name="x">The x-position of the sub-raster in the raster.</param>
		/// <param name="y">The y-position of the sub-raster in the raster.</param>
		/// <param name="width">The width of the new raster.</param>
		/// <param name="height">The height of the new raster.</param>
		/// <param name="tileWidth">The width of the tiles in cells. If zero (0) or smaller, the tile width of the original is used.</param>
		/// <param name="tileHeight">The height of the tiles in cells. If zero (0) or smaller, the tile height of the original is used.</param>
		/// <returns>The sub-raster.</returns>
		public Raster<T> GetSubRaster(int x, int y, int width, int height, int tileWidth = DefaultTileWidth, int tileHeight = DefaultTileHeight)
		{
			if (tileWidth <= 0) tileWidth = TileWidth;
			if (tileHeight <= 0) tileHeight = TileHeight;

			// Create the result depending on this.
			Raster<T> ret = new Raster<T>(width, height, true, tileWidth, tileHeight);

			// Just draw this into the result image, and let Draw handle everything.
			ret.Draw(-x, -y, this);

			return ret;
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

		#region Morphological operations
		/// <summary>
		/// Dilates the default(<typeparamref name="T"/>) regions.
		/// </summary>
		/// <param name="radius">The radius.</param>
		/// <returns>The raster with dilated default(<typeparamref name="T"/>) regions. Undilated cells keep their value.</returns>
		public Raster<T> Dilation(double radius)
		{
			int maxDistance;
			var neighbouresXYOffsets = GenerateNeighbouresXYOffsets(radius, out maxDistance);

			return Dilation(maxDistance, ConvertToInTileIndexOffsets(neighbouresXYOffsets, TileWidth), neighbouresXYOffsets);
		}

		/// <summary>
		/// Dilates the default(<typeparamref name="T"/>) regions.
		/// </summary>
		/// <param name="maxDistance">Distance of the furthest 'structuring elements' in one dimension from the current position [in number of cells].</param>
		/// <param name="neighbouresInTileIndexOffsets">Relative index offset to the current position in a tile which define the structuring elements.</param>
		/// <param name="neighbouresXYOffsets">Alternating relative x and y coordinate to the current position which define the structuring elements.</param>
		/// <returns>The raster with dilated default(<typeparamref name="T"/>) regions. Undilated cells keep their value.</returns>
		public Raster<T> Dilation(int maxDistance, int[] neighbouresInTileIndexOffsets, int[] neighbouresXYOffsets)
		{
			if (maxDistance <= 0) throw new ArgumentException("Must be greater than zero(0).", nameof(maxDistance));
			if (null == neighbouresInTileIndexOffsets) throw new ArgumentNullException(nameof(neighbouresInTileIndexOffsets));
			if (null == neighbouresXYOffsets) throw new ArgumentNullException(nameof(neighbouresXYOffsets));
			if (neighbouresXYOffsets.Length % 2 != 0) throw new ArgumentException("Must have an even number of elements.", nameof(neighbouresXYOffsets));
			if (neighbouresXYOffsets.Length / 2 != neighbouresInTileIndexOffsets.Length) throw new ArgumentException("Must have twice as many elements than " + nameof(neighbouresInTileIndexOffsets) + ".", nameof(neighbouresXYOffsets));

			// Local copies are faster.
			var dist = maxDistance;
			int[] nitio = neighbouresInTileIndexOffsets, nxyo = neighbouresXYOffsets;
			int width = Width, height = Height, tileWidth = TileWidth, tileHeight = TileHeight, numberOfTileX = NumberOfTileX;
			var orgData = Data;

			var ret = new Raster<T>(width, height, true, tileWidth, tileHeight);
			var retData = ret.Data;

			// Let's do it by tile to keep the data accesses local.
			for (int tile = 0; tile < retData.Length; tile++)
			{
				var retTile = retData[tile];
				var orgTile = orgData[tile];

				var tileStartX = tileWidth * (tile % numberOfTileX);
				var tileStartY = tileHeight * (tile / numberOfTileX);

				var amountX = Math.Min(width - tileStartX, tileWidth);
				var amountY = Math.Min(height - tileStartY, tileHeight);

				Parallel.For(0, amountY, line =>
				{
					bool fast = line > dist && line < amountY - dist; // We can use the neighbouresInTileIndexOffsets when we're not at the border of the tile.

					var index = line * tileWidth;

					var rasterLine = line + tileStartY;

					for (int samp = 0; samp < amountX; samp++, index++)
					{
						bool found = false;
						if (fast && samp > dist && samp < amountX - dist) // We can use the neighbouresInTileIndexOffsets when we're not at the border of the tile.
						{
							for (int i = 0; i < nitio.Length; i++)
							{
								if (orgTile[index + nitio[i]].Equals(default(T)))
								{
									retTile[index] = default(T);
									found = true;
									break;
								}
							}
						}
						else
						{
							var rasterSamp = samp + tileStartX;
							for (int i = 0; i < nxyo.Length;)
							{
								var x = rasterSamp + nxyo[i++]; // First read the x value...
								var y = rasterLine + nxyo[i++]; // ...then the y value.
								if (x < 0 || x >= width || y < 0 || y >= height) continue;

								int xTile = x / tileWidth, yTile = y / tileHeight, xInTile = x % tileWidth, yInTile = y % tileHeight;

								if (orgData[yTile * numberOfTileX + xTile][yInTile * tileWidth + xInTile].Equals(default(T)))
								{
									retTile[index] = default(T);
									found = true;
									break;
								}
							}
						}

						if (!found) retTile[index] = orgTile[index];
					}
				});
			}
			return ret;
		}

		/// <summary>
		/// Erodes the default(<typeparamref name="T"/>) regions.
		/// </summary>
		/// <param name="radius">The radius.</param>
		/// <param name="newValue">The value to use for non-default(<typeparamref name="T"/>) cells. Must be a non-default(<typeparamref name="T"/>) value.</param>
		/// <returns>The raster with values default((<typeparamref name="T"/>) and <paramref name="newValue"/>.</returns>
		public Raster<T> Erosion(double radius, T newValue)
		{
			int maxDistance;
			var neighbouresXYOffsets = GenerateNeighbouresXYOffsets(radius, out maxDistance);

			return Erosion(maxDistance, ConvertToInTileIndexOffsets(neighbouresXYOffsets, TileWidth), neighbouresXYOffsets, newValue);
		}

		/// <summary>
		/// Erodes the default(<typeparamref name="T"/>) regions.
		/// </summary>
		/// <param name="maxDistance">Distance of the furthest 'structuring elements' in one dimension from the current position [in number of cells].</param>
		/// <param name="neighbouresInTileIndexOffsets">Relative index offset to the current position in a tile which define the structuring elements.</param>
		/// <param name="neighbouresXYOffsets">Alternating relative x and y coordinate to the current position which define the structuring elements.</param>
		/// <param name="newValue">The value to use for non-default(<typeparamref name="T"/>) cells. Must be a non-default(<typeparamref name="T"/>) value.</param>
		/// <returns>The raster with values default((<typeparamref name="T"/>) and <paramref name="newValue"/>.</returns>
		public Raster<T> Erosion(int maxDistance, int[] neighbouresInTileIndexOffsets, int[] neighbouresXYOffsets, T newValue)
		{
			if (maxDistance <= 0) throw new ArgumentException("Must be greater than zero(0).", nameof(maxDistance));
			if (null == neighbouresInTileIndexOffsets) throw new ArgumentNullException(nameof(neighbouresInTileIndexOffsets));
			if (null == neighbouresXYOffsets) throw new ArgumentNullException(nameof(neighbouresXYOffsets));
			if (neighbouresXYOffsets.Length % 2 != 0) throw new ArgumentException("Must have an even number of elements.", nameof(neighbouresXYOffsets));
			if (neighbouresXYOffsets.Length / 2 != neighbouresInTileIndexOffsets.Length) throw new ArgumentException("Must have twice as many elements than " + nameof(neighbouresInTileIndexOffsets) + ".", nameof(neighbouresXYOffsets));
			if (newValue.Equals(default(T))) throw new ArgumentException("Must be a non-default value.", nameof(newValue));

			// Local copies are faster.
			var dist = maxDistance;
			int[] nitio = neighbouresInTileIndexOffsets, nxyo = neighbouresXYOffsets;
			int width = Width, height = Height, tileWidth = TileWidth, tileHeight = TileHeight, numberOfTileX = NumberOfTileX;
			var orgData = Data;

			var ret = new Raster<T>(width, height, true, tileWidth, tileHeight);
			var retData = ret.Data;

			// Let's do it by tile to keep the data accesses local.
			for (int tile = 0; tile < retData.Length; tile++)
			{
				var retTile = retData[tile];
				var orgTile = orgData[tile]; // Used for fast access path only.

				var tileStartX = tileWidth * (tile % numberOfTileX);
				var tileStartY = tileHeight * (tile / numberOfTileX);

				var amountX = Math.Min(width - tileStartX, tileWidth);
				var amountY = Math.Min(height - tileStartY, tileHeight);

				Parallel.For(0, amountY, line =>
				{
					bool fast = line > dist && line < amountY - dist; // We can use the neighbouresInTileIndexOffsets when we're not at the border of the tile.

					var index = line * tileWidth;

					var rasterLine = line + tileStartY;

					for (int samp = 0; samp < amountX; samp++, index++)
					{
						if (fast && samp > dist && samp < amountX - dist) // We can use the neighbouresInTileIndexOffsets when we're not at the border of the tile.
						{
							for (int i = 0; i < nitio.Length; i++)
							{
								if (!orgTile[index + nitio[i]].Equals(default(T)))
								{
									retTile[index] = newValue;
									break;
								}
							}
						}
						else
						{
							var rasterSamp = samp + tileStartX;
							for (int i = 0; i < nxyo.Length;)
							{
								var x = rasterSamp + nxyo[i++]; // First read the x value...
								var y = rasterLine + nxyo[i++]; // ...then the y value.
								if (x < 0 || x >= width || y < 0 || y >= height) continue;

								int xTile = x / tileWidth, yTile = y / tileHeight, xInTile = x % tileWidth, yInTile = y % tileHeight;

								if (!orgData[yTile * numberOfTileX + xTile][yInTile * tileWidth + xInTile].Equals(default(T)))
								{
									retTile[index] = newValue;
									break;
								}
							}
						}
					}
				});
			}
			return ret;
		}

		/// <summary>
		/// Opens the default(<typeparamref name="T"/>) regions.
		/// </summary>
		/// <param name="radius">The radius.</param>
		/// <param name="newValue">The value to use for non-default(<typeparamref name="T"/>) cells. Must be a non-default(<typeparamref name="T"/>) value.</param>
		/// <returns>The raster with values default((<typeparamref name="T"/>) and <paramref name="newValue"/>.</returns>
		public Raster<T> Opening(double radius, T newValue)
		{
			int maxDistance;
			var neighbouresXYOffsets = GenerateNeighbouresXYOffsets(radius, out maxDistance);
			var neighbouresInTileIndexOffsets = ConvertToInTileIndexOffsets(neighbouresXYOffsets, TileWidth);

			return Erosion(maxDistance, neighbouresInTileIndexOffsets, neighbouresXYOffsets, newValue).Dilation(maxDistance, neighbouresInTileIndexOffsets, neighbouresXYOffsets);
		}

		/// <summary>
		/// Closes the default(<typeparamref name="T"/>) regions.
		/// </summary>
		/// <param name="radius">The radius.</param>
		/// <param name="newValue">The value to use for non-default(<typeparamref name="T"/>) cells. Must be a non-default(<typeparamref name="T"/>) value.</param>
		/// <returns>The raster with values default((<typeparamref name="T"/>) and <paramref name="newValue"/>.</returns>
		public Raster<T> Closing(double radius, T newValue)
		{
			int maxDistance;
			var neighbouresXYOffsets = GenerateNeighbouresXYOffsets(radius, out maxDistance);
			var neighbouresInTileIndexOffsets = ConvertToInTileIndexOffsets(neighbouresXYOffsets, TileWidth);

			return Dilation(maxDistance, neighbouresInTileIndexOffsets, neighbouresXYOffsets).Erosion(maxDistance, neighbouresInTileIndexOffsets, neighbouresXYOffsets, newValue);
		}
		#endregion

		#region Filter
		/// <summary>
		/// Performs a <paramref name="filterOperation"/> on a <see cref="Raster{T}"/>.
		/// </summary>
		/// <param name="filterOperation">The function to calculate the result value for a structure element.</param>
		/// <param name="radius">The radius.</param>
		/// <param name="borderValue">The value to be given to the <paramref name="filterOperation"/> for elements outside of the raster.</param>
		/// <returns>The raster with the filtered values.</returns>
		public Raster<A> Filter<A>(Func<T[], A> filterOperation, double radius, T borderValue) where A : struct, IEquatable<A>
		{
			int maxDistance;
			var neighbouresXYOffsets = GenerateNeighbouresXYOffsets(radius, out maxDistance);
			var neighbouresInTileIndexOffsets = ConvertToInTileIndexOffsets(neighbouresXYOffsets, TileWidth);

			return Filter(filterOperation, maxDistance, neighbouresInTileIndexOffsets, neighbouresXYOffsets, borderValue);
		}

		/// <summary>
		/// Performs a <paramref name="filterOperation"/> on a <see cref="Raster{T}"/>.
		/// </summary>
		/// <param name="filterOperation">The function to calculate the result value for a structure element.</param>
		/// <param name="maxDistance">Distance of the furthest 'structuring elements' in one dimension from the current position [in number of cells].</param>
		/// <param name="neighbouresInTileIndexOffsets">Relative index offset to the current position in a tile which define the structuring elements.</param>
		/// <param name="neighbouresXYOffsets">Alternating relative x and y coordinate to the current position which define the structuring elements.</param>
		/// <param name="borderValue">The value to be given to the <paramref name="filterOperation"/> for elements outside of the raster.</param>
		/// <returns>The raster with the filtered values.</returns>
		public Raster<A> Filter<A>(Func<T[], A> filterOperation, int maxDistance, int[] neighbouresInTileIndexOffsets, int[] neighbouresXYOffsets, T borderValue) where A : struct, IEquatable<A>
		{
			if (maxDistance <= 0) throw new ArgumentException("Must be greater than zero(0).", nameof(maxDistance));
			if (null == neighbouresInTileIndexOffsets) throw new ArgumentNullException(nameof(neighbouresInTileIndexOffsets));
			if (null == neighbouresXYOffsets) throw new ArgumentNullException(nameof(neighbouresXYOffsets));
			if (neighbouresXYOffsets.Length % 2 != 0) throw new ArgumentException("Must have an even number of elements.", nameof(neighbouresXYOffsets));
			if (neighbouresXYOffsets.Length / 2 != neighbouresInTileIndexOffsets.Length) throw new ArgumentException("Must have twice as many elements than " + nameof(neighbouresInTileIndexOffsets) + ".", nameof(neighbouresXYOffsets));

			// Local copies are faster.
			var dist = maxDistance;
			int[] nitio = neighbouresInTileIndexOffsets, nxyo = neighbouresXYOffsets;
			int width = Width, height = Height, tileWidth = TileWidth, tileHeight = TileHeight, numberOfTileX = NumberOfTileX;
			var orgData = Data;

			var ret = new Raster<A>(width, height, true, tileWidth, tileHeight);
			var retData = ret.Data;

			// Let's do it by tile to keep the data accesses local.
			for (int tile = 0; tile < retData.Length; tile++)
			{
				var retTile = retData[tile];
				var orgTile = orgData[tile]; // Used for fast access path only.

				var tileStartX = tileWidth * (tile % numberOfTileX);
				var tileStartY = tileHeight * (tile / numberOfTileX);

				var amountX = Math.Min(width - tileStartX, tileWidth);
				var amountY = Math.Min(height - tileStartY, tileHeight);

				Parallel.For(0, amountY, line =>
				{
					bool fast = line > dist && line < amountY - dist; // We can use the neighbouresInTileIndexOffsets when we're not at the border of the tile.

					var index = line * tileWidth;

					var rasterLine = line + tileStartY;

					T[] structurElement = new T[nitio.Length];

					for (int samp = 0; samp < amountX; samp++, index++)
					{
						if (fast && samp > dist && samp < amountX - dist) // We can use the neighbouresInTileIndexOffsets when we're not at the border of the tile.
						{
							for (int i = 0; i < nitio.Length; i++) structurElement[i] = orgTile[index + nitio[i]];
						}
						else
						{
							var rasterSamp = samp + tileStartX;
							for (int i = 0, a = 0; i < nxyo.Length; a++)
							{
								var x = rasterSamp + nxyo[i++]; // First read the x value...
								var y = rasterLine + nxyo[i++]; // ...then the y value.
								if (x < 0 || x >= width || y < 0 || y >= height)
								{
									structurElement[a] = borderValue;
									continue;
								}

								int xTile = x / tileWidth, yTile = y / tileHeight, xInTile = x % tileWidth, yInTile = y % tileHeight;

								structurElement[a] = orgData[yTile * numberOfTileX + xTile][yInTile * tileWidth + xInTile];
							}
						}

						retTile[index] = filterOperation(structurElement);
					}
				});
			}
			return ret;
		}
		#endregion

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
