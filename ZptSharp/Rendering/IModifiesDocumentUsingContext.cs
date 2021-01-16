using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// A service which coordinates and performs the modifications to a document before
    /// it is ready to be output, using an expression context rather than just a model.
    /// </summary>
    /// <seealso cref="IModifiesDocument"/>
    public interface IModifiesDocumentUsingContext
    {
        /// <summary>
        /// Performs alterations for a specified document, using the specified expression context.
        /// This method will manipulate the <paramref name="context"/>, according to the rules of ZPT.
        /// </summary>
        /// <returns>A task indicating when the process is complete.</returns>
        /// <param name="context">The expression context to use in the operation.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        Task ModifyDocumentAsync(ExpressionContext context, CancellationToken token = default);
    }
}