using System;
namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// Special case of <see cref="PathExpressionEvaluator"/> which will use only global variables as the root of
    /// a path expression to evaluate.
    /// </summary>
    public class GlobalVariablesOnlyPathExpressionEvaluator : PathExpressionEvaluator
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GlobalVariablesOnlyPathExpressionEvaluator"/> class.
        /// </summary>
        /// <param name="expressionParser">Expression parser.</param>
        /// <param name="pathWalker">Path walker.</param>
        public GlobalVariablesOnlyPathExpressionEvaluator(IParsesPathExpression expressionParser,
                                                         IWalksAndEvaluatesPathExpressionWithGlobalVariablesOnly pathWalker)
            : base(expressionParser, pathWalker) { }
    }
}
