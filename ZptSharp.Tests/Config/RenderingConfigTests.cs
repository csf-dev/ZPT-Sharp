using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Config
{
    [TestFixture,Parallelizable]
    public class RenderingConfigTests
    {
        [Test, AutoMoqData]
        public void Builder_creates_config_with_correct_properties(RenderingConfig.Builder sut,
                                                                   IReadsAndWritesDocument documentProvider,
                                                                   IDictionary<string,object> keywordOptions,
                                                                   IServiceProvider serviceProvider)
        {
            sut.DocumentEncoding = Encoding.ASCII;
            sut.DocumentProvider = documentProvider;
            sut.IncludeSourceAnnotation = true;
            sut.KeywordOptions = keywordOptions;
            sut.OmitXmlDeclaration = true;
            sut.ServiceProvider = serviceProvider;

            var result = sut.GetConfig();

            Assert.That(result,
                        Has.Property(nameof(RenderingConfig.DocumentEncoding)).EqualTo(Encoding.ASCII)
                        .And.Property(nameof(RenderingConfig.DocumentProvider)).SameAs(documentProvider)
                        .And.Property(nameof(RenderingConfig.IncludeSourceAnnotation)).True
                        .And.Property(nameof(RenderingConfig.KeywordOptions)).SameAs(keywordOptions)
                        .And.Property(nameof(RenderingConfig.OmitXmlDeclaration)).True
                        .And.Property(nameof(RenderingConfig.ServiceProvider)).SameAs(serviceProvider));
        }

        [Test, AutoMoqData]
        public void Builder_creates_config_with_correct_context_factory(RenderingConfig.Builder sut,
                                                                        IGetsNamedTalesValue namedValueProvider)
        {
            IGetsNamedTalesValue ContextFactory(ExpressionContext ctx) => namedValueProvider;
            sut.BuiltinContextsProvider = ContextFactory;

            var result = sut.GetConfig().BuiltinContextsProvider(null);

            Assert.That(result, Is.SameAs(namedValueProvider));
        }


        [Test, AutoMoqData]
        public void Builder_throws_if_GetConfig_called_twice(RenderingConfig.Builder sut)
        {
            sut.GetConfig();
            Assert.That(() => sut.GetConfig(), Throws.InvalidOperationException);
        }

        [Test, AutoMoqData]
        public void Builder_throws_if_DocumentEncoding_set_after_GetConfig(RenderingConfig.Builder sut)
        {
            sut.GetConfig();
            Assert.That(() => sut.DocumentEncoding = null, Throws.InvalidOperationException);
        }

        [Test, AutoMoqData]
        public void Builder_throws_if_DocumentProvider_set_after_GetConfig(RenderingConfig.Builder sut)
        {
            sut.GetConfig();
            Assert.That(() => sut.DocumentProvider = null, Throws.InvalidOperationException);
        }

        [Test, AutoMoqData]
        public void Builder_throws_if_IncludeSourceAnnotation_set_after_GetConfig(RenderingConfig.Builder sut)
        {
            sut.GetConfig();
            Assert.That(() => sut.IncludeSourceAnnotation = true, Throws.InvalidOperationException);
        }

        [Test, AutoMoqData]
        public void Builder_throws_if_KeywordOptions_set_after_GetConfig(RenderingConfig.Builder sut)
        {
            sut.GetConfig();
            Assert.That(() => sut.KeywordOptions = null, Throws.InvalidOperationException);
        }

        [Test, AutoMoqData]
        public void Builder_throws_if_OmitXmlDeclaration_set_after_GetConfig(RenderingConfig.Builder sut)
        {
            sut.GetConfig();
            Assert.That(() => sut.OmitXmlDeclaration = true, Throws.InvalidOperationException);
        }

        [Test, AutoMoqData]
        public void Builder_throws_if_ServiceProvider_set_after_GetConfig(RenderingConfig.Builder sut)
        {
            sut.GetConfig();
            Assert.That(() => sut.ServiceProvider = null, Throws.InvalidOperationException);
        }

        [Test, AutoMoqData]
        public void Builder_throws_if_BuiltinContextsProvider_set_after_GetConfig(RenderingConfig.Builder sut)
        {
            sut.GetConfig();
            Assert.That(() => sut.BuiltinContextsProvider = null, Throws.InvalidOperationException);
        }
    }
}
