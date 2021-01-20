using Microsoft.Extensions.DependencyInjection;

namespace ZptSharp.Hosting
{
    /// <summary>
    /// A builder type which is used to activate optional/elective parts of the ZptSharp functionality,
    /// as part of setting up a ZptSharp environment.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This object facilitates addition of services, similarly to the way in which a <see cref="IServiceCollection" /> is used.
    /// It is intended for usage via its extension methods, each one performing a set of related set-up.
    /// This hosting builder object is always consumed via its interface <see cref="IBuildsHostingEnvironment"/>.
    /// It may be created from one of three ways:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>From the <see cref="ZptSharpServiceCollectionExtensions.AddZptSharp(IServiceCollection)"/> method</description>
    /// </item>
    /// <item>
    /// <description>As a parameter to the constructor of an MVC View Engine</description>
    /// </item>
    /// <item>
    /// <description>As a parameter to the <see cref="ZptSharpHost.GetHost(System.Action{IBuildsHostingEnvironment})"/> method</description>
    /// </item>
    /// </list>
    /// <para>
    /// This object is used to set-up which parts of the ZptSharp functionality are activated, such as add-on packages
    /// like document providers and expression evaluators. Activating each piece of functionality is performed via an
    /// extension method to the <see cref="IBuildsHostingEnvironment"/> interface. The absolute minimum required for a
    /// working ZptSharp environment is at least one activated document provider &amp; at least one expression evaluator.
    /// A typical application might set that up as follows:
    /// </para>
    /// <code>
    /// builder
    ///     .AddStandardZptExpressions()
    ///     .AddHapZptDocuments();
    /// </code>
    /// <para>
    /// You are encouraged to read through the avaialble extension methods to discover what is available.
    /// </para>
    /// </remarks>
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