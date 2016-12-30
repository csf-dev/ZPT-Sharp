using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Zpt.Tales;
using CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec;
using CSF.Caches;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Default implementation of <see cref="ICSharpExpressionService"/>.
  /// </summary>
  public class CSharpExpressionService : ICSharpExpressionService
  {
    #region fields

    private static readonly ICache<ExpressionSpecification,CSharpExpression> _cache;
    private static int _nextId;
    private static object _syncRoot;

    private readonly IExpressionSpecificationFactory _specFactory;
    private readonly ICSharpExpressionFactory _expressionFactory;

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

      var spec = _specFactory.CreateExpressionSpecification(text, model);

      return _cache.GetOrAdd(spec, GetExpressionCreator(spec));
    }

    /// <summary>
    /// Returns a delegate which may be used to create an instance of <see cref="CSharpExpression"/> from the specification.
    /// </summary>
    /// <returns>The expression creator delegate.</returns>
    /// <param name="spec">Expression specification.</param>
    private Func<CSharpExpression> GetExpressionCreator(ExpressionSpecification spec)
    {
      return () => {
        var model = CreateExpressionModel(spec);
        return _expressionFactory.Create(model);
      };
    }

    /// <summary>
    /// Creates an expression model matching the given text and variable names.
    /// </summary>
    /// <returns>The expression model.</returns>
    /// <param name="spec">Expression specification.</param>
    private ExpressionModel CreateExpressionModel(ExpressionSpecification spec)
    {
      lock(_syncRoot)
      {
        return new ExpressionModel(_nextId++, spec);
      }
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.CSharpExpressionService"/> class.
    /// </summary>
    /// <param name="expressionFactory">Expression factory.</param>
    /// <param name="specFactory">Expression specification factory.</param>
    public CSharpExpressionService(ICSharpExpressionFactory expressionFactory = null,
                                   IExpressionSpecificationFactory specFactory = null)
    {
      _expressionFactory = expressionFactory?? new CSharpExpressionFactory();
      _specFactory = specFactory?? new ExpressionSpecificationFactory();
    }

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.CSharpExpressionService"/> class.
    /// </summary>
    static CSharpExpressionService()
    {
      _cache = new ThreadSafeCache<ExpressionSpecification, CSharpExpression>();
      _nextId = 1;
      _syncRoot = new Object();
    }

    #endregion
  }
}

