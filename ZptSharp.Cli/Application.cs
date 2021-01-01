using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ZptSharp.BulkRendering;

namespace ZptSharp
{
    /// <summary>
    /// The main application which coordinates the logic carried out by the CLI app.
    /// Note that in production usage this is created and started by a .NET Generic
    /// Host, which is set up by <see cref="Program" />.
    /// </summary>
    public class Application : IHostedService
    {
        readonly IRendersManyFiles renderer;
        readonly IGetsBulkRenderingRequest requestFactory;
        readonly IConfiguresServices serviceConfigurator;
        readonly CliArguments args;
        readonly IHostApplicationLifetime appLifetime;

        /// <summary>
        /// Starts the application asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task which completes once the app has done its work.</returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            serviceConfigurator.ConfigureServices(args);

            try
            {
                var request = await requestFactory.GetRequestAsync(args, cancellationToken)
                    .ConfigureAwait(false);
                await renderer.RenderAllAsync(request, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch(OperationCanceledException) {}

            appLifetime.StopApplication();
        }

        /// <summary>
        /// Handles graceful-shutdown for the application.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task which completes immediately.</returns>
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        /// <summary>
        /// Initializes a new instance of <see cref="Application" />
        /// </summary>
        /// <param name="renderer">The bulk-renderer.</param>
        /// <param name="requestFactory">The request factory.</param>
        /// <param name="serviceConfigurator">The service configurator.</param>
        /// <param name="args">The command-line args.</param>
        /// <param name="appLifetime">The app lifetime.</param>
        public Application(IRendersManyFiles renderer,
                           IGetsBulkRenderingRequest requestFactory,
                           IConfiguresServices serviceConfigurator,
                           CliArguments args,
                           IHostApplicationLifetime appLifetime)
        {
            this.renderer = renderer ?? throw new System.ArgumentNullException(nameof(renderer));
            this.requestFactory = requestFactory ?? throw new System.ArgumentNullException(nameof(requestFactory));
            this.serviceConfigurator = serviceConfigurator ?? throw new System.ArgumentNullException(nameof(serviceConfigurator));
            this.args = args ?? throw new System.ArgumentNullException(nameof(args));
            this.appLifetime = appLifetime ?? throw new ArgumentNullException(nameof(appLifetime));
        }
    }
}