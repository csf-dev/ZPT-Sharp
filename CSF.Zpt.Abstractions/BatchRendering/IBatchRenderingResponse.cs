using System;
using System.Collections.Generic;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Represents the response from a batch-rendering task.
  /// </summary>
  public interface IBatchRenderingResponse
  {
    /// <summary>
    /// Gets the nature of a fatal error, if applicable.
    /// </summary>
    /// <value>The fatal error.</value>
    BatchRenderingFatalErrorType? FatalError { get; }

    /// <summary>
    /// Gets a collection of response items relating to the individual documents rendered.
    /// </summary>
    /// <value>The documents.</value>
    IEnumerable<IBatchRenderingDocumentResponse> Documents { get; }
  }
}

