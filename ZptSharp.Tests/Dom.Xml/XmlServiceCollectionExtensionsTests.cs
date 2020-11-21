using System;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class XmlServiceCollectionExtensionsTests
    {
        [Test, AutoMoqData]
        public void AddXmlZptDocuments_adds_DI_registration_for_XmlDocumentProvider()
        {
            var collection = new ServiceCollection();
            collection.AddXmlZptDocuments();
            var provider = collection.BuildServiceProvider();

            Assert.That(() => provider.GetRequiredService<XmlDocumentProvider>(), Is.Not.Null);
        }

        [Test, AutoMoqData]
        public void UseXmlZptDocuments_adds_registration_for_XmlDocumentProvider_into_IRegistersDocumentReaderWriter(IRegistersDocumentReaderWriter registry)
        {
            var collection = new ServiceCollection();
            collection.AddSingleton(registry);
            collection.AddXmlZptDocuments();
            var provider = collection.BuildServiceProvider();
            provider.UseXmlZptDocuments();

            Mock.Get(registry)
                .Verify(x => x.RegisterDocumentReaderWriter(It.IsAny<XmlDocumentProvider>()), Times.Once);
        }
    }
}
