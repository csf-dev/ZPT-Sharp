using System;
using System.IO;
using System.Web.Mvc;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Config;
using ZptSharp.Mvc.Autofixture;
using System.Threading.Tasks;
using System.Threading;

namespace ZptSharp.Mvc
{
    [TestFixture,Parallelizable]
    public class ZptSharpViewTests
    {
        [Test, AutoMoqData]
        public void Render_gets_a_stream_using_modified_config_then_copies_it_to_the_writer([Frozen] IServiceProvider serviceProvider,
                                                                                            [Frozen] RenderingConfig originalConfig,
                                                                                            ZptSharpView sut,
                                                                                            IRendersZptFile fileRenderer,
                                                                                            IWritesStreamToTextWriter streamCopier,
                                                                                            IGetsMvcRenderingConfig configProvider,
                                                                                            IGetsErrorStream errorStreamProvider,
                                                                                            RenderingConfig modifiedConfig,
                                                                                            Stream stream,
                                                                                            [MockContext] ViewContext viewContext,
                                                                                            TextWriter writer)
        {
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IRendersZptFile))).Returns(fileRenderer);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IWritesStreamToTextWriter))).Returns(streamCopier);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsMvcRenderingConfig))).Returns(configProvider);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsErrorStream))).Returns(errorStreamProvider);

            Mock.Get(configProvider)
                .Setup(x => x.GetMvcRenderingConfig(originalConfig, viewContext, It.IsAny<string>()))
                .Returns(modifiedConfig);
            Mock.Get(fileRenderer)
                .Setup(x => x.RenderAsync(It.IsAny<string>(), It.IsAny<object>(), modifiedConfig, CancellationToken.None))
                .Returns(() => Task.FromResult(stream));
            Mock.Get(streamCopier)
                .Setup(x => x.WriteToTextWriterAsync(stream, writer))
                .Returns(() => Task.CompletedTask);

            sut.Render(viewContext, writer);

            Mock.Get(streamCopier)
                .Verify(x => x.WriteToTextWriterAsync(stream, writer), Times.Once);
        }

        [Test, AutoMoqData]
        public void Render_gets_a_stream_using_error_renderer_then_copies_it_to_the_writer_if_rendering_throws([Frozen] IServiceProvider serviceProvider,
                                                                                                               [Frozen] RenderingConfig originalConfig,
                                                                                                               ZptSharpView sut,
                                                                                                               IRendersZptFile fileRenderer,
                                                                                                               IWritesStreamToTextWriter streamCopier,
                                                                                                               IGetsMvcRenderingConfig configProvider,
                                                                                                               IGetsErrorStream errorStreamProvider,
                                                                                                               RenderingConfig modifiedConfig,
                                                                                                               Stream stream,
                                                                                                               [MockContext] ViewContext viewContext,
                                                                                                               TextWriter writer)
        {
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IRendersZptFile))).Returns(fileRenderer);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IWritesStreamToTextWriter))).Returns(streamCopier);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsMvcRenderingConfig))).Returns(configProvider);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsErrorStream))).Returns(errorStreamProvider);

            Mock.Get(configProvider)
                .Setup(x => x.GetMvcRenderingConfig(originalConfig, viewContext, It.IsAny<string>()))
                .Returns(modifiedConfig);
            Mock.Get(fileRenderer)
                .Setup(x => x.RenderAsync(It.IsAny<string>(), It.IsAny<object>(), modifiedConfig, CancellationToken.None))
                .Throws<ZptRenderingException>();
            Mock.Get(errorStreamProvider)
                .Setup(x => x.GetErrorStreamAsync(It.IsAny<Exception>()))
                .Returns(() => Task.FromResult(stream));
            Mock.Get(streamCopier)
                .Setup(x => x.WriteToTextWriterAsync(stream, writer))
                .Returns(() => Task.CompletedTask);

            sut.Render(viewContext, writer);

            Mock.Get(streamCopier)
                .Verify(x => x.WriteToTextWriterAsync(stream, writer), Times.Once);
        }
    }
}
