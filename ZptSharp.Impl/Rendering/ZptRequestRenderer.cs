using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
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
        /// <param name="token">An object used to cancel the operation if required.</param>
        public Task<Stream> RenderAsync(RenderZptDocumentRequest request, CancellationToken token = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return RenderPrivateAsync(request, token);
        }

        async Task<Stream> RenderPrivateAsync(RenderZptDocumentRequest request, CancellationToken token)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                StoreConfigForLaterUse(scope, request);

                var documentReaderWriter = GetDocumentReaderWriter(scope, request);
                var renderer = GetRenderer(scope, request);

                var document = await ReadDocument(documentReaderWriter, request, token).ConfigureAwait(false);
                await renderer.ModifyDocumentAsync(document, request, token).ConfigureAwait(false);
                return await documentReaderWriter.WriteDocumentAsync(document, request.Config, token).ConfigureAwait(false);
            }
        }

        void StoreConfigForLaterUse(IServiceScope scope, RenderZptDocumentRequest request)
        {
            var configServiceLocator = scope.ServiceProvider.GetRequiredService<IStoresCurrentRenderingConfig>();
            configServiceLocator.Configuration = request.Config;
        }

        IReadsAndWritesDocument GetDocumentReaderWriter(IServiceScope scope, RenderZptDocumentRequest request)
            => request.ReaderWriter ?? scope.ServiceProvider.GetRequiredService<IReadsAndWritesDocument>();

        IModifiesDocument GetRenderer(IServiceScope scope, RenderZptDocumentRequest request)
        {
            var rendererFactory = scope.ServiceProvider.GetRequiredService<IGetsDocumentModifier>();
            return rendererFactory.GetDocumentModifier(request);
        }

        Task<IDocument> ReadDocument(IReadsAndWritesDocument documentReaderWriter, RenderZptDocumentRequest request, CancellationToken token)
            => documentReaderWriter.GetDocumentAsync(request.DocumentStream, request.Config, request.SourceInfo, token);

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
