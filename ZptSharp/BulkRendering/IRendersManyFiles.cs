using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.BulkRendering
{
    /// <summary>
    /// An object which may be used to render many template documents in a single operation.
    /// </summary>
    public interface IRendersManyFiles
    {
        /// <summary>
        /// Renders all of the documents described in the <paramref name="request" /> and
        /// gets a result object.
        /// </summary>
        /// <param name="request">The request object, describing the files to be rendered.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <returns>A result/outcome object.</returns>
        Task<BulkRenderingResult> RenderAllAsync(BulkRenderingRequest request, CancellationToken token = default);
    }
}