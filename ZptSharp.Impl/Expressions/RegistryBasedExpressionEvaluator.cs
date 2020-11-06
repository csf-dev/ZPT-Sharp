using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// <para>
    /// An implementation of <see cref="IEvaluatesExpression"/> which makes use of
    /// the registry pattern to select and delegate execution to another implementation (of
    /// the same interface).  This selection occurs based upon the expression type.
    /// </para>
    /// <para>
    /// Additionally, when delegating to an implementation of <see cref="IEvaluatesExpression"/>
    /// from the registry, any expression-type prefix upon the expression is removed before
    /// this delegation occurs.
    /// </para>
    /// </summary>
    public class RegistryBasedExpressionEvaluator : IEvaluatesExpression
    {
        readonly IGetsExpressionType typeProvider;
        readonly IGetsEvaluatorForExpressionType evaluatorProvider;
        readonly IRemovesPrefixFromExpression prefixRemover;

        /// <summary>
        /// Evaluates the expression asynchronously and returns the result.
        /// </summary>
        /// <returns>The expression result.</returns>
        /// <param name="expression">The expression string.</param>
        /// <param name="context">The expression context.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<object> EvaluateExpressionAsync(string expression,
                                                    ExpressionContext context,
                                                    CancellationToken cancellationToken = default)
        {
            var expressionType = typeProvider.GetExpressionType(expression);
            var evaluator = evaluatorProvider.GetEvaluator(expressionType);
            var expressionWithoutPrefix = prefixRemover.GetExpressionWithoutPrefix(expression);

            return evaluator.EvaluateExpressionAsync(expressionWithoutPrefix, context, cancellationToken);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryBasedExpressionEvaluator"/> class.
        /// </summary>
        /// <param name="typeProvider">Type provider.</param>
        /// <param name="evaluatorProvider">Evaluator provider.</param>
        /// <param name="prefixRemover">Prefix remover.</param>
        public RegistryBasedExpressionEvaluator(IGetsExpressionType typeProvider,
                                                IGetsEvaluatorForExpressionType evaluatorProvider,
                                                IRemovesPrefixFromExpression prefixRemover)
        {
            this.typeProvider = typeProvider ?? throw new ArgumentNullException(nameof(typeProvider));
            this.evaluatorProvider = evaluatorProvider ?? throw new ArgumentNullException(nameof(evaluatorProvider));
            this.prefixRemover = prefixRemover ?? throw new ArgumentNullException(nameof(prefixRemover));
        }
    }
}
