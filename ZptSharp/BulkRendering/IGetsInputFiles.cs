using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.BulkRendering
{
    /// <summary>
    /// An object which can get a collection of the files to be processed by a bulk-rendering operation.
    /// </summary>
    public interface IGetsInputFiles
    {
        /// <summary>
        /// Gets a collection of the input files to be processed in the bulk-rendering operation.
        /// </summary>
        /// <param name="request">The bulk-rendering request.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <returns>A collection of input files.</returns>
        Task<IEnumerable<InputFile>> GetInputFilesAsync(BulkRenderingRequest request, CancellationToken token = default);
    }
}