using System;

namespace ZptSharp.Tal
{
    /// <summary>
    /// Model type representing a single TAL variable definition.
    /// </summary>
    public sealed class VariableDefinition : IEquatable<VariableDefinition>
    {
        const string globalScope = "global", localScope = "local";

        /// <summary>
        /// Gets or sets the scope of definition.  Expected to be one of <c>local</c> or <c>global</c> or <see langword="null"/>.
        /// Any value apart from <c>global</c> will be interpreted as local.
        /// </summary>
        /// <value>The variable scope.</value>
        public string Scope { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="VariableDefinition"/> is a global definition.
        /// If <see langword="false"/> then it is a local definition.
        /// </summary>
        /// <value><c>true</c> if is global; otherwise, <c>false</c>.</value>
        public bool IsGlobal => Scope == globalScope;

        /// <summary>
        /// Gets or sets the name of the variable.
        /// </summary>
        /// <value>The name of the variable.</value>
        public string VariableName { get; set; }

        /// <summary>
        /// Gets or sets the value expression.
        /// </summary>
        /// <value>The expression.</value>
        public string Expression { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="VariableDefinition"/> is equal to the current <see cref="VariableDefinition"/>.
        /// </summary>
        /// <param name="other">The <see cref="VariableDefinition"/> to compare with the current <see cref="VariableDefinition"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="VariableDefinition"/> is equal to the current
        /// <see cref="VariableDefinition"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(VariableDefinition other)
        {
            if (other is null) return false;

            return String.Equals(VariableName, other.VariableName, StringComparison.InvariantCulture)
                && String.Equals(Expression, other.Expression, StringComparison.InvariantCulture)
                && IsGlobal == other.IsGlobal;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="VariableDefinition"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="VariableDefinition"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="VariableDefinition"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
            => Equals(obj as VariableDefinition);

        /// <summary>
        /// Returns a static value as the hash code for a <see cref="VariableDefinition"/>.
        /// Variable definitions are NOT suitable for use as keys to a hash table.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode() => 0;

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="VariableDefinition"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current <see cref="VariableDefinition"/>.</returns>
        public override string ToString()
        {
            var scope = IsGlobal ? globalScope : localScope;
            return $"{scope} {VariableName} {Expression}";
        }

        /// <summary>
        /// Gets the string which represents a globally-scoped definition.
        /// </summary>
        /// <value>The global scope name.</value>
        public static string GlobalScope => globalScope;

        /// <summary>
        /// Gets the string which represents a locally-scoped definition.
        /// </summary>
        /// <value>The global scope name.</value>
        public static string LocalScope => localScope;
    }
}
