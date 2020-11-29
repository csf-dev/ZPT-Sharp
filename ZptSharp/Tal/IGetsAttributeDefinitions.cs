using System.Collections.Generic;
using ZptSharp.Dom;

namespace ZptSharp.Tal
{
    /// <summary>
    /// An object which can provide a collection of <see cref="AttributeDefinition"/> from a string
    /// value.  That string is expected to be in the format of a TAL 'attributes' attribute value.
    /// </summary>
    public interface IGetsAttributeDefinitions
    {
        /// <summary>
        /// Gets the attribute definitions from the specified <paramref name="attributesAttributeValue"/>.
        /// </summary>
        /// <returns>The attribute definitions.</returns>
        /// <param name="attributesAttributeValue">The TAL 'attributes' attribute value.</param>
        /// <param name="element">The element node upon which the attributes are defined.</param>
        IEnumerable<AttributeDefinition> GetDefinitions(string attributesAttributeValue, INode element);
    }
}
