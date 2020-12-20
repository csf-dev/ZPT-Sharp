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
    public class ContentOrReplaceAttributeDecoratorTests
    {
        [Test, AutoMoqData]
        public async Task ProcessContextAsync_returns_wrapped_result_if_node_has_neither_attribute([Frozen] IHandlesProcessingError wrapped,
                                                                                                      [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                                      ContentOrReplaceAttributeDecorator sut,
                                                                                                      AttributeSpec content,
                                                                                                      AttributeSpec replace,
                                                                                                      [StubDom] ExpressionContext context,
                                                                                                      ExpressionContextProcessingResult wrappedResult)
        {
            Mock.Get(specProvider).SetupGet(x => x.Content).Returns(content);
            Mock.Get(specProvider).SetupGet(x => x.Replace).Returns(replace);
            context.CurrentNode.Attributes.Clear();
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(wrappedResult));

            var result = await sut.ProcessContextAsync(context);

            Assert.That(result, Is.SameAs(wrappedResult));
        }

        [Test, AutoMoqData]
        public void ProcessContextAsync_throws_if_node_has_both_attributes([Frozen] IGetsTalAttributeSpecs specProvider,
                                                                              ContentOrReplaceAttributeDecorator sut,
                                                                              AttributeSpec content,
                                                                              AttributeSpec replace,
                                                                              [StubDom] ExpressionContext context,
                                                                              [StubDom] IAttribute attrib1,
                                                                              [StubDom] IAttribute attrib2)
        {
            Mock.Get(specProvider).SetupGet(x => x.Content).Returns(content);
            Mock.Get(specProvider).SetupGet(x => x.Replace).Returns(replace);
            context.CurrentNode.Attributes.Clear();
            context.CurrentNode.Attributes.Add(attrib1);
            context.CurrentNode.Attributes.Add(attrib2);
            Mock.Get(attrib1).Setup(x => x.Matches(content)).Returns(true);
            Mock.Get(attrib2).Setup(x => x.Matches(replace)).Returns(true);

            Assert.That(() => sut.ProcessContextAsync(context).Result,
                        Throws.InstanceOf<InvalidTalAttributeException>());
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_returns_wrapped_result_if_content_attribute_aborts_action([Frozen] IHandlesProcessingError wrapped,
                                                                                                        [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                                        [Frozen] IEvaluatesDomValueExpression evaluator,
                                                                                                        ContentOrReplaceAttributeDecorator sut,
                                                                                                        AttributeSpec content,
                                                                                                        AttributeSpec replace,
                                                                                                        [StubDom] ExpressionContext context,
                                                                                                        [StubDom] IAttribute attrib,
                                                                                                        ExpressionContextProcessingResult wrappedResult)
        {
            Mock.Get(specProvider).SetupGet(x => x.Content).Returns(content);
            Mock.Get(specProvider).SetupGet(x => x.Replace).Returns(replace);
            context.CurrentNode.Attributes.Clear();
            context.CurrentNode.Attributes.Add(attrib);
            Mock.Get(attrib).Setup(x => x.Matches(content)).Returns(true);
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(wrappedResult));
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(attrib.Value, context, CancellationToken.None))
                .Returns(() => Task.FromResult(new DomValueExpressionResult(abortAction: true)));

            var result = await sut.ProcessContextAsync(context);

            Assert.That(result, Is.SameAs(wrappedResult));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_replaces_children_with_DOM_expression_result_for_content_attribute([Frozen] IHandlesProcessingError wrapped,
                                                                                                                 [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                                                 [Frozen] IEvaluatesDomValueExpression evaluator,
                                                                                                                 ContentOrReplaceAttributeDecorator sut,
                                                                                                                 AttributeSpec content,
                                                                                                                 AttributeSpec replace,
                                                                                                                 [StubDom] ExpressionContext context,
                                                                                                                 [StubDom] IAttribute attrib,
                                                                                                                 [StubDom] INode node1,
                                                                                                                 [StubDom] INode node2,
                                                                                                                 ExpressionContextProcessingResult wrappedResult)
        {
            Mock.Get(specProvider).SetupGet(x => x.Content).Returns(content);
            Mock.Get(specProvider).SetupGet(x => x.Replace).Returns(replace);
            context.CurrentNode.Attributes.Clear();
            context.CurrentNode.Attributes.Add(attrib);
            Mock.Get(attrib).Setup(x => x.Matches(content)).Returns(true);
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(wrappedResult));
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(attrib.Value, context, CancellationToken.None))
                .Returns(() => Task.FromResult(new DomValueExpressionResult(new[] { node1, node2 })));

            await sut.ProcessContextAsync(context);

            Assert.That(context.CurrentNode.ChildNodes, Is.EqualTo(new[] { node1, node2 }));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_omits_node_if_replace_attribute_aborts_action([Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                               [Frozen] IEvaluatesDomValueExpression evaluator,
                                                                                               [Frozen] IOmitsNode omitter,
                                                                                               ContentOrReplaceAttributeDecorator sut,
                                                                                               AttributeSpec content,
                                                                                               AttributeSpec replace,
                                                                                               [StubDom] ExpressionContext context,
                                                                                               [StubDom] IAttribute attrib,
                                                                                               ExpressionContextProcessingResult wrappedResult)
        {
            Mock.Get(specProvider).SetupGet(x => x.Content).Returns(content);
            Mock.Get(specProvider).SetupGet(x => x.Replace).Returns(replace);
            context.CurrentNode.Attributes.Clear();
            context.CurrentNode.Attributes.Add(attrib);
            Mock.Get(attrib).Setup(x => x.Matches(replace)).Returns(true);
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(attrib.Value, context, CancellationToken.None))
                .Returns(() => Task.FromResult(new DomValueExpressionResult(abortAction: true)));

            await sut.ProcessContextAsync(context);

            Mock.Get(omitter).Verify(x => x.Omit(context.CurrentNode), Times.Once);
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_replaces_node_with_DOM_expression_result_for_replace_attribute( [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                                                 [Frozen] IEvaluatesDomValueExpression evaluator,
                                                                                                                 [Frozen] IReplacesNode replacer,
                                                                                                                 ContentOrReplaceAttributeDecorator sut,
                                                                                                                 AttributeSpec content,
                                                                                                                 AttributeSpec replace,
                                                                                                                 [StubDom] ExpressionContext context,
                                                                                                                 [StubDom] IAttribute attrib,
                                                                                                                 [StubDom] INode node1,
                                                                                                                 [StubDom] INode node2,
                                                                                                                 ExpressionContextProcessingResult wrappedResult)
        {
            Mock.Get(specProvider).SetupGet(x => x.Content).Returns(content);
            Mock.Get(specProvider).SetupGet(x => x.Replace).Returns(replace);
            context.CurrentNode.Attributes.Clear();
            context.CurrentNode.Attributes.Add(attrib);
            Mock.Get(attrib).Setup(x => x.Matches(replace)).Returns(true);
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(attrib.Value, context, CancellationToken.None))
                .Returns(() => Task.FromResult(new DomValueExpressionResult(new[] { node1, node2 })));

            await sut.ProcessContextAsync(context);

            Mock.Get(replacer).Verify(x => x.Replace(context.CurrentNode, new[] { node1, node2 }), Times.Once);
        }
    }
}
