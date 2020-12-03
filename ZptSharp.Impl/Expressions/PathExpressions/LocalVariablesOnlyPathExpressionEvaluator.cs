using System;
namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// Special case of <see cref="PathExpressionEvaluator"/> which will use only local variables as the root of
    /// a path expression to evaluate.
    /// </summary>
    public class LocalVariablesOnlyPathExpressionEvaluator : PathExpressionEvaluator
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="LocalVariablesOnlyPathExpressionEvaluator"/> class.
        /// </summary>
        /// <param name="expressionParser">Expression parser.</param>
        /// <param name="pathWalker">Path walker.</param>
        public LocalVariablesOnlyPathExpressionEvaluator(IParsesPathExpression expressionParser,
                                                         IWalksAndEvaluatesPathExpressionWithLocalVariablesOnly pathWalker)
            : base(expressionParser, pathWalker) { }
    }
}
