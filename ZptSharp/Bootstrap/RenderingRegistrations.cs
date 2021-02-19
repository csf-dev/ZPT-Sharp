using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Rendering;

namespace ZptSharp.Bootstrap
{
    /// <summary>
    /// Dependency injection registrations for types in the Rendering namespace.
    /// </summary>
    static class RenderingRegistrations
    {
        static internal void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IGetsRootExpressionContext, ExpressionContextFactory>();
            services.AddTransient<IGetsChildExpressionContexts, ExpressionContextFactory>();
            services.AddTransient<IGetsIterativeExpressionContextProcessor, IterativeExpressionContextProcessorFactory>();
            services.AddTransient<IIterativelyModifiesDocument, IterativeDocumentModifier>();
            services.AddTransient<IGetsDocumentModifier, ZptDocumentModifierFactory>();
            services.AddTransient<IGetsZptDocumentRenderer, ZptDocumentRendererFactory>();
            services.AddTransient<ZptDocumentRenderer>();
            services.AddTransient<IRendersZptDocument>(serviceProvider => new EffectiveConfigSettingZptDocumentRendererDecorator(serviceProvider.GetRequiredService<ZptDocumentRenderer>()));
            services.AddTransient<IRendersZptDocument, EffectiveConfigSettingZptDocumentRendererDecorator>();
            services.AddTransient<IRendersZptFile, ZptFileRenderer>();
            services.AddTransient<IWritesStreamToTextWriter, StreamToTextWriterCopier>();
            services.AddTransient<IGetsZptNodeAndAttributeRemovalContextProcessor, ZptCleanupContextProcessorFactory>();
            services.AddTransient<IGetsZptDocumentRendererForFilePath, ZptDocumentRendererForFilePathFactory>();
        }
    }
}
