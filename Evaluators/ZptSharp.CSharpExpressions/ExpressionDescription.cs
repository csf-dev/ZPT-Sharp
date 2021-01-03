using System;
using System.Collections.Generic;
using System.Linq;

namespace ZptSharp.Expressions.CSharpExpressions
{
    /// <summary>
    /// Represents the identifing information and metadata for a C# expression.
    /// This serves both as a request to create an expression, but also as a unique identifying
    /// key for an expression.
    /// </summary>
    public sealed class ExpressionDescription : IEquatable<ExpressionDescription>
    {
        const int baseHashingPrime = 17, multiplierHashingPrime = 31;

        readonly HashSet<AssemblyReference> assemblyReferences;
        readonly HashSet<UsingNamespace> usingNamespaces;
        readonly HashSet<string> inScopeVariableNames;
        readonly HashSet<VariableType> typeDesignations;

        /// <summary>
        /// Gets the body of the expression itself.  This is an executable C# script.
        /// </summary>
        /// <value>The expression body.</value>
        public string Expression { get; }
        
        /// <summary>
        /// Gets a collection of the assembly references which should be available when the expression is evaluated.
        /// </summary>
        public IReadOnlyCollection<AssemblyReference> AssemblyReferences => assemblyReferences;
        
        /// <summary>
        /// Gets a collection of the <c>using</c> namespaces which should be 'in scope' when the expression is evaluated.
        /// </summary>
        public IReadOnlyCollection<UsingNamespace> UsingNamespaces => usingNamespaces;
        
        /// <summary>
        /// Gets a collection of the names of variables which are in-scope for this expression.
        /// </summary>
        public IReadOnlyCollection<string> InScopeVariableNames => inScopeVariableNames;
        
        /// <summary>
        /// Gets a collection of variable-type-designations.  These indicate any variables which should be treated as
        /// strongly-typed instances and not as <c>dynamic</c> objects.
        /// </summary>
        public IReadOnlyCollection<VariableType> TypeDesignations => typeDesignations;

        IOrderedEnumerable<AssemblyReference> SortedAssemblyReferences
            => AssemblyReferences.OrderBy(x => x.Name, StringComparer.InvariantCulture);

        IOrderedEnumerable<UsingNamespace> SortedUsingNamespaces
            => UsingNamespaces.OrderBy(x => x.Namespace, StringComparer.InvariantCulture);

        IOrderedEnumerable<string> SortedInScopeVariableNames
            => InScopeVariableNames.OrderBy(x => x, StringComparer.InvariantCulture);

        IOrderedEnumerable<VariableType> SortedTypeDesignations
            => TypeDesignations.OrderBy(x => x.VariableName, StringComparer.InvariantCulture);

        /// <summary>
        /// Gets a value which indicates if the current instance is equal to the specified <see cref="ExpressionDescription" />.
        /// </summary>
        /// <param name="other">An expression specification.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(ExpressionDescription other)
        {
            if(ReferenceEquals(other, this)) return true;
            if(other is null) return false;

            return String.Equals(Expression, other.Expression, StringComparison.InvariantCulture)
                && assemblyReferences.SetEquals(other.assemblyReferences)
                && usingNamespaces.SetEquals(other.usingNamespaces)
                && inScopeVariableNames.SetEquals(other.inScopeVariableNames)
                && typeDesignations.SetEquals(other.typeDesignations);
        }

        /// <summary>
        /// Gets a value which indicates if the current instance is equal to the specified <see cref="object" />.
        /// </summary>
        /// <param name="obj">An object.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj) => Equals(obj as ExpressionDescription);
        
        /// <summary>
        /// Gets a hash code for the current instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = baseHashingPrime;

                hash = hash * multiplierHashingPrime + Expression.GetHashCode();
                
                foreach(var reference in SortedAssemblyReferences)
                    hash = hash * multiplierHashingPrime + reference.GetHashCode();
                
                foreach(var @namespace in SortedUsingNamespaces)
                    hash = hash * multiplierHashingPrime + @namespace.GetHashCode();
                
                foreach(var name in SortedInScopeVariableNames)
                    hash = hash * multiplierHashingPrime + name.GetHashCode();
                
                foreach(var typeDesignation in SortedTypeDesignations)
                    hash = hash * multiplierHashingPrime + typeDesignation.GetHashCode();

                return hash;
            }
        }

        /// <summary>
        /// Gets a string which represents the current instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return $@"[{nameof(ExpressionDescription)} (
    Expression = ""{Expression}"",
    Assemblies = {String.Join(",\n                 ", SortedAssemblyReferences.Select(x => x.Name))}
    Namespaces = {String.Join(",\n                 ", SortedUsingNamespaces.Select(x => x.Namespace))}
    Variables  = {String.Join(", ", SortedInScopeVariableNames)}
    Var Types  = {String.Join(",\n                 ", SortedTypeDesignations.Select(x => $"{x.VariableName} is {x.Type}"))}
)]";
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ExpressionDescription" />.
        /// </summary>
        /// <param name="expression">The expression body.</param>
        /// <param name="assemblyReferences">The assembly references.</param>
        /// <param name="usingNamespaces">The using namespaces.</param>
        /// <param name="inScopeVariableNames">A collection of the names of variables which are in-scope for this expression.</param>
        /// <param name="typeDesignations">A collection of variable-type-designations.</param>
        public ExpressionDescription(string expression,
                                    IEnumerable<AssemblyReference> assemblyReferences,
                                    IEnumerable<UsingNamespace> usingNamespaces,
                                    IEnumerable<string> inScopeVariableNames,
                                    IEnumerable<VariableType> typeDesignations)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            this.typeDesignations = new HashSet<VariableType>(typeDesignations ?? throw new ArgumentNullException(nameof(typeDesignations)));
            this.inScopeVariableNames = new HashSet<string>(inScopeVariableNames ?? throw new ArgumentNullException(nameof(inScopeVariableNames)));
            this.assemblyReferences = new HashSet<AssemblyReference>(assemblyReferences ?? throw new ArgumentNullException(nameof(assemblyReferences)));
            this.usingNamespaces = new HashSet<UsingNamespace>(usingNamespaces ?? throw new ArgumentNullException(nameof(usingNamespaces)));
        }
    }
}