using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;
using ZptSharp.Dom;
using ZptSharp.Metal;
using ZptSharp.Rendering;

namespace ZptSharp.Expressions
{
    [TestFixture,Parallelizable]
    public class BuiltinContextsProviderTests
    {
        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_model_when_here_requested(object model,
                                                                  [Frozen, NoAutoProperties] ExpressionContext context,
                                                                  [Frozen, MockedConfig] RenderingConfig config,
                                                                  [Frozen, MetalDocAdapter] IGetsMetalDocumentAdapter metalDocumentAdapterFactory,
                                                                  BuiltinContextsProvider sut)
        {
            context.Model = model;
            var result = await sut.TryGetValueAsync(BuiltinContextsProvider.Here);
            Assert.That(result.Success, Is.True, "Value returned successfully");
            Assert.That(result.Value, Is.SameAs(model), "Model object returned");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_repetitions_dictionary_when_repeat_requested([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                     [Frozen, MockedConfig] RenderingConfig config,
                                                                                     [Frozen, MetalDocAdapter] IGetsMetalDocumentAdapter metalDocumentAdapterFactory,
                                                                                     BuiltinContextsProvider sut)
        {
            var result = await sut.TryGetValueAsync(BuiltinContextsProvider.Repeat);
            Assert.That(result.Success, Is.True, "Value returned successfully");
            Assert.That(result.Value, Is.SameAs(context.Repetitions), "Repititions collection returned");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_options_dictionary_when_options_requested([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                  [Frozen, MockedConfig] RenderingConfig config,
                                                                                  [Frozen, MetalDocAdapter] IGetsMetalDocumentAdapter metalDocumentAdapterFactory,
                                                                                  Dictionary<string,object> keywordOptions,
                                                                                  BuiltinContextsProvider sut)
        {
            Mock.Get(config).SetupGet(x => x.KeywordOptions).Returns(keywordOptions);
            var result = await sut.TryGetValueAsync(BuiltinContextsProvider.Options);
            Assert.That(result.Success, Is.True, "Value returned successfully");
            Assert.That(result.Value, Is.SameAs(keywordOptions), "Options collection returned");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_null_when_nothing_requested([Frozen, NoAutoProperties] ExpressionContext context,
                                                                    [Frozen, MockedConfig] RenderingConfig config,
                                                                    [Frozen, MetalDocAdapter] IGetsMetalDocumentAdapter metalDocumentAdapterFactory,
                                                                    BuiltinContextsProvider sut)
        {
            var result = await sut.TryGetValueAsync(BuiltinContextsProvider.Nothing);
            Assert.That(result.Success, Is.True, "Value returned successfully");
            Assert.That(result.Value, Is.Null, "Null returned");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_cancellation_token_when_default_requested([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                  [Frozen, MockedConfig] RenderingConfig config,
                                                                                  [Frozen, MetalDocAdapter] IGetsMetalDocumentAdapter metalDocumentAdapterFactory,
                                                                                  BuiltinContextsProvider sut)
        {
            var result = await sut.TryGetValueAsync(BuiltinContextsProvider.Default);
            Assert.That(result.Success, Is.True, "Value returned successfully");
            Assert.That(result.Value, Is.EqualTo(AbortZptActionToken.Instance), "Token returned");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_element_attributes_collection_when_attributes_requested([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                                [Frozen, MockedConfig] RenderingConfig config,
                                                                                                [Frozen, MetalDocAdapter] IGetsMetalDocumentAdapter metalDocumentAdapterFactory,
                                                                                                INode element,
                                                                                                IAttribute attribute1,
                                                                                                string name1,
                                                                                                IAttribute attribute2,
                                                                                                string name2,
                                                                                                BuiltinContextsProvider sut)
        {
            context.CurrentElement = element;
            Mock.Get(element).SetupGet(x => x.Attributes).Returns(() => new[] { attribute1, attribute2 });
            Mock.Get(attribute1).SetupGet(x => x.Name).Returns(name1);
            Mock.Get(attribute2).SetupGet(x => x.Name).Returns(name2);

            var result = await sut.TryGetValueAsync(BuiltinContextsProvider.Attributes);
            Assert.That(result.Success, Is.True, "Value returned successfully");
            Assert.That(((IDictionary<string, IAttribute>)result.Value).Values, Is.EqualTo(new[] { attribute1, attribute2 }), "Attributes returned");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_template_object_when_template_requested([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                [Frozen, MockedConfig] RenderingConfig config,
                                                                                [Frozen, MetalDocAdapter] IGetsMetalDocumentAdapter metalDocumentAdapterFactory,
                                                                                IDocument document,
                                                                                BuiltinContextsProvider sut)
        {
            context.TemplateDocument = document;
            var result = await sut.TryGetValueAsync(BuiltinContextsProvider.Template);
            Assert.That(result.Success, Is.True, "Value returned successfully");
            Assert.That(result.Value, Is.InstanceOf<MetalDocumentAdapter>(), "Template object returned");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_result_from_source_info_when_container_requested([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                                    [Frozen, MockedConfig] RenderingConfig config,
                                                                                                    [Frozen, MetalDocAdapter] IGetsMetalDocumentAdapter metalDocumentAdapterFactory,
                                                                                                    IDocument document,
                                                                                                    BuiltinContextsProvider sut,
                                                                                                    object container)
        {
            var sourceInfo = new Mock<IDocumentSourceInfo>();
            sourceInfo.As<IHasContainer>().Setup(x => x.GetContainer()).Returns(container);

            context.TemplateDocument = document;
            Mock.Get(document).SetupGet(x => x.SourceInfo).Returns(sourceInfo.Object);

            var result = await sut.TryGetValueAsync(BuiltinContextsProvider.Container);

            Assert.That(result, Has.Property(nameof(GetValueResult.Success)).True
                                .And.Property(nameof(GetValueResult.Value)).SameAs(container));
        }
    }
}
