using System;
using NUnit.Framework;

namespace ZptSharp.Tal
{
    [TestFixture,Parallelizable]
    public class TalContextProcessorFactoryTests
    {
        [Test, AutoMoqData]
        public void GetTalContextProcessor_returns_instance(TalContextProcessorFactory sut)
        {
            Assert.That(() => sut.GetTalContextProcessor(), Is.Not.Null);
        }
    }
}
