using System;
using System.Collections.Generic;
using System.Linq;
using ZptSharp.Dom;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// Extension methods for <see cref="ExpressionContext"/>.
    /// </summary>
    public static class ExpressionContextExtensions
    {
        /// <summary>
        /// Creates a collection of child contexts, from the specified <paramref name="nodes"/>.
        /// See also: <seealso cref="ExpressionContext.CreateChild(INode)"/>.
        /// </summary>
        /// <returns>The child contexts.</returns>
        /// <param name="context">Context.</param>
        /// <param name="nodes">Nodes.</param>
        public static IList<ExpressionContext> CreateChildren(this ExpressionContext context, IList<INode> nodes)
            => CreateChildren(context, (IReadOnlyList<INode>) nodes?.ToArray());

        /// <summary>
        /// Creates a collection of child contexts, from the specified <paramref name="nodes"/>.
        /// See also: <seealso cref="ExpressionContext.CreateChild(INode)"/>.
        /// </summary>
        /// <returns>The child contexts.</returns>
        /// <param name="context">Context.</param>
        /// <param name="nodes">Nodes.</param>
        public static IList<ExpressionContext> CreateChildren(this ExpressionContext context, IReadOnlyList<INode> nodes)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));

            return nodes.Select(context.CreateChild).ToList();
        }
    }
}
