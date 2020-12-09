using System;
using NUnit.Framework;

namespace ZptSharp.Rendering
{
    [TestFixture, Parallelizable]
    public class UnknownSourceInfoTests
    {
        [Test,AutoMoqData]
        public void Name_returns_unknown(UnknownSourceInfo sut)
        {
            Assert.That(sut.Name, Is.EqualTo("<unknown>"));
        }

        [Test, AutoMoqData]
        public void ToString_returns_unknown(UnknownSourceInfo sut)
        {
            Assert.That(sut.ToString(), Is.EqualTo("<unknown>"));
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_if_reference_equal(UnknownSourceInfo sut)
        {
#pragma warning disable RECS0088 // Comparing equal expression for equality is usually useless
            Assert.That(sut.Equals(sut), Is.True);
#pragma warning restore RECS0088 // Comparing equal expression for equality is usually useless
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_if_not_reference_equal(UnknownSourceInfo sut, UnknownSourceInfo other)
        {
            Assert.That(sut.Equals(other), Is.False);
        }
    }
}
