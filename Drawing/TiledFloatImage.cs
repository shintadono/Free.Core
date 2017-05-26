using System;

namespace Free.Core.Drawing
{
	/// <summary>
	/// Describes a tiled image of 1, 3 (RGB) or 4 (RGBX) 32-bit floating-point numbers per pixel.
	/// </summary>
	public class TiledFloatImage : TiledImage
	{
		#region Const
		/// <summary>
		/// Return value for <see cref="ChannelTypes"/>, for images with one channel.
		/// </summary>
		protected static readonly ChannelType[] IntensityFloat = new ChannelType[] { ChannelType.Single };

		/// <summary>
		/// Return value for <see cref="ChannelTypes"/>, for images with tree channel.
		/// </summary>
		protected static readonly ChannelType[] RGBFloat = new ChannelType[] { ChannelType.Single, ChannelType.Single, ChannelType.Single };

		/// <summary>
		/// Return value for <see cref="ChannelTypes"/>, for images with four channel.
		/// </summary>
		protected static readonly ChannelType[] RGBXFloat = new ChannelType[] { ChannelType.Single, ChannelType.Single, ChannelType.Single, ChannelType.Single };
		#endregion

		#region Variables
		/// <summary>
		/// Contains the pixels of the image. Only change access this directly, if you know what you are doing.
		/// </summary>
		/// <remarks>
		/// <para><b>Layout:</b></para>
		/// <para>
		/// If the image's <see cref="PlanarConfiguration"/> is <see cref="PlanarConfiguration.Continuously">continuously</see>
		/// this is an array of <b>TileCountX*TileCountY</b> arrays of <see cref="TileWidth"/>*<see cref="TileHeight"/>*<see cref="NumberOfChannels"/> <b>float</b>s each.
		/// </para>
		/// <para>
		/// If the image's <see cref="PlanarConfiguration"/> is <see cref="PlanarConfiguration.Separated">separated</see>
		/// this is an array of <b>TileCountX*TileCountY</b>*<see cref="NumberOfChannels"/> arrays of <see cref="TileWidth"/>*<see cref="TileHeight"/> <b>float</b>s each.
		/// </para>
		/// <para><b>ATTENTION: Do not reference the same tile more than once, in the main array. Unless you know what you are doing.</b></para>
		/// <para><b>ATTENTION: Do not share the same tile with other images.</b></para>
		/// </remarks>
		public float[][] Bits; // [tiles or chunks (*channels)][TileSize or ChunkSize (*channels)]
		#endregion

		#region Properties

