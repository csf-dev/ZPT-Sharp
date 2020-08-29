using System;
namespace ZptSharp.Dom
{
    /// <summary>
    /// Abstraction for a DOM attribute.
    /// </summary>
    public interface IAttribute
    {
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
        /// Gets a value indicating whether or not the current instance matches a specified attribute name and namespace.
        /// </summary>
        /// <returns><see langword="true"/> if the current instance matches the specified name and namespace; <see langword="false"/> if it does not.</returns>
        /// <param name="attributeName">The name to match against.</param>
        /// <param name="namespace">The namespace to match against.</param>
        bool Matches(string attributeName, Namespace @namespace);
    }
}
