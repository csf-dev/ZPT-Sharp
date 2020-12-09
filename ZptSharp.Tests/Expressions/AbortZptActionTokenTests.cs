using System;
using NUnit.Framework;

namespace ZptSharp.Expressions
{
    [TestFixture,Parallelizable]
    public class AbortZptActionTokenTests
    {
        [Test, AutoMoqData]
        public void Equals_returns_true_for_another_instance()
        {
#pragma warning disable RECS0088 // Comparing equal expression for equality is usually useless
            Assert.That(() => new AbortZptActionToken().Equals(new AbortZptActionToken()), Is.True);
#pragma warning restore RECS0088 // Comparing equal expression for equality is usually useless
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_another_object(AbortZptActionToken sut, object obj)
        {
            Assert.That(() => sut.Equals(obj), Is.False);
        }

        [Test, AutoMoqData]
        public void GetHashCode_returns_zero(AbortZptActionToken sut)
        {
            Assert.That(() => sut.GetHashCode(), Is.Zero);
        }
    }
}
