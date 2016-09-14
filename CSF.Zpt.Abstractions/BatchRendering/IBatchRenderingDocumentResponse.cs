using System;
using System.IO;

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
    BatchRenderingDocumentErrorType? ErrorType { get; }

    /// <summary>
    /// Gets an object which exposes detail about the current error, if applicable.
    /// </summary>
    /// <value>The error detail.</value>
    object ErrorDetail { get; }

    /// <summary>
    /// Gets the input location used for the current document.
    /// </summary>
    /// <value>The input location.</value>
    string InputLocation { get; }

    /// <summary>
    /// Gets the output location for the current document.
    /// </summary>
    /// <value>The output location.</value>
    string OutputLocation { get; }
  }
}

