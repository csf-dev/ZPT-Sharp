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
        /// Gets the boolean representation of the specified value.
        /// </summary>
        /// <returns>A boolean representation of the <paramref name="value"/>.</returns>
        /// <param name="value">Value.</param>
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
