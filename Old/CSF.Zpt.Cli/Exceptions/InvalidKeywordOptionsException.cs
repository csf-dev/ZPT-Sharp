using System;

namespace CSF.Zpt.Cli.Exceptions
{
  
  [Serializable]
  public class InvalidKeywordOptionsException : OptionsParsingException
  {
    public string InvalidOption
    {
      get {
        return this.Data.Contains("InvalidOption") ? (string) this.Data["InvalidOption"] : default(string);
      }
      set {
        this.Data["InvalidOption"] = value;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:InvalidKeywordOptionsException"/> class
    /// </summary>
    public InvalidKeywordOptionsException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:InvalidKeywordOptionsException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public InvalidKeywordOptionsException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:InvalidKeywordOptionsException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public InvalidKeywordOptionsException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:InvalidKeywordOptionsException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected InvalidKeywordOptionsException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
  }
}

