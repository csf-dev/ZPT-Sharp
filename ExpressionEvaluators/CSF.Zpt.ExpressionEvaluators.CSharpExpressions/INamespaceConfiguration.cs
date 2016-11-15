using System;
using System.Collections.Generic;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Interface for a configuration type which provides the namespaces which are imported for CSharp expressions.
  /// </summary>
  public interface INamespaceConfiguration
  {
    /// <summary>
    /// Gets the namespaces which are imported via <c>using</c> statements, for all CSharp expressions.
    /// </summary>
    /// <returns>The namespaces.</returns>
    IEnumerable<string> GetNamespaces();
  }
}

