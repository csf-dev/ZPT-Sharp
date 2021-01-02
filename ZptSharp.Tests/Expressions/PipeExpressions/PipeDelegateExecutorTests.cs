using System;
using NUnit.Framework;

namespace ZptSharp.Expressions.PipeExpressions
{
    [TestFixture, Parallelizable]
    public class PipeDelegateExecutorTests
    {
        delegate int CustomDelegate(int param);

        [Test,AutoMoqData]
        public void EvaluateDelegate_returns_correct_result_for_a_Func_object_object(PipeDelegateExecutor sut)
        {
            Func<object,object> dele = x => (int) x + 1;
            var source = 5;
            Assert.That(() => sut.EvaluateDelegate(source, dele), Is.EqualTo(6));
        }

        [Test,AutoMoqData]
        public void EvaluateDelegate_returns_correct_result_for_a_Func_int_int(PipeDelegateExecutor sut)
        {
            Func<int,int> dele = x => x + 1;
            var source = 5;
            Assert.That(() => sut.EvaluateDelegate(source, dele), Is.EqualTo(6));
        }

        [Test,AutoMoqData]
        public void EvaluateDelegate_returns_correct_result_for_a_Func_int_object(PipeDelegateExecutor sut)
        {
            Func<int,object> dele = x => x + 1;
            var source = 5;
            Assert.That(() => sut.EvaluateDelegate(source, dele), Is.EqualTo(6));
        }

        [Test,AutoMoqData]
        public void EvaluateDelegate_returns_correct_result_for_a_Func_object_int(PipeDelegateExecutor sut)
        {
            Func<object,int> dele = x => (int) x + 1;
            var source = 5;
            Assert.That(() => sut.EvaluateDelegate(source, dele), Is.EqualTo(6));
        }

        [Test,AutoMoqData]
        public void EvaluateDelegate_returns_correct_result_for_a_custom_delegate(PipeDelegateExecutor sut)
        {
            CustomDelegate dele = x => x + 1;
            var source = 5;
            Assert.That(() => sut.EvaluateDelegate(source, dele), Is.EqualTo(6));
        }

        [Test,AutoMoqData]
        public void EvaluateDelegate_returns_correct_result_for_a_method_group(PipeDelegateExecutor sut)
        {
            object GetResult(int value) => value + 1;
            Func<int, object> dele = GetResult;
            var source = 5;
            Assert.That(() => sut.EvaluateDelegate(source, dele), Is.EqualTo(6));
        }

        [Test,AutoMoqData]
        public void EvaluateDelegate_throws_if_delegate_object_is_not_a_delegate(PipeDelegateExecutor sut, object source)
        {
            Assert.That(() => sut.EvaluateDelegate(source, new object()), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public void EvaluateDelegate_throws_if_delegate_object_has_too_many_params(PipeDelegateExecutor sut, object source)
        {
            Func<object,object,object> dele = (o1, o2) => o1;
            Assert.That(() => sut.EvaluateDelegate(source, dele), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public void EvaluateDelegate_throws_if_delegate_object_has_too_few_params(PipeDelegateExecutor sut, object source)
        {
            Func<object> dele = () => "nope";
            Assert.That(() => sut.EvaluateDelegate(source, dele), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public void EvaluateDelegate_throws_if_delegate_object_has_void_return(PipeDelegateExecutor sut, object source)
        {
            Action<object> dele = o => {};
            Assert.That(() => sut.EvaluateDelegate(source, dele), Throws.ArgumentException);
        }
    }
}