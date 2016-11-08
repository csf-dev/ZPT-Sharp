using System;
using System.Collections.Generic;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  public class CSharpExpressionFactory
  {
    public virtual CSharpExpression CreateExpression(string expressionText, IEnumerable<string> variableNames)
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    private string BuildExpressionHostCode(int id, string expressionText, string[] variableNames)
    {
      var model = new ExpressionHostBuilderModel(id, expressionText, variableNames);
      var builder = new ExpressionHostBuilder(model);
      return builder.TransformText();
    }
  }
}

