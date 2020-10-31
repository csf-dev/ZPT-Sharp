using System;
namespace ZptSharp.Dom
{
    /// <summary>
    /// A specification for an attribute.  This may be used to find matching attributes.
    /// </summary>
    public sealed class AttributeSpec : IEquatable<AttributeSpec>
    {
        /// <summary>
        /// Gets the attribute name, without namespace qualifiers.  This is AKA the "local name".
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the attribute namespace.
        /// </summary>
        /// <value>The namespace.</value>
        public Namespace Namespace { get; }

        /// <summary>
        /// Determines whether the specified <see cref="AttributeSpec"/> is equal to the current <see cref="AttributeSpec"/>.
        /// </summary>
        /// <param name="other">The <see cref="AttributeSpec"/> to compare with the current <see cref="AttributeSpec"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="AttributeSpec"/> is equal to the current
        /// <see cref="AttributeSpec"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(AttributeSpec other) => String.Equals(Name, other?.Name) && Namespace == other?.Namespace;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeSpec"/> class.
        /// </summary>
        /// <param name="name">Attribute name.</param>
        /// <param name="namespace">Attribute namespace.</param>
        public AttributeSpec(string name, Namespace @namespace)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Namespace = @namespace ?? throw new ArgumentNullException(nameof(@namespace));
        }
    }
}
