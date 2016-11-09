using System;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  public interface ICSharpExpressionFactory
  {
    CSharpExpression Create(ExpressionModel model);
  }
}

