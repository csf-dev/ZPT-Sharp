using System;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Rendering;

namespace ZptSharp.Hosting
{
    [TestFixture, Parallelizable]
    public class ZptSharpSelfHosterTests
    {
        [Test,AutoMoqData]
        public void FileRenderer_gets_service_from_service_provider([Frozen] IServiceProvider serviceProvider,
                                                                    ZptSharpSelfHoster sut,
                                                                    IRendersZptFile fileRenderer)
        {
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IRendersZptFile))).Returns(fileRenderer);
            Assert.That(() => sut.FileRenderer, Is.SameAs(fileRenderer));
        }

        [Test,AutoMoqData]
        public void DocumentRendererForPathFactory_gets_service_from_service_provider([Frozen] IServiceProvider serviceProvider,
                                                                                      ZptSharpSelfHoster sut,
                                                                                      IGetsZptDocumentRendererForFilePath docRenderer)
        {
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsZptDocumentRendererForFilePath))).Returns(docRenderer);
            Assert.That(() => sut.DocumentRendererForPathFactory, Is.SameAs(docRenderer));
        }
    }
}