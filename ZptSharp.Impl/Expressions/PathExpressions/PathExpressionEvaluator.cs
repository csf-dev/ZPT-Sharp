using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// An implementation of <see cref="IEvaluatesExpression"/> which evaluates TALES 'path' expressions.
    /// </summary>
    public class PathExpressionEvaluator : IEvaluatesExpression
    {
        readonly IParsesPathExpression expressionParser;
        readonly IWalksAndEvaluatesPathExpression pathWalker;

        /// <summary>
        /// Evaluates the expression asynchronously and returns the result.
        /// </summary>
        /// <returns>The expression result.</returns>
        /// <param name="expression">The expression string.</param>
        /// <param name="context">The expression context.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<object> EvaluateExpressionAsync(string expression, ExpressionContext context, CancellationToken cancellationToken = default)
        {
            var path = ParseExpression(expression, context);

            var exceptions = new List<Exception>();

            foreach (var alternateExpression in path.Alternates)
            {
                try
                {
                    var task = EvaluateExpressionAsync(alternateExpression, context, cancellationToken);
                    // We must wait for the task to complete in order to force it to throw an exception here
                    // if it throws at all.
                    task.Wait();
                    return task;
                }
                catch(Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw GetCannotEvaluateException(exceptions, expression);
        }

        PathExpression ParseExpression(string expression, ExpressionContext context)
        {
            try
            {
                return expressionParser.Parse(expression);
            }
            catch(CannotParsePathException ex)
            {
                var message = String.Format(Resources.ExceptionMessage.CannotParsePathExpressionAttribute, expression, context.CurrentElement);
                throw new CannotParsePathException(message, ex);
            }
        }

        Task<object> EvaluateExpressionAsync(PathExpression.AlternateExpression path,
                                             ExpressionContext context,
                                             CancellationToken cancellationToken)
        {
            var pathEvaluationContext = PathEvaluationContext.CreateRoot(context);
            return pathWalker.WalkAndEvaluatePathExpressionAsync(path, pathEvaluationContext, cancellationToken);
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

        /// <summary>
        /// Initializes a new instance of the <see cref="PathExpressionEvaluator"/> class.
        /// </summary>
        /// <param name="expressionParser">Expression parser.</param>
        /// <param name="pathWalker">Path walker.</param>
        public PathExpressionEvaluator(IParsesPathExpression expressionParser,
                                       IWalksAndEvaluatesPathExpression pathWalker)
        {
            this.expressionParser = expressionParser ?? throw new ArgumentNullException(nameof(expressionParser));
            this.pathWalker = pathWalker ?? throw new ArgumentNullException(nameof(pathWalker));
        }
    }
}
