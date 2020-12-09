using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// A no-op implementation of <see cref="IModifiesDocument"/> makes no change to the document.
    /// </summary>
    public class NoOpDocumentModifier : IModifiesDocument
    {
        /// <summary>
        /// Performs alterations for a specified document, using the specified rendering request.
        /// This method will manipulate the <paramref name="document"/>, according to the rules of ZPT.
        /// </summary>
        /// <returns>A task indicating when the process is complete.</returns>
        /// <param name="document">The document to render.</param>
        /// <param name="request">The rendering request.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        public Task ModifyDocumentAsync(IDocument document, RenderZptDocumentRequest request, CancellationToken token = default)
            => Task.CompletedTask;
    }
}
