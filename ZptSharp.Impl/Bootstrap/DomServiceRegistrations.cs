using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Dom;

namespace ZptSharp.Bootstrap
{
    /// <summary>
    /// Dependency injection registrations for types in the DOM namespace.
    /// </summary>
    class DomServiceRegistrations
    {
        internal void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<ISearchesForAttributes, AttributeSearcher>();
            services.AddSingleton<DocumentReaderWriterRegistry>();
            services.AddTransient<IGetsDocumentReaderWriterForFile>(s => s.GetRequiredService<DocumentReaderWriterRegistry>());
            services.AddTransient<IRegistersDocumentReaderWriter>(s => s.GetRequiredService<DocumentReaderWriterRegistry>());
            services.AddTransient<IGetsWellKnownNamespace, WellKnownNamespaceProvider>();
            services.AddScoped<IStoresCurrentReaderWriter, ReaderWriterServiceLocator>();
            services.AddScoped(s => s.GetRequiredService<IStoresCurrentReaderWriter>().GetReaderWriter());
        }
    }
}
