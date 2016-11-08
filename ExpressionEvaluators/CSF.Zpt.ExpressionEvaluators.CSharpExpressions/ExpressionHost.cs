using System;

namespace CsExpr
{
  public class ExpressionHost : CSF.Zpt.ExpressionEvaluators.CSharpExpressions.IExpressionHost
  {
    #region fields

    private dynamic propertyOne;
    private dynamic propertyTwo;

    #endregion

    #region methods

    object CSF.Zpt.ExpressionEvaluators.CSharpExpressions.IExpressionHost.Evaluate()
    {
      return propertyOne.Foo + propertyTwo.Bar;
    }

    void CSF.Zpt.ExpressionEvaluators.CSharpExpressions.IExpressionHost.SetPropertyValue(string propertyName,
                                                                                         object value)
    {
      switch(propertyName)
      {
      case "propertyOne":
        propertyOne = value;
        break;

      case "propertyTwo":
        propertyTwo = value;
        break;

      default:
        // TODO: Improve this expression type and add a message to a res file
        throw new InvalidOperationException();
      }
    }

    #endregion
  }
}

