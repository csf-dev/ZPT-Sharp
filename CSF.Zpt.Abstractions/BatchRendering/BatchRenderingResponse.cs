using System.Collections.Generic;

namespace CSF.Zpt.BatchRendering
{
    /// <summary>
    /// Represents the response from a batch-rendering operation.
    /// </summary>
    public class BatchRenderingResponse
    {
        public FatalBatchRenderingErrorType FatalError { get; private set; }

        public IEnumerable<BatchRenderingDocumentResponse> Documents { get; private set; }
    }
}