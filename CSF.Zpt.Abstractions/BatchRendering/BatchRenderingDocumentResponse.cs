using System.IO;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.BatchRendering
{
    public class BatchRenderingDocumentResponse
    {
        public bool Success { get; private set; }

        public RenderingException Exception { get; private set; }

        public ISourceInfo SourceInfo { get; private set; }

        public string OutputInfo { get; private set; }
    }
}