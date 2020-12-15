using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;
using ZptSharp.Expressions;
using ZptSharp.Rendering;

namespace ZptSharp.Tal
{
    /// <summary>
    /// Decorator for <see cref="IHandlesProcessingError"/> which handles TAL 'attributes' attributes.
    /// </summary>
    public class AttributesAttributeDecorator : IHandlesProcessingError
    {
        readonly IHandlesProcessingError wrapped;
        readonly IGetsTalAttributeSpecs specProvider;
        readonly IEvaluatesExpression evaluator;
        readonly IInterpretsExpressionResult resultInterpreter;
        readonly IGetsAttributeDefinitions definitionsProvider;

        /// <summary>
        /// Processes the context using the rules defined within this object.
        /// </summary>
        /// <returns>A result object indicating the outcome of processing.</returns>
        /// <param name="context">The context to process.</param>
        /// <param name="token">An optional cancellation token.</param>
        public async Task<ExpressionContextProcessingResult> ProcessContextAsync(ExpressionContext context, CancellationToken token = default)
        {
            var attribute = context.CurrentElement.GetMatchingAttribute(specProvider.Attributes);
            if (attribute == null)
                return await wrapped.ProcessContextAsync(context, token).ConfigureAwait(false);

            var definitions = definitionsProvider.GetDefinitions(attribute.Value, context.CurrentElement);
            foreach(var definition in definitions)
                await HandleAttributeDefinition(definition, context, token).ConfigureAwait(false);

            return await wrapped.ProcessContextAsync(context, token).ConfigureAwait(false);
        }

        async Task HandleAttributeDefinition(AttributeDefinition definition,
                                             ExpressionContext context,
                                             CancellationToken token)
        {
            var element = context.CurrentElement;
            var expressionResult = await evaluator.EvaluateExpressionAsync(definition.Expression, context, token)
                .ConfigureAwait(false);

            if (resultInterpreter.DoesResultAbortTheAction(expressionResult))
                return;

            var spec = new AttributeSpec(definition.Name, new Namespace(definition.Prefix));
            var existingAttribute = element.GetMatchingAttribute(spec);

            if (expressionResult == null || Equals(expressionResult, false))
            {
                if(existingAttribute != null)
                    element.Attributes.Remove(existingAttribute);

                return;
            }

            var attribute = existingAttribute ?? element.CreateAttribute(spec);
            if (Equals(expressionResult, true)) attribute.Value = attribute.Name;
            else attribute.Value = expressionResult.ToString();

            if (!element.Attributes.Contains(attribute))
                element.Attributes.Add(attribute);
        }

        Task<ErrorHandlingResult> IHandlesProcessingError.HandleErrorAsync(Exception ex, ExpressionContext context, CancellationToken token)
            => wrapped.HandleErrorAsync(ex, context, token);

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributesAttributeDecorator"/> class.
        /// </summary>
        /// <param name="wrapped">Wrapped.</param>
        /// <param name="specProvider">Spec provider.</param>
        /// <param name="evaluator">Evaluator.</param>
        /// <param name="resultInterpreter">Result interpreter.</param>
        /// <param name="definitionsProvider">Definitions provider.</param>
        public AttributesAttributeDecorator(IHandlesProcessingError wrapped,
                                            IGetsTalAttributeSpecs specProvider,
                                            IEvaluatesExpression evaluator,
                                            IInterpretsExpressionResult resultInterpreter,
                                            IGetsAttributeDefinitions definitionsProvider)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));
            this.resultInterpreter = resultInterpreter ?? throw new ArgumentNullException(nameof(resultInterpreter));
            this.definitionsProvider = definitionsProvider ?? throw new ArgumentNullException(nameof(definitionsProvider));
        }
    }
}
