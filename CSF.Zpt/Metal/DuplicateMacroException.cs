using System;
namespace CSF.Zpt.Metal
{
  /// <summary>
  /// Exception raised when there are duplicate METAL macros found within a document.
  /// </summary>
  [System.Serializable]
  public class DuplicateMacroException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:DuplicateMacroException"/> class
    /// </summary>
    public DuplicateMacroException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:DuplicateMacroException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public DuplicateMacroException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:DuplicateMacroException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public DuplicateMacroException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:DuplicateMacroException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected DuplicateMacroException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
  }
}
