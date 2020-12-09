using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Rendering;

namespace ZptSharp.Bootstrap
{
    /// <summary>
    /// Dependency injection registrations for types in the Rendering namespace.
    /// </summary>
    class RenderingRegistrations
    {
        internal void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IGetsRootExpressionContext, ExpressionContextFactory>();
            services.AddTransient<IGetsChildExpressionContexts, ExpressionContextFactory>();
            services.AddTransient<IGetsIterativeExpressionContextProcessor, IterativeExpressionContextProcessorFactory>();
            services.AddTransient<IIterativelyModifiesDocument, IterativeDocumentModifier>();
            services.AddTransient<IGetsDocumentModifier, ZptDocumentModifierFactory>();
            services.AddTransient<IGetsZptDocumentRenderer, ZptDocumentRendererFactory>();
            services.AddTransient<IRendersRenderingRequest, ZptRequestRenderer>();
            services.AddTransient<IRendersZptDocument, ZptDocumentRenderer>();
            services.AddTransient<IRendersZptFile, ZptFileRenderer>();
            services.AddTransient<IGetsZptElementAndAttributeRemovalContextProcessor, ZptCleanupContextProcessorFactory>();
        }
    }
}
