using System;
using System.Collections.Generic;
using CSF.Collections;
using System.Linq;

namespace CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec
{
  /// <summary>
  /// Immutable type representing the specification for a CSharp expression.
  /// </summary>
  public class ExpressionSpecification : IEquatable<ExpressionSpecification>
  {
    #region properties

    /// <summary>
    /// Gets the expression text.
    /// </summary>
    /// <value>The text.</value>
    public string Text
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets a collection of the variables which are 'in scope' for the expression.
    /// </summary>
    /// <value>The variables.</value>
    public IEnumerable<VariableSpecification> Variables
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets a collection of the assemblies to be referenced for evaluating the expression.
    /// </summary>
    /// <value>The assemblies.</value>
    public IEnumerable<ReferencedAssemblySpecification> Assemblies
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets a collection of the namespaces to be imported for the expression.
    /// </summary>
    /// <value>The namespaces.</value>
    public IEnumerable<UsingNamespaceSpecification> Namespaces
    {
      get;
      private set;
    }

    #endregion

    #region methods

    /// <summary>
    /// Determines whether the specified
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ExpressionSpecification"/> is equal to the
    /// current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ExpressionSpecification"/>.
    /// </summary>
    /// <param name="other">The <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ExpressionSpecification"/> to compare with
    /// the current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ExpressionSpecification"/>.</param>
    /// <returns><c>true</c> if the specified
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ExpressionSpecification"/> is equal to the
    /// current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ExpressionSpecification"/>; otherwise, <c>false</c>.</returns>
    public bool Equals(ExpressionSpecification other)
    {
      if(ReferenceEquals(null, other))
      {
        return false;
      }
      else if(ReferenceEquals(this, other))
      {
        return true;
      }

      bool
        textEqual = Text == other.Text,
        variablesEqual = Variables.AreContentsSameAs(other.Variables),
        assembliesEqual = Assemblies.AreContentsSameAs(other.Assemblies),
        namespacesEqual = Namespaces.AreContentsSameAs(other.Namespaces);

      return (textEqual && variablesEqual && assembliesEqual && namespacesEqual);
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ExpressionSpecification"/>.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ExpressionSpecification"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ExpressionSpecification"/>; otherwise, <c>false</c>.</returns>
    public override bool Equals(object obj)
    {
      var other = obj as ExpressionSpecification;

      if(ReferenceEquals(null, other))
      {
        return false;
      }

      return Equals(other);
    }

    /// <summary>
    /// Serves as a hash function for a
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ExpressionSpecification"/> object.
    /// </summary>
    /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
    public override int GetHashCode()
    {
      int
        text = Text.GetHashCode(),
        variables = GetSortedCollectionHashCode(Variables),
        assemblies = GetSortedCollectionHashCode(Assemblies),
        namespaces = GetSortedCollectionHashCode(Namespaces);

      return text ^ variables ^ assemblies ^ namespaces;
    }

    /// <summary>
    /// Gets a hash code for the collection.
    /// </summary>
    /// <returns>The hash code.</returns>
    /// <param name="collection">Collection.</param>
    /// <typeparam name="TCollection">The 1st type parameter.</typeparam>
    private int GetSortedCollectionHashCode<TCollection>(IEnumerable<TCollection> collection)
      where TCollection : IComparable
    {
      if(collection == null)
      {
        throw new ArgumentNullException(nameof(collection));
      }

      return collection
        .OrderBy(x => x)
        .Aggregate(0, (acc, next) => acc ^= next.GetHashCode());
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec.ExpressionSpecification"/> class.
    /// </summary>
    /// <param name="text">The expression text.</param>
    /// <param name="variables">The expression variables.</param>
    /// <param name="assemblies">The referenced assemblies.</param>
    /// <param name="namespaces">The imported namespaces.</param>
    public ExpressionSpecification(string text,
                                   IEnumerable<VariableSpecification> variables,
                                   IEnumerable<ReferencedAssemblySpecification> assemblies,
                                   IEnumerable<UsingNamespaceSpecification> namespaces)
    {
      if(text == null)
      {
        throw new ArgumentNullException(nameof(text));
      }
      if(variables == null)
      {
        throw new ArgumentNullException(nameof(variables));
      }
      if(assemblies == null)
      {
        throw new ArgumentNullException(nameof(assemblies));
      }
      if(namespaces == null)
      {
        throw new ArgumentNullException(nameof(namespaces));
      }

      Text = text;
      Variables = variables;
      Assemblies = assemblies;
      Namespaces = namespaces;
    }

    #endregion
  }
}

