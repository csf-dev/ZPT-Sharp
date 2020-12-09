using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;
using ZptSharp.Dom;
using ZptSharp.Rendering;
using ZptSharp.Util;

namespace ZptSharp
{
    [TestFixture,Parallelizable]
    public class ZptFileRendererTests
    {
        [Test, AutoMoqData, Parallelizable(ParallelScope.None)]
        public async Task RenderAsync_can_render_a_file_using_a_detected_reader_writer([MockedConfig] RenderingConfig config,
                                                                                       [Frozen] IServiceProvider serviceProvider,
                                                                                       ZptFileRenderer sut,
                                                                                       IGetsDocumentReaderWriterForFile readerWriterFactory,
                                                                                       IReadsAndWritesDocument readerWriter,
                                                                                       IGetsZptDocumentRenderer rendererFactory,
                                                                                       IRendersZptDocument renderer,
                                                                                       object model,
                                                                                       Stream outputStream)
        {
            string filePath = TestFiles.GetPath("SampleZptDocument.txt");

            Mock.Get(serviceProvider)
                .Setup(x => x.GetService(typeof(IGetsDocumentReaderWriterForFile)))
                .Returns(readerWriterFactory);
            Mock.Get(serviceProvider)
                .Setup(x => x.GetService(typeof(IGetsZptDocumentRenderer)))
                .Returns(rendererFactory);
            Mock.Get(readerWriterFactory)
                .Setup(x => x.GetDocumentProvider(filePath))
                .Returns(readerWriter);
            Mock.Get(rendererFactory)
                .Setup(x => x.GetDocumentRenderer(readerWriter))
                .Returns(renderer);
            Mock.Get(renderer)
                .Setup(x => x.RenderAsync(It.IsAny<Stream>(), model, config, It.IsAny<CancellationToken>(), It.IsAny<FileSourceInfo>()))
                .Returns(() => Task.FromResult(outputStream));

            var result = await sut.RenderAsync(filePath, model, config);

            Assert.That(result, Is.SameAs(outputStream));
        }

        [Test, AutoMoqData]
        public void RenderAsync_throws_file_not_found_exception_if_source_file_does_not_exist(ZptFileRenderer sut,
                                                                                              object model,
                                                                                              Stream outputStream)
        {
            string filePath = TestFiles.GetPath("NonExistentFile.txt");

            Assert.That(() => sut.RenderAsync(filePath, model).Result, Throws.InstanceOf<FileNotFoundException>());
        }

        [Test, AutoMoqData, Parallelizable(ParallelScope.None)]
        public void RenderAsync_throws_no_matching_readerwriter_exception_if_one_cannot_be_inferred([Frozen] IServiceProvider serviceProvider,
                                                                                                    ZptFileRenderer sut,
                                                                                                    IGetsDocumentReaderWriterForFile readerWriterFactory,
                                                                                                    IGetsZptDocumentRenderer rendererFactory,
                                                                                                    object model,
                                                                                                    Stream outputStream)
        {
            string filePath = TestFiles.GetPath("SampleZptDocument.txt");

            Mock.Get(serviceProvider)
                .Setup(x => x.GetService(typeof(IGetsDocumentReaderWriterForFile)))
                .Returns(readerWriterFactory);
            Mock.Get(serviceProvider)
                .Setup(x => x.GetService(typeof(IGetsZptDocumentRenderer)))
                .Returns(rendererFactory);
            Mock.Get(readerWriterFactory)
                .Setup(x => x.GetDocumentProvider(filePath))
                .Returns(() => null);

            Assert.That(() => sut.RenderAsync(filePath, model).Result, Throws.InstanceOf<NoMatchingReaderWriterException>());
        }
    }
}
