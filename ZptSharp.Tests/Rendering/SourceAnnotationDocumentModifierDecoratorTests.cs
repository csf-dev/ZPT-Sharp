using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;
using ZptSharp.Dom;
using ZptSharp.SourceAnnotation;

namespace ZptSharp.Rendering
{
    [TestFixture,Parallelizable]
    public class SourceAnnotationDocumentModifierDecoratorTests
    {
        [Test, AutoMoqData]
        public async Task ModifyDocumentAsync_uses_the_iterative_modifier_with_the_source_annotation_processor_when_annotation_enabled([Frozen] IGetsSourceAnnotationContextProcessor contextProcessorFactory,
                                                                                                                                       [Frozen] IIterativelyModifiesDocument iterativeModifier,
                                                                                                                                       [Frozen] IModifiesDocument wrapped,
                                                                                                                                       [MockedConfig, Frozen] RenderingConfig config,
                                                                                                                                       SourceAnnotationDocumentModifierDecorator sut,
                                                                                                                                       IProcessesExpressionContext processor,
                                                                                                                                       IDocument document,
                                                                                                                                       RenderZptDocumentRequest request)
        {
            Mock.Get(config).SetupGet(x => x.IncludeSourceAnnotation).Returns(true);
            Mock.Get(contextProcessorFactory)
                .Setup(x => x.GetSourceAnnotationContextProcessor())
                .Returns(processor);
            Mock.Get(iterativeModifier)
                .Setup(x => x.ModifyDocumentAsync(document, request, processor, CancellationToken.None))
                .Returns(Task.CompletedTask);
            Mock.Get(wrapped)
                .Setup(x => x.ModifyDocumentAsync(document, request, CancellationToken.None))
                .Returns(Task.CompletedTask);

            await sut.ModifyDocumentAsync(document, request);

            Mock.Get(iterativeModifier)
                .Verify(x => x.ModifyDocumentAsync(document, request, processor, CancellationToken.None), Times.Once);
        }

        [Test, AutoMoqData]
        public async Task ModifyDocumentAsync_does_nothing_when_annotation_disabled([Frozen] IGetsSourceAnnotationContextProcessor contextProcessorFactory,
                                                                                    [Frozen] IIterativelyModifiesDocument iterativeModifier,
                                                                                    [Frozen] IModifiesDocument wrapped,
                                                                                    [MockedConfig, Frozen] RenderingConfig config,
                                                                                    SourceAnnotationDocumentModifierDecorator sut,
                                                                                    IProcessesExpressionContext processor,
                                                                                    IDocument document,
                                                                                    RenderZptDocumentRequest request)
        {
            Mock.Get(config).SetupGet(x => x.IncludeSourceAnnotation).Returns(false);
            Mock.Get(contextProcessorFactory)
                .Setup(x => x.GetSourceAnnotationContextProcessor())
                .Returns(processor);
            Mock.Get(iterativeModifier)
                .Setup(x => x.ModifyDocumentAsync(document, request, processor, CancellationToken.None))
                .Returns(Task.CompletedTask);
            Mock.Get(wrapped)
                .Setup(x => x.ModifyDocumentAsync(document, request, CancellationToken.None))
                .Returns(Task.CompletedTask);

            await sut.ModifyDocumentAsync(document, request);

            Mock.Get(iterativeModifier)
                .Verify(x => x.ModifyDocumentAsync(document, request, processor, CancellationToken.None), Times.Never);
        }
    }
}
