using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// An object which gets the outcoe of a <see cref="RenderZptDocumentRequest"/> and outputs a stream,
    /// containing the resulting document.
    /// </summary>
    public interface IRendersRenderingRequest
    {
        /// <summary>
        /// Gets the outcome of the specified rendering request as a stream.
        /// </summary>
        /// <returns>A task which provides an output stream, containing the result of the operation.</returns>
        /// <param name="request">A request to render a ZPT document.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        Task<Stream> RenderAsync(RenderZptDocumentRequest request, CancellationToken token);
    }
}
