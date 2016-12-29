using System;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Host
{
  /// <summary>
  /// Interface for a service which generates an <see cref="ExpressionHostCreator"/> instances, by compiling dynamic
  /// code.
  /// </summary>
  public interface IExpressionHostCompiler
  {
    /// <summary>
    /// Compiles and returns an <see cref="IExpressionHostCreator"/> using data from the given model.
    /// </summary>
    /// <returns>The host creator.</returns>
    /// <param name="model">Model.</param>
    IExpressionHostCreator GetHostCreator(ExpressionModel model);
  }
}

