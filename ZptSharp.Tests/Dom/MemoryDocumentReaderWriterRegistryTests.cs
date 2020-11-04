using System;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class MemoryDocumentReaderWriterRegistryTests
    {
        [Test, AutoMoqData]
        public void RegisterDocumentReaderWriter_adds_instance_if_it_is_not_already_present(MemoryDocumentReaderWriterRegistry sut,
                                                                                            IReadsAndWritesDocument readerWriter)
        {
            sut.RegisterDocumentReaderWriter(readerWriter);
            Assert.That(sut.Registry, Is.EquivalentTo(new[] { readerWriter }));
        }

        [Test, AutoMoqData]
        public void RegisterDocumentReaderWriter_does_not_add_anything_if_it_has_already_been_added(MemoryDocumentReaderWriterRegistry sut,
                                                                                                    IReadsAndWritesDocument readerWriter)
        {
            sut.RegisterDocumentReaderWriter(readerWriter);
            sut.RegisterDocumentReaderWriter(readerWriter);
            Assert.That(sut.Registry, Is.EquivalentTo(new[] { readerWriter }));
        }

        [Test, AutoMoqData]
        public void RegisterDocumentReaderWriter_can_add_two_different_reader_writers(MemoryDocumentReaderWriterRegistry sut,
                                                                                      IReadsAndWritesDocument readerWriter1,
                                                                                      IReadsAndWritesDocument readerWriter2)
        {
            sut.RegisterDocumentReaderWriter(readerWriter1);
            sut.RegisterDocumentReaderWriter(readerWriter2);
            Assert.That(sut.Registry, Is.EquivalentTo(new[] { readerWriter1, readerWriter2 }));
        }

        [Test, AutoMoqData]
        public void RegisterDocumentReaderWriter_throws_ANE_if_readerwriter_is_null(MemoryDocumentReaderWriterRegistry sut)
        {
            Assert.That(() => sut.RegisterDocumentReaderWriter(null), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void GetDocumentProvider_returns_matching_readerwriter_if_it_is_present(MemoryDocumentReaderWriterRegistry sut,
                                                                                      IReadsAndWritesDocument readerWriter1,
                                                                                      IReadsAndWritesDocument readerWriter2,
                                                                                      string filename)
        {
            Mock.Get(readerWriter1).Setup(x => x.CanReadWriteForFilename(filename)).Returns(false);
            Mock.Get(readerWriter2).Setup(x => x.CanReadWriteForFilename(filename)).Returns(true);
            sut.RegisterDocumentReaderWriter(readerWriter1);
            sut.RegisterDocumentReaderWriter(readerWriter2);

            Assert.That(() => sut.GetDocumentProvider(filename), Is.SameAs(readerWriter2));
        }

        [Test, AutoMoqData]
        public void GetDocumentProvider_returns_null_if_no_matching_instances_present(MemoryDocumentReaderWriterRegistry sut,
                                                                                      IReadsAndWritesDocument readerWriter1,
                                                                                      IReadsAndWritesDocument readerWriter2,
                                                                                      string filename)
        {
            Mock.Get(readerWriter1).Setup(x => x.CanReadWriteForFilename(filename)).Returns(false);
            Mock.Get(readerWriter2).Setup(x => x.CanReadWriteForFilename(filename)).Returns(false);

            Assert.That(() => sut.GetDocumentProvider(filename), Is.Null);
        }
    }
}
