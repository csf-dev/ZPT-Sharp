using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Expressions
{
    [TestFixture,Parallelizable]
    public class EvaluatorTypeResolverTests
    {
        [Test, AutoMoqData]
        public void GetEvaluator_returns_instance_resolved_using_resolver_and_registry([Frozen] IRegistersExpressionEvaluator registry,
                                                                                       [Frozen] IServiceProvider resolver,
                                                                                       EvaluatorTypeResolver sut,
                                                                                       string expressionType)
        {
            Mock.Get(registry).Setup(x => x.GetEvaluatorType(expressionType)).Returns(typeof(StubEvaluator));
            Mock.Get(resolver).Setup(x => x.GetService(typeof(StubEvaluator))).Returns(() => new StubEvaluator());

            Assert.That(() => sut.GetEvaluator(expressionType), Is.InstanceOf<StubEvaluator>());
        }

        #region Stub evaluator type

        public class StubEvaluator : IEvaluatesExpression
        {
            public Task<object> EvaluateExpressionAsync(string expression, ExpressionContext context, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
