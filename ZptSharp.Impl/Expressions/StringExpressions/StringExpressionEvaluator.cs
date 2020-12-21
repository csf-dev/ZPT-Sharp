using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.Expressions.StringExpressions
{
    /// <summary>
    /// An implementation of <see cref="IEvaluatesExpression"/> which evaluates TALES 'string' expressions.
    /// </summary>
    public class StringExpressionEvaluator : IEvaluatesExpression
    {
        const string
            escapedPlaceholderPattern = @"\$\$",
            placeholderCharacter = "$",
            placeholderReplacementPattern = @"\$(?:(?:\{([a-zA-Z0-9 /_.,~|?-]+)\})|([a-zA-Z0-9_/]+))";

        static readonly Regex
            escapedPlaceholder = new Regex(escapedPlaceholderPattern),
            placeholderReplacement = new Regex(placeholderReplacementPattern);

        readonly IEvaluatesExpression evaluator;

        /// <summary>
        /// Evaluates the expression asynchronously and returns the result.
        /// </summary>
        /// <returns>The expression result.</returns>
        /// <param name="expression">The expression string.</param>
        /// <param name="context">The expression context.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<object> EvaluateExpressionAsync(string expression, ExpressionContext context, CancellationToken cancellationToken = default)
        {
            var unescapedExpression = GetUnescapedExpression(expression, out var indexesOfEscapedPlaceholders);
            var result = GetResultWithReplacements(unescapedExpression, context, cancellationToken, indexesOfEscapedPlaceholders);

            // This use of ContinueWith is just to downcast Task<string> to Task<object>
            return result.ContinueWith(t => (object) t.Result);
        }

        /// <summary>
        /// Gets an unescaped version of the specified <paramref name="expression"/>, replacing <c>$$</c>
        /// with single <c>$</c> characters.  Also exposes a collection of <paramref name="indexesOfEscapedPlaceholders"/>,
        /// which may be used to detect the positions at which escaped placeholders were found.
        /// </summary>
        /// <returns>The unescaped expression.</returns>
        /// <param name="expression">Expression.</param>
        /// <param name="indexesOfEscapedPlaceholders">Indexes of escaped placeholders.</param>
        string GetUnescapedExpression(string expression, out ICollection<int> indexesOfEscapedPlaceholders)
        {
            var indexes = new List<int>();
            var matchesFound = 0;

            var unescapedExpression = escapedPlaceholder.Replace(expression, match => {
                indexes.Add(match.Index - (matchesFound++));
                return placeholderCharacter;
            });

            indexesOfEscapedPlaceholders = indexes;
            return unescapedExpression;
        }

        /// <summary>
        /// Makes the placeholder replacements into the expression string and returns the result.
        /// </summary>
        /// <returns>The expression string, with replacements made.</returns>
        /// <param name="expression">Expression.</param>
        /// <param name="context">Context.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="indexesOfEscapedPlaceholders">Indexes of escaped placeholders.</param>
        async Task<string> GetResultWithReplacements(string expression,
                                                     ExpressionContext context,
                                                     CancellationToken cancellationToken,
                                                     IEnumerable<int> indexesOfEscapedPlaceholders)
        {
            var output = expression;

            var matches = placeholderReplacement.Matches(expression)
                .Cast<Match>()
                .ToList();
            // By starting at the end and working backwards, the index of any match
            // does not change until after it has been dealt-with.
            matches.Reverse();

            foreach(var match in matches)
            {
                // Skip matches which correspond to escaped placeholders
                if (indexesOfEscapedPlaceholders.Contains(match.Index))
                    continue;

                var replacementExpression = GetReplacementExpression(match);
                var expressionResult = await evaluator.EvaluateExpressionAsync(replacementExpression, context, cancellationToken)
                    .ConfigureAwait(false);

                output = output
                    .Remove(match.Index, match.Length)
                    .Insert(match.Index, expressionResult?.ToString() ?? String.Empty);
            }

            return output;
        }

        /// <summary>
        /// Gets the replacement expression, which is always a 'path' expression.
        /// </summary>
        /// <returns>The replacement expression.</returns>
        /// <param name="match">Match.</param>
        string GetReplacementExpression(Match match)
        {
            var expressionBody = match.Groups[1].Success ? match.Groups[1].Value : match.Groups[2].Value;
            return $"{WellKnownExpressionPrefix.Path}:{expressionBody}";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringExpressionEvaluator"/> class.
        /// </summary>
        /// <param name="evaluator">Evaluator.</param>
        public StringExpressionEvaluator(IEvaluatesExpression evaluator)
        {
            this.evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));
        }
    }
}
