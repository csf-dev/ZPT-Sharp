using System;
using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Bootstrap;
using ZptSharp.Expressions;
using ZptSharp.Hosting;

namespace ZptSharp
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> instances.
    /// </summary>
    public static class ZptSharpServiceCollectionExtensions
    {
        /// <summary>
        /// Adds service registrations to the <paramref name="serviceCollection"/> in order
        /// to enable the basic/core ZPT-Sharp functionality.
        /// </summary>
        /// <returns>A ZptSharp hosting builder.</returns>
        /// <param name="serviceCollection">A service collection to which registrations will be added.</param>
        public static IBuildsHostingEnvironment AddZptSharp(this IServiceCollection serviceCollection)
        {
            var builder = new HostingBuilder(serviceCollection);
            ZptSharpCoreServiceRegistrations.RegisterServices(builder.ServiceCollection);
            builder.ServiceCollection.AddSingleton(builder.ServiceRegistry);
            return builder;
        }
    }
}
