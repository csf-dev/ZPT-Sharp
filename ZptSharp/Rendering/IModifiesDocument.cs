using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// A service which coordinates and performs the modifications to a document before
    /// it is ready to be output.
    /// </summary>
    public interface IModifiesDocument
    {
        /// <summary>
        /// Performs alterations for a specified document, using the specified rendering request.
        /// This method will manipulate the <paramref name="document"/>, according to the rules of ZPT.
        /// </summary>
        /// <returns>A task indicating when the process is complete.</returns>
        /// <param name="document">The document to render.</param>
        /// <param name="model">The model to render.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        Task ModifyDocumentAsync(IDocument document, object model, CancellationToken token = default);
    }
}
