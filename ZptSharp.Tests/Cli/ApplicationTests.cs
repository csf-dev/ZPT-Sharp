using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.BulkRendering;

namespace ZptSharp.Cli
{
    [TestFixture, Parallelizable]
    public class ApplicationTests
    {
        [Test,AutoMoqData]
        public async Task StartAsync_renders_all_documents_using_request([Frozen] IRendersManyFiles renderer,
                                                                         [Frozen] IGetsBulkRenderingRequest requestFactory,
                                                                         [Frozen] CliArguments arguments,
                                                                         Application sut,
                                                                         BulkRenderingRequest request)
        {
            Mock.Get(requestFactory)
                .Setup(x => x.GetRequestAsync(arguments, It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(request));
            Mock.Get(renderer)
                .Setup(x => x.RenderAllAsync(request, It.IsAny<CancellationToken>()))
                .Returns((BulkRenderingRequest req, CancellationToken t) => Task.FromResult(new BulkRenderingResult(req, Enumerable.Empty<BulkRenderingFileResult>())));

            await sut.StartAsync(CancellationToken.None);

            Mock.Get(renderer)
                .Verify(x => x.RenderAllAsync(request, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test,AutoMoqData]
        public void StartAsync_does_not_throw_if_operation_is_cancelled([Frozen] IRendersManyFiles renderer,
                                                                        [Frozen] IGetsBulkRenderingRequest requestFactory,
                                                                        [Frozen] CliArguments arguments,
                                                                        Application sut,
                                                                        BulkRenderingRequest request)
        {
            Mock.Get(requestFactory)
                .Setup(x => x.GetRequestAsync(arguments, It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(request));
            Mock.Get(renderer)
                .Setup(x => x.RenderAllAsync(request, It.IsAny<CancellationToken>()))
                .Throws<OperationCanceledException>();

            Assert.That(async () => await sut.StartAsync(CancellationToken.None), Throws.Nothing);
        }
    }
}