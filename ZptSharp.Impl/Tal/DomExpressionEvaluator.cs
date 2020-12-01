using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
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
            structureName = "structure";
        static readonly Regex domExpression = new Regex(domExpressionPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        readonly IEvaluatesExpression evaluator;
        readonly IInterpretsExpressionResult resultInterpreter;

        /// <summary>
        /// Evaluates the expression asynchronously and returns the result.
        /// </summary>
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
            var isStructure = structureOrText == structureName;
            var innerExpression = expressionMatch.Groups[2].Value;

            var result = await evaluator.EvaluateExpressionAsync(innerExpression, context, cancellationToken)
                .ConfigureAwait(false);

            if (resultInterpreter.DoesResultAbortTheAction(result))
                return new DomValueExpressionResult(abortAction: true);

            var nodes = GetNodes(result, isStructure, context);
            return new DomValueExpressionResult(nodes);
        }

        IList<INode> GetNodes(object expressionResult, bool isStructure, ExpressionContext context)
        {
            var value = expressionResult?.ToString();
            if (value == null) return new INode[0];

            var ele = context.CurrentElement;
            return isStructure ? ele.ParseAsNodes(value) : new[] { ele.CreateTextNode(value) };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ZptSharp.Tal.DomExpressionEvaluator"/> class.
        /// </summary>
        /// <param name="evaluator">Evaluator.</param>
        /// <param name="resultInterpreter">Result interpreter.</param>
        public DomExpressionEvaluator(IEvaluatesExpression evaluator, 
                                      IInterpretsExpressionResult resultInterpreter)
        {
            this.evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));
            this.resultInterpreter = resultInterpreter ?? throw new ArgumentNullException(nameof(resultInterpreter));
        }
    }
}
