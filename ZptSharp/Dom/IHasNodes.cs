using System.Collections.Generic;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An object which has may contain child instances of <see cref="INode"/>.
    /// </summary>
    public interface IHasNodes
    {
        /// <summary>
        /// Gets the child nodes.
        /// </summary>
        /// <returns>The child nodes.</returns>
        IEnumerable<INode> GetChildNodes();
    }
}
