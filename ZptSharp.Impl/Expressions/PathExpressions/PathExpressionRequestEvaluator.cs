using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// Implementation of <see cref="IEvaluatesPathExpressionRequest"/> which evaluates path
    /// expression requests.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The main work done by this class is the parsing and splitting-up of the path expression
    /// into one or more <see cref="PathExpression.AlternateExpression"/> instances.  Each of these
    /// is evaluated independently, in order, until either one succeeds in returning a result
    /// or until they have thrown an exception.
    /// </para>
    /// <para>
    /// If an alternate expression returns a result then this is the result of the evaluation operation.
    /// If, on the other hand, we exhaust all of the alternate expressions and every one of them raises
    /// an exception, this the <see cref="EvaluateAsync(PathExpressionEvaluationRequest, CancellationToken)"/>
    /// method will itself raise an exception.
    /// </para>
    /// </remarks>
    public class PathExpressionRequestEvaluator : IEvaluatesPathExpressionRequest
    {
        readonly IGetsPathWalkingExpressionEvaluator walkingEvaluatorFactory;
        readonly IParsesPathExpression expressionParser;

        /// <summary>
        /// Evaluate the expression and return the result.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="request">Request.</param>
        /// <param name="token">Token.</param>
        public Task<object> EvaluateAsync(PathExpressionEvaluationRequest request, CancellationToken token = default)
        {
            var path = ParseExpression(request);
            var walkingEvaluator = walkingEvaluatorFactory.GetEvaluator(request.ScopeLimitation);
            return EvaluatePrivateAsync(walkingEvaluator,
                                        path,
                                        request.Expression,
                                        request.ExpressionContext,
                                        token);
        }

        static async Task<object> EvaluatePrivateAsync(IWalksAndEvaluatesPathExpression walkingEvaluator,
                                                       PathExpression path,
                                                       string expression,
                                                       ExpressionContext context,
                                                       CancellationToken token)
        {
            var exceptions = new List<Exception>();

            foreach (var alternateExpression in path.Alternates)
            {
                try
                {
                    var pathEvaluationContext = PathEvaluationContext.CreateRoot(context);
                    return await walkingEvaluator.WalkAndEvaluatePathExpressionAsync(alternateExpression,
                                                                                     pathEvaluationContext,
                                                                                     token);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            // If we reach this point then every alternate expression has failed to produce a result.
            // So, throw an aggregate exception containing all of the exceptions we found in the process.
            var message = String.Format(Resources.ExceptionMessage.CannotEvaluatePathExpressionAggregate, expression);
            throw new AggregateException(message, exceptions);
        }

        PathExpression ParseExpression(PathExpressionEvaluationRequest request)
        {
            try
            {
                return expressionParser.Parse(request.Expression);
            }
            catch (CannotParsePathException ex)
            {
                var message = String.Format(Resources.ExceptionMessage.CannotParsePathExpressionAttribute,
                                            request.Expression,
                                            request.ExpressionContext.CurrentElement);
                throw new CannotParsePathException(message, ex);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathExpressionRequestEvaluator"/> class.
        /// </summary>
        /// <param name="walkingEvaluatorFactory">Path-walking evaluator.</param>
        /// <param name="expressionParser">Expression parser.</param>
        public PathExpressionRequestEvaluator(IGetsPathWalkingExpressionEvaluator walkingEvaluatorFactory,
                                              IParsesPathExpression expressionParser)
        {
            this.walkingEvaluatorFactory = walkingEvaluatorFactory ?? throw new ArgumentNullException(nameof(walkingEvaluatorFactory));
            this.expressionParser = expressionParser ?? throw new ArgumentNullException(nameof(expressionParser));
        }
    }
}
