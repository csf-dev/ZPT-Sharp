using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Represents an exception which is raised whilst evaluating an expression within a <see cref="IModel"/>.
  /// </summary>
  [Serializable]
  public class ModelEvaluationException : RenderingException
  {
    #region properties

    /// <summary>
    /// Gets or sets the text of the expression which raised the current exception.
    /// </summary>
    /// <value>The expression text.</value>
    public string ExpressionText
    {
      get {
        return this.Data.Contains("ExpressionText") ? (string) this.Data["ExpressionText"] : default(string);
      }
      set {
        this.Data["ExpressionText"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the name of the element for which the expression was being evaluated.
    /// </summary>
    /// <value>The name of the element.</value>
    public string ElementName
    {
      get {
        return this.Data.Contains("ElementName") ? (string) this.Data["ElementName"] : default(string);
      }
      set {
        this.Data["ElementName"] = value;
      }
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="T:ModelEvaluationException"/> class
    /// </summary>
    public ModelEvaluationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:ModelEvaluationException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public ModelEvaluationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:ModelEvaluationException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public ModelEvaluationException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:ModelEvaluationException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected ModelEvaluationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }

    #endregion
  }
}

