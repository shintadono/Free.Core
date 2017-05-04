using System;

namespace Free.Core.Drawing
{
	/// <summary>
	/// Describes a single-chunked image of 1, 3 (RGB) or 4 (RGBX) unsigned 16-bit integers per pixel.
	/// </summary>
	/// <remarks>HINT: If the <see cref="PlanarConfiguration"/> of the image is <see cref="PlanarConfiguration.Separated"/>
	/// then there is a chunk for each channel.</remarks>
	[CLSCompliant(false)]
	public class Image16 : ChunkedImage16
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="width">The width of the image in pixels. Must be greater zero (0)</param>
		/// <param name="height">The height of the image in pixels. Must be greater zero (0)</param>
		/// <param name="RGB">Indicates whether the image shall be RGB or single-channeled.</param>
		/// <param name="extraChannel">Indicates whether a RGB image shall contain an extra channel, e.g. for alpha. Must be <b>false</b> if <paramref name="RGB"/> is <b>false</b>.</param>
		/// <param name="init">Indicates whether the <see cref="Bits"/> array shall be created and initialize with zeros (0).</param>
		/// <param name="planarConfig">The planar configuration to use. Single-channel images must be continuously.</param>
		public Image16(int width, int height, bool RGB, bool extraChannel, bool init, PlanarConfiguration planarConfig = PlanarConfiguration.Continuously) :
			base(width, height, RGB, extraChannel, init, height, planarConfig)
		{ }

		/// <summary>
		/// Creates a new instance with a specified color.
		/// </summary>
		/// <param name="width">The width of the image in pixels. Must be greater zero (0)</param>
		/// <param name="height">The height of the image in pixels. Must be greater zero (0)</param>
		/// <param name="RGB">Indicates whether the image shall be RGB or single-channeled.</param>
		/// <param name="extraChannel">Indicates whether a RGB image shall contain an extra channel, e.g. for alpha. Must be <b>false</b> if <paramref name="RGB"/> is <b>false</b>.</param>
		/// <param name="initColor">The initial color. ( Layout: [I] or [R] [G] [B] (/[X]) )</param>
		/// <param name="planarConfig">The planar configuration to use. Single-channel images must be continuously.</param>
		public Image16(int width, int height, bool RGB, bool extraChannel, ushort[] initColor, PlanarConfiguration planarConfig = PlanarConfiguration.Continuously) :
			base(width, height, RGB, extraChannel, initColor, height, planarConfig)
		{ }

		/// <summary>
		/// Creates a new instance with a specified color.
		/// </summary>
		/// <param name="width">The width of the image in pixels. Must be greater zero (0)</param>
		/// <param name="height">The height of the image in pixels. Must be greater zero (0)</param>
		/// <param name="RGB">Indicates whether the image shall be RGB or single-channeled.</param>
		/// <param name="extraChannel">Indicates whether a RGB image shall contain an extra channel, e.g. for alpha. Must be <b>false</b> if <paramref name="RGB"/> is <b>false</b>.</param>
		/// <param name="initColor">The initial color. (Layout: 0xXXXXRRRRGGGGBBBB)</param>
		/// <param name="planarConfig">The planar configuration to use. Single-channel images must be continuously.</param>
		public Image16(int width, int height, bool RGB, bool extraChannel, long initColor = 0, PlanarConfiguration planarConfig = PlanarConfiguration.Continuously) :
			base(width, height, RGB, extraChannel, initColor, height, planarConfig)
		{ }
		#endregion
	}
}
