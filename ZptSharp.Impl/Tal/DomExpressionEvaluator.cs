using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Tal
{
    /// <summary>
    /// Implementation of <see cref="IEvaluatesDomValueExpression"/> which evaluates DOM expressions
    /// returning an object that contains a list of nodes and supplemental information.
    /// </summary>
    public class DomExpressionEvaluator : IEvaluatesDomValueExpression
    {
        const string
            domExpressionPattern = @"^(?:(text|structure)\s+)?(.*)$",
            structureName = "structure",
            textName = "text";
        static readonly Regex domExpression = new Regex(domExpressionPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        readonly IEvaluatesExpression evaluator;
        readonly IInterpretsExpressionResult resultInterpreter;
        readonly ILogger logger;

        /// <summary>
        /// Evaluates the expression asynchronously and returns the result.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method also deals with the differentiation between <c>structure</c> and <c>text</c>
        /// expressions.  Structure expressions are inserted directly into the DOM without escaping or encoding.
        /// Text expressions are treated explicitly as text and are encoded/escaped to ensure that they are not
        /// accidentally treated as markup.
        /// </para>
        /// <para>
        /// Expressions are treated as text by default.  An expression is only treated as structure if:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description>It is prefixed by a keyword <c>structure</c> and a single space, for example <c>structure myExpression</c></description>
        /// </item>
        /// <item>
        /// <description>The expression result implements <see cref="IGetsStructuredMarkup"/> and the expression is NOT prefixed by
        /// the keyword <c>text</c> and a single space, for example <c>text myExpression</c></description>
        /// </item>
        /// </list>
        /// <para>
        /// Apart from aborting the treatment of <see cref="IGetsStructuredMarkup"/> as structure, the <c>text</c> prefix
        /// keyword for expressions is redundant, as it is the default.  It is supported though, for situations where you wish
        /// to be explicit.
        /// </para>
        /// </remarks>
        /// <returns>The expression result.</returns>
        /// <param name="expression">An expression which might be prefixed to indicate that it is to be treated as structure.</param>
        /// <param name="context">Context.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task<DomValueExpressionResult> EvaluateExpressionAsync(string expression,
                                                                      ExpressionContext context,
                                                                      CancellationToken cancellationToken = default)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (context== null)
                throw new ArgumentNullException(nameof(context));

            return EvaluateExpressionPrivateAsync(expression, context, cancellationToken);
        }

        async Task<DomValueExpressionResult> EvaluateExpressionPrivateAsync(string expression,
                                                                            ExpressionContext context,
                                                                            CancellationToken cancellationToken)
        {
            var expressionMatch = domExpression.Match(expression);

            var structureOrText = expressionMatch.Groups[1].Value;
            var innerExpression = expressionMatch.Groups[2].Value;

            var result = await evaluator.EvaluateExpressionAsync(innerExpression, context, cancellationToken)
                .ConfigureAwait(false);

            if (resultInterpreter.DoesResultAbortTheAction(result))
                return new DomValueExpressionResult(abortAction: true);

            var isStructure = structureOrText == structureName;
            if (structureOrText != textName && result is IGetsStructuredMarkup markupProvider)
            {
                isStructure = true;
                result = await markupProvider.GetMarkupAsync().ConfigureAwait(false);
            }

            if(logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace(@"Evaluated a DOM expression.
Expression:{expression}
    Result:{result}
 Structure:{isStructure}",
                                expression,
                                result,
                                isStructure);
            }

            var nodes = GetNodes(result, isStructure, context);

            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace(@"Got a collection of {node_count} node(s) for the DOM expression.
Nodes:{nodes}",
                                nodes.Count,
                                nodes);
            }

            return new DomValueExpressionResult(nodes);
        }

        IList<INode> GetNodes(object expressionResult, bool isStructure, ExpressionContext context)
        {
            var value = expressionResult?.ToString();
            if (value == null) return new INode[0];

            var ele = context.CurrentNode;
            return isStructure ? ele.ParseAsNodes(value) : new[] { ele.CreateTextNode(value) };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ZptSharp.Tal.DomExpressionEvaluator"/> class.
        /// </summary>
        /// <param name="evaluator">Evaluator.</param>
        /// <param name="resultInterpreter">Result interpreter.</param>
        /// <param name="logger">A logger.</param>
        public DomExpressionEvaluator(IEvaluatesExpression evaluator, 
                                      IInterpretsExpressionResult resultInterpreter,
                                      ILogger<DomExpressionEvaluator> logger)
        {
            this.evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));
            this.resultInterpreter = resultInterpreter ?? throw new ArgumentNullException(nameof(resultInterpreter));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
