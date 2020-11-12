using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// Implementation of <see cref="IGetsZptElementAndAttributeRemovalContextProcessor"/> which returns
    /// a context processor suitable for performing cleanup on a rendered document.
    /// </summary>
    public class ZptCleanupContextProcessorFactory : IGetsZptElementAndAttributeRemovalContextProcessor
    {
        /// <summary>
        /// Gets the source-annotation context processor.
        /// </summary>
        /// <returns>The source-annotation context processor.</returns>
        public IProcessesExpressionContext GetElementAndAttributeRemovalProcessor() => new NoOpZptCleanupContextProcessor();

        class NoOpZptCleanupContextProcessor : IProcessesExpressionContext
        {
            /// <summary>
            /// Processes the context using the rules defined within this object.
            /// </summary>
            /// <returns>A result object indicating the outcome of processing.</returns>
            /// <param name="context">The context to process.</param>
            /// <param name="token">An optional cancellation token.</param>
            public Task<ExpressionContextProcessingResult> ProcessContextAsync(ExpressionContext context, CancellationToken token = default)
            {
                return Task.FromResult(new ExpressionContextProcessingResult());
            }
        }
    }
}
