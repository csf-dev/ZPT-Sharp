using System;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Rendering
{
    [TestFixture,Parallelizable]
    public class ZptDocumentRendererFactoryTests
    {
        [Test, AutoMoqData]
        public void GetDocumentRenderer_returns_instance([Frozen] IServiceProvider provider,
                                                         ZptDocumentRendererFactory sut)
        {
            Assert.That(() => sut.GetDocumentRenderer(), Is.Not.Null);
        }
    }
}
