using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ZptSharp.Expressions.NotExpressions
{
    /// <summary>
    /// An implementation of <see cref="IEvaluatesExpression"/> which evaluates TALES 'nots' expressions.
    /// </summary>
    public class NotExpressionEvaluator : IEvaluatesExpression
    {
        readonly ICoercesValueToBoolean valueConverter;
        readonly IEvaluatesExpression evaluator;
        readonly ILogger logger;

        /// <summary>
        /// Evaluates the expression asynchronously and returns the result.
        /// </summary>
        /// <returns>The expression result.</returns>
        /// <param name="expression">The expression string.</param>
        /// <param name="context">The expression context.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public async Task<object> EvaluateExpressionAsync(string expression, ExpressionContext context, CancellationToken cancellationToken = default)
        {
            var innerExpressionResult = await evaluator.EvaluateExpressionAsync(expression, context, cancellationToken)
                .ConfigureAwait(false);

            if(logger.IsEnabled(LogLevel.Trace))
                logger.LogTrace(@"Not evaluator inverting {inner_result}", innerExpressionResult);

            var booleanResult = valueConverter.CoerceToBoolean(innerExpressionResult);
            return !booleanResult;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotExpressionEvaluator"/> class.
        /// </summary>
        /// <param name="valueConverter">Value converter.</param>
        /// <param name="evaluator">Evaluator.</param>
        /// <param name="logger">Logger</param>
        public NotExpressionEvaluator(ICoercesValueToBoolean valueConverter, 
                                      IEvaluatesExpression evaluator,
                                      ILogger<NotExpressionEvaluator> logger)
        {
            this.valueConverter = valueConverter ?? throw new ArgumentNullException(nameof(valueConverter));
            this.evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
