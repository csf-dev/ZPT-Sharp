using System;

namespace ZptSharp.Expressions.CSharpExpressions
{
    /// <summary>
    /// Represents a request for C# expressions to reference an assembly by name.
    /// </summary>
    public sealed class AssemblyReference : IEquatable<AssemblyReference>
    {
        /// <summary>
        /// Gets the assembly name to reference.
        /// </summary>
        /// <value>The assembly name</value>
        public string Name { get; }

        /// <summary>
        /// Gets a value which indicates if the current instance is equal to the specified <see cref="AssemblyReference" />.
        /// </summary>
        /// <param name="other">A assembly reference.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(AssemblyReference other)
        {
            if(ReferenceEquals(other, this)) return true;
            if(other is null) return false;
            return String.Equals(Name, other.Name, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Gets a value which indicates if the current instance is equal to the specified <see cref="object" />.
        /// </summary>
        /// <param name="obj">An object.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj) => Equals(obj as AssemblyReference);
        
        /// <summary>
        /// Gets a hash code for the current instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode() => Name.GetHashCode();
        
        /// <summary>
        /// Initializes a new instance of <see cref="AssemblyReference" />.
        /// </summary>
        /// <param name="name">The assembly name.</param>
        public AssemblyReference(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}