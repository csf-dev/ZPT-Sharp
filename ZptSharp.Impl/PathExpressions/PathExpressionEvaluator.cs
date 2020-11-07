using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.PathExpressions
{
    /// <summary>
    /// An implementation of <see cref="IEvaluatesExpression"/> which evaluates TALES 'path' expressions.
    /// </summary>
    public class PathExpressionEvaluator : IEvaluatesExpression
    {
        readonly IParsesPathExpression expressionParser;

        /// <summary>
        /// Evaluates the expression asynchronously and returns the result.
        /// </summary>
        /// <returns>The expression result.</returns>
        /// <param name="expression">The expression string.</param>
        /// <param name="context">The expression context.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public async Task<object> EvaluateExpressionAsync(string expression, ExpressionContext context, CancellationToken cancellationToken = default)
        {
            var path = expressionParser.Parse(expression);

            var exceptions = new List<Exception>();

            foreach (var alternateExpression in path.Alternates)
            {
                try
                {
                    return await EvaluateExpressionAsync(alternateExpression, context, cancellationToken);
                }
                catch(AggregateException ex)
                {
                    exceptions.AddRange(ex.InnerExceptions);
                }
            }

            throw GetCannotEvaluateException(exceptions, expression);
        }

        Task<object> EvaluateExpressionAsync(PathExpression.AlternateExpression expression,
                                             ExpressionContext context,
                                             CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an appropriate exception to throw when expression evaluation fails.
        /// This might be a <see cref="EvaluationException"/> for a single exception
        /// or it might be an <see cref="AggregateException"/> if there were multiple exceptions
        /// encountered whilst evaluating.
        /// </summary>
        /// <returns>The evaluation exception.</returns>
        /// <param name="exceptions">Exceptions.</param>
        /// <param name="expression">Expression.</param>
        Exception GetCannotEvaluateException(List<Exception> exceptions, string expression)
        {
            string message;

            if (exceptions.Count == 1)
            {
                message = String.Format(Resources.ExceptionMessage.CannotEvaluatePathExpressionSingle, expression);
                return new EvaluationException(message, exceptions.First());
            }

            message = String.Format(Resources.ExceptionMessage.CannotEvaluatePathExpressionAggregate, expression);
            return new AggregateException(message, exceptions);
        }

        public PathExpressionEvaluator(IParsesPathExpression expressionParser)
        {
            this.expressionParser = expressionParser ?? throw new ArgumentNullException(nameof(expressionParser));
        }
    }
}
