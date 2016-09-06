using System;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Represents the response from a batch-rendering operation.
  /// </summary>
  public class BatchRenderingResponse : IBatchRenderingResponse
  {
    #region properties

    /// <summary>
    /// Gets the nature of a fatal error, if applicable.
    /// </summary>
    /// <value>The fatal error.</value>
    public BatchRenderingFatalErrorType? FatalError { get; private set; }

    /// <summary>
    /// Gets a collection of response items relating to the individual documents rendered.
    /// </summary>
    /// <value>The documents.</value>
    public IEnumerable<BatchRenderingDocumentResponse> Documents { get; private set; }

    #endregion

    #region IBatchRenderingResponse implementation

    IEnumerable<IBatchRenderingDocumentResponse> IBatchRenderingResponse.Documents
    {
      get {
        return this.Documents.Cast<IBatchRenderingDocumentResponse>().ToArray();
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.BatchRendering.BatchRenderingResponse"/> class.
    /// </summary>
    /// <param name="fatalError">Fatal error.</param>
    /// <param name="documents">Documents.</param>
    private BatchRenderingResponse(BatchRenderingFatalErrorType? fatalError,
                                   IEnumerable<BatchRenderingDocumentResponse> documents)
    {
      if(fatalError.HasValue && !fatalError.Value.IsDefinedValue())
      {
        throw new ArgumentException("TODO", nameof(fatalError));
      }

      this.Documents = documents?? new BatchRenderingDocumentResponse[0];
      this.FatalError = fatalError;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.BatchRendering.BatchRenderingResponse"/> class.
    /// </summary>
    /// <param name="fatalError">The nature of a fatal error.</param>
    public BatchRenderingResponse(BatchRenderingFatalErrorType fatalError) : this(fatalError, null) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.BatchRendering.BatchRenderingResponse"/> class.
    /// </summary>
    /// <param name="documents">A collection of the documents rendered.</param>
    public BatchRenderingResponse(IEnumerable<BatchRenderingDocumentResponse> documents) : this(null, documents) {}

    #endregion
  }
}