using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Base class for a TALES expression evaluator implementation.
  /// </summary>
  public abstract class ExpressionEvaluator
  {
    #region fields

    private EvaluatorRegistry _evaluatorRegistry;

    #endregion

    #region properties

    /// <summary>
    /// Gets the expression prefix handled by the current evaluator instance.
    /// </summary>
    /// <value>The prefix.</value>
    public abstract string ExpressionPrefix { get; }

    /// <summary>
    /// Gets the expression evaluator registry.
    /// </summary>
    /// <value>The evaluator registry.</value>
    protected EvaluatorRegistry EvaluatorRegistry
    {
      get {
        return _evaluatorRegistry;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Evaluate the specified expression, for the given element and model.
    /// </summary>
    /// <param name="expression">The expression to evaluate.</param>
    /// <param name="element">The <see cref="ZptElement"/> for which the expression is being evaluated.</param>
    /// <param name="model">The ZPT model, providing the context for evaluation.</param>
    public abstract ExpressionResult Evaluate(Expression expression, ZptElement element, TalesModel model);

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.ExpressionEvaluator"/> class.
    /// </summary>
    /// <param name="evaluatorRegistry">Evaluator registry.</param>
    public ExpressionEvaluator(EvaluatorRegistry evaluatorRegistry)
    {
      if(evaluatorRegistry == null)
      {
        throw new ArgumentNullException("evaluatorRegistry");
      }

      _evaluatorRegistry = evaluatorRegistry;
    }

    #endregion
  }
}

