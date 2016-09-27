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
  public class SimpleEvaluatorSelector : IEvaluatorSelector
  {
    #region constants

    private static SimpleEvaluatorSelector _default;

    #endregion

    #region fields

    private IDictionary<string,IExpressionEvaluator> _evaluatorsByPrefix;
    private IDictionary<Type,IExpressionEvaluator> _evaluatorsByType;
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
        throw new ArgumentNullException(nameof(expression));
      }

      IExpressionEvaluator output;
      var prefix = expression.Prefix;

      if(prefix != null)
      {
        if(_evaluatorsByPrefix.ContainsKey(prefix))
        {
          output = _evaluatorsByPrefix[prefix];
        }
        else
        {
          string message = String.Format(Resources.ExceptionMessages.TalesExpressionEvaluatorNotFoundForPrefix,
                                         typeof(IExpressionEvaluator).FullName,
                                         prefix,
                                         expression);
          throw new InvalidExpressionException(message) {
            ExpressionText = expression.ToString()
          };
        }
      }
      else
      {
        output = _defaultEvaluator;
      }

      return output;
    }

    /// <summary>
    /// Gets an expression evaluator matching a desired type.
    /// </summary>
    /// <returns>The evaluator instance.</returns>
    /// <typeparam name="TEvaluator">The desired evaluator type.</typeparam>
    public IExpressionEvaluator GetEvaluator<TEvaluator>() where TEvaluator : IExpressionEvaluator
    {
      return this.GetEvaluator(typeof(TEvaluator));
    }

    /// <summary>
    /// Gets an expression evaluator matching a desired type.
    /// </summary>
    /// <returns>The evaluator instance.</returns>
    /// <param name="evaluatorType">The desired evaluator type.</param>
    public IExpressionEvaluator GetEvaluator(Type evaluatorType)
    {
      if(evaluatorType == null)
      {
        throw new ArgumentNullException(nameof(evaluatorType));
      }
      else if(!_evaluatorsByType.ContainsKey(evaluatorType))
      {
        string message = String.Format(Resources.ExceptionMessages.TalesExpressionEvaluatorNotFoundByType,
                                       typeof(IExpressionEvaluator).FullName,
                                       evaluatorType);
        throw new Rendering.ModelEvaluationException(message);
      }

      return _evaluatorsByType[evaluatorType];
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
    /// Organises a collection of evaluators, initialising the state of the current instance.
    /// </summary>
    /// <param name="evaluators">A collection of expression evaluators.</param>
    /// <param name="defaultEvaluatorType">The default expression evaluator type.</param>
    private void OrganiseEvaluators(IEnumerable<IExpressionEvaluator> evaluators, Type defaultEvaluatorType)
    {
      if(evaluators == null)
      {
        throw new ArgumentNullException(nameof(evaluators));
      }

      _defaultEvaluator = evaluators
        .Single(x => x.GetType() == defaultEvaluatorType);

      _evaluatorsByPrefix = evaluators
        .Where(x => x != null)
        .ToDictionary(k => k.ExpressionPrefix, v => v);

      _evaluatorsByType = evaluators
        .Where(x => x != null)
        .ToDictionary(k => k.GetType(), v => v);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.SimpleEvaluatorRegistry"/> class.
    /// </summary>
    /// <param name="evaluatorTypes">A collection of expression evaluator types.</param>
    /// <param name="defaultEvaluatorType">The default expression evaluator type.</param>
    public SimpleEvaluatorSelector(IEnumerable<Type> evaluatorTypes,
                                   Type defaultEvaluatorType)
    {
      if(evaluatorTypes == null)
      {
        throw new ArgumentNullException(nameof(evaluatorTypes));
      }
      if(defaultEvaluatorType == null)
      {
        throw new ArgumentNullException(nameof(defaultEvaluatorType));
      }

      var evaluators = this.InstantiateEvaluators(evaluatorTypes);
      this.OrganiseEvaluators(evaluators, defaultEvaluatorType);
    }

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.Tales.SimpleEvaluatorRegistry"/> class.
    /// </summary>
    static SimpleEvaluatorSelector()
    {
      var evaluatorTypes = new [] {
        typeof(PathExpressionEvaluator),
        typeof(StringExpressionEvaluator),
        typeof(NotExpressionEvaluator),
        typeof(LocalVariablePathExpressionEvaluator),
      };

      _default = new SimpleEvaluatorSelector(evaluatorTypes, typeof(PathExpressionEvaluator));
    }

    #endregion

    #region static properties

    /// <summary>
    /// Gets the default evaluator registry.
    /// </summary>
    /// <value>The default registry.</value>
    public static SimpleEvaluatorSelector Default
    {
      get {
        return _default;
      }
    }

    #endregion
  }
}

