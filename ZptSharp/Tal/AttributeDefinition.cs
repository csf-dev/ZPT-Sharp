using System;
namespace ZptSharp.Tal
{
    /// <summary>
    /// Model for an attribute definition, as would be used by a TAL 'attributes' attribute.
    /// </summary>
    public sealed class AttributeDefinition : IEquatable<AttributeDefinition>
    {
        /// <summary>
        /// Gets or sets the attribute prefix.
        /// </summary>
        /// <value>The prefix.</value>
        public string Prefix { get; set; }

        /// <summary>
        /// Gets or sets the attribute name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the expression for the attribute value.
        /// </summary>
        /// <value>The expression.</value>
        public string Expression { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="ZptSharp.Tal.AttributeDefinition"/> is equal to the current <see cref="T:ZptSharp.Tal.AttributeDefinition"/>.
        /// </summary>
        /// <param name="other">The <see cref="ZptSharp.Tal.AttributeDefinition"/> to compare with the current <see cref="T:ZptSharp.Tal.AttributeDefinition"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="ZptSharp.Tal.AttributeDefinition"/> is equal to the current
        /// <see cref="T:ZptSharp.Tal.AttributeDefinition"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(AttributeDefinition other)
        {
            if (ReferenceEquals(other, this)) return true;
            if (ReferenceEquals(other, null)) return false;

            return (Prefix == other.Prefix
                 && Name == other.Name
                 && Expression == other.Expression);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="T:ZptSharp.Tal.AttributeDefinition"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="T:ZptSharp.Tal.AttributeDefinition"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="T:ZptSharp.Tal.AttributeDefinition"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => Equals(obj as AttributeDefinition);

        /// <summary>
        /// Serves as a hash function for a <see cref="AttributeDefinition"/> object.
        /// Intances of that class ARE NOT suitable for storing in hash-tables.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode() => 0;

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:ZptSharp.Tal.AttributeDefinition"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:ZptSharp.Tal.AttributeDefinition"/>.</returns>
        public override string ToString()
        {
            var output = $"{Name} {Expression}";

            return (Prefix != null) ? $"{Prefix}:{output}" : output;
        }
    }
}