		/// <summary>
		/// Gets/Sets the value of the specified pixel and channel.
		/// </summary>
		/// <param name="channel">The channel.</param>
		/// <param name="x">The column of the pixel.</param>
		/// <param name="y">The row of the pixel.</param>
		/// <returns>The value of the channel of the pixel.</returns>
		public float this[int channel, int x, int y]
		{
			get
			{
				if (channel < 0 || channel >= NumberOfChannels) throw new IndexOutOfRangeException(nameof(channel) + " index must be greater or equal to zero (0), and smaller than the " + nameof(NumberOfChannels) + ".");
				if (x < 0 || x >= Width) throw new IndexOutOfRangeException(nameof(x) + " index must be greater or equal to zero (0), and smaller than the " + nameof(Width) + ".");
				if (y < 0 || y >= Height) throw new IndexOutOfRangeException(nameof(y) + " index must be greater or equal to zero (0), and smaller than the " + nameof(Height) + ".");

				int xTile = x / TileWidth;
				int yTile = y / TileHeight;

				int xInTile = x % TileWidth;
				int yInTile = y % TileHeight;

				if (PlanarConfiguration == PlanarConfiguration.Continuously)
					return Bits[yTile * NumberOfTileX + xTile][(yInTile * TileWidth + xInTile) * NumberOfChannels + channel];

				return Bits[(yTile * NumberOfTileX + xTile) * NumberOfChannels + channel][yInTile * TileWidth + xInTile];
			}
			set
			{
				if (channel < 0 || channel >= NumberOfChannels) throw new IndexOutOfRangeException(nameof(channel) + " index must be greater or equal to zero (0), and smaller than the " + nameof(NumberOfChannels) + ".");
				if (x < 0 || x >= Width) throw new IndexOutOfRangeException(nameof(x) + " index must be greater or equal to zero (0), and smaller than the " + nameof(Width) + ".");
				if (y < 0 || y >= Height) throw new IndexOutOfRangeException(nameof(y) + " index must be greater or equal to zero (0), and smaller than the " + nameof(Height) + ".");

				int xTile = x / TileWidth;
				int yTile = y / TileHeight;

				int xInTile = x % TileWidth;
				int yInTile = y % TileHeight;

				if (PlanarConfiguration == PlanarConfiguration.Continuously)
					Bits[yTile * NumberOfTileX + xTile][(yInTile * TileWidth + xInTile) * NumberOfChannels + channel] = value;
				else Bits[(yTile * NumberOfTileX + xTile) * NumberOfChannels + channel][yInTile * TileWidth + xInTile] = value;
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
		/// Gets the width of the image.
		/// </summary>
		public int Width { get; protected set; }

		/// <summary>
		/// Gets the height of the image.
		/// </summary>
		public int Height { get; protected set; }

		/// <summary>
		/// Gets the number of channels in the image.
		/// </summary>
		public int NumberOfChannels { get; protected set; }

		/// <summary>
		/// Gets the types of all <see cref="NumberOfChannels"/> many channels.
		/// </summary>
		public ChannelType[] ChannelTypes { get; protected set; }

		/// <summary>
		/// Gets the planar configuration of the image storage. This is <see cref="PlanarConfiguration.Continuously"/>
		/// if <see cref="NumberOfChannels"/> is one (1).
		/// </summary>
		public PlanarConfiguration PlanarConfiguration { get; protected set; }

		/// <summary>
		/// Gets the layout of the image storage.
		/// </summary>
		public StorageLayout StorageLayout { get; protected set; }
		#endregion

		#region Implements TiledImage
		/// <summary>
		/// Gets the width of the tiles in the image. All tiles have the same width (number of samples).
		/// </summary>
		public int TileWidth { get; protected set; }

		/// <summary>
		/// Gets the height of the tiles in the image. All tiles have the same height (number of lines).
		/// </summary>
		public int TileHeight { get; protected set; }
		#endregion

		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="width">The width of the image in pixels. Must be greater zero (0).</param>
		/// <param name="height">The height of the image in pixels. Must be greater zero (0).</param>
		/// <param name="RGB">Indicates whether the image shall be RGB or single-channeled.</param>
		/// <param name="extraChannel">Indicates whether a RGB image shall contain an extra channel, e.g. for alpha. Must be <b>false</b> if <paramref name="RGB"/> is <b>false</b>.</param>
		/// <param name="init">Indicates whether the <see cref="Bits"/> array shall be created and initialize with zeros (0).</param>
		/// <param name="tileWidth">The width of the tiles in pixels. Must be greater zero (0).</param>
		/// <param name="tileHeight">The height of the tiles in pixels. Must be greater zero (0).</param>
		/// <param name="planarConfig">The planar configuration to use. Single-channel images must be continuously.</param>
		public TiledFloatImage(int width, int height, bool RGB, bool extraChannel, bool init, int tileWidth = 256, int tileHeight = 256, PlanarConfiguration planarConfig = PlanarConfiguration.Continuously)
		{
			if (width < 1) throw new ArgumentOutOfRangeException(nameof(width), "Must be greater zero (0).");
			if (height < 1) throw new ArgumentOutOfRangeException(nameof(height), "Must be greater zero (0).");
			if (tileWidth < 1) throw new ArgumentOutOfRangeException(nameof(tileWidth), "Must be greater zero (0).");
			if (tileHeight < 1) throw new ArgumentOutOfRangeException(nameof(tileHeight), "Must be greater zero (0).");
			if (extraChannel && !RGB) throw new ArgumentException("Invalid channel setup. No extra channel, unless RGB.");
			if (!RGB && planarConfig != PlanarConfiguration.Continuously) throw new ArgumentException("Invalid planar configuration. Single-channel images must be continuously.", nameof(planarConfig));

			Width = width;
			Height = height;
			TileWidth = tileWidth;
			TileHeight = tileHeight;
			PlanarConfiguration = planarConfig;

			NumberOfTileX = (width + tileWidth - 1) / tileWidth;
			NumberOfTileY = (height + tileHeight - 1) / tileHeight;

			NumberOfChannels = RGB ? (extraChannel ? 4 : 3) : 1;
			ChannelTypes = RGB ? (extraChannel ? RGBXFloat : RGBFloat) : IntensityFloat;
			StorageLayout = NumberOfTileX > 1 || NumberOfTileY > 1 ? StorageLayout.Tiled : StorageLayout.SingleChunked;

			if (init)
			{
				int tiles = NumberOfTileX * NumberOfTileY * (planarConfig == PlanarConfiguration.Separated ? NumberOfChannels : 1);
				int tileSize = TileWidth * TileHeight * (planarConfig == PlanarConfiguration.Continuously ? NumberOfChannels : 1);

				Bits = new float[tiles][];
				for (int i = 0; i < tiles; i++) Bits[i] = new float[tileSize];
			}
		}

		/// <summary>
		/// Creates a new instance with a specified color.
		/// </summary>
		/// <param name="width">The width of the image in pixels. Must be greater zero (0).</param>
		/// <param name="height">The height of the image in pixels. Must be greater zero (0).</param>
		/// <param name="RGB">Indicates whether the image shall be RGB or single-channeled.</param>
		/// <param name="extraChannel">Indicates whether a RGB image shall contain an extra channel, e.g. for alpha. Must be <b>false</b> if <paramref name="RGB"/> is <b>false</b>.</param>
		/// <param name="initColor">The initial color. ( Layout: [I] or [R] [G] [B] (/[X]) )</param>
		/// <param name="tileWidth">The width of the tiles in pixels. Must be greater zero (0).</param>
		/// <param name="tileHeight">The height of the tiles in pixels. Must be greater zero (0).</param>
		/// <param name="planarConfig">The planar configuration to use. Single-channel images must be continuously.</param>
		public TiledFloatImage(int width, int height, bool RGB, bool extraChannel, float[] initColor, int tileWidth = 256, int tileHeight = 256, PlanarConfiguration planarConfig = PlanarConfiguration.Continuously) :
			this(width, height, RGB, extraChannel, true, tileWidth, tileHeight, planarConfig)
		{
			Clear(initColor);
		}
		#endregion

		#region Methods

		void InitSingleChannelImage(float intensity)
		{
			int tiles = Bits.Length;
			int tileSize = TileWidth * TileHeight;

			for (int i = 0; i < tiles;)
			{
				var tile = Bits[i++];
				for (int j = 0; j < tileSize;) tile[j++] = intensity;
			}
		}

		void InitSeperatedImageRGB(float red, float green, float blue)
		{
			int tiles = Bits.Length;
			int tileSize = TileWidth * TileHeight;

			for (int i = 0; i < tiles;)
			{
				var tile = Bits[i++];
				for (int j = 0; j < tileSize;) tile[j++] = red;

				tile = Bits[i++];
				for (int j = 0; j < tileSize;) tile[j++] = green;

				tile = Bits[i++];
				for (int j = 0; j < tileSize;) tile[j++] = blue;
			}
		}

		void InitSeperatedImageRGBX(float red, float green, float blue, float extra)
		{
			int tiles = Bits.Length;
			int tileSize = TileWidth * TileHeight;

			for (int i = 0; i < tiles;)
			{
				var tile = Bits[i++];
				for (int j = 0; j < tileSize;) tile[j++] = red;

				tile = Bits[i++];
				for (int j = 0; j < tileSize;) tile[j++] = green;

				tile = Bits[i++];
				for (int j = 0; j < tileSize;) tile[j++] = blue;

				tile = Bits[i++];
				for (int j = 0; j < tileSize;) tile[j++] = extra;
			}
		}

		void InitContinuousImageRGB(float red, float green, float blue)
		{
			int tiles = Bits.Length;
			int tileSize = TileWidth * TileHeight * 3;

			for (int i = 0; i < tiles; i++)
			{
				var tile = Bits[i];
				for (int j = 0; j < tileSize;) { tile[j++] = red; tile[j++] = green; tile[j++] = blue; }
			}
		}

		void InitContinuousImageRGBX(float red, float green, float blue, float extra)
		{
			int tiles = Bits.Length;
			int tileSize = TileWidth * TileHeight * 4;

			for (int i = 0; i < tiles; i++)
			{
				var tile = Bits[i];
				for (int j = 0; j < tileSize;) { tile[j++] = red; tile[j++] = green; tile[j++] = blue; tile[j++] = extra; }
			}
		}

		/// <summary>
		/// Sets all pixel with the specified value.
		/// </summary>
		/// <param name="color">The color. ( Layout: [I] or [R] [G] [B] (/[X]) )</param>
		public void Clear(params float[] color)
		{
			if (color == null) throw new ArgumentNullException(nameof(color));
			if (color.Length < NumberOfChannels) throw new ArgumentException("Must contain a value for every channel.", nameof(color));

			switch (NumberOfChannels)
			{
				case 1: InitSingleChannelImage(color[0]); break;
				case 3:
					if (PlanarConfiguration == PlanarConfiguration.Continuously) InitContinuousImageRGB(color[0], color[1], color[2]);
					else InitSeperatedImageRGB(color[0], color[1], color[2]);
					break;
				case 4:
					if (PlanarConfiguration == PlanarConfiguration.Continuously) InitContinuousImageRGBX(color[0], color[1], color[2], color[3]);
					else InitSeperatedImageRGBX(color[0], color[1], color[2], color[3]);
					break;
				default: throw new Exception("Invalid " + nameof(NumberOfChannels) + ".");
			}
		}

		/// <summary>
		/// Sets the specified <paramref name="channel"/> componet of all pixel with the specified <paramref name="value"/>.
		/// </summary>
		/// <param name="channel">The channel.</param>
		/// <param name="value">The value.</param>
		public void ClearChannel(int channel, float value = 0)
		{
			if (channel < 0 || channel >= NumberOfChannels) throw new ArgumentOutOfRangeException(nameof(channel) + " index must be greater or equal to zero (0), and smaller than the " + nameof(NumberOfChannels) + ".");

			int tiles = Bits.Length;
			int noc = NumberOfChannels;

			if (PlanarConfiguration == PlanarConfiguration.Continuously)
			{
				int tileSize = TileWidth * TileHeight * noc;

				for (int i = 0; i < tiles; i++)
				{
					var tile = Bits[i];
					for (int j = channel; j < tileSize; j += noc) tile[j] = value;
				}
			}
			else
			{
				int tileSize = TileWidth * TileHeight;

				for (int i = channel; i < tiles; i += noc)
				{
					var tile = Bits[i];
					for (int j = 0; j < tileSize;) tile[j++] = value;
				}
			}
		}

		/// <summary>
		/// Draws the content of a specified <paramref name="source"/> image to this image at the specified position using the given <paramref name="blendMode"/>.
		/// If RGB is draw into a single-channel image, the mean of red, green ad blue is drawn.
		/// </summary>
		/// <param name="x">The x position of the <paramref name="source"/> image in this image.</param>
		/// <param name="y">The y position of the <paramref name="source"/> image in this image.</param>
		/// <param name="source">The image to draw.</param>
		/// <param name="blendMode">The blend mode.</param>
		public void Draw(int x, int y, TiledFloatImage source, DrawBlendMode blendMode = DrawBlendMode.Overwrite)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));

