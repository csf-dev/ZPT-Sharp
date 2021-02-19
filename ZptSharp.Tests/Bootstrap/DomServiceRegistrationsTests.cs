using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using ZptSharp.Dom;

namespace ZptSharp.Bootstrap
{
    [TestFixture,Parallelizable]
    public class DomServiceRegistrationsTests
    {
        [Test]
        public void Resolving_a_document_reader_writer_returns_an_instance_if_an_impl_has_been_stored_in_a_scope_for_later_use()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddZptSharp().AddHapZptDocuments().AddAngleSharpZptDocuments();

            using (var provider = serviceCollection.BuildServiceProvider())
            using (var scope = provider.CreateScope())
            {
                var store = scope.ServiceProvider.GetRequiredService<IStoresCurrentReaderWriter>();
                store.ReaderWriter = scope.ServiceProvider.GetRequiredService<HapDocumentProvider>();

                Assert.That(() => scope.ServiceProvider.GetRequiredService<IReadsAndWritesDocument>(), Is.Not.Null);
            }
        }

        [Test]
        public void Resolving_a_document_reader_writer_returns_an_instance_if_only_a_single_impl_is_registered()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddZptSharp().AddHapZptDocuments();

            using (var provider = serviceCollection.BuildServiceProvider())
            using (var scope = provider.CreateScope())
            {
                Assert.That(() => scope.ServiceProvider.GetRequiredService<IReadsAndWritesDocument>(), Is.Not.Null);
            }
        }

        [Test]
        public void Resolving_a_document_reader_writer_throws_if_multiple_instances_are_registered_but_none_stored_for_later_use()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddZptSharp().AddHapZptDocuments().AddAngleSharpZptDocuments();

            using (var provider = serviceCollection.BuildServiceProvider())
            using (var scope = provider.CreateScope())
            {
                Assert.That(() => scope.ServiceProvider.GetRequiredService<IReadsAndWritesDocument>(), Throws.InstanceOf<InvalidOperationException>());
            }
        }
    }
}