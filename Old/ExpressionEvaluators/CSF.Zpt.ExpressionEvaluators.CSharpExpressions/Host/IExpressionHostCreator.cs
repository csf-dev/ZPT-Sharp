using System;
using CSF.Zpt.ExpressionEvaluators.CSharpFramework;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Host
{
  /// <summary>
  /// Interface for an object which is capable of creating an <see cref="IExpressionHost"/> instance, which contains
  /// the compiled code for a single CSharp expression.
  /// </summary>
  public interface IExpressionHostCreator
  {
    /// <summary>
    /// Creates and returns the host instance.
    /// </summary>
    /// <returns>The host instance.</returns>
    IExpressionHost CreateHostInstance();
  }
}

