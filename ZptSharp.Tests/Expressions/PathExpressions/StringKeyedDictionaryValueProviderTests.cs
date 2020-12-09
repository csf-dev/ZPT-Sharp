using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Expressions;

namespace ZptSharp.Expressions.PathExpressions
{
    [TestFixture, Parallelizable]
    public class StringKeyedDictionaryValueProviderTests
    {
        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_result_from_wrapped_service_if_object_is_not_a_dictionary_keyed_by_string([Frozen] IGetsValueFromObject wrapped,
                                                                                                                             StringKeyedDictionaryValueProvider sut,
                                                                                                                             string key,
                                                                                                                             object obj,
                                                                                                                             GetValueResult expected)
        {
            Mock.Get(wrapped)
                .Setup(x => x.TryGetValueAsync(key, obj, CancellationToken.None))
                .Returns(Task.FromResult(expected));

            var result = await sut.TryGetValueAsync(key, obj);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_success_result_for_existing_string_object_dictionary_item(StringKeyedDictionaryValueProvider sut,
                                                                                                              object @object)
        {
            var obj = new Dictionary<string, object>
            {
                { "foo", @object }
            };

            var result = await sut.TryGetValueAsync("foo", obj);

            Assert.That(result.Success, Is.True, "Result indicates success");
            Assert.That(result.Value, Is.SameAs(@object), "Value from result");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_failure_result_for_nonexistent_string_object_dictionary_item(StringKeyedDictionaryValueProvider sut,
                                                                                                                 object @object)
        {
            var obj = new Dictionary<string, object>
            {
                { "foo", @object }
            };

            var result = await sut.TryGetValueAsync("bar", obj);

            Assert.That(result.Success, Is.False);
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_success_result_for_existing_string_string_dictionary_item(StringKeyedDictionaryValueProvider sut,
                                                                                                              string @object)
        {
            var obj = new Dictionary<string, string>
            {
                { "foo", @object }
            };

            var result = await sut.TryGetValueAsync("foo", obj);

            Assert.That(result.Success, Is.True, "Result indicates success");
            Assert.That(result.Value, Is.SameAs(@object), "Value from result");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_failure_result_for_nonexistent_string_string_dictionary_item(StringKeyedDictionaryValueProvider sut,
                                                                                                                 string @object)
        {
            var obj = new Dictionary<string, string>
            {
                { "foo", @object }
            };

            var result = await sut.TryGetValueAsync("bar", obj);

            Assert.That(result.Success, Is.False);
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_success_result_for_existing_item_in_expando_object(StringKeyedDictionaryValueProvider sut)
        {
            dynamic obj = new ExpandoObject();
            obj.Foo = "bar";

            var result = await sut.TryGetValueAsync("Foo", obj);

            Assert.That(result.Success, Is.True, "Result indicates success");
            Assert.That(result.Value, Is.EqualTo("bar"), "Value from result");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_failure_result_for_nonexistent_item_in_expando_object(StringKeyedDictionaryValueProvider sut)
        {
            dynamic obj = new ExpandoObject();
            obj.Foo = "bar";

            var result = await sut.TryGetValueAsync("Nope", obj);

            Assert.That(result.Success, Is.False);
        }
    }
}
