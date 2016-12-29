using System;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec
{
  /// <summary>
  /// Represents the specification for a variable to be included in an expression.
  /// </summary>
  public class VariableSpecification : IEquatable<VariableSpecification>
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
  }
}

