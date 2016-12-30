using System;
using CSF.Zpt.Tales;
using System.Collections.Generic;
using CSF.Configuration;
using System.Linq;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec
{
  /// <summary>
  /// Default implementation of <see cref="IExpressionSpecificationFactory"/>.
  /// </summary>
  public class ExpressionSpecificationFactory : IExpressionSpecificationFactory
  {
    #region fields

    private IExpressionConfiguration _expressionConfiguration;

    #endregion

    #region methods

    /// <summary>
    /// Creates the expression specification from an expression text and a TALES model.
    /// </summary>
    /// <returns>The expression specification.</returns>
    /// <param name="expressionText">Expression text.</param>
    /// <param name="model">The TALES model.</param>
    public ExpressionSpecification CreateExpressionSpecification(string expressionText, ITalesModel model)
    {
      if(expressionText == null)
      {
        throw new ArgumentNullException(nameof(expressionText));
      }
      if(model == null)
      {
        throw new ArgumentNullException(nameof(model));
      }

      var allDefinitions = model.GetAllDefinitions();

      var referencedAssemblies = GetReferencedAssemblies(allDefinitions);
      var importedNamespaces = GetImportedNamespaces(allDefinitions);
      var variables = GetVariables(allDefinitions);

      return new ExpressionSpecification(expressionText, variables, referencedAssemblies, importedNamespaces);
    }

    private IEnumerable<ReferencedAssemblySpecification> GetReferencedAssemblies(IDictionary<string,object> allDefinitions)
    {
      return allDefinitions.Values
        .Where(x => x is ReferencedAssemblySpecification)
        .Cast<ReferencedAssemblySpecification>()
        .Union(_expressionConfiguration.GetReferencedAssemblies())
        .Distinct()
        .ToArray();
    }

    private IEnumerable<UsingNamespaceSpecification> GetImportedNamespaces(IDictionary<string,object> allDefinitions)
    {
      return allDefinitions.Values
        .Where(x => x is UsingNamespaceSpecification)
        .Cast<UsingNamespaceSpecification>()
        .Union(_expressionConfiguration.GetImportedNamespaces())
        .Distinct()
        .ToArray();
    }

    private IEnumerable<VariableSpecification> GetVariables(IDictionary<string,object> allDefinitions)
    {
      return VariableSpecification.GetVariableSpecifications(allDefinitions);
    }

    private IExpressionConfiguration GetConfiguration()
    {
      var output = ConfigurationHelper.GetSection<ExpressionConfigurationSection>();
      return output?? FallbackExpressionConfiguration.Instance;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ExpressionSpecificationFactory"/> class.
    /// </summary>
    /// <param name="expressionConfig">Expression config.</param>
    public ExpressionSpecificationFactory(IExpressionConfiguration expressionConfig = null)
    {
      _expressionConfiguration = expressionConfig?? GetConfiguration();
    }

    #endregion
  }
}

