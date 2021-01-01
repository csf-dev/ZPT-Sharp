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

        CancellationTokenSource cancellationSource;
        Task completed;

        /// <summary>
        /// Starts the application asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task which completes once the app has done its work.</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            completed = StartPrivateAsync(cancellationToken);
            return completed;
        }

        async Task StartPrivateAsync(CancellationToken cancellationToken)
        {
            cancellationSource = new CancellationTokenSource();
            cancellationToken.Register(() => cancellationSource.Cancel());

            serviceConfigurator.ConfigureServices(args);
            var request = await requestFactory.GetRequestAsync(args, cancellationSource.Token);

            try
            {
                await renderer.RenderAllAsync(request, cancellationSource.Token);
            }
            catch(OperationCanceledException) {}
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task which completes immediately.</returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            cancellationSource?.Cancel();
            return new Task(() => WaitForCompletion(cancellationToken));
        }

        void WaitForCompletion(CancellationToken cancellationToken)
        {
            if(completed == null) return;
            completed.Wait(cancellationToken);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Application" />
        /// </summary>
        /// <param name="renderer">The bulk-renderer.</param>
        /// <param name="requestFactory">The request factory.</param>
        /// <param name="serviceConfigurator">The service configurator.</param>
        /// <param name="args">The command-line args.</param>
        public Application(IRendersManyFiles renderer,
                           IGetsBulkRenderingRequest requestFactory,
                           IConfiguresServices serviceConfigurator,
                           CliArguments args)
        {
            this.renderer = renderer ?? throw new System.ArgumentNullException(nameof(renderer));
            this.requestFactory = requestFactory ?? throw new System.ArgumentNullException(nameof(requestFactory));
            this.serviceConfigurator = serviceConfigurator ?? throw new System.ArgumentNullException(nameof(serviceConfigurator));
            this.args = args ?? throw new System.ArgumentNullException(nameof(args));
        }
    }
}