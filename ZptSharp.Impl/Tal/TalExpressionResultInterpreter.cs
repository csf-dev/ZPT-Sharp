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
        /// following the rules of boolean type coercion defined in TALES.
        /// </summary>
        /// <returns><c>true</c>, if result should be treated as boolean truth, <c>false</c> otherwise.</returns>
        /// <param name="expressionResult">Expression result.</param>
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
        public bool DoesResultCancelTheAction(object expressionResult)
            => Equals(expressionResult, AbortZptActionToken.Instance);

        static object GetDefaultOfType(object value)
        {
            if (value is null) return null;

            var valueType = value.GetType();
            return valueType.IsValueType? Activator.CreateInstance(valueType) : null;
        }
    }
}
