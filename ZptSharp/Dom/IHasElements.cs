using System.Collections.Generic;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An object which has may contain child instances of <see cref="INode"/>.
    /// </summary>
    public interface IHasElements
    {
        /// <summary>
        /// Gets the child elements.
        /// </summary>
        /// <returns>The child elements.</returns>
        IEnumerable<INode> GetChildElements();
    }
}
