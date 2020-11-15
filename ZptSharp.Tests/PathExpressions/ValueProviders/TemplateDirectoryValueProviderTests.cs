using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;
using ZptSharp.Dom;
using ZptSharp.Expressions;
using ZptSharp.Metal;
using ZptSharp.Rendering;
using ZptSharp.Util;

namespace ZptSharp.PathExpressions.ValueProviders
{
    [TestFixture,Parallelizable]
    public class TemplateDirectoryValueProviderTests
    {
        [Test, AutoMoqData]
        public void TryGetValueAsync_returns_value_from_wrapped_service_if_object_is_not_TemplateDirectory([Frozen] IGetsValueFromObject wrapped,
                                                                                                           TemplateDirectoryValueProvider sut,
                                                                                                           string name,
                                                                                                           object @object,
                                                                                                           GetValueResult wrappedResult)
        {
            Mock.Get(wrapped)
                .Setup(x => x.TryGetValueAsync(name, @object, CancellationToken.None))
                .Returns(() => Task.FromResult(wrappedResult));

            Assert.That(() => sut.TryGetValueAsync(name, @object).Result.Value, Is.SameAs(wrappedResult.Value));
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_TemplateDirectory_if_result_is_a_directory(TemplateDirectoryValueProvider sut)
        {
            var path = TestFiles.GetPath("ZptIntegrationTests");
            var obj = new TemplateDirectory(path);
            var name = "SourceDocuments";
            var expectedPath = Path.Combine(path, name);

            var result = await sut.TryGetValueAsync(name, obj);

            Assert.That(result.Value, Is.InstanceOf<TemplateDirectory>()
                                      .And.Property(nameof(TemplateDirectory.Path)).EqualTo(expectedPath));
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_document_if_result_is_a_file([Frozen] IReadsAndWritesDocument readerWriter,
                                                                                [Frozen,MockedConfig] RenderingConfig config,
                                                                                [Frozen] IGetsMetalDocumentAdapter adapterFactory,
                                                                                TemplateDirectoryValueProvider sut,
                                                                                IDocument document,
                                                                                MetalDocumentAdapter adapter)
        {
            var path = TestFiles.GetPath("ZptIntegrationTests/SourceDocuments");
            var obj = new TemplateDirectory(path);
            var name = "acme_template.html";

            Mock.Get(readerWriter)
                .Setup(x => x.GetDocumentAsync(It.IsAny<Stream>(), config, It.IsAny<FileSourceInfo>(), CancellationToken.None))
                .Returns(() => Task.FromResult(document));
            Mock.Get(adapterFactory)
                .Setup(x => x.GetMetalDocumentAdapter(document))
                .Returns(adapter);

            var result = await sut.TryGetValueAsync(name, obj);

            Assert.That(result.Value, Is.SameAs(adapter));
        }
    }
}
