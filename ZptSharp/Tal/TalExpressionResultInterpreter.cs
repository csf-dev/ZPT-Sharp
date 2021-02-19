using System;
using ZptSharp.Expressions;

namespace ZptSharp.Tal
{
    /// <summary>
    /// Implementation of <see cref="IInterpretsExpressionResult"/> which provides
    /// interpretations of expression results, following the rules of TAL.
    /// </summary>
    public class TalExpressionResultInterpreter : IInterpretsExpressionResult
    {
        /// <summary>
        /// Gets a value which is the boolean representation of the specified result,
        /// following the rules of boolean type coercion for ZPT.
        /// This is used by various TAL attributes to determine whether a value is 'truthy' or 'falsey'.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method will return <c>false</c> if the <paramref name="expressionResult"/> is equal
        /// to the default of its data-type, otherwise it returns <c>true</c>.
        /// </para>
        /// <para>
        /// For any data-type, the default of the type is what would be returned by <c>default(T)</c> for that
        /// type T.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// For <c>System.String</c> this method returns <c>false</c> if the value is <c>null</c>, otherwise
        /// it returns <c>true</c>.
        /// </para>
        /// <para>
        /// For <c>System.Int32</c> this method returns <c>false</c> if the value is equal to zero, otherwise
        /// it returns <c>true</c>.
        /// </para>
        /// </example>
        /// <returns><c>true</c>, if result should be treated as boolean truth, <c>false</c> otherwise.</returns>
        /// <param name="expressionResult">An expression result.</param>
        public bool CoerceResultToBoolean(object expressionResult)
        {
            var defaultOfType = GetDefaultOfType(expressionResult);
            return !Equals(expressionResult, defaultOfType);
        }

        /// <summary>
        /// Gets a value which indicates whether or not the expression result cancels the current action.
        /// See also: <see cref="T:ZptSharp.Expressions.AbortZptActionToken"/>.
        /// </summary>
        /// <returns><c>true</c> if the result cancels the action; otherwise, <c>false</c>.</returns>
        /// <param name="expressionResult">Expression result.</param>
        public bool DoesResultAbortTheAction(object expressionResult)
            => Equals(expressionResult, AbortZptActionToken.Instance);

        static object GetDefaultOfType(object value)
        {
            if (value is null) return null;

            var valueType = value.GetType();
            return valueType.IsValueType? Activator.CreateInstance(valueType) : null;
        }
    }
}
