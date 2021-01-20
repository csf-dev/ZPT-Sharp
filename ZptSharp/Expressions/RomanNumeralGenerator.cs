using System;
using System.Collections.Generic;
using System.Text;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// Implementation of <see cref="IGetsRomanNumeralForNumber"/> which generates roman numerals from integers.
    /// </summary>
    public class RomanNumeralGenerator : IGetsRomanNumeralForNumber
    {
        static readonly Dictionary<int, string> fixedNumberValues = new Dictionary<int, string>
        {
            { 1000, "M" },
            { 900,  "CM" },
            { 500,  "D" },
            { 400,  "CD" },
            { 100,  "C" },
            { 90,   "XC" },
            { 50,   "L" },
            { 40,   "XL" },
            { 10,   "X" },
            { 9,    "IX" },
            { 5,    "V" },
            { 4,    "IV" },
            { 1,    "I" },
        };

        /// <summary>
        /// Gets the roman numeral for the specified number.
        /// </summary>
        /// <returns>The roman numeral.</returns>
        /// <param name="number">Number.</param>
        public string GetRomanNumeral(int number)
        {
            if (number <= 0) throw new ArgumentOutOfRangeException(nameof(number));

            var output = new StringBuilder();

            foreach (var item in fixedNumberValues)
            {
                while (number >= item.Key)
                {
                    output.Append(item.Value);
                    number -= item.Key;
                }
            }

            return output.ToString();
        }
    }
}
