using System;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  public class CSharpExpressionService
  {
    #region fields

    private static readonly ICSharpExpressionCache _cache;
    private static int _nextId;
    private static object _syncRoot;

    private readonly ICSharpExpressionFactory _factory;

    #endregion

    #region methods

    public CSharpExpression GetExpression(string text, IEnumerable<string> variableNames)
    {
      if(text == null)
      {
        throw new ArgumentNullException(nameof(text));
      }
      if(variableNames == null)
      {
        throw new ArgumentNullException(nameof(variableNames));
      }

      var sortedVariableNames = variableNames.OrderBy(x => x).ToArray();
      return _cache.GetOrAddExpression(text, sortedVariableNames, CreateExpression);
    }

    private CSharpExpression CreateExpression(string text, string[] variableNames)
    {
      var model = CreateExpressionModel(text, variableNames);
      return _factory.Create(model);
    }

    private ExpressionModel CreateExpressionModel(string text, string[] variableNames)
    {
      lock(_syncRoot)
      {
        return new ExpressionModel(_nextId++, text, variableNames);
      }
    }

    #endregion

    #region constructors

    public CSharpExpressionService(ICSharpExpressionFactory factory = null)
    {
      _factory = factory?? new CSharpExpressionFactory();
    }

    static CSharpExpressionService()
    {
      _cache = new CSharpExpressionCache();
      _nextId = 1;
      _syncRoot = new Object();
    }

    #endregion
  }
}

