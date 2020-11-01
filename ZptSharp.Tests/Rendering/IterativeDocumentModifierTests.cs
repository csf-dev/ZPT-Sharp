using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    [TestFixture,Parallelizable]
    public class IterativeDocumentModifierTests
    {
        [Test, AutoMoqData]
        public async Task ModifyDocumentAsync_uses_iterator_from_factory_with_context_processor([Frozen] IGetsIterativeExpressionContextProcessor iteratorFactory,
                                                                                                [Frozen] IGetsRootExpressionContext rootContextProvider,
                                                                                                IDocument document,
                                                                                                [MockedConfig] RenderZptDocumentRequest request,
                                                                                                IProcessesExpressionContext contextProcessor,
                                                                                                ExpressionContext context,
                                                                                                IIterativelyProcessesExpressionContexts iterator,
                                                                                                IterativeDocumentModifier sut)
        {
            Mock.Get(iteratorFactory).Setup(x => x.GetContextIterator(contextProcessor)).Returns(iterator);
            Mock.Get(rootContextProvider).Setup(x => x.GetExpressionContext(document, request)).Returns(context);

            await sut.ModifyDocumentAsync(document, request, contextProcessor);

            Mock.Get(iterator).Verify(x => x.IterateContextAndChildrenAsync(context), Times.Once);
        }
    }
}
