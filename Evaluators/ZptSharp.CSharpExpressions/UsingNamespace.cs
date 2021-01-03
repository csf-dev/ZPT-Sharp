using System;

namespace ZptSharp.Expressions.CSharpExpressions
{
    /// <summary>
    /// Represents a request for C# expressions to be evaluated in the context of the specified namespace,
    /// as if it were specified with a <c>using</c> keyword.
    /// </summary>
    public sealed class UsingNamespace : IEquatable<UsingNamespace>
    {
        /// <summary>
        /// Gets the namespace to include.
        /// </summary>
        /// <value>The namespace</value>
        public string Namespace { get; }

        /// <summary>
        /// Gets a value which indicates if the current instance is equal to the specified <see cref="UsingNamespace" />.
        /// </summary>
        /// <param name="other">A namespace.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(UsingNamespace other)
        {
            if(ReferenceEquals(other, this)) return true;
            if(other is null) return false;
            return String.Equals(Namespace, other.Namespace, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Gets a value which indicates if the current instance is equal to the specified <see cref="object" />.
        /// </summary>
        /// <param name="obj">An object.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj) => Equals(obj as UsingNamespace);
        
        /// <summary>
        /// Gets a hash code for the current instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode() => Namespace.GetHashCode();
        
        /// <summary>
        /// Initializes a new instance of <see cref="UsingNamespace" />.
        /// </summary>
        /// <param name="namespace">The namespace.</param>
        public UsingNamespace(string @namespace)
        {
            Namespace = @namespace ?? throw new ArgumentNullException(nameof(@namespace));
        }
    }
}