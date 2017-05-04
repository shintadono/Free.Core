namespace Free.Core.Drawing
{
	/// <summary>
	/// Describes the blend modes for drawing operations.
	/// </summary>
	public enum DrawBlendMode
	{
		/// <summary>
		/// Overrides the values in the destination image with the values from the source image. (Including extra channel, if given.)
		/// Gray / intensity values are expanded the RGB, if necessary. If source doesn't have extra channel values, the extra
		/// channel of the destination image remains unchanged.
		/// </summary>
		Overwrite,

		/// <summary>
		/// Pixel of source and destination image will be blend using the extra channel values of the source image.
		/// (<c>result = dst * (1-sourceAlpha) + src * sourceAlpha;</c>) If source image doesn't
		/// have extra channel values, <see cref="Overwrite"/> mode is applied.
		/// </summary>
		SourceAlpha,

		/// <summary>
		/// Pixel of source and destination image will be blend using the extra channel values of the source image.
		/// (<c>result = dst * sourceAlpha + src * (1-sourceAlpha);</c>) If source image doesn't
		/// have extra channel values, <see cref="Overwrite"/> mode is applied.
		/// </summary>
		MinusSourceAlpha,
	}
}
