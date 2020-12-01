using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Expressions;

namespace ZptSharp.Expressions.PathExpressions
{
    [TestFixture,Parallelizable]
    public class EnumerableValueProviderTests
    {
        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_result_from_wrapped_service_if_object_is_not_enumerable([Frozen] IGetsValueFromObject wrapped,
                                                                                                           EnumerableValueProvider sut,
                                                                                                           GetValueResult expected)
        {
            var obj = new MyNonEnumerableType();

            Mock.Get(wrapped)
                .Setup(x => x.TryGetValueAsync(5.ToString(), obj, CancellationToken.None))
                .Returns(Task.FromResult(expected));

            var result = await sut.TryGetValueAsync(5.ToString(), obj);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_can_return_successful_result_with_item_from_list(EnumerableValueProvider sut)
        {
            var obj = new List<string> { "Foo", "Bar", "Baz" };

            var result = await sut.TryGetValueAsync(2.ToString(), obj);

            Assert.That(result.Success, Is.True, "Result indicates success");
            Assert.That(result.Value, Is.EqualTo("Baz"), "Correct value");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_can_return_failure_result_from_list_for_index_higher_than_last_index(EnumerableValueProvider sut)
        {
            var obj = new List<string> { "Foo", "Bar", "Baz" };

            var result = await sut.TryGetValueAsync(5.ToString(), obj);

            Assert.That(result.Success, Is.False, "Result indicates failure");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_can_return_failure_result_from_list_for_index_lower_than_zero(EnumerableValueProvider sut)
        {
            var obj = new List<string> { "Foo", "Bar", "Baz" };

            var result = await sut.TryGetValueAsync((-1).ToString(), obj);

            Assert.That(result.Success, Is.False, "Result indicates failure");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_can_return_successful_result_with_item_from_enumerable(EnumerableValueProvider sut)
        {
            var obj = new HashSet<string> { "Foo", "Bar", "Baz" };

            var result = await sut.TryGetValueAsync(2.ToString(), obj);

            Assert.That(result.Success, Is.True, "Result indicates success");
            Assert.That(result.Value, Is.EqualTo("Baz"), "Correct value");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_can_return_failure_result_from_enumerable_for_index_higher_than_last_index(EnumerableValueProvider sut)
        {
            var obj = new HashSet<string> { "Foo", "Bar", "Baz" };

            var result = await sut.TryGetValueAsync(5.ToString(), obj);

            Assert.That(result.Success, Is.False, "Result indicates failure");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_can_return_failure_result_from_enumerable_for_index_lower_than_zero(EnumerableValueProvider sut)
        {
            var obj = new HashSet<string> { "Foo", "Bar", "Baz" };

            var result = await sut.TryGetValueAsync((-1).ToString(), obj);

            Assert.That(result.Success, Is.False, "Result indicates failure");
        }

        #region contained type

        class MyNonEnumerableType
        {
            readonly List<string> strings = new List<string>(20);

            public string this[int key]
            {
                get => strings[key];
                set => strings[key] = value;
            }
        }

        #endregion
    }
}
