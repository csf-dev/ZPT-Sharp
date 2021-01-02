using System.Threading;
using System.Threading.Tasks;
using System;
using System.Text.RegularExpressions;

namespace ZptSharp.Expressions.PipeExpressions
{
    /// <summary>
    /// An implementation of <see cref="IEvaluatesExpression"/> which evaluates TALES 'pipe' expressions.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A pipe expression is essentially an invocation of a <see cref="Func{T,TResult}" /> upon an existing
    /// variable.  The function delegate's result/output type is always <see cref="Object" />, although the
    /// input parameter may vary in type.
    /// </para>
    /// <para>
    /// The result of the expression is the output of the function delegate, when executed against the
    /// specified variable.  Pipe expressions are most useful for transforming or formatting values for output.
    /// </para>
    /// </remarks>
    public class PipeExpressionEvaluator : IEvaluatesExpression
    {
        const string pipePattern = @"^([a-zA-Z_][a-zA-Z0-9_]*)\s+(.+)$";
        static readonly Regex pipe = new Regex(pipePattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        readonly IEvaluatesExpression evaluator;
        readonly IEvaluatesPipeDelegate delegateEvaluator;

        /// <summary>
        /// Evaluates the expression asynchronously and returns the result.
        /// </summary>
        /// <returns>The expression result.</returns>
        /// <param name="expression">The expression string.</param>
        /// <param name="context">The expression context.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<object> EvaluateExpressionAsync(string expression, ExpressionContext context, CancellationToken cancellationToken = default)
        {
            if (expression is null)
                throw new ArgumentNullException(nameof(expression));
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var (sourceVariable, pipeExpression) = GetVariableAndPipeExpression(expression, context);
            return EvaluateExpressionPrivateAsync(sourceVariable, pipeExpression, context, cancellationToken);
        }

        async Task<object> EvaluateExpressionPrivateAsync(string sourceVariable,
                                                          string pipeExpression,
                                                          ExpressionContext context,
                                                          CancellationToken cancellationToken)
        {
            var sourceObject = await GetSourceObjectAsync(sourceVariable, context, cancellationToken)
                .ConfigureAwait(false);
            var pipeDelegate = await GetDelegateObjectAsync(pipeExpression, context, cancellationToken)
                .ConfigureAwait(false);
            
            try
            {
                return delegateEvaluator.EvaluateDelegate(sourceObject, pipeDelegate);
            }
            catch(Exception ex)
            {
                var message = String.Format(Resources.ExceptionMessage.CannotExecutePipeExpressionDelegate, context.CurrentNode);
                throw new PipeExpressionException(message, ex);
            }
        }

        async Task<object> GetSourceObjectAsync(string sourceVariable,
                                                ExpressionContext context,
                                                CancellationToken cancellationToken)
        {
            try
            {
                return await evaluator.EvaluateExpressionAsync($"{WellKnownExpressionPrefix.Path}:{sourceVariable}",
                                                               context,
                                                               cancellationToken)
                    .ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                var message = String.Format(Resources.ExceptionMessage.CannotGetPipeExpressionSourceObject, context.CurrentNode);
                throw new PipeExpressionException(message, ex);
            }
        }

        async Task<object> GetDelegateObjectAsync(string pipeExpression,
                                                  ExpressionContext context,
                                                  CancellationToken cancellationToken)
        {
            try
            {
                return await evaluator.EvaluateExpressionAsync(pipeExpression,
                                                               context,
                                                               cancellationToken)
                    .ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                var message = String.Format(Resources.ExceptionMessage.CannotGetPipeExpressionDelegate, context.CurrentNode);
                throw new PipeExpressionException(message, ex);
            }
        }

        (string,string) GetVariableAndPipeExpression(string expression, ExpressionContext context)
        {
            var pipeMatch = pipe.Match(expression);
            if(!pipeMatch.Success)
            {
                var message = String.Format(Resources.ExceptionMessage.PipeExpressionIsSyntacticallyInvalid,
                                            context.CurrentNode,
                                            expression);
                throw new CannotParsePipeExpressionException(message);
            }

            return (pipeMatch.Groups[1].Value, pipeMatch.Groups[2].Value);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PipeExpressionEvaluator" />.
        /// </summary>
        /// <param name="evaluator">An expression evaluator.</param>
        /// <param name="delegateEvaluator">A delegate evaluator.</param>
        public PipeExpressionEvaluator(IEvaluatesExpression evaluator,
                                       IEvaluatesPipeDelegate delegateEvaluator)
        {
            this.evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));
            this.delegateEvaluator = delegateEvaluator ?? throw new ArgumentNullException(nameof(delegateEvaluator));
        }
    }
}