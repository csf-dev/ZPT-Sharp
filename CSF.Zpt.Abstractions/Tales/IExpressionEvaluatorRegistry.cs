using System;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Interface for a service which provides access to all of the <see cref="IExpressionEvaluator"/> instances
  /// registered in the current environment.
  /// </summary>
  public interface IExpressionEvaluatorRegistry
  {
    #region properties

    /// <summary>
    /// Gets the default evaluator.
    /// </summary>
    /// <value>The default evaluator.</value>
    IExpressionEvaluator DefaultEvaluator { get; }

    #endregion

    #region methods

    /// <summary>
    /// Gets an evaluator by its string expression prefix.
    /// </summary>
    /// <returns>The evaluator.</returns>
    /// <param name="prefix">The associated expression prefix.</param>
    IExpressionEvaluator GetEvaluator(string prefix);

    /// <summary>
    /// Gets an evaluator by its <c>System.Type</c>.
    /// </summary>
    /// <returns>The evaluator.</returns>
    /// <param name="evaluatorType">Evaluator type.</param>
    IExpressionEvaluator GetEvaluator(Type evaluatorType);

    #endregion
  }
}

