using System;
using ZptSharp.Expressions;

namespace ZptSharp.PathExpressions
{
    /// <summary>
    /// A model which represents the context of evaluating a part of a path expression.
    /// </summary>
    public sealed class PathEvaluationContext
    {
        /// <summary>
        /// Gets the expression context associated with this evaluation context.
        /// </summary>
        /// <value>The expression context.</value>
        public ExpressionContext ExpressionContext { get; }

        /// <summary>
        /// Gets or sets the current object (the result of the previous traversal).
        /// This will be <see langword="null"/> if <see cref="IsRoot"/> is <see langword="true"/>.
        /// </summary>
        /// <value>The current object.</value>
        public object CurrentObject { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="PathEvaluationContext"/> is the root context in a traversal.
        /// If this is <see langword="true"/> then the <see cref="CurrentObject"/> will be <see langword="null"/>.
        /// </summary>
        /// <value><c>true</c> if this is the root context; otherwise, <c>false</c>.</value>
        public bool IsRoot { get; }

        /// <summary>
        /// Creates a child context, based upon the current instance but with a different <see cref="CurrentObject"/>.
        /// </summary>
        /// <returns>The child context.</returns>
        /// <param name="currentObject">The new 'current' object for the created context.</param>
        public PathEvaluationContext CreateChild(object currentObject)
            => new PathEvaluationContext(ExpressionContext, currentObject, false);

        PathEvaluationContext(ExpressionContext expressionContext, object currentObject, bool isRoot)
        {
            ExpressionContext = expressionContext ?? throw new ArgumentNullException(nameof(expressionContext));
            CurrentObject = currentObject;
            IsRoot = isRoot;
        }

        /// <summary>
        /// Creates a new root context instance.
        /// </summary>
        /// <returns>The root path evaluation context.</returns>
        /// <param name="expressionContext">The expression context.</param>
        public static PathEvaluationContext CreateRoot(ExpressionContext expressionContext)
            => new PathEvaluationContext(expressionContext, null, true);
    }
}
