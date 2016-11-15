using System;
using System.Collections.Generic;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Fallback implementation of <see cref="INamespaceConfiguration"/>.
  /// </summary>
  public class FallbackNamespaceConfiguration : INamespaceConfiguration
  {
    #region fields

    private static INamespaceConfiguration _default;

    #endregion

    #region methods

    /// <summary>
    /// Gets the namespaces which are imported via <c>using</c> statements, for all CSharp expressions.
    /// </summary>
    /// <returns>The namespaces.</returns>
    public IEnumerable<string> GetNamespaces()
    {
      return new [] {
        typeof(System.DateTime).Namespace,
//        typeof(System.Linq.Enumerable).Namespace,
      };
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.FallbackNamespaceConfiguration"/> class.
    /// </summary>
    static FallbackNamespaceConfiguration()
    {
      _default = new FallbackNamespaceConfiguration();
    }

    #endregion

    #region singleton

    /// <summary>
    /// Gets a singleton instance of this type.
    /// </summary>
    /// <value>The default.</value>
    public static INamespaceConfiguration Default
    {
      get {
        return _default;
      }
    }

    #endregion
  }
}

