using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;
using ZptSharp.Expressions;
using ZptSharp.Rendering;

namespace ZptSharp.Tal
{
    /// <summary>
    /// Decorator for <see cref="IHandlesProcessingError"/> which handles TAL 'repeat' attributes.
    /// </summary>
    public class RepeatAttributeDecorator : IHandlesProcessingError
    {
        const string attributePattern = @"^([^ ]+)\s+(.+)$";
        static readonly Regex attributeMatcher = new Regex(attributePattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        readonly IHandlesProcessingError wrapped;
        readonly IGetsTalAttributeSpecs specProvider;
        readonly IEvaluatesExpression evaluator;
        readonly IInterpretsExpressionResult resultInterpreter;
        readonly IGetsRepetitionContexts contextProvider;

        /// <summary>
        /// Processes the context using the rules defined within this object.
        /// </summary>
        /// <returns>A result object indicating the outcome of processing.</returns>
        /// <param name="context">The context to process.</param>
        /// <param name="token">An optional cancellation token.</param>
        public async Task<ExpressionContextProcessingResult> ProcessContextAsync(ExpressionContext context, CancellationToken token = default)
        {
            var attribute = context.CurrentElement.GetMatchingAttribute(specProvider.Repeat);
            if (attribute == null)
                return await wrapped.ProcessContextAsync(context, token).ConfigureAwait(false);

            var parsedAttribute = ParseAttribute(attribute, context);

            var expressionResult = await evaluator.EvaluateExpressionAsync(parsedAttribute.Expression, context, token)
                .ConfigureAwait(false);
            if (resultInterpreter.DoesResultAbortTheAction(expressionResult) || expressionResult == null)
                return await wrapped.ProcessContextAsync(context, token).ConfigureAwait(false);

            var contexts = contextProvider.GetRepetitionContexts(expressionResult,
                                                                 context,
                                                                 parsedAttribute.VariableName);

            var separatedContexts = GetWhitespaceSeparatedContexts(contexts, context).ToList();

            // With a repeat attribute, we actually don't process the
            // current/existing context, because we end up cloning it
            // for each repetition.  The current context is used only
            // as a template of sorts, hence aborting processing its
            // children.
            context.CurrentElement.Remove();
            var result = ExpressionContextProcessingResult.WithoutChildren;
            result.AdditionalContexts = separatedContexts;

            return result;
        }

        /// <summary>
        /// Contexts returned as a result of a repeat attribute need to be separated by whitespace, so that
        /// they are not bunched up.  This doesn't usually matter for elements, which are generally
        /// whitespace-neutral anyway, but it's important if we are omitting the element and just using
        /// text nodes.
        /// </summary>
        /// <returns>The whitespace separated contexts.</returns>
        /// <param name="contexts">Contexts.</param>
        /// <param name="context">Context.</param>
        IEnumerable<ExpressionContext> GetWhitespaceSeparatedContexts(IList<ExpressionContext> contexts,
                                                                      ExpressionContext context)
        {
            for(var i = 0; i < contexts.Count; i++)
            {
                yield return contexts[i];
                if (i == contexts.Count - 1) continue;
                yield return context.CreateChild(context.CurrentElement.CreateTextNode(Environment.NewLine));
            }
        }

        /// <summary>
        /// Gets the two parts of the attribute value (the variable name and the expression).
        /// </summary>
        /// <returns>The attribute.</returns>
        /// <param name="attribute">Attribute.</param>
        /// <param name="context">Expression context.</param>
        RepeatAttribute ParseAttribute(IAttribute attribute, ExpressionContext context)
        {
            var attributeMatch = attributeMatcher.Match(attribute.Value);
            if (!attributeMatch.Success)
            {
                var message = String.Format(Resources.ExceptionMessage.InvalidRepeatAttribute, context.CurrentElement);
                throw new InvalidTalAttributeException(message);
            }

            return new RepeatAttribute
            {
                VariableName = attributeMatch.Groups[1].Value,
                Expression = attributeMatch.Groups[2].Value,
            };
        }

        Task<ErrorHandlingResult> IHandlesProcessingError.HandleErrorAsync(Exception ex, ExpressionContext context, CancellationToken token)
            => wrapped.HandleErrorAsync(ex, context, token);

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatAttributeDecorator"/> class.
        /// </summary>
        /// <param name="wrapped">Wrapped.</param>
        /// <param name="specProvider">Spec provider.</param>
        /// <param name="evaluator">Evaluator.</param>
        /// <param name="resultInterpreter">Result interpreter.</param>
        /// <param name="contextProvider">Repetition contexts provider.</param>
        public RepeatAttributeDecorator(IHandlesProcessingError wrapped,
                                        IGetsTalAttributeSpecs specProvider,
                                        IEvaluatesExpression evaluator,
                                        IInterpretsExpressionResult resultInterpreter,
                                        IGetsRepetitionContexts contextProvider)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));
            this.resultInterpreter = resultInterpreter ?? throw new ArgumentNullException(nameof(resultInterpreter));
            this.contextProvider = contextProvider ?? throw new ArgumentNullException(nameof(contextProvider));
        }

        #region contained model

        internal class RepeatAttribute
        {
            internal string VariableName { get; set; }
            internal string Expression { get; set; }
        }

        #endregion
    }
}
