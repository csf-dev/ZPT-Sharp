using System;
using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Config;
using ZptSharp.Rendering;

namespace ZptSharp.Bootstrap
{
    public class ServiceProviderFactory
    {
        public IServiceProvider GetServiceProvider(RenderingConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (config.ServiceProvider != null) return config.ServiceProvider;

            if (config.DocumentProvider == null)
                throw new ArgumentException($"If the default service provider is to be used (IE: {nameof(RenderingConfig)}.{nameof(RenderingConfig.ServiceProvider)} is null), " +
                	                        $"the configuration must specify a non-null {nameof(RenderingConfig.DocumentProvider)}.");

            var builder = new ServiceCollection();
            RegisterCommonServices(builder);
            builder.AddSingleton(config.DocumentProvider);
            return builder.BuildServiceProvider();
        }

        void RegisterCommonServices(IServiceCollection builder)
        {
            builder.AddTransient<ZptRequestRenderer>();
        }
    }
}
