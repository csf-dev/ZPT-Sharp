using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Config;
using ZptSharp.Dom;
using ZptSharp.Expressions;
using ZptSharp.Rendering;

namespace ZptSharp.Tal
{
    /// <summary>
    /// Implementation of <see cref="IHandlesProcessingError"/> which handles simple content
    /// substitutions in text nodes and/or attribute values.  This is only performed if the
    /// functionality is enabled.
    /// </summary>
    /// <seealso cref="RenderingConfig.UseSimpleValueSubstitutions"/>
    public class SimpleSubstitutionsDecorator : IHandlesProcessingError
    {
        const string replacementPattern = @"(\\)?$\{([^}]+)\}";
        static readonly Regex replacer = new Regex(replacementPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        readonly IHandlesProcessingError wrapped;
        readonly IGetsTalAttributeSpecs specProvider;
        readonly IEvaluatesDomValueExpression evaluator;
        readonly RenderingConfig config;
        readonly IGetsWellKnownNamespace namespaceProvider;

        public Task<ExpressionContextProcessingResult> ProcessContextAsync(ExpressionContext context, CancellationToken token = default)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            if (!config.UseSimpleValueSubstitutions)
                return wrapped.ProcessContextAsync(context, token);

            return ProcessContextPrivateAsync(context, token);
        }

        async Task<ExpressionContextProcessingResult> ProcessContextPrivateAsync(ExpressionContext context, CancellationToken token)
        {
            var node = context.CurrentNode;

            if (node.GetMatchingAttribute(specProvider.Content) == null && node.GetMatchingAttribute(specProvider.Repeat) == null)
                PerformSubstitutionsInTextNodes(context, token);

            if (node.GetMatchingAttribute(specProvider.Attributes) == null)
                PerformSubstitutionsInAttributes(context, token);

            return await wrapped.ProcessContextAsync(context, token).ConfigureAwait(false);
        }

        void PerformSubstitutionsInTextNodes(ExpressionContext context, CancellationToken token)
        {
            var textNodes = context.CurrentNode.ChildNodes.Where(x => x.IsTextNode).ToList();
            foreach (var node in textNodes)
                node.Text = replacer.Replace(node.Text, GetMatchEvaluator(context, token));
        }

        void PerformSubstitutionsInAttributes(ExpressionContext context, CancellationToken token)
        {
            var attributes = context.CurrentNode.Attributes
                .Where(x => !x.IsInNamespace(namespaceProvider.TalNamespace) && !x.IsInNamespace(namespaceProvider.MetalNamespace))
                .ToList();
            
            foreach (var attribute in attributes)
                attribute.Value = replacer.Replace(attribute.Value, GetMatchEvaluator(context, token));
        }

        MatchEvaluator GetMatchEvaluator(ExpressionContext context, CancellationToken token)
        {
            string ReplaceMatch(Match match)
            {
                if (match.Groups[1].Value == "\\")
                    return $"${{{match.Groups[2].Value}}}";

                var expression = match.Groups[2].Value;
                var expressionResult = evaluator.EvaluateExpressionAsync(expression, context, token).Result;
                expressionResult.
            }

        }

        Task<ErrorHandlingResult> IHandlesProcessingError.HandleErrorAsync(Exception ex, ExpressionContext context, CancellationToken token)
            => wrapped.HandleErrorAsync(ex, context, token);

        public SimpleSubstitutionsDecorator(IHandlesProcessingError wrapped,
                                            IGetsTalAttributeSpecs specProvider,
                                            IEvaluatesDomValueExpression evaluator,
                                            RenderingConfig config,
                                            IGetsWellKnownNamespace namespaceProvider)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.namespaceProvider = namespaceProvider ?? throw new ArgumentNullException(nameof(namespaceProvider));
        }
    }
}