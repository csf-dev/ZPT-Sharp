using System;

namespace CSF.Zpt
{
  /// <summary>
  /// Base exception type for all non-assertion type exceptions which could be raised by Zpt-Sharp.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Zpt-Sharp can raise other exception types, but these would be for fundamental misuse of the API.  They would be
  /// things like <c>ArgumentException</c> etc.
  /// </para>
  /// </remarks>
  [Serializable]
  public class ZptException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:ZptException"/> class
    /// </summary>
    public ZptException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:ZptException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public ZptException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:ZptException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public ZptException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:ZptException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected ZptException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
  }
}

