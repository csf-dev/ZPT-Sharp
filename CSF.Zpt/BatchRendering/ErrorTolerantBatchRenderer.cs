using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.BatchRendering
{
  public class ErrorTolerantBatchRenderer : BatchRenderer
  {
    protected override IBatchRenderingDocumentResponse Render(IRenderingJob job,
                                                              IRenderingOptions options,
                                                              IBatchRenderingOptions batchOptions,
                                                              Action<RenderingContext> contextConfigurator)
    {
      try
      {
        return base.Render(job, options, batchOptions, contextConfigurator);
      }
      catch(RenderingException ex)
      {
        return new BatchRenderingDocumentResponse(job.Document.GetSourceInfo(), ex);
      }
    }

    public ErrorTolerantBatchRenderer(IRenderingJobFactory renderingJobFactory = null) : base(renderingJobFactory) {}
  }
}

