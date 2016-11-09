using System;
using System.Collections.Generic;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  public interface ICSharpExpressionService
  {
    CSharpExpression GetExpression(string text, IEnumerable<string> variableNames);
  }
}

