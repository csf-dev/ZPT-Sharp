using System;
using CSF.Zpt.Tales;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Evaluator type for CSharp expressions.
  /// </summary>
  public class CSharpExpressionEvaluator : ExpressionEvaluatorBase
  {
    #region constants

    private static readonly string Prefix = "csharp";

    #endregion

    #region fields

    private readonly ICSharpExpressionService _expressionService;

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
    /// <param name="context">The rendering context for the expression being evaluated.</param>
    /// <param name="model">The ZPT model, providing the context for evaluation.</param>
    public override ExpressionResult Evaluate(Expression expression, IRenderingContext context, ITalesModel model)
    {
      if(expression == null)
      {
        throw new ArgumentNullException(nameof(expression));
      }
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }
      if(model == null)
      {
        throw new ArgumentNullException(nameof(model));
      }

      var expressionText = expression.Content;
      var allDefinitions = model.GetAllDefinitions();

      var csharpExpression = _expressionService.GetExpression(expressionText, allDefinitions.Keys);

      return new ExpressionResult(csharpExpression.Evaluate(allDefinitions));
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.CSharpExpressionEvaluator"/> class.
    /// </summary>
    public CSharpExpressionEvaluator() : this(null) {}

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.CSharpExpressionEvaluator"/> class.
    /// </summary>
    /// <param name="service">Service.</param>
    public CSharpExpressionEvaluator(ICSharpExpressionService service)
    {
      _expressionService = service?? new CSharpExpressionService();
    }

    #endregion
  }
}

