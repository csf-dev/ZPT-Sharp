using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions.StructureExpressions
{
    /// <summary>
    /// An implementation of <see cref="IEvaluatesExpression"/> which evaluates TALES 'string' expressions.
    /// This returns all results wrapped within an instance of <see cref="StructuredMarkupObjectAdapter"/>,
    /// so that they will be treated as markup by default.
    /// </summary>
    public class StructureExpressionEvaluator : IEvaluatesExpression
    {
        readonly IEvaluatesExpression evaluator;

        /// <summary>
        /// Evaluates the expression asynchronously and returns the result.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method must decisively evaluate the expression, if an expression cannot be evaluated
        /// to a conclusive result then this method must throw <see cref="EvaluationException"/> or
        /// an exception derived from an evaluation exception.
        /// It is acceptable to return <see langword="null"/> from this method though, if that is the
        /// result of evaluating the expression.
        /// </para>
        /// <para>
        /// Whilst the API of this method is asynchronous (returning a task), it will not be suitable
        /// or appropriate for all implementations to act asynchronously.
        /// Thus, implementors are not certain to honour the <paramref name="cancellationToken"/> if
        /// it is used to cancel/abort evalaution early.
        /// Implementors are encouraged to act asynchronously when it is appropriate to do so, but may
        /// choose to act synchronously and return an already-completed task.
        /// </para>
        /// <para>
        /// Note that any expression result might also be an <see cref="AbortZptActionToken"/>,
        /// which is a special value indicating that the current ZPT operation should be aborted.
        /// </para>
        /// </remarks>
        /// <returns>The expression result.</returns>
        /// <param name="expression">The expression string.</param>
        /// <param name="context">The expression context.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        /// <exception cref="EvaluationException">Thrown if evaluating the expression fails.</exception>
        /// <exception cref="System.AggregateException">Thrown if evaluating the expression fails asynchronously.</exception>
        /// <seealso cref="EvaluationException"/>
        /// <seealso cref="AbortZptActionToken"/>
        /// <seealso cref="ExpressionContext"/>
        public async Task<object> EvaluateExpressionAsync(string expression, ExpressionContext context, CancellationToken cancellationToken = default)
        {
            var innerResult = await evaluator.EvaluateExpressionAsync(expression, context, cancellationToken)
                .ConfigureAwait(false);
            return new StructuredMarkupObjectAdapter(innerResult);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="StructureExpressionEvaluator"/>.
        /// </summary>
        /// <param name="evaluator">A wrapped expression evaluator.</param>
        public StructureExpressionEvaluator(IEvaluatesExpression evaluator)
        {
            this.evaluator = evaluator ?? throw new System.ArgumentNullException(nameof(evaluator));
        }
    }
}