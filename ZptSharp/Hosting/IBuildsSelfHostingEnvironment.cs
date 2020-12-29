using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace ZptSharp.Hosting
{
    /// <summary>
    /// A helper object which is used to build a self-hosting ZptSharp environment.
    /// This permits the addition of services to that environment, similarly to the way
    /// in which a <see cref="IServiceCollection" /> is used.
    /// </summary>
    public interface IBuildsSelfHostingEnvironment
    {
        /// <summary>
        /// Gets a collection of registration callbacks to be executed upon a service collection.
        /// </summary>
        /// <value>The service registrations.</value>
        ICollection<Action<IServiceCollection>> ServiceRegistrations { get; }

        /// <summary>
        /// Gets a collection of service-usage callbacks to be executed upon a service provider.
        /// </summary>
        /// <value>The service usages.</value>
        ICollection<Action<IServiceProvider>> ServiceUsages { get; }
    }
}