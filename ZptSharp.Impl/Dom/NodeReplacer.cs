using System;
using System.Collections.Generic;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Implementation of <see cref="IReplacesNode"/> which replaces a node (upon
    /// its parent) using a collection of replacement nodes.
    /// </summary>
    public class NodeReplacer : IReplacesNode
    {
        /// <summary>
        /// Replace the specified node with a collection of replacements.
        /// </summary>
        /// <param name="toReplace">The node to replace.</param>
        /// <param name="replacements">The replacements.</param>
        public void Replace(INode toReplace, IList<INode> replacements)
        {
            if (toReplace == null)
                throw new ArgumentNullException(nameof(toReplace));
            if (replacements == null)
                throw new ArgumentNullException(nameof(replacements));

            var parent = toReplace.ParentElement ?? throw new ArgumentException(Resources.ExceptionMessage.MustNotBeRootElement, nameof(toReplace));
            var targetIndex = parent.ChildNodes.IndexOf(toReplace);

            foreach (var replacement in replacements)
                replacement.PreReplacementSourceInfo = toReplace.SourceInfo;

            parent.AddChildren(replacements, targetIndex);
            parent.ChildNodes.Remove(toReplace);
        }
    }
}
