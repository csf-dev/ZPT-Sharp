using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoFixture;
using AutoFixture.AutoMoq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Config
{
    [TestFixture,Parallelizable]
    public class RenderingConfigTests
    {
        [Test, AutoMoqData]
        public void Builder_creates_config_with_correct_properties(System.Type documentProvider,
                                                                   IDictionary<string,object> keywordOptions)
        {
            var sut = RenderingConfig.CreateBuilder();

            sut.DocumentEncoding = Encoding.ASCII;
            sut.DocumentProviderType = documentProvider;
            sut.IncludeSourceAnnotation = true;
            sut.KeywordOptions = keywordOptions;
            sut.OmitXmlDeclaration = true;

            var result = sut.GetConfig();

            Assert.That(result,
                        Has.Property(nameof(RenderingConfig.DocumentEncoding)).EqualTo(Encoding.ASCII)
                        .And.Property(nameof(RenderingConfig.DocumentProviderType)).SameAs(documentProvider)
                        .And.Property(nameof(RenderingConfig.IncludeSourceAnnotation)).True
                        .And.Property(nameof(RenderingConfig.KeywordOptions)).EqualTo(keywordOptions)
                        .And.Property(nameof(RenderingConfig.OmitXmlDeclaration)).True);
        }

        [Test, AutoMoqData]
        public void Builder_creates_config_with_correct_context_factory(IGetsDictionaryOfNamedTalesValues namedValueProvider)
        {
            var sut = RenderingConfig.CreateBuilder();

            IGetsDictionaryOfNamedTalesValues ContextFactory(ExpressionContext ctx) => namedValueProvider;
            sut.RootContextsProvider = ContextFactory;

            var result = sut.GetConfig().RootContextsProvider(null);

            Assert.That(result, Is.SameAs(namedValueProvider));
        }

        [Test, AutoMoqData]
        public void Builder_creates_config_with_correct_context_builder_action(IGetsNamedTalesValue namedValueProvider)
        {
            var sut = RenderingConfig.CreateBuilder();

            Action<IConfiguresRootContext, IServiceProvider> contextBuilder = (c, s) => { };
            sut.ContextBuilder = contextBuilder;

            var result = sut.GetConfig().ContextBuilder;

            Assert.That(result, Is.SameAs(contextBuilder));
        }


        [Test, AutoMoqData]
        public void Builder_throws_if_GetConfig_called_twice()
        {
            var sut = RenderingConfig.CreateBuilder();

            sut.GetConfig();
            Assert.That(() => sut.GetConfig(), Throws.InvalidOperationException);
        }

        [Test, AutoMoqData]
        public void Builder_throws_if_DocumentEncoding_set_after_GetConfig()
        {
            var sut = RenderingConfig.CreateBuilder();

            sut.GetConfig();
            Assert.That(() => sut.DocumentEncoding = null, Throws.InvalidOperationException);
        }

        [Test, AutoMoqData]
        public void Builder_throws_if_DocumentProvider_set_after_GetConfig()
        {
            var sut = RenderingConfig.CreateBuilder();

            sut.GetConfig();
            Assert.That(() => sut.DocumentProviderType = null, Throws.InvalidOperationException);
        }

        [Test, AutoMoqData]
        public void Builder_throws_if_IncludeSourceAnnotation_set_after_GetConfig()
        {
            var sut = RenderingConfig.CreateBuilder();

            sut.GetConfig();
            Assert.That(() => sut.IncludeSourceAnnotation = true, Throws.InvalidOperationException);
        }

        [Test, AutoMoqData]
        public void Builder_throws_if_KeywordOptions_set_after_GetConfig()
        {
            var sut = RenderingConfig.CreateBuilder();

            sut.GetConfig();
            Assert.That(() => sut.KeywordOptions = null, Throws.InvalidOperationException);
        }

        [Test, AutoMoqData]
        public void Builder_throws_if_OmitXmlDeclaration_set_after_GetConfig()
        {
            var sut = RenderingConfig.CreateBuilder();

            sut.GetConfig();
            Assert.That(() => sut.OmitXmlDeclaration = true, Throws.InvalidOperationException);
        }

        [Test, AutoMoqData]
        public void Builder_throws_if_BuiltinContextsProvider_set_after_GetConfig()
        {
            var sut = RenderingConfig.CreateBuilder();

            sut.GetConfig();
            Assert.That(() => sut.RootContextsProvider = null, Throws.InvalidOperationException);
        }

        [Test, AutoMoqData]
        public void All_configuration_instance_properties_must_be_virtual()
        {
            var properties = typeof(RenderingConfig).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            Assert.That(properties, Has.All.Matches<PropertyInfo>(p => p.GetMethod.IsVirtual));
        }

        [Test]
        public void CloneToNewBuilder_then_GetConfig_returns_config_with_equal_properties_to_original([ValueSource(nameof(GetConfigProperties))] string propertyName)
        {
            var fixture = new Fixture();
            new AutoConfigBuilderCustomization().Customize(fixture);
            new AutoMoqCustomization().Customize(fixture);
            var builder = fixture.Create<RenderingConfig.Builder>();

            var originalConfig = builder.GetConfig();
            var clonedConfig = originalConfig.CloneToNewBuilder().GetConfig();

            var property = typeof(RenderingConfig).GetProperty(propertyName);
            var expected = property.GetValue(originalConfig);
            var actual = property.GetValue(clonedConfig);

            Assert.That(actual, Is.EqualTo(expected));
        }

        public static IEnumerable<string> GetConfigProperties()
            => typeof(RenderingConfig).GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(x => x.Name);
    }
}
