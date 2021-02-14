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
        /// <remarks>
        /// <para>
        /// A TALES expression result is considered to be <c>false</c> by this mechanism if it is
        /// equal to the default of its data-type.  Otherwise, it is <c>true</c>.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// For example, a <c>System.String</c> or any expression result which is a reference type is coerced
        /// to <c>true</c> if it is not-null and to <c>false</c> if it is null.  This is because <c>null</c>
        /// is the default value for all reference types.
        /// </para>
        /// <para>
        /// A <c>System.Int32</c> or any expression result which is a value type is coerced to <c>false</c> if it
        /// is equal to its default value, or <c>true</c> if it is not.  For built-in numeric types that is zero,
        /// for <c>System.DateTime</c> that is the minimum value and for <c>System.Boolean</c> that is
        /// <c>false</c>.
        /// </para>
        /// </example>
        /// <returns><c>true</c>, if result should be treated as boolean truth, <c>false</c> otherwise.</returns>
        /// <param name="expressionResult">Expression result.</param>
        bool CoerceResultToBoolean(object expressionResult);
    }
}
