using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Expressions
{
    [TestFixture,Parallelizable]
    public class RegistryBasedExpressionEvaluatorTests
    {
        [Test, AutoMoqData]
        public void EvaluateExpressionAsync_returns_result_from_resolved_evaluator([Frozen] IGetsExpressionType typeProvider,
                                                                                   [Frozen] IGetsEvaluatorForExpressionType evaluatorProvider,
                                                                                   [Frozen] IRemovesPrefixFromExpression prefixRemover,
                                                                                   RegistryBasedExpressionEvaluator sut,
                                                                                   string expression,
                                                                                   string type,
                                                                                   string expressionNoPrefix,
                                                                                   IEvaluatesExpression evaluator,
                                                                                   object expected,
                                                                                   ExpressionContext context)
        {
            Mock.Get(typeProvider)
                .Setup(x => x.GetExpressionType(expression))
                .Returns(type);
            Mock.Get(evaluatorProvider)
                .Setup(x => x.GetEvaluator(type))
                .Returns(evaluator);
            Mock.Get(prefixRemover)
                .Setup(x => x.GetExpressionWithoutPrefix(expression))
                .Returns(expressionNoPrefix);
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(expressionNoPrefix, context, CancellationToken.None))
                .Returns(() => Task.FromResult(expected));

            Assert.That(() => sut.EvaluateExpressionAsync(expression, context, CancellationToken.None).Result, Is.SameAs(expected));
        }
    }
}
