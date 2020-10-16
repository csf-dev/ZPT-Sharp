using System;
namespace ZptSharp.Expressions
{
    /// <summary>
    /// An object which provides its own logic for getting named values when resolving a TALES expression.
    /// </summary>
    /// <remarks>
    /// <para>
    /// General objects are traversed using common rules defined within ZPT.  However, objects implementing this
    /// interface declare that they provide their own specific logic for traversal.  The <see cref="GetValue(string)"/>
    /// string method will be used instead of the usual traversal rules in order to get the value of a named reference,
    /// relative to the current object.
    /// </para>
    /// </remarks>
    public interface IGetsNamedTalesValue
    {
        /// <summary>
        /// Attempts to get a value for a named reference, relative to the current instance.
        /// </summary>
        /// <returns>A boolean indicating whether a value was successfully retrieved or not.</returns>
        /// <param name="name">The name of the value to retrieve.</param>
        /// <param name="value">Exposes the retrieved value if this method returns success.</param>
        bool TryGetValue(string name, out object value);
    }
}
