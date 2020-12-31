using Microsoft.Extensions.DependencyInjection;
using ZptSharp.BulkRendering;

namespace ZptSharp.Bootstrap
{
    /// <summary>
    /// Dependency injection registrations for types in the BulkRendering namespace.
    /// </summary>
    class BulkRenderingRegistrations
    {
        static internal void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IRendersManyFiles, BulkFileRenderer>();
            services.AddTransient<IValidatesBulkRenderingRequest, BulkRenderingRequestValidator>();
            services.AddTransient<IRendersInputFile, FileRendererAndSaver>();
            services.AddTransient<IGetsInputFiles, InputFilesProvider>();
        }
    }
}
