using System;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class AngleSharpHostingBuilderExtensionsTests
    {
        [Test, AutoMoqData]
        public void AddAngleSharpZptDocuments_adds_DI_registration_for_HapDocumentProvider()
        {
            var builder = new Hosting.HostingBuilder(new ServiceCollection());
            builder.AddAngleSharpZptDocuments();
            var provider = builder.ServiceCollection.BuildServiceProvider();

            Assert.That(() => provider.GetRequiredService<AngleSharpDocumentProvider>(), Is.Not.Null);
        }

        [Test, AutoMoqData]
        public void AddAngleSharpZptDocuments_adds_registry_entry_for_AngleSharp()
        {
            var builder = new Hosting.HostingBuilder(new ServiceCollection());
            builder.AddAngleSharpZptDocuments();
            var provider = builder.ServiceCollection.BuildServiceProvider();

            Assert.That(() => builder.ServiceRegistry.DocumentProviderTypes, Has.One.EqualTo(typeof(AngleSharpDocumentProvider)));
        }
    }
}
