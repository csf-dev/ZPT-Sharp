using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// An implementation of <see cref="IEvaluatesExpression"/> which evaluates TALES 'local' expressions.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class is little more than an adapter for <see cref="IEvaluatesPathExpressionRequest"/>.
    /// This is because there are a number of different 'path expression' variants which have
    /// almost-but-not-quite identical logic.  This class builds a
    /// <see cref="PathExpressionEvaluationRequest"/> and passes it on down the stack.
    /// </para>
    /// </remarks>
    public class LocalVariablesOnlyPathExpressionEvaluator : IEvaluatesExpression
    {
        readonly IEvaluatesPathExpressionRequest requestEvaluator;

        /// <summary>
        /// Evaluates the expression asynchronously and returns the result.
        /// </summary>
        /// <returns>The expression result.</returns>
        /// <param name="expression">The expression string.</param>
        /// <param name="context">The expression context.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<object> EvaluateExpressionAsync(string expression, ExpressionContext context, CancellationToken cancellationToken = default)
        {
            var request = new PathExpressionEvaluationRequest(expression, context, RootScopeLimitation.LocalVariablesOnly);
            return requestEvaluator.EvaluateAsync(request, cancellationToken);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="LocalVariablesOnlyPathExpressionEvaluator"/> class.
        /// </summary>
        /// <param name="requestEvaluator">Request evaluator.</param>
        public LocalVariablesOnlyPathExpressionEvaluator(IEvaluatesPathExpressionRequest requestEvaluator)
        {
            this.requestEvaluator = requestEvaluator ?? throw new ArgumentNullException(nameof(requestEvaluator));
        }
    }
}
