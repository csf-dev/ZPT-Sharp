using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// An object which can walk a path described by an <see cref="PathExpression.AlternateExpression"/>
    /// and get a result.
    /// </summary>
    public interface IWalksAndEvaluatesPathExpression
    {
        /// <summary>
        /// Walks the specified <paramref name="path"/> using the specified <paramref name="context"/>
        /// and gets a result.
        /// </summary>
        /// <returns>The evaluation result from walking the path &amp; context.</returns>
        /// <param name="path">The path the walk/traverse.</param>
        /// <param name="context">A path evaluation context.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<object> WalkAndEvaluatePathExpressionAsync(PathExpression.AlternateExpression path,
                                                        PathEvaluationContext context,
                                                        CancellationToken cancellationToken = default);
    }
}
