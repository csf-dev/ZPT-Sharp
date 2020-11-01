using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// A service which evaluates and returns the result of a TALES expression.
    /// </summary>
    public interface IEvaluatesExpression
    {
        /// <summary>
        /// Evaluates the expression asynchronously and returns the result.
        /// </summary>
        /// <returns>The expression result.</returns>
        /// <param name="expression">The expression string.</param>
        /// <param name="context">The expression context.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        Task<object> EvaluateExpressionAsync(string expression,
                                             ExpressionContext context,
                                             CancellationToken cancellationToken = default);
    }
}
