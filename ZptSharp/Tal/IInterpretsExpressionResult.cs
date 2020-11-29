namespace ZptSharp.Tal
{
    /// <summary>
    /// An object which interprets the result of a TALES expression, for the purposes of TAL.
    /// </summary>
    public interface IInterpretsExpressionResult
    {
        /// <summary>
        /// Gets a value which indicates whether or not the expression result cancels the current action.
        /// See also: <see cref="Expressions.AbortZptActionToken"/>.
        /// </summary>
        /// <returns><c>true</c> if the result cancels the action; otherwise, <c>false</c>.</returns>
        /// <param name="expressionResult">Expression result.</param>
        bool DoesResultAbortTheAction(object expressionResult);

        /// <summary>
        /// Gets a value which is the boolean representation of the specified result,
        /// following the rules of boolean type coercion defined in TALES.
        /// </summary>
        /// <returns><c>true</c>, if result should be treated as boolean truth, <c>false</c> otherwise.</returns>
        /// <param name="expressionResult">Expression result.</param>
        bool CoerceResultToBoolean(object expressionResult);
    }
}
