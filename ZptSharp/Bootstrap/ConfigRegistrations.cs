using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Config;

namespace ZptSharp.Bootstrap
{
    /// <summary>
    /// Dependency injection registrations for types in the Config namespace.
    /// </summary>
    static class ConfigRegistrations
    {
        static internal void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IStoresCurrentRenderingConfig, ConfigurationServiceLocator>();
            services.AddScoped(s => s.GetRequiredService<IStoresCurrentRenderingConfig>().GetConfiguration());
        }
    }
}
