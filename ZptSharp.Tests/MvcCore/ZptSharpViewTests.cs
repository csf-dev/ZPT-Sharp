using System;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Config;
using ZptSharp.Autofixture;
using System.Threading.Tasks;
using System.Threading;
using ZptSharp.Hosting;

namespace ZptSharp.MvcCore
{
    [TestFixture,Parallelizable]
    public class ZptSharpViewTests
    {
        [Test, AutoMoqData]
        public async Task RenderAsync_gets_a_stream_using_modified_config_then_copies_it_to_the_writer([Frozen] IHostsZptSharp host,
                                                                                                       [Frozen] RenderingConfig originalConfig,
                                                                                                       [Frozen] IGetsMvcRenderingConfig configProvider,
                                                                                                       [Frozen] IGetsErrorStream errorStreamProvider,
                                                                                                       ZptSharpView sut,
                                                                                                       IWritesStreamToTextWriter streamCopier,
                                                                                                       IRendersZptFile fileRenderer,
                                                                                                       RenderingConfig modifiedConfig,
                                                                                                       Stream stream,
                                                                                                       [MockContext] ViewContext viewContext)
        {
            Mock.Get(host).SetupGet(x => x.FileRenderer).Returns(fileRenderer);
            Mock.Get(host).SetupGet(x => x.StreamCopier).Returns(streamCopier);

            Mock.Get(configProvider)
                .Setup(x => x.GetMvcRenderingConfig(originalConfig, viewContext, It.IsAny<string>()))
                .Returns(modifiedConfig);
            Mock.Get(fileRenderer)
                .Setup(x => x.RenderAsync(It.IsAny<string>(), It.IsAny<object>(), modifiedConfig, CancellationToken.None))
                .Returns(() => Task.FromResult(stream));
            Mock.Get(streamCopier)
                .Setup(x => x.WriteToTextWriterAsync(stream, viewContext.Writer, CancellationToken.None))
                .Returns(() => Task.CompletedTask);

            await sut.RenderAsync(viewContext);

            Mock.Get(streamCopier)
                .Verify(x => x.WriteToTextWriterAsync(stream, viewContext.Writer, CancellationToken.None), Times.Once);
        }

        [Test, AutoMoqData]
        public async Task RenderAsync_gets_a_stream_using_error_renderer_then_copies_it_to_the_writer_if_rendering_throws([Frozen] IHostsZptSharp host,
                                                                                                                          [Frozen] RenderingConfig originalConfig,
                                                                                                                          [Frozen] IGetsMvcRenderingConfig configProvider,
                                                                                                                          [Frozen] IGetsErrorStream errorStreamProvider,
                                                                                                                          ZptSharpView sut,
                                                                                                                          IRendersZptFile fileRenderer,
                                                                                                                          IWritesStreamToTextWriter streamCopier,
                                                                                                                          RenderingConfig modifiedConfig,
                                                                                                                          Stream stream,
                                                                                                                          [MockContext] ViewContext viewContext)
        {
            Mock.Get(host).SetupGet(x => x.FileRenderer).Returns(fileRenderer);
            Mock.Get(host).SetupGet(x => x.StreamCopier).Returns(streamCopier);

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
                .Setup(x => x.WriteToTextWriterAsync(stream, viewContext.Writer, CancellationToken.None))
                .Returns(() => Task.CompletedTask);

            await sut.RenderAsync(viewContext);

            Mock.Get(streamCopier)
                .Verify(x => x.WriteToTextWriterAsync(stream, viewContext.Writer, CancellationToken.None), Times.Once);
        }
    }
}
