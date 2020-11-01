using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.Metal
{
    /// <summary>
    /// An object which expands a <see cref="MetalMacro"/>.  Expansion is the process of filling any slots
    /// and applying macro extension (where applicable).
    /// </summary>
    public interface IExpandsMacro
    {
        /// <summary>
        /// Expands the specified macro and returns the result.
        /// </summary>
        /// <returns>The expanded macro.</returns>
        /// <param name="macro">The macro to expand.</param>
        /// <param name="context">The current expression context.</param>
        /// <param name="token">An optional cancellation token.</param>
        Task<MetalMacro> ExpandMacroAsync(MetalMacro macro,
                                          ExpressionContext context,
                                          CancellationToken token = default);
    }
}
