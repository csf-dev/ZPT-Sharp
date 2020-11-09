using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Expressions;

namespace ZptSharp.PathExpressions.ValueProviders
{
    [TestFixture,Parallelizable]
    public class ReflectionObjectValueProviderTests
    {
        [Test, AutoMoqData]
        public async Task ReflectionObjectValueProvider_returns_value_from_wrapped_service_if_object_is_null([Frozen] IGetsValueFromObject wrapped,
                                                                                                             ReflectionObjectValueProvider sut,
                                                                                                             string name,
                                                                                                             GetValueResult expected)
        {
            Mock.Get(wrapped)
                .Setup(x => x.TryGetValueAsync(name, null, CancellationToken.None))
                .Returns(Task.FromResult(expected));

            var result = await sut.TryGetValueAsync(name, null);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test, AutoMoqData]
        public async Task ReflectionObjectValueProvider_returns_value_from_property_when_name_matches(ReflectionObjectValueProvider sut,
                                                                                                      MyCustomType obj,
                                                                                                      GetValueResult expected)
        {
            var result = await sut.TryGetValueAsync(nameof(MyCustomType.MyProperty), obj);

            Assert.That(result.Success, Is.True, "Result indicates success");
            Assert.That(result.Value, Is.EqualTo(obj.MyProperty), "Correct value");
        }

        [Test, AutoMoqData]
        public async Task ReflectionObjectValueProvider_returns_value_from_method_when_name_matches(ReflectionObjectValueProvider sut,
                                                                                                    MyCustomType obj,
                                                                                                    GetValueResult expected)
        {
            var result = await sut.TryGetValueAsync(nameof(MyCustomType.MyMethod), obj);

            Assert.That(result.Success, Is.True, "Result indicates success");
            Assert.That(result.Value, Is.EqualTo(obj.MyMethod()), "Correct value");
        }

        [Test, AutoMoqData]
        public async Task ReflectionObjectValueProvider_returns_value_from_field_when_name_matches(ReflectionObjectValueProvider sut,
                                                                                                   MyCustomType obj,
                                                                                                   GetValueResult expected)
        {
            var result = await sut.TryGetValueAsync(nameof(MyCustomType.MyField), obj);

            Assert.That(result.Success, Is.True, "Result indicates success");
            Assert.That(result.Value, Is.EqualTo(obj.MyField), "Correct value");
        }

        [Test, AutoMoqData]
        public async Task ReflectionObjectValueProvider_returns_value_from_wrapped_service_if_no_members_match([Frozen] IGetsValueFromObject wrapped,
                                                                                                               ReflectionObjectValueProvider sut,
                                                                                                               MyCustomType obj,
                                                                                                               GetValueResult expected)
        {
            Mock.Get(wrapped)
                .Setup(x => x.TryGetValueAsync("Nope", obj, CancellationToken.None))
                .Returns(Task.FromResult(expected));

            var result = await sut.TryGetValueAsync("Nope", obj);

            Assert.That(result, Is.EqualTo(expected));
        }

        #region contained types

        public class MyCustomType
        {
            public bool MyField = true;

            public string MyProperty { get; set; } = "Hello";

            public int MyMethod() => 42;
        }

        #endregion
    }
}
