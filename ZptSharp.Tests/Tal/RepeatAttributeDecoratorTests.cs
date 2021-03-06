﻿using System;
using System.Collections.Generic;
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
    public class RepeatAttributeDecoratorTests
    {
        [Test, AutoMoqData]
        public void ProcessContextAsync_returns_wrapped_result_if_attribute_not_present([Frozen] IHandlesProcessingError wrapped,
                                                                                        [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                        RepeatAttributeDecorator sut,
                                                                                        AttributeSpec spec,
                                                                                        [StubDom] ExpressionContext context,
                                                                                        ExpressionContextProcessingResult wrappedResult)
        {
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(wrappedResult));
            Mock.Get(specProvider).SetupGet(x => x.Repeat).Returns(spec);
            context.CurrentNode.Attributes.Clear();

            Assert.That(() => sut.ProcessContextAsync(context).Result, Is.SameAs(wrappedResult));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_returns_wrapped_result_if_attribute_value_aborts_the_action([Frozen] IHandlesProcessingError wrapped,
                                                                                                          [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                                          [Frozen] IEvaluatesExpression evaluator,
                                                                                                          [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                                          RepeatAttributeDecorator sut,
                                                                                                          AttributeSpec spec,
                                                                                                          [StubDom] IAttribute attribute,
                                                                                                          [StubDom] ExpressionContext context,
                                                                                                          ExpressionContextProcessingResult wrappedResult,
                                                                                                          object expressionResult)
        {
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(wrappedResult));
            Mock.Get(specProvider).SetupGet(x => x.Repeat).Returns(spec);
            context.CurrentNode.Attributes.Clear();
            context.CurrentNode.Attributes.Add(attribute);
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            Mock.Get(attribute).SetupGet(x => x.Value).Returns("varName expression");
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync("expression", context, CancellationToken.None))
                .Returns(() => Task.FromResult(expressionResult));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultAbortTheAction(expressionResult))
                .Returns(true);

            var result = await sut.ProcessContextAsync(context);

            Assert.That(result, Is.SameAs(wrappedResult));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_returns_contexts_from_service_when_repetitions_exist([Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                                   [Frozen] IEvaluatesExpression evaluator,
                                                                                                   [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                                   [Frozen] IGetsRepetitionContexts contextProvider,
                                                                                                   RepeatAttributeDecorator sut,
                                                                                                   AttributeSpec spec,
                                                                                                   [StubDom] IAttribute attribute,
                                                                                                   [StubDom] ExpressionContext context,
                                                                                                   object expressionResult,
                                                                                                   IList<ExpressionContext> contexts,
                                                                                                   [StubDom] INode parent)
        {
            Mock.Get(specProvider).SetupGet(x => x.Repeat).Returns(spec);
            context.CurrentNode.Attributes.Clear();
            context.CurrentNode.Attributes.Add(attribute);
            context.CurrentNode.ParentNode = parent;
            parent.ChildNodes.Add(context.CurrentNode);
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            Mock.Get(attribute).SetupGet(x => x.Value).Returns("varName expression");
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync("expression", context, CancellationToken.None))
                .Returns(() => Task.FromResult(expressionResult));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultAbortTheAction(expressionResult))
                .Returns(false);
            Mock.Get(contextProvider)
                .Setup(x => x.GetRepetitionContexts(expressionResult, context, "varName"))
                .Returns(contexts);

            var result = await sut.ProcessContextAsync(context);

            Assert.That(result?.AdditionalContexts, Is.EqualTo(contexts));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_prevents_further_processing_when_repetitions_exist([Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                                 [Frozen] IEvaluatesExpression evaluator,
                                                                                                 [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                                 [Frozen] IGetsRepetitionContexts contextProvider,
                                                                                                 RepeatAttributeDecorator sut,
                                                                                                 AttributeSpec spec,
                                                                                                 [StubDom] IAttribute attribute,
                                                                                                 [StubDom] ExpressionContext context,
                                                                                                 object expressionResult,
                                                                                                 IList<ExpressionContext> contexts,
                                                                                                 [StubDom] INode parent)
        {
            Mock.Get(specProvider).SetupGet(x => x.Repeat).Returns(spec);
            context.CurrentNode.Attributes.Clear();
            context.CurrentNode.Attributes.Add(attribute);
            context.CurrentNode.ParentNode = parent;
            parent.ChildNodes.Add(context.CurrentNode);
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            Mock.Get(attribute).SetupGet(x => x.Value).Returns("varName expression");
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync("expression", context, CancellationToken.None))
                .Returns(() => Task.FromResult(expressionResult));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultAbortTheAction(expressionResult))
                .Returns(false);
            Mock.Get(contextProvider)
                .Setup(x => x.GetRepetitionContexts(expressionResult, context, "varName"))
                .Returns(contexts);

            var result = await sut.ProcessContextAsync(context);

            Assert.That(result?.DoNotProcessChildren, Is.True);
        }
    }
}
