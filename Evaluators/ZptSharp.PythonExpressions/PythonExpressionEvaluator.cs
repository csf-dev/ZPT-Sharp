using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace ZptSharp.Expressions.PythonExpressions
{
    /// <summary>
    /// An implementation of <see cref="IEvaluatesExpression"/> which evaluates TALES 'python' expressions.
    /// </summary>
    public class PythonExpressionEvaluator : IEvaluatesExpression
    {
        internal const string ExpressionPrefix = "python";

        readonly IGetsAllVariablesFromContext variableProvider;
        readonly IEvaluatesPythonExpression pythonEvaluator;

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
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return EvaluateExpressionPrivateAsync(expression, context, cancellationToken);
        }

        async Task<object> EvaluateExpressionPrivateAsync(string expression,
                                                          ExpressionContext context,
                                                          CancellationToken cancellationToken)
        {
            var variables = await GetVariablesAsync(context).ConfigureAwait(false);
            return await pythonEvaluator.EvaluateExpressionAsync(expression, variables, cancellationToken)
                .ConfigureAwait(false);
        }

        async Task<IList<Variable>> GetVariablesAsync(ExpressionContext context)
        {
            var variables = await variableProvider.GetAllVariablesAsync(context).ConfigureAwait(false);
            return variables.Select(x => new Variable(x.Key, x.Value)).ToList();
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="PythonExpressionEvaluator"/> class.
        /// </summary>
        /// <param name="variableProvider">Variable provider.</param>
        /// <param name="pythonEvaluator">Python evaluator.</param>
        public PythonExpressionEvaluator(IGetsAllVariablesFromContext variableProvider,
                                         IEvaluatesPythonExpression pythonEvaluator)
        {
            this.variableProvider = variableProvider ?? throw new ArgumentNullException(nameof(variableProvider));
            this.pythonEvaluator = pythonEvaluator ?? throw new ArgumentNullException(nameof(pythonEvaluator));
        }
    }
}
