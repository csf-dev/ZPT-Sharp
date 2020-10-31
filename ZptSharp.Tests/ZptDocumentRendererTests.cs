using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;
using ZptSharp.Rendering;

namespace ZptSharp
{
    [TestFixture,Parallelizable]
    public class ZptDocumentRendererTests
    {
        [Test, AutoMoqData]
        public void RenderAsync_uses_rendering_service_from_service_provider([Frozen,MockedConfig] RenderingConfig config,
                                                                             [Frozen] IServiceProvider serviceProvider,
                                                                             ZptDocumentRenderer sut,
                                                                             Stream stream,
                                                                             object model,
                                                                             CancellationToken token,
                                                                             IRendersRenderingRequest renderer)
        {
            Mock.Get(config).SetupGet(x => x.ServiceProvider).Returns(serviceProvider);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IRendersRenderingRequest))).Returns(renderer);
            Action<IConfiguresRootContext> contextBuilder = c => {};

            sut.RenderAsync(stream, model, token, contextBuilder);

            Mock.Get(renderer)
                .Verify(x => x.RenderAsync(It.Is<RenderZptDocumentRequest>(r => r.Config == config
                                                                                && r.DocumentStream == stream
                                                                                && r.Model == model
                                                                                && r.ContextBuilder == contextBuilder),
                                           token),
                        Times.Once);
        }

        [Test, AutoMoqData]
        public void RenderAsync_uses_fallback_context_builder_if_none_provided([Frozen, MockedConfig] RenderingConfig config,
                                                                               [Frozen] IServiceProvider serviceProvider,
                                                                               ZptDocumentRenderer sut,
                                                                               Stream stream,
                                                                               object model,
                                                                               CancellationToken token,
                                                                               IRendersRenderingRequest renderer)
        {
            Mock.Get(config).SetupGet(x => x.ServiceProvider).Returns(serviceProvider);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IRendersRenderingRequest))).Returns(renderer);

            sut.RenderAsync(stream, model, token);

            Mock.Get(renderer)
                .Verify(x => x.RenderAsync(It.Is<RenderZptDocumentRequest>(r => r.ContextBuilder != null), token),
                        Times.Once);
        }

        [Test, AutoMoqData]
        public void RenderAsync_returns_result_from_renderer_service([Frozen, MockedConfig] RenderingConfig config,
                                                                     [Frozen] IServiceProvider serviceProvider,
                                                                     ZptDocumentRenderer sut,
                                                                     Stream input,
                                                                     Stream output,
                                                                     object model,
                                                                     CancellationToken token,
                                                                     IRendersRenderingRequest renderer)
        {
            Mock.Get(config).SetupGet(x => x.ServiceProvider).Returns(serviceProvider);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IRendersRenderingRequest))).Returns(renderer);
            Mock.Get(renderer)
                .Setup(x => x.RenderAsync(It.IsAny<RenderZptDocumentRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(output));

            Assert.That(() => sut.RenderAsync(input, model, token).Result, Is.SameAs(output));
        }

        [Test, AutoMoqData]
        public void RenderAsync_throws_ANE_if_the_stream_is_null([Frozen, MockedConfig] RenderingConfig config,
                                                                 ZptDocumentRenderer sut,
                                                                 object model,
                                                                 CancellationToken token)
        {
            Assert.That(() => sut.RenderAsync((Stream) null, model, token), Throws.InstanceOf<ArgumentNullException>());
        }
    }
}
