using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    [TestFixture,Parallelizable]
    public class ExpressionContextFactoryTests
    {
        [Test, AutoMoqData]
        public void GetExpressionContext_returns_context_with_correct_document_root_element_and_model(IDocument document,
                                                                                                      [DefaultConfig] RenderZptDocumentRequest request,
                                                                                                      INode element,
                                                                                                      ExpressionContextFactory sut)
        {
            Mock.Get(document).SetupGet(x => x.RootElement).Returns(element);
            var result = sut.GetExpressionContext(document, request);

            Assert.That(result.TemplateDocument, Is.SameAs(document), $"{nameof(ExpressionContext.TemplateDocument)} is correct");
            Assert.That(result.CurrentElement, Is.SameAs(element), $"{nameof(ExpressionContext.CurrentElement)} is correct");
            Assert.That(result.Model, Is.SameAs(request.Model), $"{nameof(ExpressionContext.Model)} is correct");
        }

        [Test, AutoMoqData]
        public void GetChildContexts_returns_a_context_for_each_child_element(ExpressionContext context,
                                                                              INode child1,
                                                                              INode child2,
                                                                              ExpressionContextFactory sut)
        {
            Mock.Get(child1).SetupGet(x => x.IsElement).Returns(true);
            Mock.Get(child2).SetupGet(x => x.IsElement).Returns(true);
            Mock.Get(context.CurrentElement).SetupGet(x => x.ChildNodes).Returns(() => new[] { child1, child2 });

            Assert.That(() => sut.GetChildContexts(context), Has.Count.EqualTo(2));
        }

        [Test, AutoMoqData]
        public void GetChildContexts_does_not_create_contexts_for_nodes_which_are_not_elements(ExpressionContext context,
                                                                                               INode child1,
                                                                                               INode child2,
                                                                                               ExpressionContextFactory sut)
        {
            Mock.Get(child1).SetupGet(x => x.IsElement).Returns(true);
            Mock.Get(child2).SetupGet(x => x.IsElement).Returns(false);
            Mock.Get(context.CurrentElement).SetupGet(x => x.ChildNodes).Returns(() => new[] { child1, child2 });

            Assert.That(() => sut.GetChildContexts(context), Has.Count.EqualTo(1));
        }

        [Test, AutoMoqData]
        public void GetChildContexts_returns_a_context_with_correct_properties(ExpressionContext context,
                                                                               INode child1,
                                                                               ExpressionContextFactory sut)
        {
            Mock.Get(child1).SetupGet(x => x.IsElement).Returns(true);
            Mock.Get(context.CurrentElement).SetupGet(x => x.ChildNodes).Returns(() => new[] { child1 });

            var result = sut.GetChildContexts(context).Single();

            Assert.That(result.CurrentElement, Is.SameAs(child1), $"{nameof(ExpressionContext.CurrentElement)} is correct");
            Assert.That(result.TemplateDocument, Is.SameAs(context.TemplateDocument), $"{nameof(ExpressionContext.TemplateDocument)} is correct");
            Assert.That(result.Model, Is.SameAs(context.Model), $"{nameof(ExpressionContext.Model)} is correct");
            Assert.That(result.LocalDefinitions, Is.EqualTo(context.LocalDefinitions), $"{nameof(ExpressionContext.LocalDefinitions)} is correct");
            Assert.That(result.GlobalDefinitions, Is.EqualTo(context.GlobalDefinitions), $"{nameof(ExpressionContext.GlobalDefinitions)} is correct");
            Assert.That(result.Repetitions, Is.EqualTo(context.Repetitions), $"{nameof(ExpressionContext.Repetitions)} is correct");
        }
    }
}
