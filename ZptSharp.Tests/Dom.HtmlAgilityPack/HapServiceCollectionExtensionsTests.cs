using System;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class HapServiceCollectionExtensionsTests
    {
        [Test, AutoMoqData]
        public void AddHapZptDocuments_adds_DI_registration_for_HapDocumentProvider()
        {
            var collection = new ServiceCollection();
            collection.AddHapZptDocuments();
            var provider = collection.BuildServiceProvider();

            Assert.That(() => provider.GetRequiredService<HapDocumentProvider>(), Is.Not.Null);
        }

        [Test, AutoMoqData]
        public void UseHapZptDocuments_adds_registration_for_HapDocumentProvider_into_IRegistersDocumentReaderWriter(IRegistersDocumentReaderWriter registry)
        {
            var collection = new ServiceCollection();
            collection.AddSingleton(registry);
            collection.AddHapZptDocuments();
            var provider = collection.BuildServiceProvider();
            provider.UseHapZptDocuments();

            Mock.Get(registry)
                .Verify(x => x.RegisterDocumentReaderWriter(It.IsAny<HapDocumentProvider>()), Times.Once);
        }
    }
}
