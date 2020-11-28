using System;
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
        /// Gets a value indicating whether the result should be treated as DOM structure.
        /// If <see langword="true"/> then the <see cref="Nodes"/> will be interpreted and used
        /// as a series of nodes which could contain rich DOM structure such as elements, attributes etc.
        /// If <see langword="false"/> then the nodes are interpreted simply as text content.
        /// </summary>
        /// <value><c>true</c> if treat as structure; otherwise, <c>false</c>.</value>
        public bool TreatAsStructure { get; }

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
        /// <param name="treatAsStructure">If set to <c>true</c> treat as structure.</param>
        /// <param name="abortAction">If set to <c>true</c> then this resul aborts the action.</param>
        public DomValueExpressionResult(IList<INode> nodes = null, bool treatAsStructure = false, bool abortAction = false)
        {
            Nodes = (nodes ?? Enumerable.Empty<INode>()).ToArray();
            TreatAsStructure = treatAsStructure;
            AbortAction = abortAction;
        }
    }
}
