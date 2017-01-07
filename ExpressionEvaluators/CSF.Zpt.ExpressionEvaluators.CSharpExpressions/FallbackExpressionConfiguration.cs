using System;
using System.Reflection;
using CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Rrepresents a set of fallback/default configuration for <see cref="IExpressionConfiguration"/>, should no other
  /// implementation be available.
  /// </summary>
  public class FallbackExpressionConfiguration : IExpressionConfiguration
  {
    #region constants

    private static readonly string[] DefaultAssemblies = new [] {
      "System.dll",
    };

    private static readonly string[] DefaultNamespaces = new [] {
      typeof(System.String).Namespace,
      typeof(System.Linq.Enumerable).Namespace,
    };

    #endregion

    #region fields

    private static readonly IExpressionConfiguration _singleton;

    #endregion

    #region methods

    /// <summary>
    /// Gets the imported namespaces.
    /// </summary>
    /// <returns>The imported namespaces.</returns>
    public IEnumerable<UsingNamespaceSpecification> GetImportedNamespaces()
    {
      return DefaultNamespaces
        .Select(x => new UsingNamespaceSpecification(x))
        .ToArray();
    }

    /// <summary>
    /// Gets the referenced assemblies.
    /// </summary>
    /// <returns>The referenced assemblies.</returns>
    public IEnumerable<ReferencedAssemblySpecification> GetReferencedAssemblies()
    {
      return DefaultAssemblies
        .Select(x => new ReferencedAssemblySpecification(x))
        .ToArray();
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.FallbackExpressionConfiguration"/> class.
    /// </summary>
    private FallbackExpressionConfiguration() { }

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.FallbackExpressionConfiguration"/> class.
    /// </summary>
    static FallbackExpressionConfiguration()
    {
      _singleton = new FallbackExpressionConfiguration();
    }

    #endregion

    #region singleton

    /// <summary>
    /// Gets the default/singleton instance.
    /// </summary>
    /// <value>The instance.</value>
    public static IExpressionConfiguration Instance
    {
      get {
        return _singleton;
      }
    }

    #endregion
  }
}

