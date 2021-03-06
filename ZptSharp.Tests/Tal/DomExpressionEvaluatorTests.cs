﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Tal
{
    [TestFixture,Parallelizable]
    public class DomExpressionEvaluatorTests
    {
        [Test, AutoMoqData]
        public async Task EvaluateExpressionAsync_returns_a_text_node_from_a_naked_expression([Frozen] IEvaluatesExpression evaluator,
                                                                                              [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                              DomExpressionEvaluator sut,
                                                                                              string expression,
                                                                                              string expressionResult,
                                                                                              [StubDom] ExpressionContext context,
                                                                                              [StubDom] INode textNode)
        {
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult<object>(expressionResult));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultAbortTheAction(expressionResult))
                .Returns(false);
            Mock.Get(context.CurrentNode).Setup(x => x.CreateTextNode(expressionResult)).Returns(textNode);

            var result = await sut.EvaluateExpressionAsync(expression, context);

            Assert.That(result,
                        Has.Property(nameof(DomValueExpressionResult.Nodes)).EqualTo(new[] { textNode })
                           .And.Property(nameof(DomValueExpressionResult.AbortAction)).False);
        }

        [Test, AutoMoqData]
        public async Task EvaluateExpressionAsync_returns_a_text_node_from_a_text_expression([Frozen] IEvaluatesExpression evaluator,
                                                                                             [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                             DomExpressionEvaluator sut,
                                                                                             string expression,
                                                                                             string expressionResult,
                                                                                             [StubDom] ExpressionContext context,
                                                                                             [StubDom] INode textNode)
        {
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult<object>(expressionResult));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultAbortTheAction(expressionResult))
                .Returns(false);
            Mock.Get(context.CurrentNode).Setup(x => x.CreateTextNode(expressionResult)).Returns(textNode);

            var result = await sut.EvaluateExpressionAsync($"text {expression}", context);

            Assert.That(result,
                        Has.Property(nameof(DomValueExpressionResult.Nodes)).EqualTo(new[] { textNode })
                           .And.Property(nameof(DomValueExpressionResult.AbortAction)).False);
        }

        [Test, AutoMoqData]
        public async Task EvaluateExpressionAsync_returns_parsed_nodes_from_a_structure_expression([Frozen] IEvaluatesExpression evaluator,
                                                                                                   [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                                   DomExpressionEvaluator sut,
                                                                                                   string expression,
                                                                                                   string expressionResult,
                                                                                                   [StubDom] ExpressionContext context,
                                                                                                   [StubDom] INode textNode)
        {
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult<object>(expressionResult));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultAbortTheAction(expressionResult))
                .Returns(false);
            Mock.Get(context.CurrentNode).Setup(x => x.ParseAsNodes(expressionResult)).Returns(() => new[] { textNode });

            var result = await sut.EvaluateExpressionAsync($"structure {expression}", context);

            Assert.That(result,
                        Has.Property(nameof(DomValueExpressionResult.Nodes)).EqualTo(new[] { textNode })
                           .And.Property(nameof(DomValueExpressionResult.AbortAction)).False);
        }

        [Test, AutoMoqData]
        public async Task EvaluateExpressionAsync_returns_parsed_nodes_if_expression_result_is_IGetsStructuredMarkup([Frozen] IEvaluatesExpression evaluator,
                                                                                                                     [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                                                     DomExpressionEvaluator sut,
                                                                                                                     string expression,
                                                                                                                     IGetsStructuredMarkup expressionResult,
                                                                                                                     string markup,
                                                                                                                     [StubDom] ExpressionContext context,
                                                                                                                     [StubDom] INode textNode)
        {
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult<object>(expressionResult));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultAbortTheAction(expressionResult))
                .Returns(false);
            Mock.Get(expressionResult)
                .Setup(x => x.GetMarkupAsync())
                .Returns(() => Task.FromResult(markup));
            Mock.Get(context.CurrentNode).Setup(x => x.ParseAsNodes(markup)).Returns(() => new[] { textNode });

            var result = await sut.EvaluateExpressionAsync(expression, context);

            Assert.That(result,
                        Has.Property(nameof(DomValueExpressionResult.Nodes)).EqualTo(new[] { textNode })
                           .And.Property(nameof(DomValueExpressionResult.AbortAction)).False);
        }

        [Test, AutoMoqData]
        public async Task EvaluateExpressionAsync_returns_text_content_if_expression_result_is_IGetsStructuredMarkup_but_expression_is_explicitly_text([Frozen] IEvaluatesExpression evaluator,
                                                                                                                                                       [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                                                                                       DomExpressionEvaluator sut,
                                                                                                                                                       string expression,
                                                                                                                                                       IGetsStructuredMarkup expressionResult,
                                                                                                                                                       string stringRepresentation,
                                                                                                                                                       [StubDom] ExpressionContext context,
                                                                                                                                                       [StubDom] INode textNode)
        {
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult<object>(expressionResult));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultAbortTheAction(expressionResult))
                .Returns(false);
            Mock.Get(expressionResult)
                .Setup(x => x.ToString())
                .Returns(() => stringRepresentation);
            Mock.Get(context.CurrentNode).Setup(x => x.CreateTextNode(stringRepresentation)).Returns(() => textNode);

            var result = await sut.EvaluateExpressionAsync($"text {expression}", context);

            Assert.That(result,
                        Has.Property(nameof(DomValueExpressionResult.Nodes)).EqualTo(new[] { textNode })
                           .And.Property(nameof(DomValueExpressionResult.AbortAction)).False);
        }

        [Test, AutoMoqData]
        public async Task EvaluateExpressionAsync_returns_empty_node_list_if_expression_cancels_action([Frozen] IEvaluatesExpression evaluator,
                                                                                                       [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                                       DomExpressionEvaluator sut,
                                                                                                       string expression,
                                                                                                       string expressionResult,
                                                                                                       [StubDom] ExpressionContext context)
        {
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult<object>(expressionResult));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultAbortTheAction(expressionResult))
                .Returns(true);

            var result = await sut.EvaluateExpressionAsync(expression, context);

            Assert.That(result,
                        Has.Property(nameof(DomValueExpressionResult.AbortAction)).True
                           .And.Property(nameof(DomValueExpressionResult.Nodes)).Empty);
        }
    }
}
