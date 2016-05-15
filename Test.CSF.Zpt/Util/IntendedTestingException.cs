using System;

namespace Test.CSF.Zpt.Util
{
  /// <summary>
  /// Represents an exception raised intentionally due to testing.
  /// </summary>
  [Serializable]
  public class IntendedTestingException : Exception
  {
    private const string DEFAULT_MESSAGE = "This exception is intentionally thrown by testing.";

    /// <summary>
    /// Initializes a new instance of the <see cref="T:IntendedTestingException"/> class
    /// </summary>
    public IntendedTestingException() : this(DEFAULT_MESSAGE)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:IntendedTestingException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public IntendedTestingException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:IntendedTestingException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public IntendedTestingException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:IntendedTestingException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected IntendedTestingException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
  }
}

