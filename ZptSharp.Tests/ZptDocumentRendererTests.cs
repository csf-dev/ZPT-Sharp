using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;
using ZptSharp.Dom;
using ZptSharp.Rendering;

namespace ZptSharp
{
    [TestFixture,Parallelizable]
    public class ZptDocumentRendererTests
    {
        [Test, AutoMoqData]
        public void RenderAsync_uses_rendering_service_from_service_provider([MockedConfig] RenderingConfig config,
                                                                             [Frozen] IServiceProvider serviceProvider,
                                                                             ZptDocumentRenderer sut,
                                                                             Stream stream,
                                                                             object model,
                                                                             IRendersRenderingRequest renderer)
        {
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IRendersRenderingRequest))).Returns(renderer);

            sut.RenderAsync(stream, model, config);

            Mock.Get(renderer)
                .Verify(x => x.RenderAsync(It.Is<RenderZptDocumentRequest>(r => r.DocumentStream == stream
                                                                                && r.Model == model),
                                           It.IsAny<RenderingConfig>(),
                                           CancellationToken.None),
                        Times.Once);
        }

        [Test, AutoMoqData]
        public void RenderAsync_uses_same_config_if_it_is_not_null_and_no_document_provider_specified([MockedConfig] RenderingConfig config,
                                                                                                      IServiceProvider serviceProvider,
                                                                                                      Stream stream,
                                                                                                      object model,
                                                                                                      IRendersRenderingRequest renderer)
        {
            var sut = new ZptDocumentRenderer(serviceProvider);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IRendersRenderingRequest))).Returns(renderer);

            sut.RenderAsync(stream, model, config);

            Mock.Get(renderer)
                .Verify(x => x.RenderAsync(It.IsAny<RenderZptDocumentRequest>(),
                                           config,
                                           CancellationToken.None),
                        Times.Once);
        }

        [Test, AutoMoqData]
        public void RenderAsync_uses_config_with_overridden_document_provider_when_specified([MockedConfig] RenderingConfig config,
                                                                                             [Frozen] System.Type provider,
                                                                                             [Frozen] IServiceProvider serviceProvider,
                                                                                             ZptDocumentRenderer sut,
                                                                                             Stream stream,
                                                                                             object model,
                                                                                             IRendersRenderingRequest renderer)
        {
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IRendersRenderingRequest))).Returns(renderer);

            sut.RenderAsync(stream, model, config);

            Mock.Get(renderer)
                .Verify(x => x.RenderAsync(It.IsAny<RenderZptDocumentRequest>(),
                                           It.Is<RenderingConfig>(c => c.DocumentProviderType == provider),
                                           CancellationToken.None),
                        Times.Once);
        }

        [Test, AutoMoqData]
        public void RenderAsync_returns_result_from_renderer_service([MockedConfig] RenderingConfig config,
                                                                     [Frozen] IServiceProvider serviceProvider,
                                                                     ZptDocumentRenderer sut,
                                                                     Stream input,
                                                                     Stream output,
                                                                     object model,
                                                                     CancellationToken token,
                                                                     IRendersRenderingRequest renderer)
        {
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IRendersRenderingRequest))).Returns(renderer);
            Mock.Get(renderer)
                .Setup(x => x.RenderAsync(It.IsAny<RenderZptDocumentRequest>(), It.IsAny<RenderingConfig>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(output));

            Assert.That(() => sut.RenderAsync(input, model, config, token).Result, Is.SameAs(output));
        }

        [Test, AutoMoqData]
        public void RenderAsync_throws_ANE_if_the_stream_is_null([MockedConfig] RenderingConfig config,
                                                                 ZptDocumentRenderer sut,
                                                                 object model,
                                                                 CancellationToken token)
        {
            Assert.That(() => sut.RenderAsync(null, model, config, token), Throws.InstanceOf<ArgumentNullException>());
        }
    }
}
