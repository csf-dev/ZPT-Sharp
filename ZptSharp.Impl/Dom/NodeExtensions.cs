using System;
using System.Collections.Generic;
using System.Linq;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Extension methods for <see cref="INode"/>.
    /// </summary>
    public static class NodeExtensions
    {
        /// <summary>
        /// Gets the first <see cref="IAttribute"/> from the node which matches a <paramref name="spec"/>.
        /// </summary>
        /// <returns>The matching attribute, or a <see langword="null"/> reference if there is no match.</returns>
        /// <param name="node">The node from which to get the attribute.</param>
        /// <param name="spec">An attribute specification.</param>
        public static IAttribute GetMatchingAttribute(this INode node, AttributeSpec spec)
            => GetMatchingAttribute(node, new[] { spec }, out _);

        /// <summary>
        /// Gets the first <see cref="IAttribute"/> from the node which matches any of the specified a <paramref name="specs"/>.
        /// </summary>
        /// <returns>The matching attribute, or a <see langword="null"/> reference if there is no match.</returns>
        /// <param name="node">The node from which to get the attribute.</param>
        /// <param name="specs">A collection of attribute specification.</param>
        /// <param name="matchingSpec">Exposes the specification (if any) which was matched.</param>
        public static IAttribute GetMatchingAttribute(this INode node, IEnumerable<AttributeSpec> specs, out AttributeSpec matchingSpec)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            matchingSpec = null;
            if (specs == null) return null;

            foreach(var spec in specs)
            {
                if (spec == null) continue;
                var attribute = node.Attributes.FirstOrDefault(x => x.Matches(spec));
                if (attribute != null)
                {
                    matchingSpec = spec;
                    return attribute;
                }
            }

            return null;
        }

        /// <summary>
        /// <para>
        /// Adds the specified <paramref name="nodesToAdd"/> to the specified <paramref name="node"/>.
        /// </para>
        /// <para>
        /// By default, the nodes to add are appended to the end of the node's children.
        /// In an <paramref name="insertIndex"/> is specified, though, the nodes to add are inserted into
        /// node's children at the specified (zero-based) index position.
        /// </para>
        /// </summary>
        /// <param name="node">The node to which the new children should be added.</param>
        /// <param name="nodesToAdd">Nodes to add.</param>
        /// <param name="insertIndex">If specified and non-null then the nodes to add are inserted into the
        /// node's children at this index.  Otherwise the nodes to add are appended to the end of
        /// node's children.</param>
        public static void AddChildren(this INode node, IEnumerable<INode> nodesToAdd, int? insertIndex = null)
            => AddChildren(node, nodesToAdd.ToList(), insertIndex);

        /// <summary>
        /// <para>
        /// Adds the specified <paramref name="nodesToAdd"/> to the specified <paramref name="node"/>.
        /// </para>
        /// <para>
        /// By default, the nodes to add are appended to the end of the node's children.
        /// In an <paramref name="insertIndex"/> is specified, though, the nodes to add are inserted into
        /// node's children at the specified (zero-based) index position.
        /// </para>
        /// </summary>
        /// <param name="node">The node to which the new children should be added.</param>
        /// <param name="nodesToAdd">Nodes to add.</param>
        /// <param name="insertIndex">If specified and non-null then the nodes to add are inserted into the
        /// node's children at this index.  Otherwise the nodes to add are appended to the end of
        /// node's children.</param>
        public static void AddChildren(this INode node, IList<INode> nodesToAdd, int? insertIndex = null)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (nodesToAdd == null)
                throw new ArgumentNullException(nameof(nodesToAdd));

            var targetIndex = insertIndex ?? node.ChildNodes.Count;

            // By inserting in reverse order, using the same index, the nodes to add
            // will remain in their original order.
            var reversedNodesToAdd = GetReversedCollectionOfNodes(nodesToAdd);
            foreach (var node in reversedNodesToAdd)
                node.ChildNodes.Insert(targetIndex, node);
        }

        static IList<INode> GetReversedCollectionOfNodes(IList<INode> nodes)
        {
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));

            var list = new List<INode>(nodes);
            list.Reverse();
            return list;
        }

        /// <summary>
        /// Removes the specified <paramref name="node"/> from its parent.
        /// From this point onwards, the node should be discarded, as it
        /// is no longer a valid part of the document.
        /// </summary>
        /// <param name="node">The node to remove.</param>
        public static void Remove(this INode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (node.ParentNode == null)
                throw new ArgumentException(Resources.ExceptionMessage.NodeMustHaveAParent, nameof(node));

            node.ParentNode.ChildNodes.Remove(node);
        }
    }
}
