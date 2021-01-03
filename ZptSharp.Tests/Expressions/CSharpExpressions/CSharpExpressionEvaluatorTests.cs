using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Expressions.CSharpExpressions
{
    [TestFixture, Parallelizable]
    public class CSharpExpressionEvaluatorTests
    {
        [Test,AutoMoqData]
        public async Task EvaluateExpressionAsync_gets_and_evaluates_expression_from_cache_if_it_exists([Frozen] IGetsAllVariablesFromContext allValuesProvider,
                                                                                                        [Frozen] ICachesCSharpExpressions expressionCache,
                                                                                                        [Frozen] IGetsExpressionDescription identityFactory,
                                                                                                        CSharpExpressionEvaluator sut,
                                                                                                        ExpressionContext context,
                                                                                                        IDictionary<string, object> allVariables,
                                                                                                        ExpressionDescription identifier,
                                                                                                        string expressionBody,
                                                                                                        object expectedResult)
        {
            var expression = GetExpression(v => expectedResult);

            Mock.Get(allValuesProvider)
                .Setup(x => x.GetAllVariablesAsync(context))
                .Returns(() => Task.FromResult(allVariables));
            Mock.Get(identityFactory)
                .Setup(x => x.GetDescription(expressionBody, allVariables, It.IsAny<IEnumerable<AssemblyReference>>(), It.IsAny<IEnumerable<UsingNamespace>>()))
                .Returns(identifier);
            Mock.Get(expressionCache)
                .Setup(x => x.GetExpression(identifier))
                .Returns(expression);
            
            var result = await sut.EvaluateExpressionAsync(expressionBody, context);

            Assert.That(result, Is.SameAs(expectedResult));
        }

        [Test,AutoMoqData]
        public async Task EvaluateExpressionAsync_uses_compiled_expression_if_it_is_not_cached([Frozen] IGetsAllVariablesFromContext allValuesProvider,
                                                                                               [Frozen] ICachesCSharpExpressions expressionCache,
                                                                                               [Frozen] ICreatesCSharpExpressions expressionFactory,
                                                                                               [Frozen] IGetsExpressionDescription identityFactory,
                                                                                               CSharpExpressionEvaluator sut,
                                                                                               ExpressionContext context,
                                                                                               IDictionary<string, object> allVariables,
                                                                                               ExpressionDescription identifier,
                                                                                               string expressionBody,
                                                                                               object expectedResult)
        {
            var expression = GetExpression(v => expectedResult);

            Mock.Get(allValuesProvider)
                .Setup(x => x.GetAllVariablesAsync(context))
                .Returns(() => Task.FromResult(allVariables));
            Mock.Get(identityFactory)
                .Setup(x => x.GetDescription(expressionBody, allVariables, It.IsAny<IEnumerable<AssemblyReference>>(), It.IsAny<IEnumerable<UsingNamespace>>()))
                .Returns(identifier);
            Mock.Get(expressionCache)
                .Setup(x => x.GetExpression(identifier))
                .Returns(() => null);
            Mock.Get(expressionFactory)
                .Setup(x => x.GetExpressionAsync(identifier, It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(expression));
            
            var result = await sut.EvaluateExpressionAsync(expressionBody, context);

            Assert.That(result, Is.SameAs(expectedResult));
        }

        [Test,AutoMoqData]
        public async Task EvaluateExpressionAsync_adds_new_expression_to_cache_after_compilation([Frozen] IGetsAllVariablesFromContext allValuesProvider,
                                                                                                 [Frozen] ICachesCSharpExpressions expressionCache,
                                                                                                 [Frozen] ICreatesCSharpExpressions expressionFactory,
                                                                                                 [Frozen] IGetsExpressionDescription identityFactory,
                                                                                                 CSharpExpressionEvaluator sut,
                                                                                                 ExpressionContext context,
                                                                                                 IDictionary<string, object> allVariables,
                                                                                                 ExpressionDescription identifier,
                                                                                                 string expressionBody,
                                                                                                 object expectedResult)
        {
            var expression = GetExpression(v => expectedResult);

            Mock.Get(allValuesProvider)
                .Setup(x => x.GetAllVariablesAsync(context))
                .Returns(() => Task.FromResult(allVariables));
            Mock.Get(identityFactory)
                .Setup(x => x.GetDescription(expressionBody, allVariables, It.IsAny<IEnumerable<AssemblyReference>>(), It.IsAny<IEnumerable<UsingNamespace>>()))
                .Returns(identifier);
            Mock.Get(expressionCache)
                .Setup(x => x.GetExpression(identifier))
                .Returns(() => null);
            Mock.Get(expressionFactory)
                .Setup(x => x.GetExpressionAsync(identifier, It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(expression));
            
            await sut.EvaluateExpressionAsync(expressionBody, context);

            Mock.Get(expressionCache)
                .Verify(x => x.AddExpression(identifier, expression), Times.Once);
        }

        [Test,AutoMoqData]
        public void EvaluateExpressionAsync_throws_CSharpEvaluationException_if_compilation_throws([Frozen] IGetsAllVariablesFromContext allValuesProvider,
                                                                                                   [Frozen] ICachesCSharpExpressions expressionCache,
                                                                                                   [Frozen] ICreatesCSharpExpressions expressionFactory,
                                                                                                   [Frozen] IGetsExpressionDescription identityFactory,
                                                                                                   CSharpExpressionEvaluator sut,
                                                                                                   ExpressionContext context,
                                                                                                   IDictionary<string, object> allVariables,
                                                                                                   ExpressionDescription identifier,
                                                                                                   string expressionBody,
                                                                                                   object expectedResult)
        {
            var expression = GetExpression(v => expectedResult);

            Mock.Get(allValuesProvider)
                .Setup(x => x.GetAllVariablesAsync(context))
                .Returns(() => Task.FromResult(allVariables));
            Mock.Get(identityFactory)
                .Setup(x => x.GetDescription(expressionBody, allVariables, It.IsAny<IEnumerable<AssemblyReference>>(), It.IsAny<IEnumerable<UsingNamespace>>()))
                .Returns(identifier);
            Mock.Get(expressionCache)
                .Setup(x => x.GetExpression(identifier))
                .Returns(() => null);
            Mock.Get(expressionFactory)
                .Setup(x => x.GetExpressionAsync(identifier, It.IsAny<CancellationToken>()))
                .Throws<Exception>();
            
            Assert.That(async () => await sut.EvaluateExpressionAsync(expressionBody, context),
                        Throws.InstanceOf<CSharpEvaluationException>().And.Message.StartWith("An exception was raised whilst compiling a TALES 'csharp' expression"));
        }

        [Test,AutoMoqData]
        public void EvaluateExpressionAsync_throws_CSharpEvaluationException_if_execution_throws([Frozen] IGetsAllVariablesFromContext allValuesProvider,
                                                                                                 [Frozen] ICachesCSharpExpressions expressionCache,
                                                                                                 [Frozen] ICreatesCSharpExpressions expressionFactory,
                                                                                                 [Frozen] IGetsExpressionDescription identityFactory,
                                                                                                 CSharpExpressionEvaluator sut,
                                                                                                 ExpressionContext context,
                                                                                                 IDictionary<string, object> allVariables,
                                                                                                 ExpressionDescription identifier,
                                                                                                 string expressionBody)
        {
            var expression = GetExpression(v => throw new Exception());

            Mock.Get(allValuesProvider)
                .Setup(x => x.GetAllVariablesAsync(context))
                .Returns(() => Task.FromResult(allVariables));
            Mock.Get(identityFactory)
                .Setup(x => x.GetDescription(expressionBody, allVariables, It.IsAny<IEnumerable<AssemblyReference>>(), It.IsAny<IEnumerable<UsingNamespace>>()))
                .Returns(identifier);
            Mock.Get(expressionCache)
                .Setup(x => x.GetExpression(identifier))
                .Returns(expression);
            
            Assert.That(async () => await sut.EvaluateExpressionAsync(expressionBody, context),
                        Throws.InstanceOf<CSharpEvaluationException>().And.Message.StartWith("An exception was raised whilst evaluating a TALES 'csharp' expression"));
        }

        CSharpExpression GetExpression(CSharpExpression ex) => ex;
    }
}