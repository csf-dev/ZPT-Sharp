using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;
using ZptSharp.Rendering;

namespace ZptSharp.Tal
{
    /// <summary>
    /// A <see cref="IProcessesExpressionContext"/> which performs no TAL-related activity.
    /// This is used at the centre of the decorator stack.  If no particular TAL attributes are
    /// found then an empty processing result is returned.
    /// </summary>
    public class NoOpTalContextProcessor : IHandlesProcessingError
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

        Task<ErrorHandlingResult> IHandlesProcessingError.HandleErrorAsync(Exception ex, ExpressionContext context, CancellationToken token)
            => Task.FromResult(ErrorHandlingResult.Failure);
    }
}
