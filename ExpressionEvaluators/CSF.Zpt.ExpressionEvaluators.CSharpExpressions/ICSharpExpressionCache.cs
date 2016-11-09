using System;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  public interface ICSharpExpressionCache
  {
    CSharpExpression GetOrAddExpression(string text,
                                        string[] variableNames,
                                        Func<string,string[],CSharpExpression> expressionCreator);
  }
}

