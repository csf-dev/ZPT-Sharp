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

    private Lazy<IEvaluatorSelector> _evaluatorSelector;
    private IExpressionFactory _expressionCreator;

    #endregion

    #region properties

    /// <summary>
    /// Gets the expression prefix handled by the current evaluator instance.
    /// </summary>
    /// <value>The prefix.</value>
    public abstract string ExpressionPrefix { get; }

    /// <summary>
    /// Gets the expression evaluator selector, so that sub-expressions may be evaluated within the current expression.
    /// </summary>
    /// <value>The evaluator selector.</value>
    protected virtual IEvaluatorSelector EvaluatorSelector
    {
      get {
        return _evaluatorSelector.Value;
      }
    }

    /// <summary>
    /// Gets an expression creator.
    /// </summary>
    /// <value>The expression creator.</value>
    protected virtual IExpressionFactory ExpressionCreator
    {
      get {
        return _expressionCreator;
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
    public abstract ExpressionResult Evaluate(Expression expression, IRenderingContext context, ITalesModel model);

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.ExpressionEvaluatorBase"/> class.
    /// </summary>
    /// <param name="evaluatorSelector">Evaluator selector.</param>
    /// <param name="expressionCreator">The expression factory to use.</param>
    public ExpressionEvaluatorBase(IEvaluatorSelector evaluatorSelector = null,
                                   IExpressionFactory expressionCreator = null)
    {
      _expressionCreator = expressionCreator?? new ExpressionFactory();

      if(evaluatorSelector != null)
      {
        _evaluatorSelector = new Lazy<IEvaluatorSelector>(() => evaluatorSelector);
      }
      else
      {
        _evaluatorSelector = new Lazy<IEvaluatorSelector>(() => new SimpleEvaluatorSelector());
      }
    }

    #endregion
  }
}

