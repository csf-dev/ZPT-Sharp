﻿using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;
using ZptSharp.Expressions;
using ZptSharp.Rendering;

namespace ZptSharp.Tal
{
    /// <summary>
    /// Decorator for <see cref="IHandlesProcessingError"/> which handles TAL 'condition' attributes.
    /// </summary>
    public class ConditionAttributeDecorator : IHandlesProcessingError
    {
        readonly IHandlesProcessingError wrapped;
        readonly IGetsTalAttributeSpecs specProvider;
        readonly IEvaluatesExpression evaluator;
        readonly IInterpretsExpressionResult resultInterpreter;

        /// <summary>
        /// Processes the context using the rules defined within this object.
        /// </summary>
        /// <returns>A result object indicating the outcome of processing.</returns>
        /// <param name="context">The context to process.</param>
        /// <param name="token">An optional cancellation token.</param>
        public async Task<ExpressionContextProcessingResult> ProcessContextAsync(ExpressionContext context, CancellationToken token = default)
        {
            var conditionAttribute = context.CurrentNode.GetMatchingAttribute(specProvider.Condition);
            if (conditionAttribute == null) return await wrapped.ProcessContextAsync(context, token).ConfigureAwait(false);

            var expressionResult = await evaluator.EvaluateExpressionAsync(conditionAttribute.Value, context, token)
                .ConfigureAwait(false);
            if (ShouldRemoveAttribute(expressionResult))
            {
                context.CurrentNode.Remove();
                return ExpressionContextProcessingResult.WithoutChildren;
            }

            return await wrapped.ProcessContextAsync(context, token).ConfigureAwait(false);
        }

        bool ShouldRemoveAttribute(object expressionResult)
        {
            if (resultInterpreter.DoesResultAbortTheAction(expressionResult)) return false;
            return !resultInterpreter.CoerceResultToBoolean(expressionResult);
        }

        Task<ErrorHandlingResult> IHandlesProcessingError.HandleErrorAsync(Exception ex, ExpressionContext context, CancellationToken token)
            => wrapped.HandleErrorAsync(ex, context, token);

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionAttributeDecorator"/> class.
        /// </summary>
        /// <param name="wrapped">Wrapped.</param>
        /// <param name="specProvider">Spec provider.</param>
        /// <param name="evaluator">Evaluator.</param>
        /// <param name="resultInterpreter">Result interpreter.</param>
        public ConditionAttributeDecorator(IHandlesProcessingError wrapped,
                                           IGetsTalAttributeSpecs specProvider,
                                           IEvaluatesExpression evaluator,
                                           IInterpretsExpressionResult resultInterpreter)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));
            this.resultInterpreter = resultInterpreter ?? throw new ArgumentNullException(nameof(resultInterpreter));
        }
    }
}
