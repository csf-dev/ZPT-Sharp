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
    /// Implementation of <see cref="IHandlesProcessingError"/> which handles both TAL 'content' or
    /// TAL 'replace' attributes.  Their functionality is somewhat similar, and they cannot coexist on
    /// the same DOM element.
    /// </summary>
    public class ContentOrReplaceAttributeDecorator : IHandlesProcessingError
    {
        readonly IHandlesProcessingError wrapped;
        readonly IGetsTalAttributeSpecs specProvider;
        readonly IEvaluatesDomValueExpression evaluator;
        readonly IReplacesNode replacer;
        readonly IOmitsNode omitter;

        /// <summary>
        /// Processes the context using the rules defined within this object.
        /// </summary>
        /// <returns>A result object indicating the outcome of processing.</returns>
        /// <param name="context">The context to process.</param>
        /// <param name="token">An optional cancellation token.</param>
        public Task<ExpressionContextProcessingResult> ProcessContextAsync(ExpressionContext context, CancellationToken token = default)
        {
            var content = context.CurrentElement.GetMatchingAttribute(specProvider.Content);
            var replace = context.CurrentElement.GetMatchingAttribute(specProvider.Replace);

            if (content != null && replace != null)
            {
                var message = String.Format(Resources.ExceptionMessage.ContentAndReplaceAttributesMayNotCoexist, context.CurrentElement);
                throw new InvalidTalAttributeException(message);
            }
            if (content == null && replace == null)
                return wrapped.ProcessContextAsync(context, token);

            return (content != null)
                ? HandleContentAsync(content, context, token)
                : HandleReplaceAsync(replace, context, token);
        }

        /// <summary>
        /// Handles a TAL 'content' attribute
        /// </summary>
        /// <returns>The processing result.</returns>
        /// <param name="attribute">Attribute.</param>
        /// <param name="context">Context.</param>
        /// <param name="token">Token.</param>
        async Task<ExpressionContextProcessingResult> HandleContentAsync(IAttribute attribute,
                                                                         ExpressionContext context,
                                                                         CancellationToken token)
        {
            var domResult = await GetExpressionResultAsync(attribute, context, token).ConfigureAwait(false);

            if (domResult.AbortAction)
                return await wrapped.ProcessContextAsync(context, token).ConfigureAwait(false);

            context.CurrentElement.ChildNodes.Clear();
            context.CurrentElement.AddChildren(domResult.Nodes);

            return await wrapped.ProcessContextAsync(context, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Handles a TAL 'replace' attribute
        /// </summary>
        /// <returns>The processing result.</returns>
        /// <param name="attribute">Attribute.</param>
        /// <param name="context">Context.</param>
        /// <param name="token">Token.</param>
        async Task<ExpressionContextProcessingResult> HandleReplaceAsync(IAttribute attribute,
                                                                         ExpressionContext context,
                                                                         CancellationToken token)
        {
            var domResult = await GetExpressionResultAsync(attribute, context, token).ConfigureAwait(false);

            if (domResult.AbortAction)
            {
                var childNodeContexts = context.CreateChildren(context.CurrentElement.ChildNodes);
                omitter.Omit(context.CurrentElement);

                return await GetReplacementResultAsync(context, childNodeContexts, token).ConfigureAwait(false);
            }

            var replacementContexts = context.CreateChildren(domResult.Nodes);
            replacer.Replace(context.CurrentElement, domResult.Nodes);

            return await GetReplacementResultAsync(context, replacementContexts, token).ConfigureAwait(false);
        }

        async Task<ExpressionContextProcessingResult> GetReplacementResultAsync(ExpressionContext originalContext,
                                                                                IList<ExpressionContext> replacementContexts,
                                                                                CancellationToken token)
        {
            MoveTalAttributesToReplacementNodesWhereApplicable(replacementContexts, originalContext);

            if (replacementContexts.Count != 1)
                return new ExpressionContextProcessingResult { AdditionalContexts = replacementContexts, };

            // If there is precisely one replacement context, then continue processing as if that was the current context.
            // In that manner, further wrapped services may act upon that context.

            return await wrapped.ProcessContextAsync(replacementContexts[0], token).ConfigureAwait(false);
        }

        async Task<DomValueExpressionResult> GetExpressionResultAsync(IAttribute attribute,
                                                                      ExpressionContext context,
                                                                      CancellationToken token)
        {
            try
            {
                return await evaluator.EvaluateExpressionAsync(attribute.Value, context, token)
                    .ConfigureAwait(false);
            }
            catch(EvaluationException ex)
            {
                var message = String.Format(Resources.ExceptionMessage.CouldNotEvaluateContentOrReplaceExpression,
                                            context.CurrentElement,
                                            attribute.Value);
                throw new TalExpressionEvaluationException(message, ex);
            }
        }

        /// <summary>
        /// TAL 'attributes' and 'omit-tag' attributes are still relevant on the replacement node(s) and
        /// should still be processed.  This function copies those attributes from the current element to
        /// the replacement elements where they are present.
        /// </summary>
        /// <param name="contexts">Contexts.</param>
        /// <param name="replaceElementContext">Replace element context.</param>
        void MoveTalAttributesToReplacementNodesWhereApplicable(IList<ExpressionContext> contexts,
                                                                ExpressionContext replaceElementContext)
        {
            var attributesAttribute = replaceElementContext.CurrentElement.GetMatchingAttribute(specProvider.Attributes);
            var omitTagAttribute = replaceElementContext.CurrentElement.GetMatchingAttribute(specProvider.OmitTag);

            foreach (var context in contexts)
            {
                // Don't copy attributes to non-elements
                if (!context.CurrentElement.IsElement) continue;

                if (attributesAttribute != null)
                    context.CurrentElement.Attributes.Add(attributesAttribute);
                if (omitTagAttribute != null)
                    context.CurrentElement.Attributes.Add(omitTagAttribute);
            }
        }

        Task<ErrorHandlingResult> IHandlesProcessingError.HandleErrorAsync(Exception ex, ExpressionContext context, CancellationToken token)
            => wrapped.HandleErrorAsync(ex, context, token);

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentOrReplaceAttributeDecorator"/> class.
        /// </summary>
        /// <param name="wrapped">Wrapped.</param>
        /// <param name="specProvider">Spec provider.</param>
        /// <param name="evaluator">Evaluator.</param>
        /// <param name="replacer">Replacer.</param>
        /// <param name="omitter">Omitter.</param>
        public ContentOrReplaceAttributeDecorator(IHandlesProcessingError wrapped,
                                                  IGetsTalAttributeSpecs specProvider,
                                                  IEvaluatesDomValueExpression evaluator,
                                                  IReplacesNode replacer,
                                                  IOmitsNode omitter)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));
            this.replacer = replacer ?? throw new ArgumentNullException(nameof(replacer));
            this.omitter = omitter ?? throw new ArgumentNullException(nameof(omitter));
        }
    }
}
