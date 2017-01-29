using System;
using System.Reflection;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec
{
  /// <summary>
  /// Represents the specification for an assembly to be referenced by the expression.
  /// </summary>
  public class ReferencedAssemblySpecification : IEquatable<ReferencedAssemblySpecification>, IComparable<ReferencedAssemblySpecification>, IComparable
  {
    #region properties

    /// <summary>
    /// Gets the assembly name.
    /// </summary>
    /// <value>The name.</value>
    public string Name
    {
      get;
      private set;
    }

    #endregion

    #region methods

    /// <summary>
    /// Serves as a hash function for a
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ReferencedAssemblySpecification"/> object.
    /// </summary>
    /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
    public override int GetHashCode()
    {
      return Name.GetHashCode();
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ReferencedAssemblySpecification"/>.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ReferencedAssemblySpecification"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ReferencedAssemblySpecification"/>; otherwise, <c>false</c>.</returns>
    public override bool Equals(object obj)
    {
      return (Equals(obj as ReferencedAssemblySpecification));
    }

    /// <summary>
    /// Determines whether the specified
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ReferencedAssemblySpecification"/> is equal to the current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ReferencedAssemblySpecification"/>.
    /// </summary>
    /// <param name="other">The <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ReferencedAssemblySpecification"/> to compare with the
    /// current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ReferencedAssemblySpecification"/>.</param>
    /// <returns><c>true</c> if the specified
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ReferencedAssemblySpecification"/> is equal to the current
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ReferencedAssemblySpecification"/>; otherwise, <c>false</c>.</returns>
    public bool Equals(ReferencedAssemblySpecification other)
    {
      return (other != null
              && this.Name == other.Name);
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
      var spec = other as ReferencedAssemblySpecification;

      if(spec == null)
      {
        return -1;
      }

      return CompareTo(spec);
    }

    /// <summary>
    /// Compares the current instance to a given <see cref="ReferencedAssemblySpecification"/>.
    /// </summary>
    /// <returns>
    /// A number indicating whether the current instance should be considered greater, less or equal to the
    /// given instance.
    /// </returns>
    /// <param name="other">The object with which to compare.</param>
    public int CompareTo(ReferencedAssemblySpecification other)
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
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ReferencedAssemblySpecification"/> class.
    /// </summary>
    /// <param name="name">The assembly name.</param>
    public ReferencedAssemblySpecification(string name)
    {
      if(name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      Name = name;
    }

    #endregion
  }
}

