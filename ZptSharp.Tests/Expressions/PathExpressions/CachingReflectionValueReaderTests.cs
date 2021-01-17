using System;
using System.Diagnostics;
using System.Reflection;
using NUnit.Framework;

namespace ZptSharp.Expressions.PathExpressions
{
    [TestFixture,NonParallelizable]
    public class CachingReflectionValueReaderTests
    {
        [Test, AutoMoqData]
        public void GetValue_returns_correct_value_from_property(CachingReflectionValueReader sut)
        {
            Assert.That(() => sut.GetValue(MyCustomType.MyPropertyInfo, new MyCustomType()),
                        Is.EqualTo("Hello"));
        }

        [Test, AutoMoqData]
        public void GetValue_returns_correct_value_from_field(CachingReflectionValueReader sut)
        {
            Assert.That(() => sut.GetValue(MyCustomType.MyFieldInfo, new MyCustomType()),
                        Is.EqualTo(true));
        }

        [Test, AutoMoqData]
        public void GetValue_returns_correct_value_from_method(CachingReflectionValueReader sut)
        {
            Assert.That(() => sut.GetValue(MyCustomType.MyMethodInfo, new MyCustomType()),
                        Is.EqualTo(42));
        }

        [Test, AutoMoqData]
        public void GetValue_is_at_least_ten_times_faster_over_1000_property_reads_using_caching_than_using_plain_reflection(CachingReflectionValueReader cachingImpl,
                                                                                                                                 PlainReflectionValueReader plainReflectionImpl)
        {
            var member = MyCustomType.MyPropertyInfo;
            var iterationCount = 100000;
            var timer = new Stopwatch();

            timer.Start();
            PerformIterations((sut, obj) => sut.GetValue(member, obj), plainReflectionImpl, iterationCount);
            timer.Stop();

            var plainReflectionTime = timer.ElapsedTicks;

            timer.Reset();
            timer.Start();
            PerformIterations((sut, obj) => sut.GetValue(member, obj), cachingImpl, iterationCount);
            timer.Stop();

            var cachingTime = timer.ElapsedTicks;

            TestContext.Out.WriteLine($@"Plain reflection time (ticks):{plainReflectionTime}
         Caching time (ticks):{cachingTime}");

            Assert.That(cachingTime, Is.LessThan(plainReflectionTime / 10));
        }

        [Test, AutoMoqData]
        public void GetValue_is_at_least_ten_times_faster_over_1000_field_reads_using_caching_than_using_plain_reflection(CachingReflectionValueReader cachingImpl,
                                                                                                                              PlainReflectionValueReader plainReflectionImpl)
        {
            var member = MyCustomType.MyFieldInfo;
            var iterationCount = 100000;
            var timer = new Stopwatch();

            timer.Start();
            PerformIterations((sut, obj) => sut.GetValue(member, obj), plainReflectionImpl, iterationCount);
            timer.Stop();

            var plainReflectionTime = timer.ElapsedTicks;

            timer.Reset();
            timer.Start();
            PerformIterations((sut, obj) => sut.GetValue(member, obj), cachingImpl, iterationCount);
            timer.Stop();

            var cachingTime = timer.ElapsedTicks;

            TestContext.Out.WriteLine($@"Plain reflection time (ticks):{plainReflectionTime}
         Caching time (ticks):{cachingTime}");

            Assert.That(cachingTime, Is.LessThan(plainReflectionTime / 10));
        }

        [Test, AutoMoqData]
        public void GetValue_is_at_least_ten_times_faster_over_1000_method_invocations_using_caching_than_using_plain_reflection(CachingReflectionValueReader cachingImpl,
                                                                                                                                     PlainReflectionValueReader plainReflectionImpl)
        {
            var member = MyCustomType.MyMethodInfo;
            var iterationCount = 100000;
            var timer = new Stopwatch();

            timer.Start();
            PerformIterations((sut, obj) => sut.GetValue(member, obj), plainReflectionImpl, iterationCount);
            timer.Stop();

            var plainReflectionTime = timer.ElapsedTicks;

            timer.Reset();
            timer.Start();
            PerformIterations((sut, obj) => sut.GetValue(member, obj), cachingImpl, iterationCount);
            timer.Stop();

            var cachingTime = timer.ElapsedTicks;

            TestContext.Out.WriteLine($@"Plain reflection time (ticks):{plainReflectionTime}
         Caching time (ticks):{cachingTime}");

            Assert.That(cachingTime, Is.LessThan(plainReflectionTime / 10));
        }

        void PerformIterations(Func<IGetsValueFromReflection, MyCustomType, object> action, IGetsValueFromReflection impl, int iterationCount)
        {
            var obj = new MyCustomType();

            for (var i = 0; i < iterationCount; i++)
                action(impl, obj);
        }

        #region contained types

        public class MyCustomType
        {
            public bool MyField = true;

            public string MyProperty { get; set; } = "Hello";

            public int MyMethod() => 42;

            public static PropertyInfo MyPropertyInfo => typeof(MyCustomType).GetProperty(nameof(MyCustomType.MyProperty));

            public static FieldInfo MyFieldInfo => typeof(MyCustomType).GetField(nameof(MyCustomType.MyField));

            public static MethodInfo MyMethodInfo => typeof(MyCustomType).GetMethod(nameof(MyCustomType.MyMethod));
        }

        public class PlainReflectionValueReader : IGetsValueFromReflection
        {
            public object GetValue(PropertyInfo property, object target) => property.GetValue(target);
            public object GetValue(MethodInfo method, object target) => method.Invoke(target, new object[0]);
            public object GetValue(FieldInfo field, object target) => field.GetValue(target);
        }

        #endregion
    }
}