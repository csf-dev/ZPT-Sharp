using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions.LoadExpressions
{
    /// <summary>
    /// An implementation of <see cref="IEvaluatesExpression"/> which evaluates TALES 'load' expressions.
    /// </summary>
    public class LoadExpressionEvaluator : IEvaluatesExpression
    {
        readonly IEvaluatesExpression evaluator;
        readonly IRendersLoadedObject objectRenderer;

        /// <summary>
        /// Evaluates the expression asynchronously and returns the result.
        /// </summary>
        /// <returns>The expression result.</returns>
        /// <param name="expression">The expression string.</param>
        /// <param name="context">The expression context.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<object> EvaluateExpressionAsync(string expression, ExpressionContext context, CancellationToken cancellationToken = default)
        {
            if (expression is null)
                throw new System.ArgumentNullException(nameof(expression));
            if (context is null)
                throw new System.ArgumentNullException(nameof(context));

            return EvaluateExpressionPrivateAsync(expression, context, cancellationToken);
        }

        async Task<object> EvaluateExpressionPrivateAsync(string expression, ExpressionContext context, CancellationToken cancellationToken)
        {
            var innerExpressionResult = await evaluator.EvaluateExpressionAsync(expression, context, cancellationToken);
            return await objectRenderer.RenderObjectAsync(innerExpressionResult, context, cancellationToken);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="LoadExpressionEvaluator"/>.
        /// </summary>
        /// <param name="evaluator">An evaluator used to evaluate the inner expression.</param>
        /// <param name="objectRenderer">A service to render the result of the inner expression.</param>
        public LoadExpressionEvaluator(IEvaluatesExpression evaluator, IRendersLoadedObject objectRenderer)
        {
            this.objectRenderer = objectRenderer ?? throw new System.ArgumentNullException(nameof(objectRenderer));
            this.evaluator = evaluator ?? throw new System.ArgumentNullException(nameof(evaluator));
        }
    }
}