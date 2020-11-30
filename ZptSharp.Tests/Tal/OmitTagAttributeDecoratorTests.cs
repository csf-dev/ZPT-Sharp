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
    public class OmitTagAttributeDecoratorTests
    {
        [Test, AutoMoqData]
        public async Task ProcessContextAsync_returns_wrapped_result_if_attribute_not_present([Frozen] IProcessesExpressionContext wrapped,
                                                                                              [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                              OmitTagAttributeDecorator sut,
                                                                                              [StubDom] ExpressionContext context,
                                                                                              AttributeSpec spec,
                                                                                              ExpressionContextProcessingResult wrappedResult)
        {
            Mock.Get(specProvider).SetupGet(x => x.OmitTag).Returns(spec);
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(wrappedResult));

            var result = await sut.ProcessContextAsync(context);

            Assert.That(result, Is.SameAs(wrappedResult));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_returns_wrapped_result_if_expression_aborts_the_action([Frozen] IProcessesExpressionContext wrapped,
                                                                                                     [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                                     [Frozen] IEvaluatesExpression evaluator,
                                                                                                     [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                                     OmitTagAttributeDecorator sut,
                                                                                                     [StubDom] ExpressionContext context,
                                                                                                     AttributeSpec spec,
                                                                                                     [StubDom] IAttribute attribute,
                                                                                                     object expressionResult,
                                                                                                     ExpressionContextProcessingResult wrappedResult)
        {
            Mock.Get(specProvider).SetupGet(x => x.OmitTag).Returns(spec);
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(wrappedResult));
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            context.CurrentElement.Attributes.Clear();
            context.CurrentElement.Attributes.Add(attribute);
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(attribute.Value, context, CancellationToken.None))
                .Returns(() => Task.FromResult(expressionResult));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultAbortTheAction(expressionResult))
                .Returns(true);
            Mock.Get(resultInterpreter)
                .Setup(x => x.CoerceResultToBoolean(expressionResult))
                .Returns(true);

            var result = await sut.ProcessContextAsync(context);

            Assert.That(result, Is.SameAs(wrappedResult));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_returns_wrapped_result_if_expression_is_falsey([Frozen] IProcessesExpressionContext wrapped,
                                                                                             [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                             [Frozen] IEvaluatesExpression evaluator,
                                                                                             [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                             OmitTagAttributeDecorator sut,
                                                                                             [StubDom] ExpressionContext context,
                                                                                             AttributeSpec spec,
                                                                                             [StubDom] IAttribute attribute,
                                                                                             object expressionResult,
                                                                                             ExpressionContextProcessingResult wrappedResult)
        {
            Mock.Get(specProvider).SetupGet(x => x.OmitTag).Returns(spec);
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(wrappedResult));
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            context.CurrentElement.Attributes.Clear();
            context.CurrentElement.Attributes.Add(attribute);
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(attribute.Value, context, CancellationToken.None))
                .Returns(() => Task.FromResult(expressionResult));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultAbortTheAction(expressionResult))
                .Returns(false);
            Mock.Get(resultInterpreter)
                .Setup(x => x.CoerceResultToBoolean(expressionResult))
                .Returns(false);

            var result = await sut.ProcessContextAsync(context);

            Assert.That(result, Is.SameAs(wrappedResult));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_omits_element_if_expression_is_truthy([Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                    [Frozen] IEvaluatesExpression evaluator,
                                                                                    [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                    [Frozen] IOmitsNode omitter,
                                                                                    OmitTagAttributeDecorator sut,
                                                                                    [StubDom] ExpressionContext context,
                                                                                    AttributeSpec spec,
                                                                                    [StubDom] IAttribute attribute,
                                                                                    object expressionResult)
        {
            Mock.Get(specProvider).SetupGet(x => x.OmitTag).Returns(spec);
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            context.CurrentElement.Attributes.Clear();
            context.CurrentElement.Attributes.Add(attribute);
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(attribute.Value, context, CancellationToken.None))
                .Returns(() => Task.FromResult(expressionResult));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultAbortTheAction(expressionResult))
                .Returns(false);
            Mock.Get(resultInterpreter)
                .Setup(x => x.CoerceResultToBoolean(expressionResult))
                .Returns(true);

            await sut.ProcessContextAsync(context);

            Mock.Get(omitter)
                .Verify(x => x.Omit(context.CurrentElement), Times.Once);
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_omits_element_if_expression_is_empty_string([Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                          [Frozen] IOmitsNode omitter,
                                                                                          OmitTagAttributeDecorator sut,
                                                                                          [StubDom] ExpressionContext context,
                                                                                          AttributeSpec spec,
                                                                                          [StubDom] IAttribute attribute,
                                                                                          object expressionResult)
        {
            Mock.Get(specProvider).SetupGet(x => x.OmitTag).Returns(spec);
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            context.CurrentElement.Attributes.Clear();
            context.CurrentElement.Attributes.Add(attribute);
            attribute.Value = String.Empty;

            await sut.ProcessContextAsync(context);

            Mock.Get(omitter)
                .Verify(x => x.Omit(context.CurrentElement), Times.Once);
        }
    }
}
