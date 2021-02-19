using System;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class XmlHostingBuilderExtensionsTests
    {
        [Test, AutoMoqData]
        public void AddXmlZptDocuments_adds_DI_registration_for_XmlDocumentProvider()
        {
            var builder = new Hosting.HostingBuilder(new ServiceCollection());
            builder.AddXmlZptDocuments();
            var provider = builder.ServiceCollection.BuildServiceProvider();

            Assert.That(() => provider.GetRequiredService<XmlDocumentProvider>(), Is.Not.Null);
        }

        [Test, AutoMoqData]
        public void AddXmlZptDocuments_adds_registry_entry_for_AngleSharp()
        {
            var builder = new Hosting.HostingBuilder(new ServiceCollection());
            builder.AddXmlZptDocuments();
            var provider = builder.ServiceCollection.BuildServiceProvider();

            Assert.That(() => builder.ServiceRegistry.DocumentProviderTypes, Has.One.EqualTo(typeof(XmlDocumentProvider)));
        }
    }
}
