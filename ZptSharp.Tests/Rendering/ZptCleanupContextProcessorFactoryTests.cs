using System;
using NUnit.Framework;

namespace ZptSharp.Rendering
{
    [TestFixture,Parallelizable]
    public class ZptCleanupContextProcessorFactoryTests
    {
        [Test, AutoMoqData]
        public void GetNodeAndAttributeRemovalProcessor_returns_instance(ZptCleanupContextProcessorFactory sut)
        {
            Assert.That(() => sut.GetNodeAndAttributeRemovalProcessor(), Is.Not.Null);
        }
    }
}
