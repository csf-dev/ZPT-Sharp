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
    public class ConditionAttributeDecoratorTests
    {
        [Test, AutoMoqData]
        public async Task ProcessContextAsync_removes_the_element_if_a_condition_attribute_evaluates_to_falsey_value([Frozen] IProcessesExpressionContext wrapped,
                                                                                                                     [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                                                     [Frozen] IEvaluatesExpression evaluator,
                                                                                                                     [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                                                     ConditionAttributeDecorator sut,
                                                                                                                     AttributeSpec spec,
                                                                                                                     [StubDom] ExpressionContext context,
                                                                                                                     [StubDom] IAttribute attribute,
                                                                                                                     [StubDom] INode parent,
                                                                                                                     object expressionResult)
        {
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(ExpressionContextProcessingResult.Noop));
            Mock.Get(specProvider)
                .SetupGet(x => x.Condition)
                .Returns(spec);
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(attribute.Value, context, CancellationToken.None))
                .Returns(() => Task.FromResult(expressionResult));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultAbortTheAction(expressionResult))
                .Returns(false);
            Mock.Get(resultInterpreter)
                .Setup(x => x.CoerceResultToBoolean(expressionResult))
                .Returns(false);
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            context.CurrentElement.Attributes.Clear();
            context.CurrentElement.Attributes.Add(attribute);
            context.CurrentElement.ParentElement = parent;
            parent.ChildNodes.Add(context.CurrentElement);

            await sut.ProcessContextAsync(context);

            Assert.That(parent.ChildNodes, Has.None.SameAs(context.CurrentElement));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_return_abort_processing_result_if_a_condition_attribute_evaluates_to_falsey_value([Frozen] IProcessesExpressionContext wrapped,
                                                                                                                                [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                                                                [Frozen] IEvaluatesExpression evaluator,
                                                                                                                                [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                                                                ConditionAttributeDecorator sut,
                                                                                                                                AttributeSpec spec,
                                                                                                                                [StubDom] ExpressionContext context,
                                                                                                                                [StubDom] IAttribute attribute,
                                                                                                                                [StubDom] INode parent,
                                                                                                                                object expressionResult)
        {
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(ExpressionContextProcessingResult.Noop));
            Mock.Get(specProvider)
                .SetupGet(x => x.Condition)
                .Returns(spec);
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(attribute.Value, context, CancellationToken.None))
                .Returns(() => Task.FromResult(expressionResult));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultAbortTheAction(expressionResult))
                .Returns(false);
            Mock.Get(resultInterpreter)
                .Setup(x => x.CoerceResultToBoolean(expressionResult))
                .Returns(false);
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            context.CurrentElement.Attributes.Clear();
            context.CurrentElement.Attributes.Add(attribute);
            context.CurrentElement.ParentElement = parent;
            parent.ChildNodes.Add(context.CurrentElement);

            var result = await sut.ProcessContextAsync(context);

            Assert.That(result?.AbortFurtherProcessing, Is.True);
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_does_not_remove_element_if_a_condition_attribute_evaluates_to_truthy_value([Frozen] IProcessesExpressionContext wrapped,
                                                                                                                         [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                                                         [Frozen] IEvaluatesExpression evaluator,
                                                                                                                         [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                                                         ConditionAttributeDecorator sut,
                                                                                                                         AttributeSpec spec,
                                                                                                                         [StubDom] ExpressionContext context,
                                                                                                                         [StubDom] IAttribute attribute,
                                                                                                                         [StubDom] INode parent,
                                                                                                                         object expressionResult)
        {
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(ExpressionContextProcessingResult.Noop));
            Mock.Get(specProvider)
                .SetupGet(x => x.Condition)
                .Returns(spec);
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(attribute.Value, context, CancellationToken.None))
                .Returns(() => Task.FromResult(expressionResult));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultAbortTheAction(expressionResult))
                .Returns(false);
            Mock.Get(resultInterpreter)
                .Setup(x => x.CoerceResultToBoolean(expressionResult))
                .Returns(true);
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            context.CurrentElement.Attributes.Clear();
            context.CurrentElement.Attributes.Add(attribute);
            context.CurrentElement.ParentElement = parent;
            parent.ChildNodes.Add(context.CurrentElement);

            await sut.ProcessContextAsync(context);

            Assert.That(parent.ChildNodes, Has.One.SameAs(context.CurrentElement));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_does_not_remove_element_if_a_condition_attribute_evaluates_to_cancellation([Frozen] IProcessesExpressionContext wrapped,
                                                                                                                         [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                                                         [Frozen] IEvaluatesExpression evaluator,
                                                                                                                         [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                                                         ConditionAttributeDecorator sut,
                                                                                                                         AttributeSpec spec,
                                                                                                                         [StubDom] ExpressionContext context,
                                                                                                                         [StubDom] IAttribute attribute,
                                                                                                                         [StubDom] INode parent,
                                                                                                                         object expressionResult)
        {
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(ExpressionContextProcessingResult.Noop));
            Mock.Get(specProvider)
                .SetupGet(x => x.Condition)
                .Returns(spec);
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(attribute.Value, context, CancellationToken.None))
                .Returns(() => Task.FromResult(expressionResult));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultAbortTheAction(expressionResult))
                .Returns(true);
            Mock.Get(resultInterpreter)
                .Setup(x => x.CoerceResultToBoolean(expressionResult))
                .Returns(false);
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            context.CurrentElement.Attributes.Clear();
            context.CurrentElement.Attributes.Add(attribute);
            context.CurrentElement.ParentElement = parent;
            parent.ChildNodes.Add(context.CurrentElement);

            await sut.ProcessContextAsync(context);

            Assert.That(parent.ChildNodes, Has.One.SameAs(context.CurrentElement));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_does_not_remove_element_if_attribute_does_not_match_spec([Frozen] IProcessesExpressionContext wrapped,
                                                                                                       [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                                       [Frozen] IEvaluatesExpression evaluator,
                                                                                                       [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                                       ConditionAttributeDecorator sut,
                                                                                                       AttributeSpec spec,
                                                                                                       [StubDom] ExpressionContext context,
                                                                                                       [StubDom] IAttribute attribute,
                                                                                                       [StubDom] INode parent,
                                                                                                       object expressionResult)
        {
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(ExpressionContextProcessingResult.Noop));
            Mock.Get(specProvider)
                .SetupGet(x => x.Condition)
                .Returns(spec);
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(attribute.Value, context, CancellationToken.None))
                .Returns(() => Task.FromResult(expressionResult));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultAbortTheAction(expressionResult))
                .Returns(false);
            Mock.Get(resultInterpreter)
                .Setup(x => x.CoerceResultToBoolean(expressionResult))
                .Returns(false);
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(false);
            context.CurrentElement.Attributes.Clear();
            context.CurrentElement.Attributes.Add(attribute);
            context.CurrentElement.ParentElement = parent;
            parent.ChildNodes.Add(context.CurrentElement);

            await sut.ProcessContextAsync(context);

            Assert.That(parent.ChildNodes, Has.One.SameAs(context.CurrentElement));
        }
    }
}
