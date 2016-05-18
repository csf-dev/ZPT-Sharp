using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Tal
{
  /// <summary>
  /// Exception raised when the parsing of a TAL-enabled document fails.
  /// </summary>
  [Serializable]
  public class ParserException : RenderingException
  {
    #region properties

    /// <summary>
    /// Gets or sets the name of the <see cref="ZptElement"/> which is the source of the current error.
    /// </summary>
    /// <value>The name of the source element.</value>
    public string SourceElementName
    {
      get {
        return this.Data.Contains("SourceElementName") ? (string) this.Data["SourceElementName"] : default(string);
      }
      set {
        this.Data["SourceElementName"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the name of the <see cref="ZptAttribute"/> which is the source of the current error.
    /// </summary>
    /// <value>The name of the source attribute.</value>
    public string SourceAttributeName
    {
      get {
        return this.Data.Contains("SourceAttributeName") ? (string) this.Data["SourceAttributeName"] : default(string);
      }
      set {
        this.Data["SourceAttributeName"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the value of the <see cref="ZptAttribute"/> which is the source of the current error.
    /// </summary>
    /// <value>The value of the source attribute.</value>
    public string SourceAttributeValue
    {
      get {
        return this.Data.Contains("SourceAttributeValue") ? (string) this.Data["SourceAttributeValue"] : default(string);
      }
      set {
        this.Data["SourceAttributeValue"] = value;
      }
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="T:ParserException"/> class
    /// </summary>
    public ParserException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:ParserException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public ParserException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:ParserException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public ParserException(string message, System.Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:ParserException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected ParserException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }

    #endregion
  }
}

