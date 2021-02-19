using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Dom;
using ZptSharp.Expressions;
using ZptSharp.Rendering;

namespace ZptSharp.Tal
{
    [TestFixture,Parallelizable]
    public class TalDocumentModifierDecoratorTests
    {
        [Test, AutoMoqData]
        public async Task ModifyDocumentAsync_uses_tal_context_processor_with_iterative_modifier_and_calls_wrapped_service([Frozen] IGetsTalContextProcessor contextProcessorFactory,
                                                                                                                           [Frozen] IIterativelyModifiesDocument iterativeModifier,
                                                                                                                           [Frozen] IModifiesDocument wrapped,
                                                                                                                           [Frozen] IGetsRootExpressionContext rootContextProvider,
                                                                                                                           TalDocumentModifierDecorator sut,
                                                                                                                           IHandlesProcessingError processor,
                                                                                                                           [StubDom] IDocument document,
                                                                                                                           object model,
                                                                                                                           ExpressionContext context)
        {
            Mock.Get(contextProcessorFactory)
                .Setup(x => x.GetTalContextProcessor())
                .Returns(processor);
            Mock.Get(rootContextProvider)
                .Setup(x => x.GetExpressionContext(document, model))
                .Returns(context);

            await sut.ModifyDocumentAsync(document, model);

            Mock.Get(iterativeModifier)
                .Verify(x => x.ModifyDocumentAsync(context, processor, CancellationToken.None), Times.Once, "Iterative modifier should be used correctly");
            Mock.Get(wrapped)
                .Verify(x => x.ModifyDocumentAsync(document, model, CancellationToken.None), Times.Once, "Wrapped service should be used correctly");
        }
    }
}
