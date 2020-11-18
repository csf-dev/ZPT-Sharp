using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;
using ZptSharp.Rendering;

namespace ZptSharp.Tal
{
    /// <summary>
    /// Implementation of <see cref="IGetsTalContextProcessor"/> which returns
    /// a context processor suitable for processing the TAL model-binding syntax.
    /// </summary>
    public class TalContextProcessorFactory : IGetsTalContextProcessor
    {
        /// <summary>
        /// Gets the TAL context processor.
        /// </summary>
        /// <returns>The TAL context processor.</returns>
        public IProcessesExpressionContext GetTalContextProcessor() => new NoOpTalContextProcessor();

        class NoOpTalContextProcessor : IProcessesExpressionContext
        {
            /// <summary>
            /// Processes the context using the rules defined within this object.
            /// </summary>
            /// <returns>A result object indicating the outcome of processing.</returns>
            /// <param name="context">The context to process.</param>
            /// <param name="token">An optional cancellation token.</param>
            public Task<ExpressionContextProcessingResult> ProcessContextAsync(ExpressionContext context, CancellationToken token = default)
            {
                return Task.FromResult(ExpressionContextProcessingResult.Noop);
            }
        }
    }
}
