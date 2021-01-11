using Microsoft.Extensions.DependencyInjection;

namespace ZptSharp.Hosting
{
    /// <summary>
    /// A helper object which is used to build a self-hosting ZptSharp environment.
    /// This permits the addition of services to that environment, similarly to the way
    /// in which a <see cref="IServiceCollection" /> is used.
    /// </summary>
    public interface IBuildsHostingEnvironment
    {
        /// <summary>
        /// Gets the service collection associated with the current builder.
        /// </summary>
        IServiceCollection ServiceCollection { get; }

        /// <summary>
        /// Gets the service registry associated with the current builder.
        /// </summary>
        EnvironmentRegistry ServiceRegistry { get; }
    }
}