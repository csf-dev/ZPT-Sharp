using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Dom;
using ZptSharp.Metal;

namespace ZptSharp.Rendering
{
    [TestFixture,Parallelizable]
    public class MetalDocumentModifierDecoratorTests
    {
        [Test, AutoMoqData]
        public async Task ModifyDocumentAsync_uses_the_iterative_modifier_with_the_METAL_context_processor([Frozen] IGetsMetalContextProcessor contextProcessorFactory,
                                                                                                           [Frozen] IIterativelyModifiesDocument iterativeModifier,
                                                                                                           [Frozen] IModifiesDocument wrapped,
                                                                                                           MetalDocumentModifierDecorator sut,
                                                                                                           IProcessesExpressionContext processor,
                                                                                                           IDocument document,
                                                                                                           [MockedConfig] RenderZptDocumentRequest request)
        {
            Mock.Get(contextProcessorFactory)
                .Setup(x => x.GetMetalContextProcessor())
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
    }
}
