using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class AngleSharpDocumentProviderTests
    {
        [Test, AutoMoqData]
        public async Task GetDocumentAsync_returns_instance_of_correct_type_when_parsing_html([MockedConfig] RenderingConfig config,
                                                                                              AngleSharpDocumentProvider sut)
        {
            var doc = "<html><head><title>Hello there</title></head><body><p>I am a paragraph</p></body></html>";
            var stream = DocumentToFromStream.ToStream(doc);

            var result = await sut.GetDocumentAsync(stream, config);

            Assert.That(result, Is.InstanceOf<AngleSharpDocument>());
        }

        [Test, AutoMoqData, Description("This test ensures that reading a document from a stream, then writing it back to stream produces the same document as was input")]
        public async Task Provider_can_roundtrip_a_valid_html_document([MockedConfig] RenderingConfig config,
                                                                       AngleSharpDocumentProvider sut)
        {
            var doc = "<html><head><title>Hello there</title></head><body><p>I am a paragraph</p></body></html>";
            var inputStream = DocumentToFromStream.ToStream(doc);

            var angleSharpDoc = await sut.GetDocumentAsync(inputStream, config);
            var outputStream = await sut.WriteDocumentAsync(angleSharpDoc, config);
            var outputDoc = DocumentToFromStream.FromStream(outputStream);

            Assert.That(outputDoc, Is.EqualTo(doc));
        }

        [Test]
        public void CanReadWriteForFilename_returns_true_for_supported_filenames([ValueSource(nameof(GetSupportedHtmlFilenames))] string filename)
        {
            var sut = new AngleSharpDocumentProvider();
            Assert.That(() => sut.CanReadWriteForFilename(filename), Is.True);
        }

        [Test, AutoMoqData]
        public void CanReadWriteForFilename_returns_false_for_null_filename(AngleSharpDocumentProvider sut)
        {
            Assert.That(() => sut.CanReadWriteForFilename(null), Is.False);
        }

        [Test, AutoMoqData]
        public void CanReadWriteForFilename_returns_false_for_empty_filename(AngleSharpDocumentProvider sut)
        {
            Assert.That(() => sut.CanReadWriteForFilename(String.Empty), Is.False);
        }

        [Test]
        public void CanReadWriteForFilename_returns_false_for_unsupported_filename([ValueSource(nameof(GetUnsupportedFilenames))] string filename)
        {
            var sut = new AngleSharpDocumentProvider();
            Assert.That(() => sut.CanReadWriteForFilename(filename), Is.False);
        }

        public static IEnumerable<string> GetSupportedHtmlFilenames()
        {
            return new[]
            {
                "sample.pt",
                "index.html",
                "MyTemplate.htm",
                "A long name with spaces.html",
                "_.pt",
                "foo.bar.bar.htm",
                @"c:\Windows\Style\Path\File.htm",
                "/posix/style/path/file.pt",
            };
        }

        public static IEnumerable<string> GetUnsupportedFilenames()
        {
            return new[]
            {
                "sample.ptx",
                "index.xml",
                "MyTemplate.txt",
                "foo",
                "foo.bar.bar",
            };
        }
    }
}
