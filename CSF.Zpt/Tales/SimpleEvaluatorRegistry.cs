using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Simple implementation of <see cref="IExpressionEvaluator"/> which keeps a collection of the available evaluators
  /// in an <c>IDictionary</c>, indexed by their applicable prefixes.
  /// </summary>
  public class SimpleEvaluatorRegistry : IEvaluatorRegistry
  {
    #region constants

    private static SimpleEvaluatorRegistry _default;

    #endregion

    #region fields

    private IDictionary<string,IExpressionEvaluator> _evaluators;
    private IExpressionEvaluator _defaultEvaluator;

    #endregion

    #region public API

    /// <summary>
    /// Gets an evaluator implementation suitable for a given expression.
    /// </summary>
    /// <returns>The evaluator.</returns>
    /// <param name="expression">The expression.</param>
    public virtual IExpressionEvaluator GetEvaluator(Expression expression)
    {
      if(expression == null)
      {
        throw new ArgumentNullException("expression");
      }

      IExpressionEvaluator output;
      var prefix = expression.GetPrefix();

      if(prefix != null)
      {
        if(_evaluators.ContainsKey(prefix))
        {
          output = _evaluators[prefix];
        }
        else
        {
          string message = String.Format(Resources.ExceptionMessages.TalesExpressionEvaluatorNotFoundForPrefix,
                                         typeof(IExpressionEvaluator).FullName,
                                         prefix,
                                         expression);
          throw new InvalidExpressionException(message) {
            ExpressionText = expression.Source
          };
        }
      }
      else
      {
        output = _defaultEvaluator;
      }

      return output;
    }

    #endregion

    #region methods

    /// <summary>
    /// Instantiates a collection of expression evaluators, from a collection of evaluator types.
    /// </summary>
    /// <returns>The created evaluators.</returns>
    /// <param name="types">The evaluator types.</param>
    private IEnumerable<IExpressionEvaluator> InstantiateEvaluators(IEnumerable<Type> types)
    {
      return types
        .Where(x => x != null
                    && typeof(IExpressionEvaluator).IsAssignableFrom(x))
        .Select(x => Activator.CreateInstance(x, new object[] { this }))
        .Cast<IExpressionEvaluator>();
    }

    /// <summary>
    /// Organises a collection of evaluators, returning a dictionary-based collection, with all of the instances indexed
    /// by their <see cref="IExpressionEvaluator.ExpressionPrefix"/>.
    /// </summary>
    /// <returns>The organised evaluators.</returns>
    /// <param name="evaluators">A collection of expression evaluators.</param>
    private IDictionary<string,IExpressionEvaluator> OrganiseEvaluators(IEnumerable<IExpressionEvaluator> evaluators)
    {
      if(evaluators == null)
      {
        throw new ArgumentNullException("evaluators");
      }

      return evaluators
        .Where(x => x != null)
        .ToDictionary(k => k.ExpressionPrefix, v => v);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.SimpleEvaluatorRegistry"/> class.
    /// </summary>
    /// <param name="evaluatorTypes">A collection of expression evaluator types.</param>
    /// <param name="defaultEvaluatorType">The default expression evaluator type.</param>
    public SimpleEvaluatorRegistry(IEnumerable<Type> evaluatorTypes,
                                   Type defaultEvaluatorType)
    {
      if(evaluatorTypes == null)
      {
        throw new ArgumentNullException("evaluatorTypes");
      }
      if(defaultEvaluatorType == null)
      {
        throw new ArgumentNullException("defaultEvaluatorType");
      }

      var evaluators = this.InstantiateEvaluators(evaluatorTypes);
      _defaultEvaluator = evaluators.Single(x => x.GetType() == defaultEvaluatorType);
      _evaluators = OrganiseEvaluators(evaluators);
    }

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.Tales.SimpleEvaluatorRegistry"/> class.
    /// </summary>
    static SimpleEvaluatorRegistry()
    {
      var evaluatorTypes = new [] {
        typeof(PathExpressionEvaluator),
        typeof(StringExpressionEvaluator),
        typeof(NotExpressionEvaluator),
      };

      _default = new SimpleEvaluatorRegistry(evaluatorTypes, typeof(PathExpressionEvaluator));
    }

    #endregion

    #region static properties

    /// <summary>
    /// Gets the default evaluator registry.
    /// </summary>
    /// <value>The default registry.</value>
    public static SimpleEvaluatorRegistry Default
    {
      get {
        return _default;
      }
    }

    #endregion
  }
}

