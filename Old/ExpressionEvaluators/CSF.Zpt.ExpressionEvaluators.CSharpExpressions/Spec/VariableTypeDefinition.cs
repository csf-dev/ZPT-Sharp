using System;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec
{
  /// <summary>
  /// Represents the assignment of a variable type to a variable name.
  /// </summary>
  public class VariableTypeDefinition
  {
    #region properties

    /// <summary>
    /// Gets the name of the variable.
    /// </summary>
    /// <value>The name of the variable.</value>
    public string VariableName
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the name of the type.
    /// </summary>
    /// <value>The name of the type.</value>
    public string TypeName
    {
      get;
      private set;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.VariableTypeDefinition"/> class.
    /// </summary>
    /// <param name="variableName">Variable name.</param>
    /// <param name="typeName">Type name.</param>
    public VariableTypeDefinition(string variableName, string typeName)
    {
      if(variableName == null)
      {
        throw new ArgumentNullException(nameof(variableName));
      }
      if(typeName == null)
      {
        throw new ArgumentNullException(nameof(typeName));
      }

      VariableName = variableName;
      TypeName = typeName;
    }

    #endregion
  }
}

