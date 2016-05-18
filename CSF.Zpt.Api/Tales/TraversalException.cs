using System;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Represents an exception whilst traversing a TALES path.
  /// </summary>
  [Serializable]
  public class TraversalException : System.Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:TraversalException"/> class
    /// </summary>
    public TraversalException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:TraversalException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public TraversalException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:TraversalException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public TraversalException(string message, System.Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:TraversalException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected TraversalException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
  }
}

