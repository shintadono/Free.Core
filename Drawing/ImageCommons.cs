namespace Free.Core.Drawing
{
	/// <summary>
	/// Describes the common interface of all image and raster classes.
	/// </summary>
	public interface ImageCommons
	{
		/// <summary>
		/// Gets the width of the image / raster.
		/// </summary>
		int Width { get; }

		/// <summary>
		/// Gets the height of the image / raster.
		/// </summary>
		int Height { get; }

		/// <summary>
		/// Gets the number of channels in the image / raster.
		/// </summary>
		/// <remarks>
		/// The elements of a structure channel build one channel (not a channel for each element of the structure). Although
		/// you can understand the elements of a structure as serveral channels, this does not represent the idea of the property.
		/// If a <see cref="ChannelType"/> of <see cref="ChannelType.Structure">Structure</see> is used, it is highly recommended,
		/// to use just one channel.
		/// </remarks>
		int NumberOfChannels { get; }

		/// <summary>
		/// Gets the types of all <see cref="NumberOfChannels"/> many channels. The order is must represent the actual order of the
		/// channels inside the image / raster storage. Images / raster with inhomogeneous channel types must also have a
		/// <see cref="PlanarConfiguration.Separated">separated</see> <see cref="PlanarConfiguration">PlanarConfiguration</see>.
		/// </summary>
		ChannelType[] ChannelTypes { get; }

		/// <summary>
		/// Gets the planar configuration of the image / raster storage. This must be <see cref="PlanarConfiguration.Continuously"/>
		/// if <see cref="NumberOfChannels"/> is one (1).
		/// </summary>
		PlanarConfiguration PlanarConfiguration { get; }

		/// <summary>
		/// Gets the layout of the image / raster storage.
		/// </summary>
		StorageLayout StorageLayout { get; }
	}
}
