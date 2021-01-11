using System;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class DocumentReaderWriterRegistryTests
    {
        [Test, AutoMoqData]
        public void GetDocumentProvider_returns_matching_readerwriter_if_it_is_present([Frozen] Hosting.EnvironmentRegistry registry,
                                                                                       [Frozen] IServiceProvider provider,
                                                                                       DocumentReaderWriterRegistry sut,
                                                                                       IReadsAndWritesDocument readerWriter1,
                                                                                       IReadsAndWritesDocument readerWriter2,
                                                                                       string filename)
        {
            Mock.Get(provider).Setup(x => x.GetService(typeof(string))).Returns(readerWriter1);
            Mock.Get(provider).Setup(x => x.GetService(typeof(int))).Returns(readerWriter2);
            Mock.Get(readerWriter1).Setup(x => x.CanReadWriteForFilename(filename)).Returns(false);
            Mock.Get(readerWriter2).Setup(x => x.CanReadWriteForFilename(filename)).Returns(true);
            registry.DocumentProviderTypes.Clear();
            registry.DocumentProviderTypes.Add(typeof(string));
            registry.DocumentProviderTypes.Add(typeof(int));

            Assert.That(() => sut.GetDocumentProvider(filename), Is.SameAs(readerWriter2));
        }

        [Test, AutoMoqData]
        public void GetDocumentProvider_returns_null_if_no_matching_instances_present([Frozen] Hosting.EnvironmentRegistry registry,
                                                                                      [Frozen] IServiceProvider provider,
                                                                                      DocumentReaderWriterRegistry sut,
                                                                                      IReadsAndWritesDocument readerWriter1,
                                                                                      IReadsAndWritesDocument readerWriter2,
                                                                                      string filename)
        {
            Mock.Get(provider).Setup(x => x.GetService(typeof(string))).Returns(readerWriter1);
            Mock.Get(provider).Setup(x => x.GetService(typeof(int))).Returns(readerWriter2);
            Mock.Get(readerWriter1).Setup(x => x.CanReadWriteForFilename(filename)).Returns(false);
            Mock.Get(readerWriter2).Setup(x => x.CanReadWriteForFilename(filename)).Returns(false);
            registry.DocumentProviderTypes.Clear();
            registry.DocumentProviderTypes.Add(typeof(string));
            registry.DocumentProviderTypes.Add(typeof(int));

            Assert.That(() => sut.GetDocumentProvider(filename), Is.Null);
        }
    }
}
