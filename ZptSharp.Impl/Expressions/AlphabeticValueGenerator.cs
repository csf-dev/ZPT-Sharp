using System;
using System.Text;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// Implementation of <see cref="IGetsAlphabeticValueForNumber"/> which creates and returns alphabetic strings.
    /// </summary>
    public class AlphabeticValueGenerator : IGetsAlphabeticValueForNumber
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// Gets an alphabetic string for the the specified non-negative number.
        /// </summary>
        /// <returns>The alphabetic value.</returns>
        /// <param name="number">Number.</param>
        public string GetAlphabeticValue(int number)
        {
            if (number < 0) throw new ArgumentOutOfRangeException(nameof(number));

            var output = new StringBuilder();
            int next = number;

            do
            {
                var (higherOrderValue, thisCharacterIndex) = Divide(next);
                next = higherOrderValue;
                output.Insert(0, alphabet[thisCharacterIndex]);
            }
            while (next > 0);

            return output.ToString();
        }

        static (int, int) Divide(int value)
        {
            var divisor = alphabet.Length;
            var amount = (int) Math.Truncate((double) value / divisor);
            var remainder = value % divisor;
            return (amount, remainder);
        }
    }
}
