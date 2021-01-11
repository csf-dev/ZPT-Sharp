using Microsoft.Extensions.DependencyInjection;

namespace ZptSharp.Hosting
{
    /// <summary>
    /// Builder type used to set up a ZptSharp hosting environments.
    /// </summary>
    public class HostingBuilder : IBuildsHostingEnvironment
    {
        /// <summary>
        /// Gets the service collection associated with the current builder.
        /// </summary>
        public IServiceCollection ServiceCollection { get; }

        /// <summary>
        /// Gets the service registry associated with the current builder.
        /// </summary>
        public EnvironmentRegistry ServiceRegistry { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="HostingBuilder"/>.
        /// </summary>
        /// <param name="serviceCollection">The service collection used to build this environment.</param>
        public HostingBuilder(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection ?? throw new System.ArgumentNullException(nameof(serviceCollection));
            ServiceRegistry = new EnvironmentRegistry();
        }
    }
}