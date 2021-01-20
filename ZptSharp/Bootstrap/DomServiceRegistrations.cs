using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Dom;

namespace ZptSharp.Bootstrap
{
    /// <summary>
    /// Dependency injection registrations for types in the DOM namespace.
    /// </summary>
    static class DomServiceRegistrations
    {
        static internal void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<DocumentReaderWriterRegistry>();

            services.AddTransient<ISearchesForAttributes, AttributeSearcher>();
            services.AddTransient<IGetsDocumentReaderWriterForFile, DocumentReaderWriterRegistry>();
            services.AddTransient<IGetsWellKnownNamespace, WellKnownNamespaceProvider>();
            services.AddTransient<IOmitsNode, NodeOmitter>();
            services.AddTransient<IReplacesNode, NodeReplacer>();

            services.AddScoped<IStoresCurrentReaderWriter, ReaderWriterServiceLocator>();
            services.AddTransient(ResolveReaderWriter);
        }

        static IReadsAndWritesDocument ResolveReaderWriter(IServiceProvider serviceProvider)
        {
            var scopedServiceProvider = serviceProvider.GetService<IStoresCurrentReaderWriter>();
            if(scopedServiceProvider?.ReaderWriter != null) return scopedServiceProvider.ReaderWriter;

            var environment = serviceProvider.GetRequiredService<Hosting.EnvironmentRegistry>();
            if (environment.DocumentProviderTypes.Count == 1)
                return (IReadsAndWritesDocument)serviceProvider.GetRequiredService(environment.DocumentProviderTypes.Single());

            var message = String.Format(Resources.ExceptionMessage.MissingReaderWriter,
                                        nameof(IReadsAndWritesDocument),
                                        nameof(IStoresCurrentReaderWriter),
                                        $"{nameof(Config.RenderingConfig)}.{nameof(Config.RenderingConfig.DocumentProviderType)}");
            throw new InvalidOperationException(message);
        }
    }
}
