using System;
using System.IO;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;
using ZptSharp.Dom;

namespace ZptSharp.Rendering
{
    [TestFixture,Parallelizable]
    public class ZptRequestRendererTests
    {
        [Test, AutoMoqData]
        public void RenderAsync_returns_stream_using_correct_process(IReadsAndWritesDocument documentReaderWriter,
                                                                     IGetsDocumentModifier rendererFactory,
                                                                     [Frozen, ServiceProvider] IServiceProvider serviceProvider,
                                                                     IModifiesDocument renderer,
                                                                     ZptRequestRenderer sut,
                                                                     Stream input,
                                                                     Stream output,
                                                                     object model,
                                                                     IDocumentSourceInfo sourceInfo,
                                                                     IDocument document,
                                                                     [MockedConfig] RenderingConfig config,
                                                                     IStoresCurrentRenderingConfig configStore)
        {
            var request = new RenderZptDocumentRequest(input, model, config, sourceInfo: sourceInfo,  readerWriter: documentReaderWriter);
            Mock.Get(rendererFactory).Setup(x => x.GetDocumentModifier(request)).Returns(renderer);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IStoresCurrentRenderingConfig))).Returns(configStore);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsDocumentModifier))).Returns(rendererFactory);
            Mock.Get(rendererFactory).Setup(x => x.GetDocumentModifier(It.IsAny<RenderZptDocumentRequest>())).Returns(renderer);
            Mock.Get(documentReaderWriter)
                .Setup(x => x.GetDocumentAsync(input, config, sourceInfo, It.IsAny<System.Threading.CancellationToken>()))
                .Returns(() => Task.FromResult(document));
            Mock.Get(documentReaderWriter)
                .Setup(x => x.WriteDocumentAsync(document, config, It.IsAny<System.Threading.CancellationToken>()))
                .Returns(() => Task.FromResult(output));

            var result = sut.RenderAsync(request).Result;

            Assert.That(result, Is.SameAs(output), "Output stream is as returned from writer");
            Mock.Get(renderer)
                .Verify(x => x.ModifyDocumentAsync(document, request, It.IsAny<System.Threading.CancellationToken>()),
                        Times.Once,
                        "The rendering service should have been used with the document.");
        }

        [Test, AutoMoqData]
        public void RenderAsync_uses_reader_writer_from_service_provider_if_not_included_in_request(IReadsAndWritesDocument documentReaderWriter,
                                                                                                    [Frozen, ServiceProvider] IServiceProvider serviceProvider,
                                                                                                    [Frozen] IGetsDocumentModifier rendererFactory,
                                                                                                    IModifiesDocument renderer,
                                                                                                    ZptRequestRenderer sut,
                                                                                                    Stream input,
                                                                                                    Stream output,
                                                                                                    object model,
                                                                                                    IDocumentSourceInfo sourceInfo,
                                                                                                    IDocument document,
                                                                                                    [MockedConfig] RenderingConfig config,
                                                                                                    IStoresCurrentRenderingConfig configStore)
        {
            var request = new RenderZptDocumentRequest(input, model, config, sourceInfo: sourceInfo);
            Mock.Get(rendererFactory).Setup(x => x.GetDocumentModifier(request)).Returns(renderer);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IReadsAndWritesDocument))).Returns(documentReaderWriter);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IStoresCurrentRenderingConfig))).Returns(configStore);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsDocumentModifier))).Returns(rendererFactory);
            Mock.Get(rendererFactory).Setup(x => x.GetDocumentModifier(It.IsAny<RenderZptDocumentRequest>())).Returns(renderer);
            Mock.Get(documentReaderWriter)
                .Setup(x => x.GetDocumentAsync(input, config, sourceInfo, It.IsAny<System.Threading.CancellationToken>()))
                .Returns(() => Task.FromResult(document));
            Mock.Get(documentReaderWriter)
                .Setup(x => x.WriteDocumentAsync(document, config, It.IsAny<System.Threading.CancellationToken>()))
                .Returns(() => Task.FromResult(output));

            var result = sut.RenderAsync(request).Result;

            Assert.That(result, Is.SameAs(output), "Output stream is as returned from writer");
        }
    }
}
