using System.Collections.Generic;
using System.Linq;

namespace ZptSharp.BulkRendering
{
    /// <summary>
    /// Represents the result from <see cref="IRendersManyFiles.RenderAllAsync(BulkRenderingRequest, System.Threading.CancellationToken)" />.
    /// </summary>
    public class BulkRenderingResult
    {
        /// <summary>
        /// Gets the request which is associated with the current result.
        /// </summary>
        /// <value>The request.</value>
        public BulkRenderingRequest Request { get; }

        /// <summary>
        /// Gets a collection of the results for each file which was rendered.
        /// </summary>
        /// <value>The results for the rendered files.</value>
        public IReadOnlyCollection<BulkRenderingFileResult> FileResults { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="BulkRenderingResult" />.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="fileResults">The results for the rendered files.</param>
        public BulkRenderingResult(BulkRenderingRequest request, IEnumerable<BulkRenderingFileResult> fileResults)
        {
            if (fileResults is null)
                throw new System.ArgumentNullException(nameof(fileResults));

            Request = request ?? throw new System.ArgumentNullException(nameof(request));
            FileResults = fileResults.ToArray();
        }
    }
}