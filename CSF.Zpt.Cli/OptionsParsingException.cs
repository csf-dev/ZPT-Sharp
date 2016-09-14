using System;

namespace CSF.Zpt.Cli
{
  /// <summary>
  /// An exception type arising to a failure to parse the command-line options.
  /// </summary>
  [Serializable]
  public class OptionsParsingException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:OptionsParsingException"/> class
    /// </summary>
    public OptionsParsingException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:OptionsParsingException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public OptionsParsingException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:OptionsParsingException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public OptionsParsingException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:OptionsParsingException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected OptionsParsingException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
  }
}

