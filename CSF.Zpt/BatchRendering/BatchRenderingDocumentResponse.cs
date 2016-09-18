using System.IO;
using CSF.Zpt.Rendering;
using System;

namespace CSF.Zpt.BatchRendering
{
  public class BatchRenderingDocumentResponse : IBatchRenderingDocumentResponse
  {
    public bool Success { get { return Exception == null; } }

    public ZptException Exception { get; private set; }

    public ISourceInfo SourceInfo { get; private set; }

    public string OutputInfo { get; private set; }

    public BatchRenderingDocumentResponse(ISourceInfo sourceInfo, ZptException exception)
    {
      if(sourceInfo == null)
      {
        throw new ArgumentNullException(nameof(sourceInfo));
      }
      if(exception == null)
      {
        throw new ArgumentNullException(nameof(exception));
      }

      this.SourceInfo = sourceInfo;
      this.Exception = exception;
    }

    public BatchRenderingDocumentResponse(ISourceInfo sourceInfo, string outputInfo)
    {
      if(sourceInfo == null)
      {
        throw new ArgumentNullException(nameof(sourceInfo));
      }
      if(outputInfo == null)
      {
        throw new ArgumentNullException(nameof(outputInfo));
      }

      this.SourceInfo = sourceInfo;
      this.OutputInfo = outputInfo;
    }
  }
}