using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZptSharp.Config;
using ZptSharp.Dom;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// A service which coordinates the process of rendering a ZPT document from a request object.
    /// Generally-speaking it is better to use an <see cref="IRendersZptDocument"/> to begin the process.
    /// <see cref="IRendersZptDocument"/> offers a more useful API and its default implementation -
    /// <see cref="ZptDocumentRenderer"/> - provides dependency injection as well.
    /// </summary>
    public class ZptRequestRenderer : IRendersRenderingRequest
    {
        readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Gets the outcome of the specified rendering request as a stream.
        /// </summary>
        /// <returns>A task which provides an output stream, containing the result of the operation.</returns>
        /// <param name="request">A request to render a ZPT document.</param>
        /// <param name="config">The rendering configuration.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        public Task<Stream> RenderAsync(RenderZptDocumentRequest request, RenderingConfig config = null, CancellationToken token = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return RenderPrivateAsync(request, config ?? RenderingConfig.Default, token);
        }

        async Task<Stream> RenderPrivateAsync(RenderZptDocumentRequest request, RenderingConfig config, CancellationToken token)
        {
            var logScopeState = new ZptRequestLoggingScope(request.SourceInfo);

            using (var scope = serviceProvider.CreateScope())
            using(var logScope = serviceProvider.GetRequiredService<ILogger<ZptRequestRenderer>>().BeginScope(logScopeState))
            {
                StoreConfigForLaterUse(scope, config);
                var documentReaderWriter = GetDocumentReaderWriter(scope, config);
                StoreReaderWriterForLaterUse(scope, documentReaderWriter);
                var renderer = GetRenderer(scope, request);

                var document = await ReadDocumentAsync(documentReaderWriter, request, config, token).ConfigureAwait(false);
                await renderer.ModifyDocumentAsync(document, request, token).ConfigureAwait(false);
                return await documentReaderWriter.WriteDocumentAsync(document, config, token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Where a rendering operation occurs within a single service provider
        /// scope, other services inside that scope will need to be able to dependency-inject
        /// a configuration object.  However, at this stage it's (way) too late to modify the
        /// service provider itself.  To get around this, ZPT-Sharp registers a per-scope service
        /// locator for the config, resolves one of them here and adds the config to that.
        /// Another registration will make the config injectable by using the service locator
        /// as a factory.
        /// </summary>
        /// <param name="scope">Scope.</param>
        /// <param name="config">Rendering config.</param>
        static void StoreConfigForLaterUse(IServiceScope scope, RenderingConfig config)
        {
            var configServiceLocator = scope.ServiceProvider.GetRequiredService<IStoresCurrentRenderingConfig>();
            configServiceLocator.Configuration = config;
        }

        /// <summary>
        /// Where a rendering operation occurs within a single service provider
        /// scope, other services inside that scope will need to be able to dependency-inject
        /// that same reader/writer (mixing documents from different reader/writer implementations is unsupported).
        /// However, at this stage it's (way) too late to modify the
        /// service provider itself.  To get around this, ZPT-Sharp registers a per-scope service
        /// locator for the reader/writer, resolves one of them here and adds the reader/writer to that.
        /// Another registration will make the reader/writer injectable by using the service locator
        /// as a factory.
        /// </summary>
        /// <param name="scope">Scope.</param>
        /// <param name="readerWriter">Reader/writer.</param>
        static void StoreReaderWriterForLaterUse(IServiceScope scope, IReadsAndWritesDocument readerWriter)
        {
            var readerWriterServiceLocator = scope.ServiceProvider.GetRequiredService<IStoresCurrentReaderWriter>();
            readerWriterServiceLocator.ReaderWriter = readerWriter;
        }

        static IReadsAndWritesDocument GetDocumentReaderWriter(IServiceScope scope, RenderingConfig config)
        {
            var type = config.DocumentProviderType ?? typeof(IReadsAndWritesDocument);
            return (IReadsAndWritesDocument) scope.ServiceProvider.GetRequiredService(type);
        }

        static IModifiesDocument GetRenderer(IServiceScope scope, RenderZptDocumentRequest request)
        {
            var rendererFactory = scope.ServiceProvider.GetRequiredService<IGetsDocumentModifier>();
            return rendererFactory.GetDocumentModifier(request);
        }

        static Task<IDocument> ReadDocumentAsync(IReadsAndWritesDocument documentReaderWriter, RenderZptDocumentRequest request, RenderingConfig config, CancellationToken token)
            => documentReaderWriter.GetDocumentAsync(request.DocumentStream, config, request.SourceInfo, token);

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptRequestRenderer"/> class.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        public ZptRequestRenderer(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}
