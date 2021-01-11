using System;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class HapHostingBuilderExtensionsTests
    {
        [Test, AutoMoqData]
        public void AddHapZptDocuments_adds_DI_registration_for_HapDocumentProvider()
        {
            var builder = new Hosting.HostingBuilder(new ServiceCollection());
            builder.AddHapZptDocuments();
            var provider = builder.ServiceCollection.BuildServiceProvider();

            Assert.That(() => provider.GetRequiredService<HapDocumentProvider>(), Is.Not.Null);
        }

        [Test, AutoMoqData]
        public void AddHapZptDocuments_adds_registry_entry_for_AngleSharp()
        {
            var builder = new Hosting.HostingBuilder(new ServiceCollection());
            builder.AddHapZptDocuments();
            var provider = builder.ServiceCollection.BuildServiceProvider();

            Assert.That(() => builder.ServiceRegistry.DocumentProviderTypes, Has.One.EqualTo(typeof(HapDocumentProvider)));
        }
    }
}
