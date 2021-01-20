using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.BulkRendering
{
    /// <summary>
    /// An object which renders a single file as part of a bulk-rendering operation.
    /// </summary>
    public interface IRendersInputFile
    {
        /// <summary>
        /// Renders the template described by <paramref name="inputFile" /> asynchronously
        /// and returns a result object.
        /// </summary>
        /// <param name="request">The original request to bulk-render files.</param>
        /// <param name="inputFile">The file to be rendered.</param>
        /// <param name="token">An optional cancellation token</param>
        /// <returns>A result object indicating the outcome.</returns>
        Task<BulkRenderingFileResult> RenderAsync(BulkRenderingRequest request,
                                                  InputFile inputFile,
                                                  CancellationToken token = default);
    }
}