using System;
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
    public class DefineAttributeDecoratorTests
    {
        [Test, AutoMoqData]
        public async Task ProcessContextAsync_evaluates_and_adds_a_local_definition_to_the_context([Frozen] IProcessesExpressionContext wrapped,
                                                                                                   [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                                   [Frozen] IEvaluatesExpression evaluator,
                                                                                                   [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                                   [Frozen] IGetsVariableDefinitionsFromAttributeValue definitionProvider,
                                                                                                   DefineAttributeDecorator sut,
                                                                                                   AttributeSpec spec,
                                                                                                   [StubDom] ExpressionContext context,
                                                                                                   [StubDom] IAttribute attribute,
                                                                                                   object expressionResult,
                                                                                                   VariableDefinition definition)
        {
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(ExpressionContextProcessingResult.Noop));
            Mock.Get(specProvider)
                .SetupGet(x => x.Define)
                .Returns(spec);
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(definition.Expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult(expressionResult));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultCancelTheAction(expressionResult))
                .Returns(false);
            Mock.Get(definitionProvider)
                .Setup(x => x.GetDefinitions(attribute.Value))
                .Returns(() => new[] { definition });
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            context.CurrentElement.Attributes.Clear();
            context.CurrentElement.Attributes.Add(attribute);
            definition.Scope = VariableDefinition.LocalScope;

            await sut.ProcessContextAsync(context);

            Assert.That(context.LocalDefinitions[definition.VariableName], Is.SameAs(expressionResult));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_evaluates_and_adds_a_global_definition_to_the_context([Frozen] IProcessesExpressionContext wrapped,
                                                                                                    [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                                    [Frozen] IEvaluatesExpression evaluator,
                                                                                                    [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                                    [Frozen] IGetsVariableDefinitionsFromAttributeValue definitionProvider,
                                                                                                    DefineAttributeDecorator sut,
                                                                                                    AttributeSpec spec,
                                                                                                    [StubDom] ExpressionContext context,
                                                                                                    [StubDom] IAttribute attribute,
                                                                                                    object expressionResult,
                                                                                                    VariableDefinition definition)
        {
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(ExpressionContextProcessingResult.Noop));
            Mock.Get(specProvider)
                .SetupGet(x => x.Define)
                .Returns(spec);
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(definition.Expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult(expressionResult));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultCancelTheAction(expressionResult))
                .Returns(false);
            Mock.Get(definitionProvider)
                .Setup(x => x.GetDefinitions(attribute.Value))
                .Returns(() => new[] { definition });
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            context.CurrentElement.Attributes.Clear();
            context.CurrentElement.Attributes.Add(attribute);
            definition.Scope = VariableDefinition.GlobalScope;

            await sut.ProcessContextAsync(context);

            Assert.That(context.GlobalDefinitions[definition.VariableName], Is.SameAs(expressionResult));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_can_evaluate_and_add_two_local_definitions_to_the_context([Frozen] IProcessesExpressionContext wrapped,
                                                                                                        [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                                        [Frozen] IEvaluatesExpression evaluator,
                                                                                                        [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                                        [Frozen] IGetsVariableDefinitionsFromAttributeValue definitionProvider,
                                                                                                        DefineAttributeDecorator sut,
                                                                                                        AttributeSpec spec,
                                                                                                        [StubDom] ExpressionContext context,
                                                                                                        [StubDom] IAttribute attribute,
                                                                                                        object expressionResult1,
                                                                                                        object expressionResult2,
                                                                                                        VariableDefinition definition1,
                                                                                                        VariableDefinition definition2)
        {
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(ExpressionContextProcessingResult.Noop));
            Mock.Get(specProvider)
                .SetupGet(x => x.Define)
                .Returns(spec);
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(definition1.Expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult(expressionResult1));
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(definition2.Expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult(expressionResult2));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultCancelTheAction(expressionResult1))
                .Returns(false);
            Mock.Get(definitionProvider)
                .Setup(x => x.GetDefinitions(attribute.Value))
                .Returns(() => new[] { definition1, definition2 });
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            context.CurrentElement.Attributes.Clear();
            context.CurrentElement.Attributes.Add(attribute);
            definition1.Scope = VariableDefinition.LocalScope;
            definition2.Scope = VariableDefinition.LocalScope;

            await sut.ProcessContextAsync(context);

            Assert.That(context.LocalDefinitions,
                        Has.One.Matches<KeyValuePair<string,object>>(v => v.Key == definition1.VariableName && v.Value == expressionResult1)
                           .And.One.Matches<KeyValuePair<string, object>>(v => v.Key == definition2.VariableName && v.Value == expressionResult2));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_doesnt_add_to_context_if_attribute_does_not_match([Frozen] IProcessesExpressionContext wrapped,
                                                                                                [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                                [Frozen] IEvaluatesExpression evaluator,
                                                                                                [Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                                                [Frozen] IGetsVariableDefinitionsFromAttributeValue definitionProvider,
                                                                                                DefineAttributeDecorator sut,
                                                                                                AttributeSpec spec,
                                                                                                [StubDom] ExpressionContext context,
                                                                                                [StubDom] IAttribute attribute,
                                                                                                object expressionResult,
                                                                                                VariableDefinition definition)
        {
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(ExpressionContextProcessingResult.Noop));
            Mock.Get(specProvider)
                .SetupGet(x => x.Define)
                .Returns(spec);
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(definition.Expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult(expressionResult));
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultCancelTheAction(expressionResult))
                .Returns(false);
            Mock.Get(definitionProvider)
                .Setup(x => x.GetDefinitions(attribute.Value))
                .Returns(() => new[] { definition });
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(false);
            context.CurrentElement.Attributes.Clear();
            context.CurrentElement.Attributes.Add(attribute);
            definition.Scope = VariableDefinition.LocalScope;

            await sut.ProcessContextAsync(context);

            Assert.That(context.LocalDefinitions.ContainsKey(definition.VariableName), Is.False);
        }

    }
}
