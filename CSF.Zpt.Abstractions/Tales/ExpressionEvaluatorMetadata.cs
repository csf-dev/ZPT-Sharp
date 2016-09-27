using System;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Represents metadata about an implementation of <see cref="IExpressionEvaluator"/>.
  /// </summary>
  public class ExpressionEvaluatorMetadata
  {
    #region properties

    /// <summary>
    /// Gets the <c>System.Type</c> of the evaluator implementation.
    /// </summary>
    /// <value>The evaluator type.</value>
    public Type EvaluatorType
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets a value indicating whether this instance is the default expression evaluator.
    /// </summary>
    /// <value><c>true</c> if this instance is the default expression evaluator; otherwise, <c>false</c>.</value>
    public bool IsDefaultEvaluator
    {
      get;
      private set;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.ExpressionEvaluatorMetadata"/> class.
    /// </summary>
    /// <param name="evaluatorType">The evaluator type.</param>
    /// <param name="isDefault">If set to <c>true</c> then this is the default evaluator.</param>
    public ExpressionEvaluatorMetadata(Type evaluatorType, bool isDefault = false)
    {
      if(evaluatorType == null)
      {
        throw new ArgumentNullException(nameof(evaluatorType));
      }

      this.EvaluatorType = evaluatorType;
      this.IsDefaultEvaluator = isDefault;
    }

    #endregion
  }
}

