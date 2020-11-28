using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;
using ZptSharp.Expressions;
using ZptSharp.Rendering;

namespace ZptSharp.Tal
{
    /// <summary>
    /// Decorator for <see cref="IProcessesExpressionContext"/> which handles TAL 'define' attributes.
    /// </summary>
    public class DefineAttributeDecorator : IProcessesExpressionContext
    {
        readonly IProcessesExpressionContext wrapped;
        readonly IGetsTalAttributeSpecs specProvider;
        readonly IEvaluatesExpression evaluator;
        readonly IInterpretsExpressionResult resultInterpreter;
        readonly IGetsVariableDefinitionsFromAttributeValue definitionProvider;

        /// <summary>
        /// Processes the context using the rules defined within this object.
        /// </summary>
        /// <returns>A result object indicating the outcome of processing.</returns>
        /// <param name="context">The context to process.</param>
        /// <param name="token">An optional cancellation token.</param>
        public async Task<ExpressionContextProcessingResult> ProcessContextAsync(ExpressionContext context, CancellationToken token = default)
        {
            var defineAttribute = context.CurrentElement.GetMatchingAttribute(specProvider.Define);
            if (defineAttribute != null)
                await HandleDefineAttribute(defineAttribute, context, token);

            return await wrapped.ProcessContextAsync(context, token);
        }

        /// <summary>
        /// Handles the presence of a TAL define <paramref name="attribute"/>.  This means getting each of the individual
        /// definitions declared in that attribute and for each of them, evaluating the associated value
        /// expression.  The result is then set into the appropriate scope of the expression <paramref name="context"/>.
        /// </summary>
        /// <param name="attribute">Attribute.</param>
        /// <param name="context">Context.</param>
        /// <param name="token">Token.</param>
        async Task HandleDefineAttribute(IAttribute attribute, ExpressionContext context, CancellationToken token)
        {
            var definitions = GetDefinitions(attribute.Value, context);

            foreach (var definition in definitions)
            {
                var result = await EvaluateDefinitionExpression(definition, context, token);

                // If the result cancels the action then we ignore this partiaular definition
                if (resultInterpreter.DoesResultAbortTheAction(result)) continue;

                var scope = definition.IsGlobal ? context.GlobalDefinitions : context.LocalDefinitions;
                scope[definition.VariableName] = result;
            }
        }

        IEnumerable<VariableDefinition> GetDefinitions(string attributeValue, ExpressionContext context)
        {
            try
            {
                return definitionProvider.GetDefinitions(attributeValue);
            }
            catch (FormatException ex)
            {
                var message = String.Format(Resources.ExceptionMessage.InvalidTalDefineAttribute,
                                            context.CurrentElement,
                                            attributeValue);
                throw new InvalidTalAttributeException(message, ex);
            }
        }


        async Task<object> EvaluateDefinitionExpression(VariableDefinition definition, ExpressionContext context, CancellationToken token)
        {
            try
            {
                return await evaluator.EvaluateExpressionAsync(definition.Expression, context, token);
            }
            catch (Exception ex)
            {
                var message = String.Format(Resources.ExceptionMessage.UnexpectedErrorEvaluatingDefineAttribute,
                                            context.CurrentElement,
                                            definition);
                throw new DefineVariableEvaluationException(message, ex);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefineAttributeDecorator"/> class.
        /// </summary>
        /// <param name="wrapped">Wrapped.</param>
        /// <param name="specProvider">Spec provider.</param>
        /// <param name="evaluator">Evaluator.</param>
        /// <param name="resultInterpreter">Result interpreter.</param>
        /// <param name="definitionProvider">Variable definition provider.</param>
        public DefineAttributeDecorator(IProcessesExpressionContext wrapped,
                                        IGetsTalAttributeSpecs specProvider,
                                        IEvaluatesExpression evaluator,
                                        IInterpretsExpressionResult resultInterpreter,
                                        IGetsVariableDefinitionsFromAttributeValue definitionProvider)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));
            this.resultInterpreter = resultInterpreter ?? throw new ArgumentNullException(nameof(resultInterpreter));
            this.definitionProvider = definitionProvider ?? throw new ArgumentNullException(nameof(definitionProvider));
        }
    }
}
