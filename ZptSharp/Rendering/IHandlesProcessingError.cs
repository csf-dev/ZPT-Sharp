using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// A specialisation of <see cref="IProcessesExpressionContext"/> which is able to handle errors
    /// during the rendering process.
    /// </summary>
    public interface IHandlesProcessingError : IProcessesExpressionContext
    {
        /// <summary>
        /// Handles an error which was encountered whilst processing an expression context.
        /// </summary>
        /// <returns>A result object indicating the outcome of error handling.</returns>
        /// <param name="ex">The exception indicating the error.</param>
        /// <param name="context">The context to process.</param>
        /// <param name="token">An optional cancellation token.</param>
        Task<ErrorHandlingResult> HandleErrorAsync(Exception ex, ExpressionContext context, CancellationToken token = default);
    }
}
