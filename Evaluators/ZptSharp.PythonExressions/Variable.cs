using System;
namespace ZptSharp.Expressions.PythonExpressions
{
    /// <summary>
    /// Represents a variable to be used in an IronPython expression.
    /// </summary>
    public sealed class Variable : IEquatable<Variable>
    {
        /// <summary>
        /// Gets or sets the variable name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the variable value.
        /// </summary>
        /// <value>The value.</value>
        public object Value { get; }

        /// <summary>
        /// Determines whether the specified <see cref="Variable"/> is equal to
        /// the current <see cref="Variable"/>.
        /// </summary>
        /// <param name="other">The <see cref="Variable"/> to compare with the current <see cref="Variable"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Variable"/> is equal to the
        /// current <see cref="Variable"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Variable other)
        {
            if (other == null) return false;
            if (ReferenceEquals(other, this)) return true;
            return String.Equals(Name, other.Name, StringComparison.InvariantCulture)
                && Equals(Value, other.Value);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Variable"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Variable"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Variable"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => Equals(obj as Variable);

        /// <summary>
        /// Serves as a hash function for a <see cref="Variable"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode() => Name.GetHashCode() ^ ((Value?.GetHashCode()) ?? 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="Variable"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        public Variable(string name, object value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value;
        }
    }
}
