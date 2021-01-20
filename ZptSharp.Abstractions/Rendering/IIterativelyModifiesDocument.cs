using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;

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
        /// <param name="rootContext">The root expression context.</param>
        /// <param name="contextProcessor">The processor to use when processing each expression context.</param>
        /// <param name="token">A cancellation token.</param>
        Task ModifyDocumentAsync(ExpressionContext rootContext,
                                 IProcessesExpressionContext contextProcessor,
                                 CancellationToken token = default);
    }
}
