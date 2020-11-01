using System;
using System.Text.RegularExpressions;
using CSF.Zpt.Rendering;
using CSF.Zpt.Tales;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Evaluator implementation for <c>csnamespace</c> expressions.
  /// </summary>
  public class CSharpNamespaceExpressionEvaluator : ExpressionEvaluatorBase
  {
    #region constants

    private const string
    Prefix            = "csnamespace",
    VALUE_PATTERN     = "^(?:([^ ]+) )?([^ ]+)$";

    private static readonly Regex ValueMatcher = new Regex(VALUE_PATTERN, RegexOptions.Compiled);

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

      var valueMatch = ValueMatcher.Match(expression.Content);

      if(!valueMatch.Success)
      {
        var message = String.Format(Resources.ExceptionMessages.TypeExpressionMustBeInCorrectFormat,
                                    Prefix,
                                    expression.Content);
        throw new CSharpExpressionException(message);
      }

      return new ExpressionResult(new Spec.UsingNamespaceSpecification(valueMatch.Groups[2].Value,
                                                                       valueMatch.Groups[1].Value));
    }

    #endregion
  }
}

