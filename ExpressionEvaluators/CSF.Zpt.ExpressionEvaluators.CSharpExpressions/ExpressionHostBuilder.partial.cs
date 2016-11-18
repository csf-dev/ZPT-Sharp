using System;
using System.Collections.Generic;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  internal partial class ExpressionHostBuilder
  {
    protected ExpressionModel Model
    {
      get;
      private set;
    }

    protected IEnumerable<string> Namespaces
    {
      get;
      private set;
    }

    public ExpressionHostBuilder(ExpressionModel model, IEnumerable<string> namespaces)
    {
      if(model == null)
      {
        throw new ArgumentNullException(nameof(model));
      }
      if(namespaces == null)
      {
        throw new ArgumentNullException(nameof(namespaces));
      }

      Model = model;
      Namespaces = namespaces;
    }
  }
}

