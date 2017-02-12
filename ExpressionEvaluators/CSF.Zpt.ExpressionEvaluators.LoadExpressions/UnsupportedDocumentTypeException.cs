using System;

namespace CSF.Zpt.ExpressionEvaluators.LoadExpressions
{

  /// <summary>
  /// Exception raised when an unsupported document type is used with a 'load:' expression.
  /// </summary>
  [Serializable]
  public class UnsupportedDocumentTypeException : ZptException
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:UnsupportedDocumentTypeException"/> class
    /// </summary>
    public UnsupportedDocumentTypeException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:UnsupportedDocumentTypeException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public UnsupportedDocumentTypeException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:UnsupportedDocumentTypeException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public UnsupportedDocumentTypeException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:UnsupportedDocumentTypeException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected UnsupportedDocumentTypeException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
  }
}

