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
        #region RegisterEvaluatorType

        [Test, AutoMoqData]
        public void RegisterEvaluatorType_throws_ANE_if_evaluator_type_is_null(EvaluatorTypeRegistry sut,
                                                                               string expressionType)
        {
            Assert.That(() => sut.RegisterEvaluatorType(null, expressionType), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void RegisterEvaluatorType_throws_if_expression_type_is_null(EvaluatorTypeRegistry sut)
        {
            Assert.That(() => sut.RegisterEvaluatorType(typeof(StubEvaluator), null), Throws.ArgumentException);
        }

        [Test, AutoMoqData]
        public void RegisterEvaluatorType_throws_if_expression_type_is_empty_string(EvaluatorTypeRegistry sut)
        {
            Assert.That(() => sut.RegisterEvaluatorType(typeof(StubEvaluator), String.Empty), Throws.ArgumentException);
        }

        [Test, AutoMoqData]
        public void RegisterEvaluatorType_throws_if_evaluator_type_is_wrong_impl_type(EvaluatorTypeRegistry sut,
                                                                                      string expressionType)
        {
            Assert.That(() => sut.RegisterEvaluatorType(typeof(string), expressionType), Throws.ArgumentException);
        }

        [Test, AutoMoqData]
        public void RegisterEvaluatorType_does_not_throw_if_an_expression_type_is_registered_once(EvaluatorTypeRegistry sut,
                                                                                                  string expressionType)
        {
            Assert.That(() => sut.RegisterEvaluatorType(typeof(StubEvaluator), expressionType), Throws.Nothing);
        }

        [Test, AutoMoqData]
        public void RegisterEvaluatorType_throws_if_expression_type_is_already_registered(EvaluatorTypeRegistry sut,
                                                                                          string expressionType)
        {
            sut.RegisterEvaluatorType(typeof(StubEvaluator), expressionType);
            Assert.That(() => sut.RegisterEvaluatorType(typeof(StubEvaluator), expressionType), Throws.ArgumentException);
        }

        #endregion

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
        public void GetEvaluatorType_gets_type_of_evaluator_using_resolver_if_it_is_registered(EvaluatorTypeRegistry sut,
                                                                                               string expressionType,
                                                                                               StubEvaluator evaluator)
        {
            sut.RegisterEvaluatorType<StubEvaluator>(expressionType);
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
        public void IsRegistered_returns_true_if_registered(EvaluatorTypeRegistry sut, string expressionType)
        {
            sut.RegisterEvaluatorType<StubEvaluator>(expressionType);
            Assert.That(() => sut.IsRegistered(expressionType), Is.True);
        }

        #endregion

        #region Unregister

        [Test, AutoMoqData]
        public void Unregister_throws_ANE_if_expression_type_is_null(EvaluatorTypeRegistry sut)
        {
            Assert.That(() => sut.Unregister(null), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void Unregister_returns_false_if_not_registered(EvaluatorTypeRegistry sut, string expressionType)
        {
            Assert.That(() => sut.Unregister(expressionType), Is.False);
        }

        [Test, AutoMoqData]
        public void Unregister_returns_true_if_registered(EvaluatorTypeRegistry sut, string expressionType)
        {
            sut.RegisterEvaluatorType<StubEvaluator>(expressionType);
            Assert.That(() => sut.Unregister(expressionType), Is.True);
        }

        [Test, AutoMoqData]
        public void IsRegistered_returns_false_after_expression_type_is_unregistered(EvaluatorTypeRegistry sut, string expressionType)
        {
            sut.RegisterEvaluatorType<StubEvaluator>(expressionType);
            sut.Unregister(expressionType);
            Assert.That(() => sut.IsRegistered(expressionType), Is.False);
        }

        #endregion

        #region GetRegisteredExpressionTypes

        [Test, AutoMoqData]
        public void GetRegisteredExpressionTypes_gets_correct_list_of_expression_types(EvaluatorTypeRegistry sut, string expressionType)
        {
            sut.RegisterEvaluatorType<StubEvaluator>(expressionType);
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
