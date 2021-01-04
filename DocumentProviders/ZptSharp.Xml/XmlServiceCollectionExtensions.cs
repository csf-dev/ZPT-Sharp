using System;
using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Dom;
using ZptSharp.Hosting;

namespace ZptSharp
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> instances.
    /// </summary>
    public static class XmlServiceCollectionExtensions
    {
        /// <summary>
        /// <para>
        /// Adds service registrations to the <paramref name="serviceCollection"/> in order
        /// to enable reading &amp; writing of XML documents.
        /// </para>
        /// <para>
        /// You must also call <see cref="UseXmlZptDocuments(IServiceProvider)"/> in your application's
        /// startup logic, in order to register 
        /// </para>
        /// </summary>
        /// <returns>The same service collection instance, with registrations added.</returns>
        /// <param name="serviceCollection">A service collection to which registrations will be added.</param>
        public static IServiceCollection AddXmlZptDocuments(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddTransient<XmlDocumentProvider>();
            serviceCollection.AddTransient<IGetsXhtmlDtds, Dom.Resources.EmbeddedResourceXhtmlDtdProvider>();
            serviceCollection.AddTransient<IGetsXmlReaderSettings, XmlReaderSettingsFactory>();

            return serviceCollection;
        }

        /// <summary>
        /// Adds both service registrations and a usage-callback so that XML documents may be
        /// used within a self-contained ZptSharp environment.
        /// </summary>
        /// <param name="builder">The self-hosting builder.</param>
        /// <returns>The self-hosting builder instance, after setting it up.</returns>
        public static IBuildsSelfHostingEnvironment AddXmlZptDocuments(this IBuildsSelfHostingEnvironment builder)
        {
            builder.ServiceRegistrations.Add(s => s.AddXmlZptDocuments());
            builder.ServiceUsages.Add(p => p.UseXmlZptDocuments());
            return builder;
        }

        /// <summary>
        /// Configures ZPT Sharp to use the XML document provider when rendering HTML files.
        /// </summary>
        /// <returns>The same service provider instance, after setting it up.</returns>
        /// <param name="provider">The service provider.</param>
        public static IServiceProvider UseXmlZptDocuments(this IServiceProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            var registry = provider.GetRequiredService<IRegistersDocumentReaderWriter>();
            registry.RegisterDocumentReaderWriter(provider.GetRequiredService<XmlDocumentProvider>());
            return provider;
        }
    }
}