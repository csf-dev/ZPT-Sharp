using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace ZptSharp.Hosting
{
    /// <summary>
    /// Implementation of <see cref="IBuildsSelfHostingEnvironment" /> which wraps a <see cref="IServiceCollection" />.
    /// </summary>
    public class SelfHostingEnvironmentBuilder : IBuildsSelfHostingEnvironment
    {
        /// <summary>
        /// Gets a collection of registration callbacks to be executed upon a service collection.
        /// </summary>
        /// <value>The service registrations.</value>
        public ICollection<Action<IServiceCollection>> ServiceRegistrations { get; } = new List<Action<IServiceCollection>>();

        /// <summary>
        /// Gets a collection of service-usage callbacks to be executed upon a service provider.
        /// </summary>
        /// <value>The service usages.</value>
        public ICollection<Action<IServiceProvider>> ServiceUsages { get; } = new List<Action<IServiceProvider>>();
    }
}