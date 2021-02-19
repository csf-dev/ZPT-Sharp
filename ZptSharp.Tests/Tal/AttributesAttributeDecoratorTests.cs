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
    public class AttributesAttributeDecoratorTests
    {
        [Test, AutoMoqData]
        public async Task ProcessContextAsync_returns_wrapped_result_if_attribute_not_present([Frozen] IHandlesProcessingError wrapped,
                                                                                              [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                              AttributesAttributeDecorator sut,
                                                                                              [StubDom] ExpressionContext context,
                                                                                              AttributeSpec spec,
                                                                                              ExpressionContextProcessingResult wrappedResult)
        {
            Mock.Get(specProvider).SetupGet(x => x.Attributes).Returns(spec);
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(wrappedResult));

            var result = await sut.ProcessContextAsync(context);

            Assert.That(result, Is.SameAs(wrappedResult));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_can_add_or_modify_attributes([Frozen] IHandlesProcessingError wrapped,
                                                                           [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                           [Frozen] IEvaluatesExpression evaluator,
                                                                           [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                           [Frozen] IGetsAttributeDefinitions definitionsProvider,
                                                                           AttributesAttributeDecorator sut,
                                                                           [StubDom] ExpressionContext context,
                                                                           [StubDom] IAttribute attribute,
                                                                           [StubDom] IAttribute existingAttribute,
                                                                           AttributeSpec spec,
                                                                           ExpressionContextProcessingResult wrappedResult)
        {
            Mock.Get(specProvider).SetupGet(x => x.Attributes).Returns(spec);
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(wrappedResult));
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            context.CurrentNode.Attributes.Clear();
            context.CurrentNode.Attributes.Add(attribute);
            context.CurrentNode.Attributes.Add(existingAttribute);
            Mock.Get(context.CurrentNode)
                .Setup(x => x.CreateAttribute(It.IsAny<AttributeSpec>()))
                .Returns((AttributeSpec s) => new StubAttribute(s.Name));
            Mock.Get(existingAttribute).SetupGet(x => x.Name).Returns("style");
            existingAttribute.Value = "old style value";
            Mock.Get(definitionsProvider)
                .Setup(x => x.GetDefinitions(attribute.Value, context.CurrentNode))
                .Returns(() => new[] {
                    new AttributeDefinition { Name = "class", Expression = "abc" },
                    new AttributeDefinition { Name = "style", Expression = "def" },
                    new AttributeDefinition { Name = "id",    Expression = "ghi" },
                });
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync("abc", context, CancellationToken.None))
                .Returns(() => Task.FromResult<object>("new class value"));
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync("def", context, CancellationToken.None))
                .Returns(() => Task.FromResult<object>("new style value"));
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync("ghi", context, CancellationToken.None))
                .Returns(() => Task.FromResult<object>("new id value"));

            await sut.ProcessContextAsync(context);

            Assert.That(context.CurrentNode.Attributes,
                        Has.One.Matches<IAttribute>(x => x.Name == "class" && x.Value == "new class value")
                       .And.One.Matches<IAttribute>(x => x.Name == "style" && x.Value == "new style value")
                       .And.One.Matches<IAttribute>(x => x.Name == "id" && x.Value == "new id value"));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_can_remove_an_attribute_if_its_expression_evaluates_to_null([Frozen] IHandlesProcessingError wrapped,
                                                                                                          [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                                          [Frozen] IEvaluatesExpression evaluator,
                                                                                                          [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                                          [Frozen] IGetsAttributeDefinitions definitionsProvider,
                                                                                                          AttributesAttributeDecorator sut,
                                                                                                          [StubDom] ExpressionContext context,
                                                                                                          [StubDom] IAttribute attribute,
                                                                                                          [StubDom] IAttribute existingAttribute,
                                                                                                          AttributeSpec spec,
                                                                                                          ExpressionContextProcessingResult wrappedResult)
        {
            Mock.Get(specProvider).SetupGet(x => x.Attributes).Returns(spec);
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(wrappedResult));
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            context.CurrentNode.Attributes.Clear();
            context.CurrentNode.Attributes.Add(attribute);
            context.CurrentNode.Attributes.Add(existingAttribute);
            Mock.Get(context.CurrentNode)
                .Setup(x => x.CreateAttribute(It.IsAny<AttributeSpec>()))
                .Returns((AttributeSpec s) => new StubAttribute(s.Name));
            Mock.Get(existingAttribute).SetupGet(x => x.Name).Returns("style");
            existingAttribute.Value = "old style value";
            Mock.Get(definitionsProvider)
                .Setup(x => x.GetDefinitions(attribute.Value, context.CurrentNode))
                .Returns(() => new[] {
                    new AttributeDefinition { Name = "style", Expression = "def" },
                });
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync("def", context, CancellationToken.None))
                .Returns(() => Task.FromResult<object>(null));

            await sut.ProcessContextAsync(context);

            // The attributes attribute remains, but the other attribute should be removed
            Assert.That(context.CurrentNode.Attributes, Is.EqualTo(new[] { attribute }));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_leaves_an_attribute_unchanged_if_its_expression_aborts_the_action([Frozen] IHandlesProcessingError wrapped,
                                                                                                                [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                                                [Frozen] IEvaluatesExpression evaluator,
                                                                                                                [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                                                [Frozen] IGetsAttributeDefinitions definitionsProvider,
                                                                                                                AttributesAttributeDecorator sut,
                                                                                                                [StubDom] ExpressionContext context,
                                                                                                                [StubDom] IAttribute attribute,
                                                                                                                [StubDom] IAttribute existingAttribute,
                                                                                                                AttributeSpec spec,
                                                                                                                ExpressionContextProcessingResult wrappedResult,
                                                                                                                object expressionResult)
        {
            Mock.Get(specProvider).SetupGet(x => x.Attributes).Returns(spec);
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(wrappedResult));
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            context.CurrentNode.Attributes.Clear();
            context.CurrentNode.Attributes.Add(attribute);
            context.CurrentNode.Attributes.Add(existingAttribute);
            Mock.Get(existingAttribute).SetupGet(x => x.Name).Returns("style");
            existingAttribute.Value = "old style value";
            Mock.Get(definitionsProvider)
                .Setup(x => x.GetDefinitions(attribute.Value, context.CurrentNode))
                .Returns(() => new[] {
                    new AttributeDefinition { Name = "style", Expression = "def" },
                });
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync("def", context, CancellationToken.None))
                .Returns(() => Task.FromResult(expressionResult));
            Mock.Get(resultInterpreter).Setup(x => x.DoesResultAbortTheAction(expressionResult)).Returns(true);

            await sut.ProcessContextAsync(context);

            Assert.That(context.CurrentNode.Attributes, Has.One.Matches<IAttribute>(x => x.Name == "style" && x.Value == "old style value"));
        }
    }
}
