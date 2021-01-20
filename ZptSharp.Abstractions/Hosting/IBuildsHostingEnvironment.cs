using Microsoft.Extensions.DependencyInjection;

namespace ZptSharp.Hosting
{
    /// <summary>
    /// A helper object which is used to activate optional/elective parts of the ZptSharp functionality.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This object facilitates addition of services, similarly to the way in which a <see cref="IServiceCollection" /> is used.
    /// It is intended for usage via its extension methods, each one performing a set of related set-up.
    /// Implementations of this hosting builder object are generally created in one of three ways:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>From the <c>ZptSharpServiceCollectionExtensions.AddZptSharp(IServiceCollection)</c> method</description>
    /// </item>
    /// <item>
    /// <description>As a parameter to the constructor of an MVC View Engine</description>
    /// </item>
    /// <item>
    /// <description>As a parameter to the <c>ZptSharpHost.GetHost(System.Action{IBuildsHostingEnvironment})</c> method</description>
    /// </item>
    /// </list>
    /// <para>
    /// This object is used to set-up which parts of the ZptSharp functionality are activated, such as add-on packages
    /// like document providers and expression evaluators. Activating each piece of functionality is performed via an
    /// extension method to this interface. The absolute minimum required for a
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