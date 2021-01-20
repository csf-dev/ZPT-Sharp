using System;
namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// Represents a request to evaluate a path expression.  This could actually be a number of separate
    /// but very similarly-behaved expression types.
    /// </summary>
    public class PathExpressionEvaluationRequest
    {
        /// <summary>
        /// Gets the expression to be evaluated.
        /// </summary>
        /// <value>The expression.</value>
        public string Expression { get; }

        /// <summary>
        /// Gets the expression context.
        /// </summary>
        /// <value>The expression context.</value>
        public ExpressionContext ExpressionContext { get; }

        /// <summary>
        /// Gets a value which indicates how the <see cref="ExpressionContext"/> is to be limited.
        /// </summary>
        /// <value>The scope limitation.</value>
        public RootScopeLimitation ScopeLimitation { get; }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="PathExpressionEvaluationRequest"/> class.
        /// </summary>
        /// <param name="expression">Expression.</param>
        /// <param name="context">Context.</param>
        /// <param name="scopeLimitation">Scope limitation.</param>
        public PathExpressionEvaluationRequest(string expression,
                                               ExpressionContext context,
                                               RootScopeLimitation scopeLimitation = RootScopeLimitation.None)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            ExpressionContext = context ?? throw new ArgumentNullException(nameof(context));
            ScopeLimitation = scopeLimitation;
        }
    }
}
