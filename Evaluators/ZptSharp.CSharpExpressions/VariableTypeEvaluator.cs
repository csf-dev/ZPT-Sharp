using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions.CSharpExpressions
{
    /// <summary>
    /// Evaluator for 'type' expressions, which inform the C# compiler of the type
    /// of a specified variable.  This is mainly required for the use of extension methods,
    /// which cannot be bound when treating them as <c>dynamic</c> objects.
    /// </summary>
    public class VariableTypeEvaluator : IEvaluatesExpression
    {
        internal const string ExpressionPrefix = "type";
        const string expressionMatcherPattern = @"^([a-zA-Z0-9_]+)\s+(.+)";

        static readonly Regex expressionMatcher = new Regex(expressionMatcherPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        /// Evaluates the expression asynchronously and returns the result.
        /// </summary>
        /// <returns>The expression result.</returns>
        /// <param name="expression">The expression string.</param>
        /// <param name="context">The expression context.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<object> EvaluateExpressionAsync(string expression,
                                                    ExpressionContext context,
                                                    CancellationToken cancellationToken = default)
        {
            if (expression is null)
                throw new System.ArgumentNullException(nameof(expression));
            var match = expressionMatcher.Match(expression);
            if(!match.Success)
            {
                var message = String.Format(Resources.ExceptionMessage.TypeDesignationMustBeSyntacticallyValid, expression, context.CurrentNode);
                throw new CSharpEvaluationException(message);
            }

            return Task.FromResult<object>(new VariableType(match.Groups[1].Value, match.Groups[2].Value));
        }
    }
}