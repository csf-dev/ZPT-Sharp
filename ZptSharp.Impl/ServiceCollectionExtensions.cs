﻿using System;
using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Bootstrap;

namespace ZptSharp
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> instances.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds service registrations to the <paramref name="serviceCollection"/> in order
        /// to enable the basic/core ZPT-Sharp functionality.
        /// </summary>
        /// <returns>The same service collection instance, with registrations added.</returns>
        /// <param name="serviceCollection">A service collection to which registrations will be added.</param>
        public static IServiceCollection AddZptSharp(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            new ConfigRegistrations().RegisterServices(serviceCollection);
            new DomServiceRegistrations().RegisterServices(serviceCollection);
            new ExpressionServiceRegistrations().RegisterServices(serviceCollection);
            new MetalServiceRegistrations().RegisterServices(serviceCollection);
            new PathExpressionsRegistrations().RegisterServices(serviceCollection);
            new RenderingRegistrations().RegisterServices(serviceCollection);

            return serviceCollection;
        }
    }
}
