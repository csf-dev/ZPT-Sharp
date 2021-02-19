using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// A service which creates the root <see cref="ExpressionContext"/> for a document and rendering request.
    /// </summary>
    public interface IGetsRootExpressionContext
    {
        /// <summary>
        /// Gets the root expression context for the request.
        /// </summary>
        /// <returns>The expression context.</returns>
        /// <param name="document">The document for which to create an expression context.</param>
        /// <param name="model">The model for which to create an expression context.</param>
        ExpressionContext GetExpressionContext(IDocument document, object model);
    }
}
