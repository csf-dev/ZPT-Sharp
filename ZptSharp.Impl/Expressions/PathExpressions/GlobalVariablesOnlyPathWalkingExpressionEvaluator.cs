using System;
namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// Special case of <see cref="PathWalkingExpressionEvaluator"/> which may be used via the interface
    /// <see cref="IWalksAndEvaluatesPathExpressionWithGlobalVariablesOnly"/>.  This type performs the same
    /// work as its base class, except that it uses a <see cref="IGetsValueFromObjectWithGlobalVariablesOnly"/>
    /// to ensure that only global variables are considered for path-evaluation.
    /// </summary>
    public class GlobalVariablesOnlyPathWalkingExpressionEvaluator : PathWalkingExpressionEvaluator, IWalksAndEvaluatesPathExpressionWithGlobalVariablesOnly
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GlobalVariablesOnlyPathWalkingExpressionEvaluator"/> class.
        /// </summary>
        /// <param name="objectValueProvider">Object value provider.</param>
        public GlobalVariablesOnlyPathWalkingExpressionEvaluator(IGetsValueFromObjectWithGlobalVariablesOnly objectValueProvider)
            : base(objectValueProvider) { }
    }
}
