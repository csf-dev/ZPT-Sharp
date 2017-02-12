using System;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec
{
  /// <summary>
  /// Represents the specification for a namespace imported into an expression, via a <c>using</c> directive.
  /// </summary>
  public class UsingNamespaceSpecification : IEquatable<UsingNamespaceSpecification>, IComparable<UsingNamespaceSpecification>, IComparable
  {
    #region properties

    /// <summary>
    /// Gets the namespace.
    /// </summary>
    /// <value>The namespace.</value>
    public string Namespace
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the alias to be applied to that namespace.
    /// </summary>
    /// <value>The alias.</value>
    public string Alias
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets a value indicating whether this instance has an alias.
    /// </summary>
    /// <value><c>true</c> if this instance has an alias; otherwise, <c>false</c>.</value>
    public bool HasAlias
    {
      get {
        return !String.IsNullOrEmpty(Alias);
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Serves as a hash function for a
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.UsingNamespaceSpecification"/> object.
    /// </summary>
    /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
    public override int GetHashCode()
    {
      return Namespace.GetHashCode() ^ (HasAlias? Alias.GetHashCode() : 0);
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.UsingNamespaceSpecification"/>.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.UsingNamespaceSpecification"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.UsingNamespaceSpecification"/>; otherwise, <c>false</c>.</returns>
    public override bool Equals(object obj)
    {
      return (Equals(obj as UsingNamespaceSpecification));
    }

    /// <summary>
    /// Determines whether the specified
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.UsingNamespaceSpecification"/> is equal to the current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.UsingNamespaceSpecification"/>.
    /// </summary>
    /// <param name="other">The <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.UsingNamespaceSpecification"/> to compare with the
    /// current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.UsingNamespaceSpecification"/>.</param>
    /// <returns><c>true</c> if the specified
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.UsingNamespaceSpecification"/> is equal to the current
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.UsingNamespaceSpecification"/>; otherwise, <c>false</c>.</returns>
    public bool Equals(UsingNamespaceSpecification other)
    {
      return (other != null
              && this.Namespace == other.Namespace
              && this.Alias == other.Alias);
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
      var spec = other as UsingNamespaceSpecification;

      if(spec == null)
      {
        return -1;
      }

      return CompareTo(spec);
    }

    /// <summary>
    /// Compares the current instance to a given <see cref="UsingNamespaceSpecification"/>.
    /// </summary>
    /// <returns>
    /// A number indicating whether the current instance should be considered greater, less or equal to the
    /// given instance.
    /// </returns>
    /// <param name="other">The object with which to compare.</param>
    public int CompareTo(UsingNamespaceSpecification other)
    {
      if(other == null)
      {
        return 1;
      }

      var combinedNamespaceAndAlias = String.Concat(Namespace, Alias?? String.Empty);
      var otherCombinedNamespaceAndAlias = String.Concat(other.Namespace, other.Alias?? String.Empty);

      return String.Compare (combinedNamespaceAndAlias,
                             otherCombinedNamespaceAndAlias,
                             StringComparison.InvariantCulture);
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.UsingNamespaceSpecification"/> class.
    /// </summary>
    /// <param name="ns">The namespace.</param>
    public UsingNamespaceSpecification(string ns) : this(ns, null) {}

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.UsingNamespaceSpecification"/> class.
    /// </summary>
    /// <param name="ns">The namespace.</param>
    /// <param name="alias">Alias.</param>
    public UsingNamespaceSpecification(string ns, string alias)
    {
      if(ns == null)
      {
        throw new ArgumentNullException(nameof(ns));
      }

      Namespace = ns;
      Alias = alias;
    }

    #endregion
  }
}

