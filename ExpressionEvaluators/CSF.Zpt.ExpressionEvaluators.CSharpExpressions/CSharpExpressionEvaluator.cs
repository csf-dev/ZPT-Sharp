using System;
using CSF.Zpt.Tales;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  public class CSharpExpressionEvaluator : ExpressionEvaluatorBase
  {
    #region fields

    private readonly ICSharpExpressionService _expressionService;

    #endregion

    #region properties

    public override string ExpressionPrefix
    {
      get {
        return "csharp";
      }
    }

    #endregion

    #region methods

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

    public CSharpExpressionEvaluator() : this(null) {}

    public CSharpExpressionEvaluator(ICSharpExpressionService service)
    {
      _expressionService = service?? new CSharpExpressionService();
    }

    #endregion
  }
}

