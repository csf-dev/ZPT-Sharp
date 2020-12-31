using Microsoft.Extensions.DependencyInjection;
using ZptSharp.SourceAnnotation;

namespace ZptSharp.Bootstrap
{
    /// <summary>
    /// Dependency injection registrations for types in the SourceAnnotation namespace.
    /// </summary>
    class SourceAnnotationServiceRegistrations
    {
        static internal void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IGetsSourceAnnotationContextProcessor, SourceAnnotationContextProcessorFactory>();
            services.AddTransient<IGetsAnnotationForNode, AnnotationProvider>();
            services.AddTransient<IAddsComment, Commenter>();
            services.AddTransient<IGetsSourceAnnotationString, SourceAnnotationStringProvider>();
        }
    }
}