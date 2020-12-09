using System;
using AutoFixture.NUnit3;
using NUnit.Framework;
using ZptSharp.Dom;

namespace ZptSharp.Metal
{
    [TestFixture,Parallelizable]
    public class MetalDocumentAdapterFactoryTests
    {
        [Test, AutoMoqData]
        public void GetMetalDocumentAdapter_returns_instance([Frozen] ISearchesForAttributes attributeSearcher,
                                                             [Frozen] IGetsMetalAttributeSpecs specProvider,
                                                             MetalDocumentAdapterFactory sut,
                                                             IDocument doc)
        {
            Assert.That(() => sut.GetMetalDocumentAdapter(doc), Is.InstanceOf<MetalDocumentAdapter>());
        }
    }
}
