using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Dom;

namespace ZptSharp.Rendering
{
    [TestFixture, Parallelizable]
    public class RemoveZptAttributesModifierDecoratorTests
    {
        [Test, AutoMoqData]
        public async Task ModifyDocumentAsync_uses_the_iterative_modifier_with_the_cleanup_context_processor([Frozen] IGetsZptElementAndAttributeRemovalContextProcessor contextProcessorFactory,
                                                                                                             [Frozen] IIterativelyModifiesDocument iterativeModifier,
                                                                                                             [Frozen] IModifiesDocument wrapped,
                                                                                                             RemoveZptAttributesModifierDecorator sut,
                                                                                                             IProcessesExpressionContext processor,
                                                                                                             IDocument document,
                                                                                                             [MockedConfig] RenderZptDocumentRequest request)
        {
            Mock.Get(contextProcessorFactory)
                .Setup(x => x.GetElementAndAttributeRemovalProcessor())
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
