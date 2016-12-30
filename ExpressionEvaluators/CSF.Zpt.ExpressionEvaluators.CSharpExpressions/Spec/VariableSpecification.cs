using System;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec
{
  /// <summary>
  /// Represents the specification for a variable to be included in an expression.
  /// </summary>
  public class VariableSpecification : IEquatable<VariableSpecification>, IComparable<VariableSpecification>, IComparable
  {
    #region properties

    /// <summary>
    /// Gets the variable name.
    /// </summary>
    /// <value>The name.</value>
    public string Name
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the variable type.
    /// </summary>
    /// <value>The type.</value>
    public string TypeName
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets a value indicating whether this variable is dynamically-typed.
    /// </summary>
    /// <value><c>true</c> if this instance is dynamically-typed; otherwise, <c>false</c>.</value>
    public bool IsDynamicType
    {
      get {
        return TypeName == null;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Serves as a hash function for a
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.VariableSpecification"/> object.
    /// </summary>
    /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
    public override int GetHashCode()
    {
      return Name.GetHashCode() ^ (IsDynamicType? 0 : TypeName.GetHashCode());
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.VariableSpecification"/>.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.VariableSpecification"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.VariableSpecification"/>; otherwise, <c>false</c>.</returns>
    public override bool Equals(object obj)
    {
      return (Equals(obj as VariableSpecification));
    }

    /// <summary>
    /// Determines whether the specified
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.VariableSpecification"/> is equal to the current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.VariableSpecification"/>.
    /// </summary>
    /// <param name="other">The <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.VariableSpecification"/> to compare with the
    /// current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.VariableSpecification"/>.</param>
    /// <returns><c>true</c> if the specified
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.VariableSpecification"/> is equal to the current
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.VariableSpecification"/>; otherwise, <c>false</c>.</returns>
    public bool Equals(VariableSpecification other)
    {
      return (other != null
              && this.Name == other.Name
              && this.TypeName == other.TypeName);
    }

    /// <summary>
    /// Compares the current instance to a given <c>System.Object</c>.
    /// </summary>
    /// <returns>
    /// A number indicating whether the current instance should be considered greater, less or equal to the
    /// given instance.
    /// </returns>
    /// <param name="other">The object with which to compare.</param>
    public int CompareTo(object other)
    {
      var spec = other as VariableSpecification;

      if(spec == null)
      {
        return -1;
      }

      return CompareTo(spec);
    }

    /// <summary>
    /// Compares the current instance to a given <see cref="VariableSpecification"/>.
    /// </summary>
    /// <returns>
    /// A number indicating whether the current instance should be considered greater, less or equal to the
    /// given instance.
    /// </returns>
    /// <param name="other">The object with which to compare.</param>
    public int CompareTo(VariableSpecification other)
    {
      if(other == null)
      {
        return 1;
      }

      return Name.CompareTo(other.Name);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.VariableSpecification"/> class.
    /// </summary>
    /// <param name="name">Name.</param>
    public VariableSpecification(string name) : this(name, null) {}

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.VariableSpecification"/> class.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="typeName">Type name.</param>
    public VariableSpecification(string name, string typeName)
    {
      if(name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      Name = name;
      TypeName = typeName;
    }

    #endregion

    #region static methods

    /// <summary>
    /// Reads a series of variable values and gets the resultant definitions which matter for CSharp expressions.
    /// </summary>
    /// <returns>The variable specifications.</returns>
    /// <param name="variableValues">Variable values.</param>
    public static IEnumerable<VariableSpecification> GetVariableSpecifications(IDictionary<string,object> variableValues)
    {
      var variableTypes = variableValues.Values
        .Where(x => x is VariableTypeDefinition)
        .Cast<VariableTypeDefinition>()
        .ToArray();

      var allVariableNames = variableValues
        .Where(x => !(x.Value is ReferencedAssemblySpecification)
               && !(x.Value is UsingNamespaceSpecification)
               && !(x.Value is VariableTypeDefinition))
        .Select(x => x.Key)
        .ToArray();

      return (from name in allVariableNames
              join definition in variableTypes
              on name equals definition.VariableName
              into variablesAndDefinitions
              select new VariableSpecification(name, variablesAndDefinitions.FirstOrDefault()?.TypeName))
        .ToArray();
    }

    #endregion
  }
}

