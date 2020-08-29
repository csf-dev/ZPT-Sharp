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
        readonly IReadsAndWritesDocument documentReaderWriter;
        readonly IPerformsRenderingProcess renderer;

        /// <summary>
        /// Gets the outcome of the specified rendering request as a stream.
        /// </summary>
        /// <returns>A task which provides an output stream, containing the result of the operation.</returns>
        /// <param name="request">A request to render a ZPT document.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        public async Task<Stream> RenderAsync(RenderZptDocumentRequest request, CancellationToken token)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var document = await documentReaderWriter.GetDocumentAsync(request.DocumentStream, request.Config, token)
                .ConfigureAwait(false);
            await renderer.RenderDocumentAsync(document, request, token)
                .ConfigureAwait(false);
            return await documentReaderWriter.WriteDocumentAsync(document, request.Config, token)
                .ConfigureAwait(false);
        }

        public ZptRequestRenderer(IReadsAndWritesDocument documentReaderWriter, IPerformsRenderingProcess renderer)
        {
            this.documentReaderWriter = documentReaderWriter ?? throw new ArgumentNullException(nameof(documentReaderWriter));
            this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }
    }
}
