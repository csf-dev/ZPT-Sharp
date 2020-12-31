using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Metal;

namespace ZptSharp.Bootstrap
{
    /// <summary>
    /// Dependency injection registrations for types in the METAL namespace.
    /// </summary>
    class MetalServiceRegistrations
    {
        static internal void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IGetsMetalContextProcessor, MetalContextProcessorFactory>();
            services.AddTransient<IExpandsMacro, MacroExpander>();
            services.AddTransient<IGetsMacro, MacroProvider>();
            services.AddTransient<IGetsMetalAttributeSpecs, MetalAttributeSpecProvider>();
            services.AddTransient<IGetsMetalDocumentAdapter, MetalDocumentAdapterFactory>();
            services.AddTransient<IFillsSlots, SlotFiller>();
            services.AddTransient<IGetsSlots, SlotFinder>();
        }
    }
}
