using System;
namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// Special case of <see cref="PathWalkingExpressionEvaluator"/> which may be used via the interface
    /// <see cref="IWalksAndEvaluatesPathExpressionWithLocalVariablesOnly"/>.  This type performs the same
    /// work as its base class, except that it uses a <see cref="IGetsValueFromObjectWithLocalVariablesOnly"/>
    /// to ensure that only local variables are considered for path-evaluation.
    /// </summary>
    public class LocalVariablesOnlyPathWalkingExpressionEvaluator : PathWalkingExpressionEvaluator, IWalksAndEvaluatesPathExpressionWithLocalVariablesOnly
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="LocalVariablesOnlyPathWalkingExpressionEvaluator"/> class.
        /// </summary>
        /// <param name="objectValueProvider">Object value provider.</param>
        public LocalVariablesOnlyPathWalkingExpressionEvaluator(IGetsValueFromObjectWithLocalVariablesOnly objectValueProvider)
            : base(objectValueProvider) { }
    }
}
