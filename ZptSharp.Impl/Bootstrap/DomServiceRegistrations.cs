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
            services.AddTransient<IGetsDocumentReaderWriterForFile>(s => s.GetRequiredService<DocumentReaderWriterRegistry>());
            services.AddTransient<IRegistersDocumentReaderWriter>(s => s.GetRequiredService<DocumentReaderWriterRegistry>());
            services.AddTransient<IGetsWellKnownNamespace, WellKnownNamespaceProvider>();
            services.AddTransient<IOmitsNode, NodeOmitter>();
            services.AddTransient<IReplacesNode, NodeReplacer>();

            services.AddScoped<IStoresCurrentReaderWriter, ReaderWriterServiceLocator>();
            services.AddScoped(s => s.GetRequiredService<IStoresCurrentReaderWriter>().GetReaderWriter());
        }
    }
}
