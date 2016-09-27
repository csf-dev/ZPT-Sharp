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
    #region fields

    private readonly IExpressionEvaluatorRegistry _registry;

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
        output = _registry.GetEvaluator(prefix);

        if(output == null)
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
        output = _registry.DefaultEvaluator;
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

      var output = _registry.GetEvaluator(evaluatorType);

      if(output == null)
      {
        string message = String.Format(Resources.ExceptionMessages.TalesExpressionEvaluatorNotFoundByType,
                                       typeof(IExpressionEvaluator).FullName,
                                       evaluatorType);
        throw new Rendering.ModelEvaluationException(message);
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.SimpleEvaluatorSelector"/> class.
    /// </summary>
    /// <param name="registry">An expression evaluator registry.</param>
    public SimpleEvaluatorSelector(IExpressionEvaluatorRegistry registry = null)
    {
      _registry = registry?? ExpressionEvaluatorRegistry.Default;
    }

    #endregion
  }
}

