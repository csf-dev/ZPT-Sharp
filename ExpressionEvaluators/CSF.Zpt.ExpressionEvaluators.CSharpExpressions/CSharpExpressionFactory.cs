using System;
using System.Collections.Generic;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  public class CSharpExpressionFactory : ICSharpExpressionFactory
  {
    public CSharpExpression Create(ExpressionModel model)
    {
      if(model == null)
      {
        throw new ArgumentNullException(nameof(model));
      }

      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    private string GetExpressionHostCode(ExpressionModel model)
    {
      var builder = new ExpressionHostBuilder(model);
      return builder.TransformText();
    }
  }
}

