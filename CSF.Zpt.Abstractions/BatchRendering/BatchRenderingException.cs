using System;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Exception raised when batch rendering fails for an unexpected reason.
  /// </summary>
  [Serializable]
  public class BatchRenderingException : Exception
  {
    #region properties

    /// <summary>
    /// Gets or sets the nature of the fatal error.
    /// </summary>
    /// <value>The fatal error.</value>
    public BatchRenderingFatalErrorType? FatalError
    {
      get {
        return this.Data.Contains("FatalError") ? (BatchRenderingFatalErrorType?) this.Data["FatalError"] : default(BatchRenderingFatalErrorType?);
      }
      protected set {
        this.Data["FatalError"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the nature of the document error.
    /// </summary>
    /// <value>The document error.</value>
    public BatchRenderingDocumentErrorType? DocumentError
    {
      get {
        return this.Data.Contains("DocumentError") ? (BatchRenderingDocumentErrorType?) this.Data["DocumentError"] : default(BatchRenderingDocumentErrorType?);
      }
      protected set {
        this.Data["DocumentError"] = value;
      }
    }

    /// <summary>
    /// Gets or sets information about the source location of the document.
    /// </summary>
    /// <value>The document source.</value>
    public string DocumentSource
    {
      get {
        return this.Data.Contains("DocumentSource") ? (string) this.Data["DocumentSource"] : default(string);
      }
      protected set {
        this.Data["DocumentSource"] = value;
      }
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="T:BatchRenderingException"/> class
    /// </summary>
    public BatchRenderingException() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.BatchRendering.BatchRenderingException"/> class.
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception.</param>
    /// <param name="fatalError">A value indicating the nature of a fatal error.</param>
    public BatchRenderingException(string message,
                                   BatchRenderingFatalErrorType fatalError) : this(message, fatalError, null) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.BatchRendering.BatchRenderingException"/> class.
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception.</param>
    /// <param name="fatalError">A value indicating the nature of a fatal error.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public BatchRenderingException(string message,
                                   BatchRenderingFatalErrorType fatalError,
                                   Exception inner) : base(message, inner)
    {
      this.FatalError = fatalError;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.BatchRendering.BatchRenderingException"/> class.
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception.</param>
    /// <param name="documentSource">Information about the source location of the document which caused this error.</param>
    /// <param name="error">A value indicating the nature of the document rendering error.</param>
    public BatchRenderingException(string message,
                                   string documentSource,
                                   BatchRenderingDocumentErrorType error) : this(message, documentSource, error, null) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.BatchRendering.BatchRenderingException"/> class.
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception.</param>
    /// <param name="documentSource">Information about the source location of the document which caused this error.</param>
    /// <param name="error">A value indicating the nature of the document rendering error.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public BatchRenderingException(string message,
                                   string documentSource,
                                   BatchRenderingDocumentErrorType error,
                                   Exception inner) : base(message, inner)
    {
      this.DocumentSource = documentSource;
      this.DocumentError = error;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:BatchRenderingException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception.</param>
    public BatchRenderingException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:BatchRenderingException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public BatchRenderingException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:BatchRenderingException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected BatchRenderingException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }

    #endregion
  }
}

