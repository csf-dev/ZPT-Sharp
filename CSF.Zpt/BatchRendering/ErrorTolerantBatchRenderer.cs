using System;
using CSF.Zpt.Rendering;
using System.IO;

namespace CSF.Zpt.BatchRendering
{
  public class ErrorTolerantBatchRenderer : BatchRenderer
  {
    protected override IBatchRenderingDocumentResponse Render(IZptDocument doc,
                                                              Stream outputStream,
                                                              IRenderingOptions options,
                                                              Action<RenderingContext> contextConfigurator,
                                                              string outputInfo)
    {
      try
      {
        return base.Render(doc, outputStream, options, contextConfigurator, outputInfo);
      }
      catch(ZptException ex)
      {
        return new BatchRenderingDocumentResponse(doc.GetSourceInfo(), ex);
      }
    }

    public ErrorTolerantBatchRenderer(IRenderingJobFactory renderingJobFactory = null) : base(renderingJobFactory) {}
  }
}

