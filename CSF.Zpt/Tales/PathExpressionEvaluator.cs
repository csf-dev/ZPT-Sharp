using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Implementation of <see cref="ExpressionEvaluator"/> which handles TALES path expressions.
  /// </summary>
  public class PathExpressionEvaluator : ExpressionEvaluator
  {
    #region constants

    private static readonly string Prefix = "path";

    #endregion

    #region properties

    /// <summary>
    /// Gets the expression prefix handled by the current evaluator instance.
    /// </summary>
    /// <value>The prefix.</value>
    public override string ExpressionPrefix
    {
      get {
        return Prefix;
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
    public override ExpressionResult Evaluate(Expression expression, ZptElement element, TalesModel model)
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.PathExpressionEvaluator"/> class.
    /// </summary>
    /// <param name="registry">Registry.</param>
    public PathExpressionEvaluator(EvaluatorRegistry registry) : base(registry) {}

    #endregion
  }
}

