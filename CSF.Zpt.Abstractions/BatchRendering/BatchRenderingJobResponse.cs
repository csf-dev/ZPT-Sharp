using System.IO;

namespace CSF.Zpt.BatchRendering
{
    public class BatchRenderingJobResponse
    {
        public bool Success { get; private set; }

        public NonFatalBatchRenderingErrorType ErrorType { get; private set; }

        public object ErrorDetail { get; private set; }

        public FileInfo InputPath { get; private set; }

        public FileInfo OutputPath { get; private set; }
    }
}