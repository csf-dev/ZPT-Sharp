using Microsoft.Extensions.DependencyInjection;
using ZptSharp.SourceAnnotation;

namespace ZptSharp.Bootstrap
{
    /// <summary>
    /// Dependency injection registrations for types in the SourceAnnotation namespace.
    /// </summary>
    class SourceAnnotationServiceRegistrations
    {
        internal void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IGetsSourceAnnotationContextProcessor, SourceAnnotationContextProcessorFactory>();
            services.AddTransient<IGetsAnnotationForElement, AnnotationProvider>();
            services.AddTransient<IAddsComment, Commenter>();
        }
    }
}