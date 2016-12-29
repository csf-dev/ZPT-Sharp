using System;
using CSF.Caches;
using CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Default implementation of <see cref="ICSharpExpressionCache"/>.
  /// </summary>
  public class CSharpExpressionCache : ThreadSafeCache<ExpressionSpecification,CSharpExpression>, ICSharpExpressionCache
  {
    /// <summary>
    /// Either gets an expression from the cache, or creates &amp; adds it if it is not found.
    /// </summary>
    /// <returns>The expression.</returns>
    /// <param name="spec">Expression specification.</param>
    /// <param name="expressionCreator">Expression creator.</param>
    public CSharpExpression GetOrAddExpression(ExpressionSpecification spec,
                                               Func<ExpressionSpecification,CSharpExpression> expressionCreator)
    {
      if(spec == null)
      {
        throw new ArgumentNullException(nameof(spec));
      }

      return base.GetOrAdd(spec, () => expressionCreator(spec));
    }
  }
}

