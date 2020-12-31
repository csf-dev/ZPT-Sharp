using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace ZptSharp.BulkRendering
{
    [TestFixture, Parallelizable]
    public class BulkFileRendererTests
    {
        [Test,AutoMoqData]
        public async Task RenderAllAsync_returns_result_from_all_rendering_processes([Frozen] IGetsInputFiles inputFilesProvider,
                                                                                     [Frozen] IRendersInputFile inputFileRenderer,
                                                                                     BulkFileRenderer sut,
                                                                                     BulkRenderingRequest request,
                                                                                     InputFile file1,
                                                                                     InputFile file2,
                                                                                     BulkRenderingFileResult result1,
                                                                                     BulkRenderingFileResult result2)
        {
            Mock.Get(inputFilesProvider)
                .Setup(x => x.GetInputFilesAsync(request, CancellationToken.None))
                .Returns(() => Task.FromResult<IEnumerable<InputFile>>(new [] { file1, file2 }));
            Mock.Get(inputFileRenderer)
                .Setup(x => x.RenderAsync(request, file1, CancellationToken.None))
                .Returns(() => Task.FromResult(result1));
            Mock.Get(inputFileRenderer)
                .Setup(x => x.RenderAsync(request, file2, CancellationToken.None))
                .Returns(() => Task.FromResult(result2));

            var result =  await sut.RenderAllAsync(request);

            Assert.That(result.FileResults, Is.EqualTo(new [] { result1, result2 }));
        }

        [Test,AutoMoqData]
        public async Task RenderAllAsync_uses_request_validator([Frozen] IValidatesBulkRenderingRequest validator,
                                                                [Frozen] IGetsInputFiles inputFilesProvider,
                                                                [Frozen] IRendersInputFile inputFileRenderer,
                                                                BulkFileRenderer sut,
                                                                BulkRenderingRequest request,
                                                                InputFile file1,
                                                                InputFile file2,
                                                                BulkRenderingFileResult result1,
                                                                BulkRenderingFileResult result2)
        {
            Mock.Get(inputFilesProvider)
                .Setup(x => x.GetInputFilesAsync(request, CancellationToken.None))
                .Returns(() => Task.FromResult<IEnumerable<InputFile>>(new [] { file1, file2 }));
            Mock.Get(inputFileRenderer)
                .Setup(x => x.RenderAsync(request, file1, CancellationToken.None))
                .Returns(() => Task.FromResult(result1));
            Mock.Get(inputFileRenderer)
                .Setup(x => x.RenderAsync(request, file2, CancellationToken.None))
                .Returns(() => Task.FromResult(result2));

            await sut.RenderAllAsync(request);

            Mock.Get(validator).Verify(x => x.AssertIsValid(request), Times.Once);
        }

        [Test,AutoMoqData]
        public void RenderAllAsync_does_not_throw_if_rendering_one_file_fails([Frozen] IGetsInputFiles inputFilesProvider,
                                                                              [Frozen] IRendersInputFile inputFileRenderer,
                                                                              BulkFileRenderer sut,
                                                                              BulkRenderingRequest request,
                                                                              InputFile file1,
                                                                              InputFile file2,
                                                                              BulkRenderingFileResult result1,
                                                                              BulkRenderingFileResult result2)
        {
            Mock.Get(inputFilesProvider)
                .Setup(x => x.GetInputFilesAsync(request, CancellationToken.None))
                .Returns(() => Task.FromResult<IEnumerable<InputFile>>(new [] { file1, file2 }));
            Mock.Get(inputFileRenderer)
                .Setup(x => x.RenderAsync(request, file1, CancellationToken.None))
                .Returns(() => Task.FromResult(result1));
            Mock.Get(inputFileRenderer)
                .Setup(x => x.RenderAsync(request, file2, CancellationToken.None))
                .Throws<Exception>();

            Assert.That(() => sut.RenderAllAsync(request).Result, Throws.Nothing);
        }
    }
}