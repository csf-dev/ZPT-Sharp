namespace ZptSharp.Dom
{
    /// <summary>
    /// Abstraction for a DOM attribute.
    /// </summary>
    public interface IAttribute
    {
        /// <summary>
        /// Gets the element upon which this attribute appears.
        /// </summary>
        /// <value>The element.</value>
        INode Element { get; }

        /// <summary>
        /// Gets the attribute name, including any relevant prefix.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Gets the attribute value.
        /// </summary>
        /// <value>The value.</value>
        string Value { get; }

        /// <summary>
        /// Gets a value indicating whether or not the current instance matches a specified attribute.
        /// </summary>
        /// <returns><see langword="true"/> if the current instance matches the specified attribute; <see langword="false"/> if it does not.</returns>
        /// <param name="spec">The spec to match against.</param>
        bool Matches(AttributeSpec spec);

        /// <summary>
        /// Gets a value indicating whether or not the current instance is in the specified namespace.
        /// </summary>
        /// <returns><c>true</c>, if the attribute is in the specified namespace, <c>false</c> otherwise.</returns>
        /// <param name="namespace">The namespace.</param>
        bool IsInNamespace(Namespace @namespace);
    }
}
