using System;
using System.Collections.Generic;
using System.Linq;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Extension methods for <see cref="IReplacesNode"/>.
    /// </summary>
    public static class NodeReplacerExtensions
    {
        /// <summary>
        /// Replace the specified node with the specified replacement.
        /// </summary>
        /// <param name="replacer">The node replacer.</param>
        /// <param name="toReplace">The node to replace.</param>
        /// <param name="replacement">The replacement node.</param>
        public static void Replace(this IReplacesNode replacer, INode toReplace, INode replacement)
        {
            if (replacer == null)
                throw new ArgumentNullException(nameof(replacer));

            replacer.Replace(toReplace, new[] { replacement });
        }

        /// <summary>
        /// <para>
        /// Replace the specified node with a collection of replacements.
        /// </para>
        /// <para>
        /// Note that this means that the current node will be detached/removed from its parent as a side-effect.
        /// Further DOM manipulation should occur using the replacement nodes and not the replaced node.
        /// </para>
        /// </summary>
        /// <param name="replacer">The node replacer.</param>
        /// <param name="toReplace">The node to replace.</param>
        /// <param name="replacements">The replacement nodes.</param>
        public static void Replace(this IReplacesNode replacer, INode toReplace, IReadOnlyList<INode> replacements)
            => replacer.Replace(toReplace, replacements.ToList());
    }
}
