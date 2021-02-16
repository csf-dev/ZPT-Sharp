using ZptSharp.Tal;
using System;
using System.Collections;

namespace ZptSharp.Expressions.NotExpressions
{
    /// <summary>
    /// An object which converts a specified value to a boolean.  Specifically, this
    /// uses rules unique to a TALES 'not' expression, and not the rules shared by
    /// <see cref="IInterpretsExpressionResult.CoerceResultToBoolean(object)"/>.s
    /// </summary>
    public interface ICoercesValueToBoolean
    {
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
        bool CoerceToBoolean(object value);
    }
}
