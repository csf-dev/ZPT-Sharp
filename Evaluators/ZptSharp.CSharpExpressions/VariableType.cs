using System;

namespace ZptSharp.Expressions.CSharpExpressions
{
    /// <summary>
    /// Describes the type of a variable.  When an object of this type is in-scope,
    /// it forces the <see cref="ExpressionCompiler" /> to treat the object as that
    /// specific type and not as <c>dynamic</c>.  This is particularly important if
    /// you wish to use extension methods with that variable, because extension methods
    /// cannot be bound to dynamically-typed objects.
    /// </summary>
    public sealed class VariableType : IEquatable<VariableType>
    {
        /// <summary>
        /// Gets the name of the variable which is to be treated as a specific type.
        /// </summary>
        /// <value>The variable name</value>
        public string VariableName { get; }
        
        /// <summary>
        /// Gets the type for the variable.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; }
        
        /// <summary>
        /// Gets a value which indicates if the current instance is equal to the specified <see cref="VariableType" />.
        /// </summary>
        /// <param name="other">A C# variable-type designation.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(VariableType other)
        {
            if(ReferenceEquals(other, this)) return true;
            if(other is null) return false;
            return String.Equals(VariableName, other.VariableName, StringComparison.InvariantCulture)
                && String.Equals(Type, other.Type, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Gets a value which indicates if the current instance is equal to the specified <see cref="object" />.
        /// </summary>
        /// <param name="obj">An object.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj) => Equals(obj as VariableType);
        
        /// <summary>
        /// Gets a hash code for the current instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
            => VariableName.GetHashCode() ^ Type.GetHashCode();

        /// <summary>
        /// Initializes a new instance of <see cref="VariableType" />.
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="type"></param>
        public VariableType(string variableName, string type)
        {
            VariableName = variableName ?? throw new System.ArgumentNullException(nameof(variableName));
            Type = type ?? throw new System.ArgumentNullException(nameof(type));
        }
    }
}