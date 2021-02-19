using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;

namespace ZptSharp.Expressions
{
    [TestFixture, Parallelizable]
    public class RomanNumeralGeneratorTests
    {
        [Test]
        public void GetRomanNumeral_returns_correct_result([ValueSource(nameof(GetInputs))] int input)
        {
            var expected = wellKnownReferences[input];
            var sut = new RomanNumeralGenerator();

            Assert.That(() => sut.GetRomanNumeral(input), Is.EqualTo(expected));
        }

        public static IList<int> GetInputs() => wellKnownReferences.Keys.ToList();

        static readonly ReadOnlyDictionary<int, string>
            wellKnownReferences = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            {   1,        "I" },
            {   2,       "II" },
            {   4,       "IV" },
            {  26,     "XXVI" },
            {  27,    "XXVII" },
            {  44,     "XLIV" },
            {  52,      "LII" },
            {  53,     "LIII" },
            { 676,  "DCLXXVI" },
            { 677, "DCLXXVII" },
        });
    }
}
