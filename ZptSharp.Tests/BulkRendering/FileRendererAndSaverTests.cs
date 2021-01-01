using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace ZptSharp.BulkRendering
{
    [TestFixture, Parallelizable]
    public class FileRendererAndSaverTests
    {
        string ResultsDirectory => NUnit.Framework.TestContext.CurrentContext.WorkDirectory;

        [Test,AutoMoqData, Description("This integration test verifies that a file is written to the filesystem.")]
        public async Task RenderAsync_writes_stream_from_renderer_to_disk([Frozen] IRendersZptFile fileRenderer,
                                                                          [Frozen] IWritesStreamToTextWriter streamCopier,
                                                                          FileRendererAndSaver sut,
                                                                          BulkRenderingRequest request,
                                                                          string absolutePath,
                                                                          StreamToTextWriterCopier realStreamCopier)
        {
            var expectedStream = GetContentStream();
            request.OutputFileExtension = null;
            request.OutputPath = Path.Combine(ResultsDirectory, nameof(FileRendererAndSaverTests));
            var inputFile = new InputFile(absolutePath, Path.Combine("Dir", "TestFile.txt"));

            Mock.Get(fileRenderer)
                .Setup(x => x.RenderAsync(inputFile.AbsolutePath, request.Model, request.RenderingConfig, CancellationToken.None))
                .Returns(() => Task.FromResult(expectedStream));
            Mock.Get(streamCopier)
                .Setup(x => x.WriteToTextWriterAsync(It.IsAny<Stream>(), It.IsAny<TextWriter>(), It.IsAny<CancellationToken>()))
                .Returns((Stream s, TextWriter w, CancellationToken t) => realStreamCopier.WriteToTextWriterAsync(s, w, t));
            
            await sut.RenderAsync(request, inputFile);

            var writtenContent = File.ReadAllText(Path.Combine(ResultsDirectory, nameof(FileRendererAndSaverTests), "Dir", "TestFile.txt"));
            Assert.That(writtenContent, Is.EqualTo(GetContent()));
        }

        [Test,AutoMoqData, Description("This integration test verifies that a file is written to the filesystem with a different extension.")]
        public async Task RenderAsync_changes_extension_of_file_if_requested([Frozen] IRendersZptFile fileRenderer,
                                                                             [Frozen] IWritesStreamToTextWriter streamCopier,
                                                                             FileRendererAndSaver sut,
                                                                             BulkRenderingRequest request,
                                                                             string absolutePath,
                                                                             StreamToTextWriterCopier realStreamCopier)
       {
            var expectedStream = GetContentStream();
            request.OutputFileExtension = ".html";
            request.OutputPath = Path.Combine(ResultsDirectory, nameof(FileRendererAndSaverTests));
            var inputFile = new InputFile(absolutePath, Path.Combine("Dir", "TestFile.txt"));

            Mock.Get(fileRenderer)
                .Setup(x => x.RenderAsync(inputFile.AbsolutePath, request.Model, request.RenderingConfig, CancellationToken.None))
                .Returns(() => Task.FromResult(expectedStream));
            Mock.Get(streamCopier)
                .Setup(x => x.WriteToTextWriterAsync(It.IsAny<Stream>(), It.IsAny<TextWriter>(), It.IsAny<CancellationToken>()))
                .Returns((Stream s, TextWriter w, CancellationToken t) => realStreamCopier.WriteToTextWriterAsync(s, w, t));
            
            await sut.RenderAsync(request, inputFile);

            var writtenContent = File.ReadAllText(Path.Combine(ResultsDirectory, nameof(FileRendererAndSaverTests), "Dir", "TestFile.html"));
            Assert.That(writtenContent, Is.EqualTo(GetContent()));
        }

        Stream GetContentStream() => new MemoryStream(System.Text.Encoding.UTF8.GetBytes(GetContent()));

        string GetContent() => "Foo bar baz";
    }
}