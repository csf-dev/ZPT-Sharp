using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.Tal
{
    /// <summary>
    /// An object which evaluates an expression and returns a value which may be used by the DOM.
    /// </summary>
    public interface IEvaluatesDomValueExpression
    {
        /// <summary>
        /// Evaluates the expression asynchronously and returns the result.
        /// </summary>
        /// <returns>The expression result.</returns>
        /// <param name="expression">An expression which might be prefixed to indicate that it is to be treated as structure.</param>
        /// <param name="context">Context.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<DomValueExpressionResult> EvaluateExpressionAsync(string expression,
                                                               ExpressionContext context,
                                                               CancellationToken cancellationToken = default);
    }
}
