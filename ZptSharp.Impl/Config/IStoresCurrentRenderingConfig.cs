namespace ZptSharp.Config
{
    /// <summary>
    /// An object which provides access to the rendering configuration.
    /// This interface is only used within the scope of a rendering request,
    /// and so generally-speaking should not be used as part of the public API.
    /// </summary>
    public interface IStoresCurrentRenderingConfig
    {
        /// <summary>
        /// Gets or sets the current configuration.
        /// </summary>
        /// <value>The configuration.</value>
        RenderingConfig Configuration { get; set; }

        /// <summary>
        /// Gets the rendering configuration.
        /// </summary>
        /// <returns>The configuration.</returns>
        RenderingConfig GetConfiguration();
    }
}
