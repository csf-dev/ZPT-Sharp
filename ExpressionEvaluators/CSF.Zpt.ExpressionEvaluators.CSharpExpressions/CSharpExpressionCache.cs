using System;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  public class CSharpExpressionCache : ThreadSafeCache<Tuple<string,string[]>,CSharpExpression>, ICSharpExpressionCache
  {
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

