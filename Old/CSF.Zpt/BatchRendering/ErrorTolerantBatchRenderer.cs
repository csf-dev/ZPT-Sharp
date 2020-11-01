using System;
using CSF.Zpt.Rendering;
using System.IO;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Implementation of <see cref="BatchRenderer"/> which catches exceptions raised when rendering documents and
  /// rolls the failures into the output.
  /// </summary>
  public class ErrorTolerantBatchRenderer : BatchRenderer
  {
    /// <summary>
    /// Renders a single ZPT document and returns a response.
    /// </summary>
    /// <param name="doc">The document to render.</param>
    /// <param name="outputStream">The output stream.</param>
    /// <param name="options">Rendering options.</param>
    /// <param name="contextConfigurator">Context configurator.</param>
    /// <param name="outputInfo">Output info.</param>
    protected override IBatchRenderingDocumentResponse Render(IZptDocument doc,
                                                              Stream outputStream,
                                                              IRenderingSettings options,
                                                              Action<IModelValueContainer> contextConfigurator,
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

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.BatchRendering.ErrorTolerantBatchRenderer"/> class.
    /// </summary>
    /// <param name="renderingJobFactory">Rendering job factory.</param>
    public ErrorTolerantBatchRenderer(IRenderingJobFactory renderingJobFactory = null) : base(renderingJobFactory) {}
  }
}

