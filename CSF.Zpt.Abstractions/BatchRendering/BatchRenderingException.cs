using System;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Exception raised when batch rendering fails for an unexpected reason.
  /// </summary>
  [Serializable]
  public class BatchRenderingException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:BatchRenderingException"/> class
    /// </summary>
    public BatchRenderingException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:BatchRenderingException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public BatchRenderingException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:BatchRenderingException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
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
  }
}

