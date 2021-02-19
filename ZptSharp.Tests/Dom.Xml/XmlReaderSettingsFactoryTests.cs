using System.Xml;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;

namespace ZptSharp.Dom.Xml
{
    [TestFixture,Parallelizable]
    public class XmlReaderSettingsFactoryTests
    {
        [Test,AutoMoqData]
        public void Resolver_uses_configured_resolver_if_set([MockedConfig] RenderingConfig config,
                                                             [Frozen] System.IServiceProvider provider,
                                                             XmlReaderSettingsFactory sut,
                                                             XmlUrlResolver resolver)
        {
            Mock.Get(provider).Setup(x => x.GetService(typeof(RenderingConfig))).Returns(config);
            Mock.Get(config).SetupGet(x => x.XmlUrlResolver).Returns(resolver);
            Assert.That(() => sut.Resolver, Is.SameAs(resolver));
        }

        [Test,AutoMoqData]
        public void Resolver_uses_an_instance_of_XhtmlOnlyXmlUrlResolver_if_config_is_null([MockedConfig] RenderingConfig config,
                                                                                           [Frozen] System.IServiceProvider provider,
                                                                                           XmlReaderSettingsFactory sut)
        {
            Mock.Get(provider).Setup(x => x.GetService(typeof(RenderingConfig))).Returns(config);
            Mock.Get(config).SetupGet(x => x.XmlUrlResolver).Returns(() => null);
            Assert.That(() => sut.Resolver, Is.InstanceOf<XhtmlOnlyXmlUrlResolver>());
        }
    }
}