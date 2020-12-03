using System;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Tal;

namespace ZptSharp.Expressions.NotExpressions
{
    [TestFixture,Parallelizable]
    public class BooleanValueConverterTests
    {
        [Test, AutoMoqData]
        public void CoerceToBoolean_returns_false_if_value_cancels_action([Frozen] IInterpretsExpressionResult resultInterpreter,
                                                                          BooleanValueConverter sut,
                                                                          object value)
        {
            Mock.Get(resultInterpreter)
                .Setup(x => x.DoesResultAbortTheAction(value))
                .Returns(true);
            Assert.That(() => sut.CoerceToBoolean(value), Is.False);
        }

        [Test, AutoMoqData]
        public void CoerceToBoolean_returns_false_if_value_is_null(BooleanValueConverter sut)
        {
            Assert.That(() => sut.CoerceToBoolean(null), Is.False);
        }

        [Test, AutoMoqData]
        public void CoerceToBoolean_returns_false_if_value_is_false(BooleanValueConverter sut)
        {
            Assert.That(() => sut.CoerceToBoolean(false), Is.False);
        }

        [Test, AutoMoqData]
        public void CoerceToBoolean_returns_true_if_value_is_true(BooleanValueConverter sut)
        {
            Assert.That(() => sut.CoerceToBoolean(true), Is.True);
        }

        [Test, AutoMoqData]
        public void CoerceToBoolean_returns_false_if_value_is_zero(BooleanValueConverter sut)
        {
            Assert.That(() => sut.CoerceToBoolean(0), Is.False);
        }

        [Test, AutoMoqData]
        public void CoerceToBoolean_returns_false_if_value_is_empty_string(BooleanValueConverter sut)
        {
            Assert.That(() => sut.CoerceToBoolean(String.Empty), Is.False);
        }

        [Test, AutoMoqData]
        public void CoerceToBoolean_returns_true_if_value_is_non_empty_string(BooleanValueConverter sut)
        {
            Assert.That(() => sut.CoerceToBoolean("foo bar"), Is.True);
        }

        [Test, AutoMoqData]
        public void CoerceToBoolean_returns_false_if_value_is_empty_array(BooleanValueConverter sut)
        {
            Assert.That(() => sut.CoerceToBoolean(new int[0]), Is.False);
        }

        [Test, AutoMoqData]
        public void CoerceToBoolean_returns_false_if_value_is_a_non_null_object(BooleanValueConverter sut, object obj)
        {
            Assert.That(() => sut.CoerceToBoolean(obj), Is.True);
        }
    }
}
