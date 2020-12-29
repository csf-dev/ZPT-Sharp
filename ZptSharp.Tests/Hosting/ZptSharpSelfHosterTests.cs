using System;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

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
        public void DocumentRenderer_gets_service_from_service_provider([Frozen] IServiceProvider serviceProvider,
                                                                    ZptSharpSelfHoster sut,
                                                                    IRendersZptDocument docRenderer)
        {
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IRendersZptDocument))).Returns(docRenderer);
            Assert.That(() => sut.DocumentRenderer, Is.SameAs(docRenderer));
        }
    }
}