using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class DocumentReaderWriterBaseTests
    {
        [Test, AutoMoqData]
        public void GetDocumentAsync_throws_ANE_if_stream_is_null(StubDocumentReaderWriter sut,
                                                                  [MockedConfig] RenderingConfig config)
        {
            Assert.That(() => sut.GetDocumentAsync(null, config), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void GetDocumentAsync_throws_ANE_if_config_is_null(StubDocumentReaderWriter sut,
                                                                  Stream stream)
        {
            Assert.That(() => sut.GetDocumentAsync(stream, null), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void GetDocumentAsync_throws_if_operation_is_cancelled(StubDocumentReaderWriter sut,
                                                                      Stream stream,
                                                                      [MockedConfig] RenderingConfig config)
        {
            var tokenSource = new CancellationTokenSource();
            tokenSource.Cancel();

            Assert.That(() => sut.GetDocumentAsync(stream, config, token: tokenSource.Token), Throws.InstanceOf<OperationCanceledException>());
        }

        [Test, AutoMoqData]
        public void WriteDocumentAsync_throws_ANE_if_doc_is_null(StubDocumentReaderWriter sut,
                                                                [MockedConfig] RenderingConfig config)
        {
            Assert.That(() => sut.WriteDocumentAsync(null, config), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void WriteDocumentAsync_throws_ArgEx_if_doc_is_wrong_type(StubDocumentReaderWriter sut,
                                                                         [MockedConfig] RenderingConfig config,
                                                                         IDocument doc)
        {
            Assert.That(() => sut.WriteDocumentAsync(doc, config), Throws.ArgumentException);
        }

        [Test, AutoMoqData]
        public void WriteDocumentAsync_throws_ANE_if_config_is_null(StubDocumentReaderWriter sut,
                                                                    StubDocument doc)
        {
            Assert.That(() => sut.WriteDocumentAsync(doc, null), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void WriteDocumentAsync_throws_if_operation_is_cancelled(StubDocumentReaderWriter sut,
                                                                        StubDocument doc,
                                                                        [MockedConfig] RenderingConfig config)
        {
            var tokenSource = new CancellationTokenSource();
            tokenSource.Cancel();

            Assert.That(() => sut.WriteDocumentAsync(doc, config, token: tokenSource.Token), Throws.InstanceOf<OperationCanceledException>());
        }

        /// <summary>
        /// A stub reader/writer with hard-coded logic, for the purpose of testing the base class.
        /// </summary>
        public class StubDocumentReaderWriter : DocumentReaderWriterBase<StubDocument>
        {
            public override System.Type ResolvableType => GetType();

            public override bool CanReadWriteForFilename(string filenameOrPath) => true;

            protected override Task<IDocument> GetDocumentProtectedAsync(Stream stream, RenderingConfig config, IDocumentSourceInfo sourceInfo, CancellationToken token)
                => Task.FromResult(Mock.Of<IDocument>());

            protected override Task<Stream> WriteDocumentProtectedAsync(StubDocument document, RenderingConfig config, CancellationToken token)
                => Task.FromResult<Stream>(new MemoryStream());
        }
    }
}