			// First let check whether the images overlap.
			if (x + source.Width <= 0 || x >= Width) return;
			if (y + source.Height <= 0 || y >= Height) return;

			// If the source has no extra channel (alpha), we fall back the overwrite-mode.
			if (source.NumberOfChannels != 4) blendMode = DrawBlendMode.Overwrite;

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
					DrawIntoTile(dstTileY * NumberOfTileX + dstTileX, source, tileStartXDest, tileStartYDest, srcX, srcY, tileAmountX, tileAmountY, blendMode);

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

		void DrawIntoTile(int tileIndex, TiledFloatImage source, int dstStartX, int dstStartY, int srcStartX, int srcStartY, int amountX, int amountY, DrawBlendMode blendMode)
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
					if (NumberOfChannels == 1)
					{
						#region NumberOfChannels == 1
						var bits = Bits[tileIndex];
						int dstStartOfSourceLineInDestTile = dstStartOfSourceTileInDestTile;

						int srcStartOfLine = tileStartYSrc * source.TileWidth + tileStartXSrc;

						if (source.PlanarConfiguration == PlanarConfiguration.Continuously || source.NumberOfChannels == 1)
						{
							var srcBits = source.Bits[srcTileY * source.NumberOfTileX + srcTileX];

							for (int y = 0; y < tileAmountY; y++)
							{
								int dst = dstStartOfSourceLineInDestTile;
								int src = srcStartOfLine * source.NumberOfChannels;

								if (blendMode == DrawBlendMode.Overwrite)
								{
									if (source.NumberOfChannels == 1)
									{
										for (int x = 0; x < tileAmountX; x++)
											bits[dst++] = srcBits[src++];
									}
									else
									{
										for (int x = 0; x < tileAmountX; x++)
										{
											bits[dst++] = (srcBits[src++] + srcBits[src++] + srcBits[src++]) / 3;
											if (source.NumberOfChannels > 3) src++; // Skip extra
										}
									}
								}
								else if (blendMode == DrawBlendMode.SourceAlpha)
								{
									for (int x = 0; x < tileAmountX; x++)
									{
										float gry = (srcBits[src++] + srcBits[src++] + srcBits[src++]) / 3;
										float alpha = srcBits[src++];
										bits[dst++] = bits[dst] * (1 - alpha) + gry * alpha;
									}
								}
								else if (blendMode == DrawBlendMode.MinusSourceAlpha)
								{
									for (int x = 0; x < tileAmountX; x++, src++)
									{
										float gry = (srcBits[src++] + srcBits[src++] + srcBits[src++]) / 3;
										float alpha = srcBits[src++];
										bits[dst++] = bits[dst] * alpha + gry * (1 - alpha);
									}
								}

								srcStartOfLine += source.TileWidth;
								dstStartOfSourceLineInDestTile += TileWidth;
							}
						}
						else //if (source.PlanarConfiguration == PlanarConfiguration.Separated)
						{
							int srcTileIndex = (srcTileY * source.NumberOfTileX + srcTileX) * source.NumberOfChannels;
							var srcBitsRed = source.Bits[srcTileIndex];
							var srcBitsGrn = source.Bits[srcTileIndex + 1];
							var srcBitsBlu = source.Bits[srcTileIndex + 2];
							var srcBitsAlp = blendMode != DrawBlendMode.Overwrite ? source.Bits[srcTileIndex + 3] : null;

							for (int y = 0; y < tileAmountY; y++)
							{
								int dst = dstStartOfSourceLineInDestTile;
								int src = srcStartOfLine;

								if (blendMode == DrawBlendMode.Overwrite)
								{
									for (int x = 0; x < tileAmountX; x++, src++)
										bits[dst++] = (srcBitsRed[src] + srcBitsGrn[src] + srcBitsBlu[src]) / 3;
								}
								else if (blendMode == DrawBlendMode.SourceAlpha)
								{
									for (int x = 0; x < tileAmountX; x++, src++)
									{
										float alpha = srcBitsAlp[src];
										bits[dst++] = bits[dst] * (1 - alpha) + (srcBitsRed[src] + srcBitsGrn[src] + srcBitsBlu[src]) * alpha / 3;
									}
								}
								else if (blendMode == DrawBlendMode.MinusSourceAlpha)
								{
									for (int x = 0; x < tileAmountX; x++, src++)
									{
										float alpha = srcBitsAlp[src];
										bits[dst++] = bits[dst] * alpha + (srcBitsRed[src] + srcBitsGrn[src] + srcBitsBlu[src]) * (1 - alpha) / 3;
									}
								}

								srcStartOfLine += source.TileWidth;
								dstStartOfSourceLineInDestTile += TileWidth;
							}
						}
						#endregion
					}
					else if (PlanarConfiguration == PlanarConfiguration.Continuously)
					{
						#region PlanarConfiguration == PlanarConfiguration.Continuously
						var bits = Bits[tileIndex];
						int dstStartOfSourceLineInDestTile = dstStartOfSourceTileInDestTile;

						int srcStartOfLine = tileStartYSrc * source.TileWidth + tileStartXSrc;

						if (source.PlanarConfiguration == PlanarConfiguration.Continuously || source.NumberOfChannels == 1)
						{
							var srcBits = source.Bits[srcTileY * source.NumberOfTileX + srcTileX];

							for (int y = 0; y < tileAmountY; y++)
							{
								int dst = dstStartOfSourceLineInDestTile * NumberOfChannels;
								int src = srcStartOfLine * source.NumberOfChannels;

								if (blendMode == DrawBlendMode.Overwrite)
								{
									if (source.NumberOfChannels == 1)
									{
										for (int x = 0; x < tileAmountX; x++)
										{
											float val = srcBits[src++];
											bits[dst++] = val; bits[dst++] = val; bits[dst++] = val;
											if (NumberOfChannels > 3) dst++; // Leave extra untouched.
										}
									}
									else
									{
										for (int x = 0; x < tileAmountX; x++)
										{
											bits[dst++] = srcBits[src++];
											bits[dst++] = srcBits[src++];
											bits[dst++] = srcBits[src++];

											if (NumberOfChannels > 3)
											{
												if (source.NumberOfChannels > 3) bits[dst++] = srcBits[src++];
												else dst++; // Leave extra untouched.
											}
											else if (source.NumberOfChannels > 3) src++; // Skip extra
										}
									}
								}
								else if (blendMode == DrawBlendMode.SourceAlpha)
								{
									for (int x = 0; x < tileAmountX; x++)
									{
										float red = srcBits[src++]; float grn = srcBits[src++]; float blu = srcBits[src++]; float alpha = srcBits[src++];

										bits[dst++] = bits[dst] * (1 - alpha) + red * alpha;
										bits[dst++] = bits[dst] * (1 - alpha) + grn * alpha;
										bits[dst++] = bits[dst] * (1 - alpha) + blu * alpha;
										if (NumberOfChannels > 3) bits[dst++] = alpha;
									}
								}
								else if (blendMode == DrawBlendMode.MinusSourceAlpha)
								{
									for (int x = 0; x < tileAmountX; x++)
									{
										float red = srcBits[src++]; float grn = srcBits[src++]; float blu = srcBits[src++]; float alpha = srcBits[src++];

										bits[dst++] = bits[dst] * alpha + red * (1 - alpha);
										bits[dst++] = bits[dst] * alpha + grn * (1 - alpha);
										bits[dst++] = bits[dst] * alpha + blu * (1 - alpha);
										if (NumberOfChannels > 3) bits[dst++] = alpha;
									}
								}

								srcStartOfLine += source.TileWidth;
								dstStartOfSourceLineInDestTile += TileWidth;
							}
						}
						else //if (source.PlanarConfiguration == PlanarConfiguration.Separated)
						{
							int srcTileIndex = (srcTileY * source.NumberOfTileX + srcTileX) * source.NumberOfChannels;
							var srcBitsRed = source.Bits[srcTileIndex];
							var srcBitsGrn = source.Bits[srcTileIndex + 1];
							var srcBitsBlu = source.Bits[srcTileIndex + 2];
							var srcBitsAlp = source.NumberOfChannels > 3 ? source.Bits[srcTileIndex + 3] : null;

							for (int y = 0; y < tileAmountY; y++)
							{
								int dst = dstStartOfSourceLineInDestTile * NumberOfChannels;
								int src = srcStartOfLine;

								if (blendMode == DrawBlendMode.Overwrite)
								{
									for (int x = 0; x < tileAmountX; x++, src++)
									{
										bits[dst++] = srcBitsRed[src];
										bits[dst++] = srcBitsGrn[src];
										bits[dst++] = srcBitsBlu[src];

										if (NumberOfChannels > 3)
										{
											if (srcBitsAlp != null) bits[dst++] = srcBitsAlp[src];
											else dst++; // Leave extra untouched.
										}
									}
								}
								else if (blendMode == DrawBlendMode.SourceAlpha)
								{
									for (int x = 0; x < tileAmountX; x++, src++)
									{
										float alpha = srcBitsAlp[src];

										bits[dst++] = bits[dst] * (1 - alpha) + srcBitsRed[src] * alpha;
										bits[dst++] = bits[dst] * (1 - alpha) + srcBitsGrn[src] * alpha;
										bits[dst++] = bits[dst] * (1 - alpha) + srcBitsBlu[src] * alpha;
										if (NumberOfChannels > 3) bits[dst++] = alpha;
									}
								}
								else if (blendMode == DrawBlendMode.MinusSourceAlpha)
								{
									for (int x = 0; x < tileAmountX; x++, src++)
									{
										float alpha = srcBitsAlp[src];

										bits[dst++] = bits[dst] * alpha + srcBitsRed[src] * (1 - alpha);
										bits[dst++] = bits[dst] * alpha + srcBitsGrn[src] * (1 - alpha);
										bits[dst++] = bits[dst] * alpha + srcBitsBlu[src] * (1 - alpha);
										if (NumberOfChannels > 3) bits[dst++] = alpha;
									}
								}

								srcStartOfLine += source.TileWidth;
								dstStartOfSourceLineInDestTile += TileWidth;
							}
						}
						#endregion
					}
					else //if (PlanarConfiguration == PlanarConfiguration.Separated)
					{
						#region PlanarConfiguration == PlanarConfiguration.Separated
						var bitsRed = Bits[tileIndex * NumberOfChannels];
						var bitsGrn = Bits[tileIndex * NumberOfChannels + 1];
						var bitsBlu = Bits[tileIndex * NumberOfChannels + 2];
						var bitsAlp = NumberOfChannels > 3 ? Bits[tileIndex * NumberOfChannels + 3] : null;

						int dstStartOfSourceLineInDestTile = dstStartOfSourceTileInDestTile;

						int srcStartOfLine = tileStartYSrc * source.TileWidth + tileStartXSrc;

						if (source.PlanarConfiguration == PlanarConfiguration.Continuously || source.NumberOfChannels == 1)
						{
							var srcBits = source.Bits[srcTileY * source.NumberOfTileX + srcTileX];

							for (int y = 0; y < tileAmountY; y++)
							{
								int dst = dstStartOfSourceLineInDestTile;
								int src = srcStartOfLine * source.NumberOfChannels;

								if (blendMode == DrawBlendMode.Overwrite)
								{
									if (source.NumberOfChannels == 1)
									{
										for (int x = 0; x < tileAmountX; x++, dst++)
										{
											bitsRed[dst] = bitsGrn[dst] = bitsBlu[dst] = srcBits[src++];
											// Leave extra untouched.
										}
									}
									else
									{
										for (int x = 0; x < tileAmountX; x++, dst++)
										{
											bitsRed[dst] = srcBits[src++];
											bitsGrn[dst] = srcBits[src++];
											bitsBlu[dst] = srcBits[src++];

											if (bitsAlp != null)
											{
												if (source.NumberOfChannels > 3) bitsAlp[dst] = srcBits[src++];
												// Leave extra untouched.
											}
											else if (source.NumberOfChannels > 3) src++; // Skip extra
										}
									}
								}
								else if (blendMode == DrawBlendMode.SourceAlpha)
								{
									for (int x = 0; x < tileAmountX; x++, dst++)
									{
										float red = srcBits[src++]; float grn = srcBits[src++]; float blu = srcBits[src++]; float alpha = srcBits[src++];

										bitsRed[dst] = bitsRed[dst] * (1 - alpha) + red * alpha;
										bitsGrn[dst] = bitsGrn[dst] * (1 - alpha) + grn * alpha;
										bitsBlu[dst] = bitsBlu[dst] * (1 - alpha) + blu * alpha;
										if (bitsAlp != null) bitsAlp[dst] = alpha;
									}
								}
								else if (blendMode == DrawBlendMode.MinusSourceAlpha)
								{
									for (int x = 0; x < tileAmountX; x++, dst++)
									{
										float red = srcBits[src++]; float grn = srcBits[src++]; float blu = srcBits[src++]; float alpha = srcBits[src++];

										bitsRed[dst] = bitsRed[dst] * alpha + red * (1 - alpha);
										bitsGrn[dst] = bitsGrn[dst] * alpha + grn * (1 - alpha);
										bitsBlu[dst] = bitsBlu[dst] * alpha + blu * (1 - alpha);
										if (bitsAlp != null) bitsAlp[dst] = alpha;
									}
								}

								srcStartOfLine += source.TileWidth;
								dstStartOfSourceLineInDestTile += TileWidth;
							}
						}
						else //if (source.PlanarConfiguration == PlanarConfiguration.Separated)
						{
							int srcTileIndex = (srcTileY * source.NumberOfTileX + srcTileX) * source.NumberOfChannels;
							var srcBitsRed = source.Bits[srcTileIndex];
							var srcBitsGrn = source.Bits[srcTileIndex + 1];
							var srcBitsBlu = source.Bits[srcTileIndex + 2];
							var srcBitsAlp = source.NumberOfChannels > 3 ? source.Bits[srcTileIndex + 3] : null;

							for (int y = 0; y < tileAmountY; y++)
							{
								int dst = dstStartOfSourceLineInDestTile;
								int src = srcStartOfLine;

								if (blendMode == DrawBlendMode.Overwrite)
								{
									for (int x = 0; x < tileAmountX; x++, src++, dst++)
									{
										bitsRed[dst] = srcBitsRed[src];
										bitsGrn[dst] = srcBitsGrn[src];
										bitsBlu[dst] = srcBitsBlu[src];

										if (bitsAlp != null)
										{
											if (srcBitsAlp != null) bitsAlp[dst] = srcBitsAlp[src];
											// Leave extra untouched.
										}
									}
								}
								else if (blendMode == DrawBlendMode.SourceAlpha)
								{
									for (int x = 0; x < tileAmountX; x++, src++, dst++)
									{
										float alpha = srcBitsAlp[src];

										bitsRed[dst] = bitsRed[dst] * (1 - alpha) + srcBitsRed[src] * alpha;
										bitsGrn[dst] = bitsGrn[dst] * (1 - alpha) + srcBitsGrn[src] * alpha;
										bitsBlu[dst] = bitsBlu[dst] * (1 - alpha) + srcBitsBlu[src] * alpha;
										if (bitsAlp != null) bitsAlp[dst] = alpha;
									}
								}
								else if (blendMode == DrawBlendMode.MinusSourceAlpha)
								{
									for (int x = 0; x < tileAmountX; x++, src++, dst++)
									{
										float alpha = srcBitsAlp[src];

										bitsRed[dst] = bitsRed[dst] * alpha + srcBitsRed[src] * (1 - alpha);
										bitsGrn[dst] = bitsGrn[dst] * alpha + srcBitsGrn[src] * (1 - alpha);
										bitsBlu[dst] = bitsBlu[dst] * alpha + srcBitsBlu[src] * (1 - alpha);
										if (bitsAlp != null) bitsAlp[dst] = alpha;
									}
								}

								srcStartOfLine += source.TileWidth;
								dstStartOfSourceLineInDestTile += TileWidth;
							}
						}
						#endregion
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
		/// Gets an image without the extra channel. If the image doesn't have an extra channel in the first place, the image itself is returned.
		/// If a new image is returned sizes, <see cref="PlanarConfiguration"/> and type (<see cref="FloatImage"/>, <see cref="ChunkedFloatImage"/>,
		/// or <see cref="TiledFloatImage"/>) is maintained.
		/// </summary>
		/// <returns>The image (as <see cref="FloatImage"/>, <see cref="ChunkedFloatImage"/>, or <see cref="TiledFloatImage"/> depending on the
		/// original object) without the extra channel.</returns>
		public virtual TiledFloatImage ExtraChannelRemoved()
		{
			// Handle all cases not RGBX.
			if (NumberOfChannels < 4) return this;

			// Create the result depending on this.
			TiledFloatImage ret = null;
			if (this is FloatImage) ret = new FloatImage(Width, Height, true, false, true, PlanarConfiguration);
			else if (this is ChunkedFloatImage) ret = new ChunkedFloatImage(Width, Height, true, false, true, ((ChunkedFloatImage)this).ChunkHeight, PlanarConfiguration);
			else ret = new TiledFloatImage(Width, Height, true, false, true, TileWidth, TileHeight, PlanarConfiguration);

			if (PlanarConfiguration == PlanarConfiguration.Continuously)
			{
				for (int tile = 0; tile < Bits.Length; tile++) // Visit all tiles / chunks.
				{
					var src = Bits[tile];
					var dst = ret.Bits[tile];

					for (int i = 0, j = 0; i < src.Length;)
					{
						dst[j++] = src[i++]; // Copy red channel.
						dst[j++] = src[i++]; // Copy green channel.
						dst[j++] = src[i++]; // Copy blue channel.
						i++; // Skip extra channel.
					}
				}
			}
			else
			{
				for (int srcTile = 0, dstTile = 0; srcTile < Bits.Length;) // Visit all tiles / chunks.
				{
					Bits[srcTile++].CopyTo(ret.Bits[dstTile++], 0); // Copy red channel.
					Bits[srcTile++].CopyTo(ret.Bits[dstTile++], 0); // Copy green channel.
					Bits[srcTile++].CopyTo(ret.Bits[dstTile++], 0); // Copy blue channel.
					srcTile++; // Skip extra channel.
				}
			}

			return ret;
		}

		/// <summary>
		/// Gets a sub-image at the specified position with the specified size. The new image has the same <see cref="PlanarConfiguration"/> and type (<see cref="FloatImage"/>, <see cref="ChunkedFloatImage"/>,
		/// or <see cref="TiledFloatImage"/>) as the original. Requested areas outside the image will be filled with the default values.
		/// </summary>
		/// <param name="x">The x-position of the sub-image in the image.</param>
		/// <param name="y">The y-position of the sub-image in the image.</param>
		/// <param name="width">The width of the new image.</param>
		/// <param name="height">The height of the new image.</param>
		/// <returns>The sub-image.</returns>
		public TiledFloatImage GetSubImage(int x, int y, int width, int height)
		{
			// Create the result depending on this.
			TiledFloatImage ret = null;
			if (this is FloatImage) ret = new FloatImage(width, height, NumberOfChannels >= 3, NumberOfChannels > 3, true, PlanarConfiguration);
			else if (this is ChunkedFloatImage) ret = new ChunkedFloatImage(width, height, NumberOfChannels >= 3, NumberOfChannels > 3, true, ((ChunkedFloatImage)this).ChunkHeight, PlanarConfiguration);
			else ret = new TiledFloatImage(width, height, NumberOfChannels >= 3, NumberOfChannels > 3, true, TileWidth, TileHeight, PlanarConfiguration);

			// Just draw this into the result image, and let Draw handle everything.
			ret.Draw(-x, -y, this);

			return ret;
		}

		/// <summary>
		/// Gets the content of the specified tile (specified by tile index in <paramref name="x"/> and
		/// <paramref name="y"/> direction) as a new single-chunked image, with the same
		/// <see cref="PlanarConfiguration"/> as the image.
		/// </summary>
		/// <param name="x">The index of the tile in x-direction.</param>
		/// <param name="y">The index of the tile in y-direction.</param>
		/// <returns>A single-chunked image.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If at least one of <paramref name="x"/> and
		/// <paramref name="y"/> is negative, or refers to a tile outside the image.</exception>
		public FloatImage GetTile(int x, int y)
		{
			if (x < 0 || x >= NumberOfTileX) throw new ArgumentOutOfRangeException(nameof(x));
			if (y < 0 || y >= NumberOfTileY) throw new ArgumentOutOfRangeException(nameof(y));

			var ret = new FloatImage(TileWidth, TileHeight, NumberOfChannels > 1, NumberOfChannels == 4, true, PlanarConfiguration);

			if (PlanarConfiguration == PlanarConfiguration.Continuously)
			{
				Buffer.BlockCopy(Bits[y * NumberOfTileX + x], 0, ret.Bits[0], 0, TileWidth * TileHeight * NumberOfChannels * sizeof(float));
			}
			else
			{
				int tileSize = TileWidth * TileHeight;
				for (int i = 0; i < NumberOfChannels; i++)
				{
					Buffer.BlockCopy(Bits[(y * NumberOfTileX + x) * NumberOfChannels + i], 0, ret.Bits[i], 0, tileSize * sizeof(float));
				}
			}

			return ret;
		}

		/// <summary>
		/// Gets the content of the specified tile (specified by tile index in <paramref name="x"/> and
		/// <paramref name="y"/> direction) as a new single-chunked image.
		/// </summary>
		/// <param name="x">The index of the tile in x-direction.</param>
		/// <param name="y">The index of the tile in y-direction.</param>
		/// <param name="channel">The index of the channel.</param>
		/// <returns>A single-chunked image.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If at least one of <paramref name="x"/> and
		/// <paramref name="y"/> is negative, or refers to a tile outside the image.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="channel"/> is negative, or
		/// refers to a channel outside the image.</exception>
		public FloatImage GetTile(int x, int y, int channel)
		{
			if (x < 0 || x >= NumberOfTileX) throw new ArgumentOutOfRangeException(nameof(x));
			if (y < 0 || y >= NumberOfTileY) throw new ArgumentOutOfRangeException(nameof(y));
			if (channel < 0 || channel >= NumberOfChannels) throw new ArgumentOutOfRangeException(nameof(channel));

			var ret = new FloatImage(TileWidth, TileHeight, false, false, true);

			int tileSize = TileWidth * TileHeight;

			if (NumberOfChannels > 1 && PlanarConfiguration == PlanarConfiguration.Continuously)
			{
				var src = Bits[y * NumberOfTileX + x];
				var dst = ret.Bits[0];
				int noc = NumberOfChannels;
				for (int i = 0, a = channel; i < tileSize; i++, a += noc) dst[i] = src[a];
			}
			else Buffer.BlockCopy(Bits[(y * NumberOfTileX + x) * NumberOfChannels + channel], 0, ret.Bits[0], 0, tileSize * sizeof(float));

			return ret;
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
