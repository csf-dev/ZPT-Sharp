using System;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;

namespace ZptSharp.Rendering
{
    [TestFixture,Parallelizable]
    public class ZptDocumentRendererFactoryTests
    {
        [Test, AutoMoqData]
        public void GetDocumentRenderer_returns_instance(ZptDocumentRendererFactory sut)
        {
            Assert.That(() => sut.GetDocumentRenderer(), Is.Not.Null);
        }
    }
}
