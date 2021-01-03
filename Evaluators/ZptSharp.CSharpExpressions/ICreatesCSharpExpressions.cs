using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions.CSharpExpressions
{
    /// <summary>
    /// An object which compiles and returns <see cref="CSharpExpression" /> instances,
    /// based upon their identifying information.
    /// </summary>
    public interface ICreatesCSharpExpressions
    {
        /// <summary>
        /// Gets the compiled expression.
        /// </summary>
        /// <param name="description">The expression identifier.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <returns>The compiled expression.</returns>
        Task<CSharpExpression> GetExpressionAsync(ExpressionDescription description, CancellationToken token = default);
    }
}