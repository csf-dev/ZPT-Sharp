using System;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  public interface IExpressionHost
  {
    void SetPropertyValue(string propertyName, object value);

    object Evaluate();
  }
}

