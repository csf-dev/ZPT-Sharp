using System.IO;
using CSF.Zpt.Rendering;
using System;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Batch rendering document response.
  /// </summary>
  public class BatchRenderingDocumentResponse : IBatchRenderingDocumentResponse
  {
    /// <summary>
    /// Gets a value indicating whether this <see cref="CSF.Zpt.BatchRendering.IBatchRenderingDocumentResponse"/>
    /// indicates a success.
    /// </summary>
    /// <value><c>true</c> if a success; otherwise, <c>false</c>.</value>
    public bool Success { get { return Exception == null; } }

    /// <summary>
    /// Gets the nature of an error relating to the current document, if applicable.
    /// </summary>
    /// <value>The type of the error.</value>
    public ZptException Exception { get; private set; }

    /// <summary>
    /// Gets the input location used for the current document.
    /// </summary>
    /// <value>The input location.</value>
    public ISourceInfo SourceInfo { get; private set; }

    /// <summary>
    /// Gets the output location for the current document.
    /// </summary>
    /// <value>The output location.</value>
    public string OutputInfo { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.BatchRendering.BatchRenderingDocumentResponse"/> class.
    /// </summary>
    /// <param name="sourceInfo">Source info.</param>
    /// <param name="exception">Exception.</param>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.BatchRendering.BatchRenderingDocumentResponse"/> class.
    /// </summary>
    /// <param name="sourceInfo">Source info.</param>
    /// <param name="outputInfo">Output info.</param>
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