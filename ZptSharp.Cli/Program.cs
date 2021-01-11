using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ZptSharp.Cli
{
    /// <summary>
    /// Entry-point to the CLI app process, hosting the <c>static void Main</c> method.
    /// This uses .NET Generic Host to wire up and run an instance of <see cref="Application" />.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Serves as the entry point to the application process.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args) => GetHostBuilder(args).Build().Run();

        /// <summary>
        /// Gets a .NET Generic Host <see cref="IHostBuilder" /> for the app.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <returns>A host builder, from which the app may be started.</returns>
        public static IHostBuilder GetHostBuilder(string[] args)
        {
            var cliArgs = GetCliArguments(args);

            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) => {
                    services
                        .AddSingleton(cliArgs)
                        .AddTransient<IGetsBulkRenderingRequest, BulkRenderingRequestFactory>()
                        .AddTransient<ILoadsModel, ModelLoader>();

                    var builder = services.AddZptSharp();
                    var configurator = new ServiceConfigurator(builder);
                    configurator.ConfigureServices(cliArgs);

                    services.AddHostedService<Application>();
                });
        }

        static CliArguments GetCliArguments(string[] args) => CliArgumentsParser.Parse(args);
    }
}
