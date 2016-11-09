using System;

namespace CSF.Zpt.ExpressionEvaluators.CSharpFramework
{
  public interface IExpressionHost
  {
    void SetPropertyValue(string propertyName, object value);

    object Evaluate();
  }
}

