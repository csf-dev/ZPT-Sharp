using System;
using ZptSharp.Config;

namespace ZptSharp.Bootstrap
{
    public class ServiceProviderFactory
    {
        public IServiceProvider GetServiceProvider(RenderingConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (config.ServiceProvider != null) return config.ServiceProvider;

            throw new NotImplementedException();
        }
    }
}
