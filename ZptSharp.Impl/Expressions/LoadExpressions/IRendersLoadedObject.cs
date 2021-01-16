using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.Expressions.LoadExpressions
{
    /// <summary>
    /// An object which renders an object.  This will convert the object to a
    /// string based upon the semantics of a 'load' expression.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Assuming that the object is a 'loadable' object then it will be rendered to string
    /// as if it were a ZPT document of its own.  This means that a new expression context will be
    /// created for the loaded item.  That item will then be rendered , as if it were its own ZPT
    /// rendering request.
    /// </para>
    /// <para>
    /// The result of the rendering operation ()
    /// </para>
    /// </remarks>
    public interface IRendersLoadedObject
    {
        /// <summary>
        /// Renders the specified object asynchronously and returns the result.
        /// </summary>
        /// <param name="obj">The object to render.</param>
        /// <param name="context">The expression context.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        /// <returns>The result of the rendering operation.</returns>
        Task<string> RenderObjectAsync(object obj, ExpressionContext context, CancellationToken cancellationToken = default);
    }
}