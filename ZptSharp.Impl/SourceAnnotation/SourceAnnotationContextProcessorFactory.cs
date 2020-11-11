using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;
using ZptSharp.Rendering;

namespace ZptSharp.SourceAnnotation
{
    /// <summary>
    /// Implementation of <see cref="IGetsSourceAnnotationContextProcessor"/> which returns
    /// a context processor suitable for adding source annotation to a rendered document.
    /// </summary>
    public class SourceAnnotationContextProcessorFactory : IGetsSourceAnnotationContextProcessor
    {
        /// <summary>
        /// Gets the source-annotation context processor.
        /// </summary>
        /// <returns>The source-annotation context processor.</returns>
        public IProcessesExpressionContext GetSourceAnnotationContextProcessor() => new NoOpSourceAnnotationContextProcessor();

        class NoOpSourceAnnotationContextProcessor : IProcessesExpressionContext
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
