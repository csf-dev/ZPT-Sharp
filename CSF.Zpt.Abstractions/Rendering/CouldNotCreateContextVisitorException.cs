using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Exception raised when the construction of an <see cref="IContextVisitor"/> fails.
  /// </summary>
  [Serializable]
  public class CouldNotCreateContextVisitorException : Exception
  {
    /// <summary>
    /// Gets or sets the name of the type which was used unsuccessfully to try and crveate the context visitor.
    /// </summary>
    /// <value>The invalid classname.</value>
    public string InvalidClassname
    {
      get {
        return this.Data.Contains("InvalidClassname") ? (string) this.Data["InvalidClassname"] : default(string);
      }
      set {
        this.Data["InvalidClassname"] = value;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CouldNotCreateContextVisitorException"/> class
    /// </summary>
    public CouldNotCreateContextVisitorException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CouldNotCreateContextVisitorException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public CouldNotCreateContextVisitorException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CouldNotCreateContextVisitorException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public CouldNotCreateContextVisitorException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CouldNotCreateContextVisitorException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected CouldNotCreateContextVisitorException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
  }
}

