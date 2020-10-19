using System;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// A service which processes a single expression context and applies some processing rules.
    /// </summary>
    public interface IProcessesExpressionContext
    {
        /// <summary>
        /// Processes the context using the rules defined within this object.
        /// </summary>
        /// <returns>A result object indicating the outcome of processing.</returns>
        /// <param name="context">The context to process.</param>
        Task<ExpressionContextProcessingResult> ProcessContextAsync(ExpressionContext context);
    }
}
