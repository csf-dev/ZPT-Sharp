using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class XmlDocumentProviderTests
    {
        [Test,AutoMoqData]
        public async Task GetDocumentAsync_returns_instance_of_correct_type_when_parsing_html([DefaultConfig] RenderingConfig config,
                                                                                              XmlDocumentProvider sut)
        {
            var doc = @"<?xml version=""1.0"" encoding=""utf-8""?>
<html>
  <head>
    <title>Hello there</title>
  </head>
  <body>
    <p>I am a paragraph</p>
  </body>
</html>";
            var stream = DocumentToFromStream.ToStream(doc);

            var result = await sut.GetDocumentAsync(stream, config, CancellationToken.None);

            Assert.That(result, Is.InstanceOf<XmlDocument>());
        }

        [Test, AutoMoqData, Description("This test ensures that reading a document from a stream, then writing it back to stream produces the same document as was input")]
        public async Task Provider_can_roundtrip_a_valid_html_document([DefaultConfig] RenderingConfig config,
                                                                       XmlDocumentProvider sut)
        {
            var doc = @"<?xml version=""1.0"" encoding=""utf-8""?>
<html>
  <head>
    <title>Hello there</title>
  </head>
  <body>
    <p>I am a paragraph</p>
  </body>
</html>";
            var inputStream = DocumentToFromStream.ToStream(doc);

            var angleSharpDoc = await sut.GetDocumentAsync(inputStream, config, CancellationToken.None);
            var outputStream = await sut.WriteDocumentAsync(angleSharpDoc, config, CancellationToken.None);
            var outputDoc = DocumentToFromStream.FromStream(outputStream);

            Assert.That(outputDoc, Is.EqualTo(doc));
        }
    }
}
