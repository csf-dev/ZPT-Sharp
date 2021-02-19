using System.Collections.Generic;
using System.Linq;
using ZptSharp.Dom;

namespace ZptSharp.Tal
{
    /// <summary>
    /// Represents the result retrieved from <see cref="IEvaluatesDomValueExpression"/>.
    /// </summary>
    public class DomValueExpressionResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DomValueExpressionResult"/> aborts the action.
        /// See also: <seealso cref="Expressions.AbortZptActionToken"/> &amp;
        /// <seealso cref="IInterpretsExpressionResult.DoesResultAbortTheAction(object)"/>.
        /// </summary>
        /// <value><c>true</c> if abort action; otherwise, <c>false</c>.</value>
        public bool AbortAction { get; set; }

        /// <summary>
        /// Gets the nodes resulting from the expression evaluation.
        /// </summary>
        /// <value>The nodes.</value>
        public IReadOnlyList<INode> Nodes { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomValueExpressionResult"/> class.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <param name="abortAction">If set to <c>true</c> then this resul aborts the action.</param>
        public DomValueExpressionResult(IList<INode> nodes = null, bool abortAction = false)
        {
            Nodes = (nodes ?? Enumerable.Empty<INode>()).ToArray();
            AbortAction = abortAction;
        }
    }
}
