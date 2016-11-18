using System;

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
    /// <param name="text">Text.</param>
    /// <param name="variableNames">Variable names.</param>
    /// <param name="expressionCreator">Expression creator.</param>
    CSharpExpression GetOrAddExpression(string text,
                                        string[] variableNames,
                                        Func<string,string[],CSharpExpression> expressionCreator);
  }
}

