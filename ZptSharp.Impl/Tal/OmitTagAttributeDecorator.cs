using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;
using ZptSharp.Expressions;
using ZptSharp.Rendering;

namespace ZptSharp.Tal
{
    /// <summary>
    /// Decorator for <see cref="IProcessesExpressionContext"/> which handles TAL 'omit-tag' attributes.
    /// </summary>
    public class OmitTagAttributeDecorator : IProcessesExpressionContext
    {
        readonly IProcessesExpressionContext wrapped;
        readonly IGetsTalAttributeSpecs specProvider;
        readonly IOmitsNode omitter;
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
            var attribute = context.CurrentElement.GetMatchingAttribute(specProvider.OmitTag);
            if (attribute == null)
                return await wrapped.ProcessContextAsync(context, token);

            var expressionResult = await evaluator.EvaluateExpressionAsync(attribute.Value, context, token);
            if (!ShouldOmitTag(expressionResult))
                return await wrapped.ProcessContextAsync(context, token);

            var childContexts = context.CreateChildren(context.CurrentElement.ChildNodes);
            omitter.Omit(context.CurrentElement);

            return new ExpressionContextProcessingResult
            {
                AdditionalContexts = childContexts
            };
        }

        bool ShouldOmitTag(object expressionResult)
        {
            return !resultInterpreter.DoesResultAbortTheAction(expressionResult)
                && resultInterpreter.CoerceResultToBoolean(expressionResult);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OmitTagAttributeDecorator"/> class.
        /// </summary>
        /// <param name="wrapped">Wrapped.</param>
        /// <param name="specProvider">Spec provider.</param>
        /// <param name="omitter">Omitter.</param>
        /// <param name="evaluator">Evaluator.</param>
        /// <param name="resultInterpreter">Result interpreter.</param>
        public OmitTagAttributeDecorator(IProcessesExpressionContext wrapped,
                                         IGetsTalAttributeSpecs specProvider,
                                         IOmitsNode omitter,
                                         IEvaluatesExpression evaluator,
                                         IInterpretsExpressionResult resultInterpreter)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.omitter = omitter ?? throw new ArgumentNullException(nameof(omitter));
            this.evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));
            this.resultInterpreter = resultInterpreter ?? throw new ArgumentNullException(nameof(resultInterpreter));
        }
    }
}
