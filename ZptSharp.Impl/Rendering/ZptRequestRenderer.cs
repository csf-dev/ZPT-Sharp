using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// A service which coordinates the process of rendering a ZPT document from a request object.
    /// Generally-speaking it is better to use an <see cref="IRendersZptDocuments"/> to begin the process.
    /// <see cref="IRendersZptDocuments"/> offers a more useful API and its default implementation -
    /// <see cref="ZptDocumentRenderer"/> - provides dependency injection as well.
    /// </summary>
    public class ZptRequestRenderer : IRendersRenderingRequest
    {
        readonly IServiceProvider serviceProvider;
        readonly IGetsDocumentModifier rendererFactory;

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
            var documentReaderWriter = request.ReaderWriter ?? serviceProvider.Resolve<IReadsAndWritesDocument>();
            var renderer = rendererFactory.GetDocumentModifier(request);

            var document = await documentReaderWriter.GetDocumentAsync(request.DocumentStream, request.Config, request.SourceInfo, token)
                .ConfigureAwait(false);
            await renderer.ModifyDocumentAsync(document, request, token)
                .ConfigureAwait(false);
            return await documentReaderWriter.WriteDocumentAsync(document, request.Config, token)
                .ConfigureAwait(false);
        }

        public ZptRequestRenderer(IServiceProvider serviceProvider, IGetsDocumentModifier rendererFactory)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.rendererFactory = rendererFactory ?? throw new ArgumentNullException(nameof(rendererFactory));
        }
    }
}
