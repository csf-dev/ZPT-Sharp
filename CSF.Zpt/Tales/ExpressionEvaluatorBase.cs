using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Base class for a TALES expression evaluator implementation, implements <see cref="IExpressionEvaluator"/>.
  /// </summary>
  public abstract class ExpressionEvaluatorBase : IExpressionEvaluator
  {
    #region fields

    private IEvaluatorRegistry _evaluatorRegistry;

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
    protected virtual IEvaluatorRegistry EvaluatorRegistry
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
    /// <param name="context">The rendering context for the expression being evaluated.</param>
    /// <param name="model">The ZPT model, providing the context for evaluation.</param>
    public abstract ExpressionResult Evaluate(Expression expression, RenderingContext context, TalesModel model);

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.ExpressionEvaluatorBase"/> class.
    /// </summary>
    /// <param name="evaluatorRegistry">Evaluator registry.</param>
    public ExpressionEvaluatorBase(IEvaluatorRegistry evaluatorRegistry)
    {
      if(evaluatorRegistry == null)
      {
        throw new ArgumentNullException(nameof(evaluatorRegistry));
      }

      _evaluatorRegistry = evaluatorRegistry;
    }

    #endregion
  }
}

