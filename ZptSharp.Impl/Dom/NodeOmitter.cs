using System;
using System.Collections.Generic;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Implementation of <see cref="IOmitsNode"/> which makes use of an
    /// <see cref="IReplacesNode"/> to replace a node with its children.
    /// </summary>
    public class NodeOmitter : IOmitsNode
    {
        readonly IReplacesNode replacer;

        /// <summary>
        /// Omits the specified element node from its DOM.  The node's parent and children will
        /// still be a part of the DOM, but the element node itself will be removed.
        /// </summary>
        /// <param name="node">The element node to omit.</param>
        public void Omit(INode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var children = new List<INode>(node.ChildNodes);
            replacer.Replace(node, children);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeOmitter"/> class.
        /// </summary>
        /// <param name="replacer">Node replacer.</param>
        public NodeOmitter(IReplacesNode replacer)
        {
            this.replacer = replacer ?? throw new ArgumentNullException(nameof(replacer));
        }
    }
}
