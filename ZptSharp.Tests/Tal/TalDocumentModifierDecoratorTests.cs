using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Dom;
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
                                                                                                                           TalDocumentModifierDecorator sut,
                                                                                                                           IHandlesProcessingError processor,
                                                                                                                           [StubDom] IDocument document,
                                                                                                                           RenderZptDocumentRequest request)
        {
            Mock.Get(contextProcessorFactory)
                .Setup(x => x.GetTalContextProcessor())
                .Returns(processor);

            await sut.ModifyDocumentAsync(document, request);

            Mock.Get(iterativeModifier)
                .Verify(x => x.ModifyDocumentAsync(document, request, processor, CancellationToken.None), Times.Once, "Iterative modifier should be used correctly");
            Mock.Get(wrapped)
                .Verify(x => x.ModifyDocumentAsync(document, request, CancellationToken.None), Times.Once, "Wrapped service should be used correctly");
        }
    }
}
