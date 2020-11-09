using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Expressions;

namespace ZptSharp.PathExpressions.ValueProviders
{
    [TestFixture,Parallelizable]
    public class IntegerKeyedDictionaryValueProviderTests
    {
        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_result_from_wrapped_service_if_object_is_not_a_dictionary_keyed_by_integer([Frozen] IGetsValueFromObject wrapped,
                                                                                                                              IntegerKeyedDictionaryValueProvider sut,
                                                                                                                              int number,
                                                                                                                              object obj,
                                                                                                                              GetValueResult expected)
        {
            Mock.Get(wrapped)
                .Setup(x => x.TryGetValueAsync(number.ToString(), obj, CancellationToken.None))
                .Returns(Task.FromResult(expected));

            var result = await sut.TryGetValueAsync(number.ToString(), obj);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_success_result_for_existing_integer_object_dictionary_item(IntegerKeyedDictionaryValueProvider sut,
                                                                                                              object @object)
        {
            var obj = new Dictionary<int, object>
            {
                { 3, @object }
            };

            var result = await sut.TryGetValueAsync(3.ToString(), obj);

            Assert.That(result.Success, Is.True, "Result indicates success");
            Assert.That(result.Value, Is.SameAs(@object), "Value from result");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_failure_result_for_nonexistent_integer_object_dictionary_item(IntegerKeyedDictionaryValueProvider sut,
                                                                                                                 object @object)
        {
            var obj = new Dictionary<int, object>
            {
                { 3, @object }
            };

            var result = await sut.TryGetValueAsync(5.ToString(), obj);

            Assert.That(result.Success, Is.False);
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_success_result_for_existing_integer_string_dictionary_item(IntegerKeyedDictionaryValueProvider sut,
                                                                                                              string @object)
        {
            var obj = new Dictionary<int, string>
            {
                { 3, @object }
            };

            var result = await sut.TryGetValueAsync(3.ToString(), obj);

            Assert.That(result.Success, Is.True, "Result indicates success");
            Assert.That(result.Value, Is.SameAs(@object), "Value from result");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_failure_result_for_nonexistent_integer_string_dictionary_item(IntegerKeyedDictionaryValueProvider sut,
                                                                                                                 string @object)
        {
            var obj = new Dictionary<int, string>
            {
                { 3, @object }
            };

            var result = await sut.TryGetValueAsync(5.ToString(), obj);

            Assert.That(result.Success, Is.False);
        }
    }
}
