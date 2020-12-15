using System;
namespace ZptSharp.Expressions
{
    /// <summary>
    /// An object which can get an alphabetic reference for a specified non-negative integer.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Alphabetic strings use the English/Latin alphabet (invariant culture).  All alphabetic
    /// strings returned are lowercase.
    /// </para>
    /// <example>
    /// <para>
    /// Here are some examples showing the numeric values of a number of alphabetic strings.
    /// </para>
    /// <list type="bullet">
    /// <item>"a" = 0</item>
    /// <item>"b" = 1</item>
    /// <item>"z" = 25</item>
    /// <item>"aa" = 26</item>
    /// <item>"az" = 51</item>
    /// <item>"ba" = 52</item>
    /// </list>
    /// </example>
    /// </remarks>
    public interface IGetsAlphabeticValueForNumber
    {
        /// <summary>
        /// Gets an alphabetic string for the the specified non-negative number.
        /// </summary>
        /// <returns>The alphabetic value.</returns>
        /// <param name="number">Number.</param>
        string GetAlphabeticValue(int number);
    }
}
