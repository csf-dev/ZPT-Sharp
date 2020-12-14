using System;
namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// An object which can create and return instances of <see cref="IWalksAndEvaluatesPathExpression"/>
    /// based upon a <see cref="RootScopeLimitation"/>
    /// </summary>
    public interface IGetsPathWalkingExpressionEvaluator
    {
        /// <summary>
        /// Gets an instance of <see cref="IWalksAndEvaluatesPathExpression"/> suitable for use with the
        /// specified root scope <paramref name="limitation"/>.
        /// </summary>
        /// <returns>The evaluator.</returns>
        /// <param name="limitation">Limitation.</param>
        IWalksAndEvaluatesPathExpression GetEvaluator(RootScopeLimitation limitation);
    }
}
