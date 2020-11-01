using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// Extension methods used with an <see cref="IEvaluatesExpression"/>.
    /// </summary>
    public static class ExpressionEvaluatorExtensions
    {
        /// <summary>
        /// Evaluates the expression asynchronously and returns the result, with the
        /// returned value cast to a specified type.
        /// </summary>
        /// <returns>The expression result.</returns>
        /// <param name="evaluator">The expression evaluator.</param>
        /// <param name="expression">The expression string.</param>
        /// <param name="context">The expression context.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        /// <typeparam name="T">The desired/expected type of the result object.</typeparam>
        public static async Task<T> EvaluateExpressionAsync<T>(this IEvaluatesExpression evaluator,
                                                               string expression,
                                                               ExpressionContext context,
                                                               CancellationToken cancellationToken = default)
        {
            if (evaluator == null) throw new ArgumentNullException(nameof(evaluator));

            var result = await evaluator.EvaluateExpressionAsync(expression, context, cancellationToken);

            try
            {
                return (T)result;
            }
            catch(InvalidCastException ex)
            {
                var message = String.Format(Resources.ExceptionMessage.CannotConvertEvaluatedResult,
                                            nameof(EvaluateExpressionAsync),
                                            expression,
                                            typeof(T).FullName,
                                            result?.GetType().FullName ?? "<null>");
                throw new EvaluationException(message, ex);
            }
        }
    }
}
