using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Zpt.Tales;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Default implementation of <see cref="ICSharpExpressionService"/>.
  /// </summary>
  public class CSharpExpressionService : ICSharpExpressionService
  {
    #region fields

    private static readonly ICSharpExpressionCache _cache;
    private static int _nextId;
    private static object _syncRoot;

    private readonly ICSharpExpressionFactory _factory;

    #endregion

    #region methods

    /// <summary>
    /// Gets a <see cref="CSharpExpression"/> matching the given expression text and variable names.
    /// </summary>
    /// <returns>The expression.</returns>
    /// <param name="text">The expression text.</param>
    /// <param name="model">The current TALES model.</param>
    public CSharpExpression GetExpression(string text, ITalesModel model)
    {
      if(text == null)
      {
        throw new ArgumentNullException(nameof(text));
      }
      if(model == null)
      {
        throw new ArgumentNullException(nameof(model));
      }

      var allDefinitions = model.GetAllDefinitions();

      var sortedVariableNames = allDefinitions.Keys.OrderBy(x => x).ToArray();
      return _cache.GetOrAddExpression(text, sortedVariableNames, CreateExpression);
    }

    /// <summary>
    /// Creates a <see cref="CSharpExpression"/> from the text and variable names.
    /// </summary>
    /// <returns>The expression.</returns>
    /// <param name="text">Text.</param>
    /// <param name="variableNames">Variable names.</param>
    private CSharpExpression CreateExpression(string text, string[] variableNames)
    {
      var model = CreateExpressionModel(text, variableNames);
      return _factory.Create(model);
    }

    /// <summary>
    /// Creates an expression model matching the given text and variable names.
    /// </summary>
    /// <returns>The expression model.</returns>
    /// <param name="text">Text.</param>
    /// <param name="variableNames">Variable names.</param>
    private ExpressionModel CreateExpressionModel(string text, string[] variableNames)
    {
      lock(_syncRoot)
      {
        return new ExpressionModel(_nextId++, text, variableNames);
      }
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.CSharpExpressionService"/> class.
    /// </summary>
    /// <param name="factory">Factory.</param>
    public CSharpExpressionService(ICSharpExpressionFactory factory = null)
    {
      _factory = factory?? new CSharpExpressionFactory();
    }

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.CSharpExpressionService"/> class.
    /// </summary>
    static CSharpExpressionService()
    {
      _cache = new CSharpExpressionCache();
      _nextId = 1;
      _syncRoot = new Object();
    }

    #endregion
  }
}

