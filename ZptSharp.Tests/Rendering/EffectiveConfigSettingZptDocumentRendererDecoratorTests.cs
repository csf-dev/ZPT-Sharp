using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;

namespace ZptSharp.Rendering
{
    [TestFixture,Parallelizable]
    public class EffectiveConfigSettingZptDocumentRendererDecoratorTests
    {
        [Test, AutoMoqData]
        public void RenderAsync_uses_wrapped_rendering_service([MockedConfig] RenderingConfig config,
                                                               [Frozen] IRendersZptDocument wrapped,
                                                               EffectiveConfigSettingZptDocumentRendererDecorator sut,
                                                               Stream stream,
                                                               object model)
        {
            sut.RenderAsync(stream, model, config);

            Mock.Get(wrapped)
                .Verify(x => x.RenderAsync(stream,
                                           model,
                                           It.IsAny<RenderingConfig>(),
                                           CancellationToken.None,
                                           null),
                        Times.Once);
        }

        [Test, AutoMoqData]
        public void RenderAsync_uses_same_config_if_it_is_not_null_and_no_document_provider_specified([MockedConfig] RenderingConfig config,
                                                                                                      IRendersZptDocument wrapped,
                                                                                                      Stream stream,
                                                                                                      object model)
        {
            var sut = new EffectiveConfigSettingZptDocumentRendererDecorator(wrapped);

            sut.RenderAsync(stream, model, config);

            Mock.Get(wrapped)
                .Verify(x => x.RenderAsync(stream,
                                           model,
                                           It.IsAny<RenderingConfig>(),
                                           CancellationToken.None,
                                           null),
                        Times.Once);
        }

        [Test, AutoMoqData]
        public void RenderAsync_uses_config_with_overridden_document_provider_when_specified([MockedConfig] RenderingConfig config,
                                                                                             [Frozen] System.Type provider,
                                                                                             [Frozen] IRendersZptDocument wrapped,
                                                                                             EffectiveConfigSettingZptDocumentRendererDecorator sut,
                                                                                             Stream stream,
                                                                                             object model)
        {
            sut.RenderAsync(stream, model, config);

            Mock.Get(wrapped)
                .Verify(x => x.RenderAsync(stream,
                                           model,
                                           It.Is<RenderingConfig>(c => c.DocumentProviderType == provider),
                                           CancellationToken.None,
                                           null),
                        Times.Once);
        }

        [Test, AutoMoqData]
        public void RenderAsync_returns_result_from_renderer_service([MockedConfig] RenderingConfig config,
                                                                     [Frozen] IRendersZptDocument wrapped,
                                                                     EffectiveConfigSettingZptDocumentRendererDecorator sut,
                                                                     Stream input,
                                                                     Stream output,
                                                                     object model,
                                                                     CancellationToken token)
        {
            Mock.Get(wrapped)
                .Setup(x => x.RenderAsync(input, model, It.IsAny<RenderingConfig>(), It.IsAny<CancellationToken>(), null))
                .Returns(Task.FromResult(output));

            Assert.That(() => sut.RenderAsync(input, model, config, token).Result, Is.SameAs(output));
        }

        [Test, AutoMoqData]
        public void RenderAsync_throws_ANE_if_the_stream_is_null([MockedConfig] RenderingConfig config,
                                                                 EffectiveConfigSettingZptDocumentRendererDecorator sut,
                                                                 object model,
                                                                 CancellationToken token)
        {
            Assert.That(() => sut.RenderAsync(null, model, config, token), Throws.InstanceOf<ArgumentNullException>());
        }
    }
}
