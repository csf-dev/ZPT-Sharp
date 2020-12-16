using System;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class AngleSharpServiceCollectionExtensionsTests
    {
        [Test, AutoMoqData]
        public void AddHapZptDocuments_adds_DI_registration_for_HapDocumentProvider()
        {
            var collection = new ServiceCollection();
            collection.AddAngleSharpZptDocuments();
            var provider = collection.BuildServiceProvider();

            Assert.That(() => provider.GetRequiredService<AngleSharpDocumentProvider>(), Is.Not.Null);
        }

        [Test, AutoMoqData]
        public void UseHapZptDocuments_adds_registration_for_HapDocumentProvider_into_IRegistersDocumentReaderWriter(IRegistersDocumentReaderWriter registry)
        {
            var collection = new ServiceCollection();
            collection.AddSingleton(registry);
            collection.AddAngleSharpZptDocuments();
            var provider = collection.BuildServiceProvider();
            provider.UseAngleSharpZptDocuments();

            Mock.Get(registry)
                .Verify(x => x.RegisterDocumentReaderWriter(It.IsAny<AngleSharpDocumentProvider>()), Times.Once);
        }
    }
}
