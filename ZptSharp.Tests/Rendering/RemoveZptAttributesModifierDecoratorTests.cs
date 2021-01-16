using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    [TestFixture, Parallelizable]
    public class RemoveZptAttributesModifierDecoratorTests
    {
        [Test, AutoMoqData]
        public async Task ModifyDocumentAsync_uses_the_iterative_modifier_with_the_cleanup_context_processor([Frozen] IGetsZptNodeAndAttributeRemovalContextProcessor contextProcessorFactory,
                                                                                                             [Frozen] IIterativelyModifiesDocument iterativeModifier,
                                                                                                             [Frozen] IModifiesDocument wrapped,
                                                                                                             [Frozen] IGetsRootExpressionContext contextProvider,
                                                                                                             RemoveZptAttributesModifierDecorator sut,
                                                                                                             IProcessesExpressionContext processor,
                                                                                                             IDocument document,
                                                                                                             object model,
                                                                                                             ExpressionContext context)
        {
            Mock.Get(contextProcessorFactory)
                .Setup(x => x.GetNodeAndAttributeRemovalProcessor())
                .Returns(processor);
            Mock.Get(contextProvider)
                .Setup(x => x.GetExpressionContext(document, model))
                .Returns(context);
            Mock.Get(iterativeModifier)
                .Setup(x => x.ModifyDocumentAsync(context, processor, CancellationToken.None))
                .Returns(Task.CompletedTask);
            Mock.Get(wrapped)
                .Setup(x => x.ModifyDocumentAsync(document, model, CancellationToken.None))
                .Returns(Task.CompletedTask);

            await sut.ModifyDocumentAsync(document, model);

            Mock.Get(iterativeModifier)
                .Verify(x => x.ModifyDocumentAsync(context, processor, CancellationToken.None), Times.Once);
        }
    }
}
