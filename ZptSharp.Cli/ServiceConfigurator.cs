using System;

namespace ZptSharp
{
    /// <summary>
    /// Implementation of <see cref="IConfiguresServices" /> which sets up
    /// the default services to be used within a <see cref="IServiceProvider" />.
    /// </summary>
    public class ServiceConfigurator : IConfiguresServices
    {
        readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public void ConfigureServices(CliArguments args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            serviceProvider
                .UseStandardZptExpressions()
                .UseZptPythonExpressions()
                .UseXmlZptDocuments();

            if(args.UseAngleSharp)
                serviceProvider.UseAngleSharpZptDocuments();
            else
                serviceProvider.UseHapZptDocuments();
        }
        
        /// <summary>
        /// Initializes a new instance of <see cref="ServiceConfigurator" />.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public ServiceConfigurator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}