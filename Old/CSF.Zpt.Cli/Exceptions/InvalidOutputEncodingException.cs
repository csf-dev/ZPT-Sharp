using System;

namespace CSF.Zpt.Cli.Exceptions
{
  
  [Serializable]
  public class InvalidOutputEncodingException : OptionsParsingException
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:InvalidOutputEncodingException"/> class
    /// </summary>
    public InvalidOutputEncodingException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:InvalidOutputEncodingException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public InvalidOutputEncodingException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:InvalidOutputEncodingException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public InvalidOutputEncodingException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:InvalidOutputEncodingException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected InvalidOutputEncodingException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
  }
}

