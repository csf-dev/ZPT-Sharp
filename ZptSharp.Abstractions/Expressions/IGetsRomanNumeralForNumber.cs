using System;
namespace ZptSharp.Expressions
{
    /// <summary>
    /// An object which will get the roman numereral version of a specified non-zero, non-negative number.
    /// Roman numerals are generated and returned in uppercase, using the English/Latin alphabet (invariant culture).
    /// </summary>
    public interface IGetsRomanNumeralForNumber
    {
        /// <summary>
        /// Gets the roman numeral for the specified number.
        /// </summary>
        /// <returns>The roman numeral.</returns>
        /// <param name="number">Number.</param>
        string GetRomanNumeral(int number);
    }
}
