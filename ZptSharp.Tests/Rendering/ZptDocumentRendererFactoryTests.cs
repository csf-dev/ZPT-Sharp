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
        public void GetDocumentRenderer_returns_instance([MockedConfig] RenderingConfig config,
                                                         ZptDocumentRendererFactory sut)
        {
            Assert.That(() => sut.GetDocumentRenderer(config), Is.Not.Null);
        }

        [Test, AutoMoqData]
        public void GetDocumentRenderer_throws_ANE_if_config_is_null(ZptDocumentRendererFactory sut)
        {
            Assert.That(() => sut.GetDocumentRenderer(null), Throws.ArgumentNullException);
        }
    }
}
