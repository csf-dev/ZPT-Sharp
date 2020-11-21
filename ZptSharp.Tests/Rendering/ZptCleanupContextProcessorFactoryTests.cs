using System;
using NUnit.Framework;

namespace ZptSharp.Rendering
{
    [TestFixture,Parallelizable]
    public class ZptCleanupContextProcessorFactoryTests
    {
        [Test, AutoMoqData]
        public void GetElementAndAttributeRemovalProcessor_returns_instance(ZptCleanupContextProcessorFactory sut)
        {
            Assert.That(() => sut.GetElementAndAttributeRemovalProcessor(), Is.Not.Null);
        }
    }
}
