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

        [Test,AutoMoqData, Description("This integration test writes an actual file to the filesystem.  Note that it uses a real stream-to-textwriter impl.")]
        public async Task RenderAsync_writes_stream_from_renderer_to_disk([Frozen] IRendersZptFile fileRenderer,
                                                                          [Frozen] IWritesStreamToTextWriter streamCopier,
                                                                          FileRendererAndSaver sut,
                                                                          BulkRenderingRequest request,
                                                                          string absolutePath,
                                                                          StreamToTextWriterCopier realStreamCopier)
        {
            var expectedStream = GetContentStream();
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

        Stream GetContentStream() => new MemoryStream(System.Text.Encoding.UTF8.GetBytes(GetContent()));

        string GetContent() => "Foo bar baz";
    }
}