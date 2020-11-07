using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.PathExpressions
{
    public class PathExpressionEvaluator : IEvaluatesExpression
    {
        public Task<object> EvaluateExpressionAsync(string expression, ExpressionContext context, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public PathExpressionEvaluator()
        {
        }
    }
}
