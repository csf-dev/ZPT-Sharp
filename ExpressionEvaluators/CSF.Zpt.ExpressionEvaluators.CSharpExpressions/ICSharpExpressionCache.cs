using System;
using CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Cache type provides a mechanism to store compiled <see cref="CSharpExpression"/> instances for later re-use.
  /// </summary>
  public interface ICSharpExpressionCache
  {
    /// <summary>
    /// Either gets an expression from the cache, or creates &amp; adds it if it is not found.
    /// </summary>
    /// <returns>The expression.</returns>
    /// <param name="spec">Expression specification.</param>
    /// <param name="expressionCreator">Expression creator.</param>
    CSharpExpression GetOrAddExpression(ExpressionSpecification spec,
                                        Func<ExpressionSpecification,CSharpExpression> expressionCreator);
  }
}

