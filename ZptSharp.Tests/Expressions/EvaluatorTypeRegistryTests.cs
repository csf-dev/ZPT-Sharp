using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace ZptSharp.Expressions
{
    [TestFixture,Parallelizable]
    public class EvaluatorTypeRegistryTests
    {
        #region GetEvaluator

        [Test, AutoMoqData]
        public void GetEvaluatorType_throws_ANE_if_expression_type_is_null(EvaluatorTypeRegistry sut,
                                                                      string expressionType)
        {
            Assert.That(() => sut.GetEvaluatorType(null), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void GetEvaluatorType_throws_if_expression_type_is_not_registered(EvaluatorTypeRegistry sut,
                                                                             string expressionType)
        {
            Assert.That(() => sut.GetEvaluatorType(expressionType), Throws.InstanceOf<NoEvaluatorForExpressionTypeException>());
        }

        [Test, AutoMoqData]
        public void GetEvaluatorType_gets_type_of_evaluator_using_resolver_if_it_is_registered([Frozen] Hosting.EnvironmentRegistry registry,
                                                                                               EvaluatorTypeRegistry sut,
                                                                                               string expressionType,
                                                                                               StubEvaluator evaluator)
        {
            registry.ExpresionEvaluatorTypes[expressionType] = evaluator.GetType();
            Assert.That(() => sut.GetEvaluatorType(expressionType), Is.EqualTo(typeof(StubEvaluator)));
        }

        #endregion

        #region IsRegistered

        [Test, AutoMoqData]
        public void IsRegistered_throws_ANE_if_expression_type_is_null(EvaluatorTypeRegistry sut)
        {
            Assert.That(() => sut.IsRegistered(null), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void IsRegistered_returns_false_if_not_registered(EvaluatorTypeRegistry sut, string expressionType)
        {
            Assert.That(() => sut.IsRegistered(expressionType), Is.False);
        }

        [Test, AutoMoqData]
        public void IsRegistered_returns_true_if_registered([Frozen] Hosting.EnvironmentRegistry registry,
                                                            EvaluatorTypeRegistry sut,
                                                            string expressionType)
        {
            registry.ExpresionEvaluatorTypes[expressionType] = typeof(StubEvaluator);
            Assert.That(() => sut.IsRegistered(expressionType), Is.True);
        }

        #endregion

        #region GetRegisteredExpressionTypes

        [Test, AutoMoqData]
        public void GetRegisteredExpressionTypes_gets_correct_list_of_expression_types([Frozen] Hosting.EnvironmentRegistry registry,
                                                                                       EvaluatorTypeRegistry sut,
                                                                                       string expressionType)
        {
            registry.ExpresionEvaluatorTypes[expressionType] = typeof(StubEvaluator);
            Assert.That(() => sut.GetRegisteredExpressionTypes(), Is.EqualTo(new[] { expressionType }));
        }

        #endregion

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
