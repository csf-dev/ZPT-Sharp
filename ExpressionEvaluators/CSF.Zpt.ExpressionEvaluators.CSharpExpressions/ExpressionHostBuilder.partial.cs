using System;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  public partial class ExpressionHostBuilder
  {
    protected ExpressionHostBuilderModel Model
    {
      get;
      private set;
    }

    public ExpressionHostBuilder(ExpressionHostBuilderModel model)
    {
      if(model == null)
      {
        throw new ArgumentNullException(nameof(model));
      }

      Model = model;
    }
  }
}

