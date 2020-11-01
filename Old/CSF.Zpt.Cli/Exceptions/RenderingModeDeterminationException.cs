using System;

namespace CSF.Zpt.Cli.Exceptions
{
  
  [Serializable]
  public class RenderingModeDeterminationException : OptionsParsingException
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RenderingModeDeterminationException"/> class
    /// </summary>
    public RenderingModeDeterminationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:RenderingModeDeterminationException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public RenderingModeDeterminationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:RenderingModeDeterminationException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public RenderingModeDeterminationException(string message, System.Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:RenderingModeDeterminationException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected RenderingModeDeterminationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
  }
}

