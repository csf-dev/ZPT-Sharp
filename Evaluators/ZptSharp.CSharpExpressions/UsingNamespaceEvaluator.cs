using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions.CSharpExpressions
{
    /// <summary>
    /// Evaluator for 'using' expressions, which get a .NET namespace name
    /// object.  These will affect later C# expressions which are evaluated whilst
    /// this namespace is 'in scope'.
    /// </summary>
    public class UsingNamespaceEvaluator : IEvaluatesExpression
    {
        internal const string ExpressionPrefix = "using";

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
            => Task.FromResult<object>(new UsingNamespace(expression));
    }
}