using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.PathExpressions
{
    /// <summary>
    /// An implementation of <see cref="IWalksAndEvaluatesPathExpression"/> which walks each part of the
    /// path in sequence, using the specified context as a basis.  At each step during traversal,
    /// an <see cref="IGetsValueFromObject"/> is used to get the actual traversed value.
    /// </summary>
    public class PathWalkingExpressionEvaluator : IWalksAndEvaluatesPathExpression
    {
        readonly IGetsValueFromObject objectValueProvider;

        /// <summary>
        /// Walks the specified <paramref name="path"/> using the specified <paramref name="context"/>
        /// and gets a result.
        /// </summary>
        /// <returns>The evaluation result from walking the path &amp; context.</returns>
        /// <param name="path">The path the walk/traverse.</param>
        /// <param name="context">A path evaluation context.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task<object> WalkAndEvaluatePathExpressionAsync(PathExpression.AlternateExpression path,
                                                               PathEvaluationContext context,
                                                               CancellationToken cancellationToken = default)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return WalkAndEvaluatePathExpressionPrivateAsync(path, context, cancellationToken);
        }

        async Task<object> WalkAndEvaluatePathExpressionPrivateAsync(PathExpression.AlternateExpression path,
                                                                     PathEvaluationContext context,
                                                                     CancellationToken cancellationToken)
        {
            var ctx = context;

            foreach (var part in path.Parts)
            {
                var value = await GetValueFromPart(part, ctx, cancellationToken);
                ctx = ctx.CreateChild(value);
            }

            return ctx.CurrentObject;
        }

        async Task<object> GetValueFromPart(PathExpression.PathPart part,
                                            PathEvaluationContext context,
                                            CancellationToken cancellationToken)
        {


            var targetObject = context.IsRoot? context.ExpressionContext : context.CurrentObject;
            var valueName = await GetValueName(part, context, cancellationToken);

            var result = await objectValueProvider.TryGetValueAsync(valueName, targetObject, cancellationToken);
            if (result.Success) return result.Value;

            var objName = ReferenceEquals(targetObject, null) ? "<null>" : targetObject.ToString();
            var message = String.Format(Resources.ExceptionMessage.CannotTraversePathPart, part.Name, objName);
            throw new EvaluationException(message);
        }

        async Task<string> GetValueName(PathExpression.PathPart part,
                                        PathEvaluationContext context,
                                        CancellationToken cancellationToken)
        {
            if (!part.IsInterpolated) return part.Name;

            var result = await objectValueProvider.TryGetValueAsync(part.Name, context.ExpressionContext, cancellationToken);
            if (result.Success) return (string) result.Value;

            var objName = ReferenceEquals(context.ExpressionContext, null) ? "<null>" : context.ExpressionContext.ToString();
            var message = String.Format(Resources.ExceptionMessage.CannotTraversePathPart, part.Name, objName);
            throw new EvaluationException(message);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathWalkingExpressionEvaluator"/> class.
        /// </summary>
        /// <param name="objectValueProvider">Object value provider.</param>
        public PathWalkingExpressionEvaluator(IGetsValueFromObject objectValueProvider)
        {
            this.objectValueProvider = objectValueProvider ?? throw new ArgumentNullException(nameof(objectValueProvider));
        }
    }
}
