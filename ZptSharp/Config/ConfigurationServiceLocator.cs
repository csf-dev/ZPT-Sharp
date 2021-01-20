using System;
namespace ZptSharp.Config
{
    /// <summary>
    /// A really simple class which holds a reference to an instance of <see cref="RenderingConfig"/>.
    /// This is used for dependency-injecting a configuration object into per-scope services which need it.
    /// No logic except for <see cref="Rendering.ZptDocumentRenderer"/> and the Bootstrap DI module should
    /// actually reference this type.
    /// </summary>
    internal class ConfigurationServiceLocator : IStoresCurrentRenderingConfig
    {
        /// <summary>
        /// Gets or sets the current configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public RenderingConfig Configuration { get; set; }

        /// <summary>
        /// Gets the rendering configuration.
        /// </summary>
        /// <returns>The configuration.</returns>
        public RenderingConfig GetConfiguration()
            => Configuration ?? throw new InvalidOperationException(Resources.ExceptionMessage.MissingConfiguration);
    }
}
