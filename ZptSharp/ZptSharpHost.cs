using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
        public static IHostsZptSharp GetHost(Action<IBuildsHostingEnvironment> builderAction)
        {
            var serviceProvider = GetServiceProvider(builderAction);
            return new ZptSharpSelfHoster(serviceProvider);
        }

        static IServiceProvider GetServiceProvider(Action<IBuildsHostingEnvironment> builderAction)
        {
            if (builderAction is null)
                throw new System.ArgumentNullException(nameof(builderAction));

            var builder = new ServiceCollection().AddZptSharp();
            builder.ServiceCollection.AddLogging(b => b.AddProvider(NullLoggerProvider.Instance));

            builderAction(builder);

            return builder.ServiceCollection.BuildServiceProvider();
        }
    }
}