using System;
using System.IO;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Represents the response for a single document within an <see cref="IBatchRenderingResponse"/>.
  /// </summary>
  public interface IBatchRenderingDocumentResponse
  {
    /// <summary>
    /// Gets a value indicating whether this <see cref="CSF.Zpt.BatchRendering.IBatchRenderingDocumentResponse"/>
    /// indicates a success.
    /// </summary>
    /// <value><c>true</c> if a success; otherwise, <c>false</c>.</value>
    bool Success { get; }

    /// <summary>
    /// Gets the nature of an error relating to the current document, if applicable.
    /// </summary>
    /// <value>The type of the error.</value>
    RenderingException Exception { get; }

    /// <summary>
    /// Gets the input location used for the current document.
    /// </summary>
    /// <value>The input location.</value>
    ISourceInfo SourceInfo { get; }

    /// <summary>
    /// Gets the output location for the current document.
    /// </summary>
    /// <value>The output location.</value>
    string OutputInfo { get; }
  }
}

