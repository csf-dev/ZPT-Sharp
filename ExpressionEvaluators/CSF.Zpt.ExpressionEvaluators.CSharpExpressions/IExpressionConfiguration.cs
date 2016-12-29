using System;
using CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec;
using System.Collections.Generic;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  /// <summary>
  /// Represents the configuration for CSharp expressions.
  /// </summary>
  public interface IExpressionConfiguration
  {
    /// <summary>
    /// Gets the imported namespaces.
    /// </summary>
    /// <returns>The imported namespaces.</returns>
    IEnumerable<UsingNamespaceSpecification> GetImportedNamespaces();

    /// <summary>
    /// Gets the referenced assemblies.
    /// </summary>
    /// <returns>The referenced assemblies.</returns>
    IEnumerable<ReferencedAssemblySpecification> GetReferencedAssemblies();
  }
}

