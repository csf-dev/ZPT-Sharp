using System;
using NUnit.Framework;
using ZptSharp.Expressions;

namespace ZptSharp.Tal
{
    [TestFixture, Parallelizable]
    public class TalExpressionResultInterpreterTests
    {
        [Test, AutoMoqData]
        public void DoesResultCancelTheAction_returns_true_when_result_is_action_cancelling_token(TalExpressionResultInterpreter sut)
        {
            Assert.That(() => sut.DoesResultCancelTheAction(AbortZptActionToken.Instance), Is.True);
        }

        [Test, AutoMoqData]
        public void DoesResultCancelTheAction_returns_true_when_result_is_new_action_cancelling_token(TalExpressionResultInterpreter sut)
        {
            Assert.That(() => sut.DoesResultCancelTheAction(new AbortZptActionToken()), Is.True);
        }

        [Test, AutoMoqData]
        public void DoesResultCancelTheAction_returns_false_when_result_is_another_object(TalExpressionResultInterpreter sut, object result)
        {
            Assert.That(() => sut.DoesResultCancelTheAction(result), Is.False);
        }

        [Test, AutoMoqData]
        public void DoesResultCancelTheAction_returns_false_when_result_is_null(TalExpressionResultInterpreter sut)
        {
            Assert.That(() => sut.DoesResultCancelTheAction(null), Is.False);
        }

        [Test, AutoMoqData]
        public void CoerceResultToBoolean_returns_false_for_null(TalExpressionResultInterpreter sut)
        {
            Assert.That(() => sut.CoerceResultToBoolean(null), Is.False);
        }

        [Test, AutoMoqData]
        public void CoerceResultToBoolean_returns_false_for_zero(TalExpressionResultInterpreter sut)
        {
            Assert.That(() => sut.CoerceResultToBoolean(0), Is.False);
        }

        [Test, AutoMoqData]
        public void CoerceResultToBoolean_returns_false_for_false(TalExpressionResultInterpreter sut)
        {
            Assert.That(() => sut.CoerceResultToBoolean(false), Is.False);
        }

        [Test, AutoMoqData]
        public void CoerceResultToBoolean_returns_true_for_empty_string(TalExpressionResultInterpreter sut)
        {
            Assert.That(() => sut.CoerceResultToBoolean(String.Empty), Is.True);
        }

        [Test, AutoMoqData]
        public void CoerceResultToBoolean_returns_true_for_true(TalExpressionResultInterpreter sut)
        {
            Assert.That(() => sut.CoerceResultToBoolean(true), Is.True);
        }

        [Test, AutoMoqData]
        public void CoerceResultToBoolean_returns_true_for_an_object(TalExpressionResultInterpreter sut, object obj)
        {
            Assert.That(() => sut.CoerceResultToBoolean(obj), Is.True);
        }
    }
}
