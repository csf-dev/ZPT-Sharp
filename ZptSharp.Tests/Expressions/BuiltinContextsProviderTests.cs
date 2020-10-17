using System;
using System.Collections.Generic;
using System.IO;
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
        public void TryGetValue_returns_model_when_here_requested(object model,
                                                                  [Frozen, NoAutoProperties] ExpressionContext context,
                                                                  [Frozen, MockedConfig] RenderingConfig config,
                                                                  BuiltinContextsProvider sut)
        {
            context.Model = model;
            var result = sut.TryGetValue(BuiltinContextsProvider.Here, out var value);
            Assert.That(result, Is.True, "Value returned successfully");
            Assert.That(value, Is.SameAs(model), "Model object returned");
        }

        [Test, AutoMoqData]
        public void TryGetValue_returns_repetitions_dictionary_when_repeat_requested([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                     [Frozen, MockedConfig] RenderingConfig config,
                                                                                     BuiltinContextsProvider sut)
        {
            var result = sut.TryGetValue(BuiltinContextsProvider.Repeat, out var value);
            Assert.That(result, Is.True, "Value returned successfully");
            Assert.That(value, Is.SameAs(context.Repetitions), "Repititions collection returned");
        }

        [Test, AutoMoqData]
        public void TryGetValue_returns_options_dictionary_when_options_requested([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                  [Frozen, MockedConfig] RenderingConfig config,
                                                                                  Dictionary<string,object> keywordOptions,
                                                                                  BuiltinContextsProvider sut)
        {
            Mock.Get(config).SetupGet(x => x.KeywordOptions).Returns(keywordOptions);
            var result = sut.TryGetValue(BuiltinContextsProvider.Options, out var value);
            Assert.That(result, Is.True, "Value returned successfully");
            Assert.That(value, Is.SameAs(keywordOptions), "Options collection returned");
        }

        [Test, AutoMoqData]
        public void TryGetValue_returns_null_when_nothing_requested([Frozen, NoAutoProperties] ExpressionContext context,
                                                                    [Frozen, MockedConfig] RenderingConfig config,
                                                                    BuiltinContextsProvider sut)
        {
            var result = sut.TryGetValue(BuiltinContextsProvider.Nothing, out var value);
            Assert.That(result, Is.True, "Value returned successfully");
            Assert.That(value, Is.Null, "Null returned");
        }

        [Test, AutoMoqData]
        public void TryGetValue_returns_cancellation_token_when_default_requested([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                  [Frozen, MockedConfig] RenderingConfig config,
                                                                                  BuiltinContextsProvider sut)
        {
            var result = sut.TryGetValue(BuiltinContextsProvider.Default, out var value);
            Assert.That(result, Is.True, "Value returned successfully");
            Assert.That(value, Is.EqualTo(CancellationToken.Instance), "Token returned");
        }

        [Test, AutoMoqData]
        public void TryGetValue_returns_element_attributes_collection_when_attributes_requested([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                                [Frozen, MockedConfig] RenderingConfig config,
                                                                                                IElement element,
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

            var result = sut.TryGetValue(BuiltinContextsProvider.Attributes, out var value);
            Assert.That(result, Is.True, "Value returned successfully");
            Assert.That(((IDictionary<string, IAttribute>) value).Values, Is.EqualTo(new[] { attribute1, attribute2 }), "Attributes returned");
        }

        [Test, AutoMoqData]
        public void TryGetValue_returns_template_object_when_template_requested([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                [Frozen, MockedConfig] RenderingConfig config,
                                                                                IDocument document,
                                                                                BuiltinContextsProvider sut)
        {
            context.TemplateDocument = document;
            var result = sut.TryGetValue(BuiltinContextsProvider.Template, out var value);
            Assert.That(result, Is.True, "Value returned successfully");
            Assert.That(value, Is.InstanceOf<MetalDocumentAdapter>(), "Template object returned");
        }

        [Test, AutoMoqData]
        public void TryGetValue_returns_directory_info_when_container_requested([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                [Frozen, MockedConfig] RenderingConfig config,
                                                                                IDocument document,
                                                                                BuiltinContextsProvider sut)
        {
            context.TemplateDocument = document;
            var documentPath = Path.Join(TestContext.CurrentContext.TestDirectory, "document.pt");
            Mock.Get(document).SetupGet(x => x.SourceInfo).Returns(() => new FileSourceInfo(documentPath));

            var result = sut.TryGetValue(BuiltinContextsProvider.Container, out var value);
            Assert.That(result, Is.True, "Value returned successfully");
            Assert.That(value, Is.EqualTo(new DirectoryInfo(TestContext.CurrentContext.TestDirectory)), "Directory info returned");
        }
    }
}
