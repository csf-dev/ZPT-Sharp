using System;
using CSF.Zpt.Tales;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Exception raised when there is an issue evaluating a CSharp expression.
  /// </summary>
  [Serializable]
  public class CSharpExpressionException : InvalidExpressionException
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSharpExpressionExceptionException"/> class
    /// </summary>
    public CSharpExpressionException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSharpExpressionExceptionException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public CSharpExpressionException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSharpExpressionExceptionException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public CSharpExpressionException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSharpExpressionExceptionException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected CSharpExpressionException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
  }
}

