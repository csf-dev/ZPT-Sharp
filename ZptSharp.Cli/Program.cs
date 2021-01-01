using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ZptSharp
{
    /// <summary>
    /// Entry-point to the CLI app process, hosting the <c>static void Main</c> method.
    /// This uses .NET Generic Host to wire up and run an instance of <see cref="Application" />.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Serves as the entry point to the application process.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) => {
                    services
                        .AddZptSharp()
                        .AddAngleSharpZptDocuments()
                        .AddHapZptDocuments()
                        .AddXmlZptDocuments()
                        .AddZptPythonExpressions()
                        .AddSingleton(typeof(CliArguments), s => GetCliArguments(args))
                        .AddTransient<IGetsBulkRenderingRequest, BulkRenderingRequestFactory>()
                        .AddTransient<ILoadsModel, ModelLoader>()
                        .AddTransient<IConfiguresServices, ServiceConfigurator>();

                    services.AddHostedService<Application>();
                });

            host.Build().Run();
        }

        static CliArguments GetCliArguments(string[] args) => CliArgumentsParser.Parse(args);
    }
}
