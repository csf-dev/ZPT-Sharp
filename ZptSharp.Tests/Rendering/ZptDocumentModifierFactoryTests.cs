using System;
using NUnit.Framework;
using ZptSharp.Autofixture;

namespace ZptSharp.Rendering
{
    [TestFixture, Parallelizable]
    public class ZptDocumentModifierFactoryTests
    {
        [Test, AutoMoqData]
        public void GetDocumentModifier_returns_instance([MockedConfig] RenderZptDocumentRequest request,
                                                         ZptDocumentModifierFactory sut)
        {
            Assert.That(() => sut.GetDocumentModifier(request), Is.Not.Null);
        }
    }
}
