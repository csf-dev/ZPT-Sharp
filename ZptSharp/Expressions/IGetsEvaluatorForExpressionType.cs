namespace ZptSharp.Expressions
{
    /// <summary>
    /// An object which chooses a strategy (an appropriate implementation of <see cref="IEvaluatesExpression"/>)
    /// for a specified expression type.
    /// </summary>
    public interface IGetsEvaluatorForExpressionType
    {
        /// <summary>
        /// Gets the evaluator which is appropriate for use with the specified <paramref name="expressionType"/>.
        /// </summary>
        /// <returns>The evaluator.</returns>
        /// <param name="expressionType">Expression type.</param>
        IEvaluatesExpression GetEvaluator(string expressionType);
    }
}
