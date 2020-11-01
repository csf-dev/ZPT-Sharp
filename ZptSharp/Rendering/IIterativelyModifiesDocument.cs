using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// An object which uses an expression-context iterator to iteratively make
    /// alterations to the document, on a context-by-context basis.
    /// </summary>
    public interface IIterativelyModifiesDocument
    {
        /// <summary>
        /// Modifies the document using the specified context processor.
        /// </summary>
        /// <param name="document">The document to modify.</param>
        /// <param name="request">The rendering request.</param>
        /// <param name="contextProcessor">The processor to use when processing each expression context.</param>
        /// <param name="token">A cancellation token.</param>
        Task ModifyDocumentAsync(IDocument document,
                                 RenderZptDocumentRequest request,
                                 IProcessesExpressionContext contextProcessor,
                                 CancellationToken token = default);
    }
}
