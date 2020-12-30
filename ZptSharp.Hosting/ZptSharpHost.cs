using System;
using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Hosting;

namespace ZptSharp
{
    /// <summary>
    /// A utility to get an instance of <see cref="IHostsZptSharp" />, enabling usage of ZptSharp
    /// without configuring it within your own application's dependency injection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Please note that this class is provided for occasions when configuring ZptSharp within your
    /// own application's dependency injection configuration is not feasible.  It is always best
    /// to fully integrate ZptSharp into your own DI config where you can.
    /// </para>
    /// <para>
    /// The extension methods for <see cref="IBuildsSelfHostingEnvironment" /> are very similar to those
    /// for <see cref="IServiceCollection" /> &amp; <see cref="IServiceProvider" />.  Note though than when
    /// using this class, there is no equivalent for <see cref="ServiceCollectionExtensions.AddZptSharp(IServiceCollection)" />.
    /// That is implied by the usage of this class.  The extension methods which are avaialable are those which
    /// represent a meaningful choice or selection of an optional add-on.
    /// </para>
    /// </remarks>
    public static class ZptSharpHost
    {
        /// <summary>
        /// Gets the ZptSharp self-contained host.
        /// </summary>
        /// <param name="builderAction">A callback used to configure the hosting environment.
        /// At minimum you must select at least one document provider
        /// &amp; at least one expression type.</param>
        /// <returns>A self-contained ZptSharp hosting object, providing access to a file &amp; document renderer.</returns>
        public static IHostsZptSharp GetHost(Action<IBuildsSelfHostingEnvironment> builderAction)
        {
            var serviceProvider = GetServiceProvider(builderAction);
            return new ZptSharpSelfHoster(serviceProvider);
        }

        static IServiceProvider GetServiceProvider(Action<IBuildsSelfHostingEnvironment> builderAction)
        {
            if (builderAction is null)
                throw new System.ArgumentNullException(nameof(builderAction));

            var helper = new SelfHostingEnvironmentBuilder();
            builderAction(helper);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddZptSharp();
            serviceCollection.AddLogging();

            foreach (var callback in helper.ServiceRegistrations)
                callback(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            foreach (var callback in helper.ServiceUsages)
                callback(serviceProvider);

            return serviceProvider;
        }
    }
}