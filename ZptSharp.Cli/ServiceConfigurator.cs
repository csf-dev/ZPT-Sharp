using System;
using ZptSharp.Hosting;

namespace ZptSharp.Cli
{
    /// <summary>
    /// Logic which sets up the ZptSharp services to be used
    /// within an <see cref="IBuildsHostingEnvironment" />.
    /// </summary>
    public class ServiceConfigurator
    {
        readonly IBuildsHostingEnvironment builder;

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public void ConfigureServices(CliArguments args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            builder
                .AddStandardZptExpressions()
                .AddZptPythonExpressions()
                .AddXmlZptDocuments();

            if(args.UseAngleSharp)
                builder.AddAngleSharpZptDocuments();
            else
                builder.AddHapZptDocuments();
        }
        
        /// <summary>
        /// Initializes a new instance of <see cref="ServiceConfigurator" />.
        /// </summary>
        /// <param name="builder">The environment builder.</param>
        public ServiceConfigurator(IBuildsHostingEnvironment builder)
        {
            this.builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }
    }
}