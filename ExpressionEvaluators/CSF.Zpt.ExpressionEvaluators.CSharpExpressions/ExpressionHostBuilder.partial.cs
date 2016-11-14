using System;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  internal partial class ExpressionHostBuilder
  {
    protected ExpressionModel Model
    {
      get;
      private set;
    }

    public ExpressionHostBuilder(ExpressionModel model)
    {
      if(model == null)
      {
        throw new ArgumentNullException(nameof(model));
      }

      Model = model;
    }
  }
}

