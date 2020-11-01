using System;

namespace CSF.Zpt.Cli.Exceptions
{
  
  [Serializable]
  public class InvalidInputPathException : OptionsParsingException
  {
    public string Path
    {
      get {
        return this.Data.Contains("Path") ? (string) this.Data["Path"] : default(string);
      }
      set {
        this.Data["Path"] = value;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:InvalidInputPathException"/> class
    /// </summary>
    public InvalidInputPathException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:InvalidInputPathException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public InvalidInputPathException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:InvalidInputPathException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public InvalidInputPathException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:InvalidInputPathException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected InvalidInputPathException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
  }
}

