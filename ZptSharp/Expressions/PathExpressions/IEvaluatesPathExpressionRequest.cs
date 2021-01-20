using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// An object which can evaluate a <see cref="PathExpressionEvaluationRequest"/> and return
    /// a result.
    /// </summary>
    public interface IEvaluatesPathExpressionRequest
    {
        /// <summary>
        /// Evaluate the expression and return the result.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="request">Request.</param>
        /// <param name="token">Token.</param>
        Task<object> EvaluateAsync(PathExpressionEvaluationRequest request, CancellationToken token = default);
    }
}
