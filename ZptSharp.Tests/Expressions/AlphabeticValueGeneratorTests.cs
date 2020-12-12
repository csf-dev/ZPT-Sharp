using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;

namespace ZptSharp.Expressions
{
    [TestFixture,Parallelizable]
    public class AlphabeticValueGeneratorTests
    {
        [Test]
        public void GetAlphabeticValue_returns_correct_result([ValueSource(nameof(GetInputs))] int input)
        {
            var expected = wellKnownReferences[input];
            var sut = new AlphabeticValueGenerator();

            Assert.That(() => sut.GetAlphabeticValue(input), Is.EqualTo(expected));
        }

        public static IList<int> GetInputs() => wellKnownReferences.Keys.ToList();

        static readonly ReadOnlyDictionary<int, string>
            wellKnownReferences = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            {     0,    "a" },
            {     1,    "b" },
            {     5,    "f" },
            {    20,    "u" },
            {    25,    "z" },
            {    26,   "ba" },
            {    29,   "bd" },
            {    30,   "be" },
            {    51,   "bz" },
            {    52,   "ca" },
            {   500,   "tg" },
            {   600,   "xc" },
            {   626,   "yc" },
            {   650,   "za" },
            {   652,   "zc" },
            {   670,   "zu" },
            {   675,   "zz" },
            {   676,  "baa" },
            {   701,  "baz" },
            {   702,  "bba" },
            {   703,  "bbb" },
            {   729,  "bcb" },
            {  7020,  "kka" },
            { 18251, "bazz" },
            { 18252, "bbaa" },
        });
    }
}
