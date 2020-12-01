using ZptSharp.Tal;

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
        /// Gets the boolean representation of the specified value.
        /// </summary>
        /// <returns>A boolean representation of the <paramref name="value"/>.</returns>
        /// <param name="value">Value.</param>
        bool CoerceToBoolean(object value);
    }
}
