using System.IO;
using System;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Represents the response from a single job, within a <see cref="BatchRenderingResponse"/>.
  /// </summary>
  public class BatchRenderingDocumentResponse : IBatchRenderingDocumentResponse
  {
    #region properties

    /// <summary>
    /// Gets a value indicating whether this <see cref="CSF.Zpt.BatchRendering.BatchRenderingDocumentResponse"/>
    /// indicates a success.
    /// </summary>
    /// <value><c>true</c> if a success; otherwise, <c>false</c>.</value>
    public bool Success { get { return !this.ErrorType.HasValue; }}

    /// <summary>
    /// Gets the nature of an error relating to the current job, if applicable.
    /// </summary>
    /// <value>The type of the error.</value>
    public BatchRenderingDocumentErrorType? ErrorType { get; private set; }

    /// <summary>
    /// Gets an object which exposes detail about the current error, if applicable.
    /// </summary>
    /// <value>The error detail.</value>
    public object ErrorDetail { get; private set; }

    /// <summary>
    /// Gets the input location used for the current document.
    /// </summary>
    /// <value>The input location.</value>
    public string InputLocation { get; private set; }

    /// <summary>
    /// Gets the output location for the current document.
    /// </summary>
    /// <value>The output location.</value>
    public string OutputLocation { get; private set; }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.BatchRendering.BatchRenderingDocumentResponse"/> class.
    /// </summary>
    /// <param name="input">The input location.</param>
    /// <param name="output">The output location.</param>
    /// <param name="errorType">The error type (where applicable).</param>
    /// <param name="errorDetail">The rrror detail (where applicable).</param>
    public BatchRenderingDocumentResponse(string input,
                                          string output = null,
                                          BatchRenderingDocumentErrorType? errorType = null,
                                          object errorDetail = null)
    {
      if(input == null)
      {
        throw new ArgumentNullException(nameof(input));
      }
      if(errorType.HasValue && !errorType.Value.IsDefinedValue())
      {
        // TODO: Move this message to a resource file
        throw new ArgumentException("Error type must be a defined enumeration constant", nameof(errorType));
      }

      this.InputLocation = input;
      this.OutputLocation = output;
      this.ErrorType = errorType;
      this.ErrorDetail = errorDetail;
    }

    #endregion
  }
}