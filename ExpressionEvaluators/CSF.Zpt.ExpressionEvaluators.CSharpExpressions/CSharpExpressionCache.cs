using System;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Default implementation of <see cref="ICSharpExpressionCache"/>.
  /// </summary>
  public class CSharpExpressionCache : ThreadSafeCache<Tuple<string,string[]>,CSharpExpression>, ICSharpExpressionCache
  {
    /// <summary>
    /// Either gets an expression from the cache, or creates &amp; adds it if it is not found.
    /// </summary>
    /// <returns>The expression.</returns>
    /// <param name="text">Text.</param>
    /// <param name="variableNames">Variable names.</param>
    /// <param name="expressionCreator">Expression creator.</param>
    public CSharpExpression GetOrAddExpression(string text,
                                               string[] variableNames,
                                               Func<string,string[],CSharpExpression> expressionCreator)
    {
      if(text == null)
      {
        throw new ArgumentNullException(nameof(text));
      }
      if(variableNames == null)
      {
        throw new ArgumentNullException(nameof(variableNames));
      }

      var key = new Tuple<string,string[]>(text, variableNames);
      Func<CSharpExpression> creator = () => expressionCreator(key.Item1, key.Item2);

      return base.GetOrAdd(key, creator);
    }
  }
}

