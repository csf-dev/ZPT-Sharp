using System;
using CSF.Zpt.Tales;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Evaluator implementation for <c>csassembly</c> expressions.
  /// </summary>
  public class CSharpAssemblyExpressionEvaluator : ExpressionEvaluatorBase
  {
    #region constants

    private const string Prefix = "csassembly";

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
    public override ExpressionResult Evaluate(Expression expression,
                                              IRenderingContext context,
                                              ITalesModel model)
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

      return new ExpressionResult(new Spec.ReferencedAssemblySpecification(expression.Content));
    }

    #endregion
  }
}

