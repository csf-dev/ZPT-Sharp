using System;
using System.Collections;
using System.Linq;
using ZptSharp.Tal;

namespace ZptSharp.Expressions.NotExpressions
{
    /// <summary>
    /// Implementation of <see cref="ICoercesValueToBoolean"/> which converts a value
    /// to a boolean.
    /// </summary>
    public class BooleanValueConverter : ICoercesValueToBoolean
    {
        readonly IInterpretsExpressionResult resultInterpreter;

        /// <summary>
        /// Gets the boolean representation of the specified value, essentially determining whether it is
        /// 'truthy' or 'falsey'.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The ZPT standard states that the following values are treated as <c>false</c>.
        /// </para>
        /// <list type="bullet">
        /// <item><description>An instance of <see cref="AbortZptActionToken"/>.</description></item>
        /// <item><description>A <see langword="null"/> reference.</description></item>
        /// <item><description>The value <see cref="Boolean"/> <c>false</c>.</description></item>
        /// <item><description>Any value where <see cref="Object.Equals(object, object)"/> treats that value as equal to zero.</description></item>
        /// <item><description>Empty enumerables, IE any object which implements <see cref="IEnumerable"/> which has no items.</description></item>
        /// </list>
        /// <para>
        /// All other values are treated as <c>true</c> by the ZPT specification.
        /// </para>
        /// </remarks>
        /// <returns>A boolean representation of the <paramref name="value"/>.</returns>
        /// <param name="value">The value to test for 'truthiness'.</param>
        public bool CoerceToBoolean(object value)
        {
            if (resultInterpreter.DoesResultAbortTheAction(value)) return false;
            if (value == null) return false;
            if (value is bool boolValue) return boolValue;
            if (Equals(value, 0)) return false;
            if (value is IEnumerable enumerable) return enumerable.Cast<object>().Any();

            return true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanValueConverter"/> class.
        /// </summary>
        /// <param name="resultInterpreter">Result interpreter.</param>
        public BooleanValueConverter(IInterpretsExpressionResult resultInterpreter)
        {
            this.resultInterpreter = resultInterpreter ?? throw new ArgumentNullException(nameof(resultInterpreter));
        }
    }
}
