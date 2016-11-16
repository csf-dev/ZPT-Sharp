using System;
using CSF.Zpt.Tales;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.ExpressionEvaluators.PythonExpressions
{
  /// <summary>
  /// Expression evaluator implementation which evaluates Python expressions.
  /// </summary>
  public class PythonExpressionEvaluator : ExpressionEvaluatorBase
  {
    #region constants

    private const string PREFIX = "python";

    #endregion

    #region fields

    private PythonExpressionHost _host;

    #endregion

    #region properties

    /// <summary>
    /// Gets the expression prefix handled by the current evaluator instance.
    /// </summary>
    /// <value>The prefix.</value>
    public override string ExpressionPrefix
    {
      get {
        return PREFIX;
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
      object result;

      try
      {
        result = _host.Evaluate(expression.Content,  model.GetAllDefinitions());
      }
      catch(Exception ex)
      {
        string message = String.Format(Resources.ExceptionMessages.EvaluationFailureFormat, expression.Content);
        throw new ModelEvaluationException(message, ex);
      }

      return new ExpressionResult(result);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CSF.Zpt.ExpressionEvaluators.PythonExpressions.PythonExpressionEvaluator"/> class.
    /// </summary>
    public PythonExpressionEvaluator()
    {
      _host = new PythonExpressionHost();
    }

    #endregion
  }
}

