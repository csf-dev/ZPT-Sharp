using System;
using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Dom;
using ZptSharp.Hosting;

namespace ZptSharp
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> instances.
    /// </summary>
    public static class AngleSharpServiceCollectionExtensions
    {
        /// <summary>
        /// <para>
        /// Adds service registrations to the <paramref name="serviceCollection"/> in order
        /// to enable reading &amp; writing of AngleSharp documents.
        /// </para>
        /// <para>
        /// You must also call <see cref="UseAngleSharpZptDocuments(IServiceProvider)"/> in your application's
        /// startup logic, in order to register 
        /// </para>
        /// </summary>
        /// <returns>The same service collection instance, with registrations added.</returns>
        /// <param name="serviceCollection">A service collection to which registrations will be added.</param>
        public static IServiceCollection AddAngleSharpZptDocuments(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddTransient<AngleSharpDocumentProvider>();

            return serviceCollection;
        }

        /// <summary>
        /// Adds both service registrations and a usage-callback so that AngleSharp documents may be
        /// used within a self-contained ZptSharp environment.
        /// </summary>
        /// <param name="builder">The self-hosting builder.</param>
        /// <returns>The self-hosting builder instance, after setting it up.</returns>
        public static IBuildsSelfHostingEnvironment AddAngleSharpZptDocuments(this IBuildsSelfHostingEnvironment builder)
        {
            builder.ServiceRegistrations.Add(s => s.AddAngleSharpZptDocuments());
            builder.ServiceUsages.Add(p => p.UseAngleSharpZptDocuments());
            return builder;
        }

        /// <summary>
        /// Configures ZPT Sharp to use the AngleSharp document provider when rendering HTML files.
        /// </summary>
        /// <returns>The same service provider instance, after setting it up.</returns>
        /// <param name="provider">The service provider.</param>
        public static IServiceProvider UseAngleSharpZptDocuments(this IServiceProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            var registry = provider.GetRequiredService<IRegistersDocumentReaderWriter>();
            registry.RegisterDocumentReaderWriter(provider.GetRequiredService<AngleSharpDocumentProvider>());
            return provider;
        }
    }
}
