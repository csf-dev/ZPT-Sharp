using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// A service which coordinates and performs the modifications to a document before
    /// it is ready to be output.
    /// </summary>
    public interface IPerformsRenderingProcess
    {
        /// <summary>
        /// Performs the ZPT rendering process for a specified document, using the specified rendering request.
        /// </summary>
        /// <returns>A task indicating when the process is complete.</returns>
        /// <param name="document">The document to render.</param>
        /// <param name="request">The rendering request.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        Task RenderDocumentAsync(IDocument document, RenderZptDocumentRequest request, CancellationToken token);
    }
}
