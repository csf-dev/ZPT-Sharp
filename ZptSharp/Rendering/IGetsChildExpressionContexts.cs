using System;
using System.Collections.Generic;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// A service which gets a collection of child expression contexts from a specified context.
    /// </summary>
    public interface IGetsChildExpressionContexts
    {
        /// <summary>
        /// Gets the child expression contexts.
        /// </summary>
        /// <returns>The expression contexts.</returns>
        /// <param name="context">The context from which to get children.</param>
        IEnumerable<ExpressionContext> GetChildContexts(ExpressionContext context);
    }
}
