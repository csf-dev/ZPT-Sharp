using System.Collections.Generic;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Replaces the specified node with a collection of replacement nodes.
    /// </summary>
    public interface IReplacesNode
    {
        /// <summary>
        /// <para>
        /// Replace the specified node with a collection of replacements.
        /// </para>
        /// <para>
        /// Note that this means that the current node will be detached/removed from its parent as a side-effect.
        /// Further DOM manipulation should occur using the replacement nodes and not the replaced node.
        /// </para>
        /// </summary>
        /// <param name="toReplace">The node to replace.</param>
        /// <param name="replacements">The replacements.</param>
        void Replace(INode toReplace, IList<INode> replacements);
    }
}
