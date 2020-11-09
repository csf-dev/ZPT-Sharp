using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Expressions;

namespace ZptSharp.PathExpressions.ValueProviders
{
    [TestFixture,Parallelizable]
    public class DynamicObjectValueProviderTests
    {
        [Test, AutoMoqData]
        public async Task TryGetValueAsync_should_return_success_result_with_correct_value_for_a_dynamic_object(DynamicObjectValueProvider sut)
        {
            var obj = new MyDynamicObject();

            var result = await sut.TryGetValueAsync("Foo", obj);

            Assert.That(result.Success, Is.True, "Result indicates success");
            Assert.That(result.Value, Is.EqualTo("Bar"), "Correct result value");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_should_return_a_failure_result_with_incorrect_value_for_a_dynamic_object(DynamicObjectValueProvider sut)
        {
            var obj = new MyDynamicObject();

            var result = await sut.TryGetValueAsync("Nope", obj);

            Assert.That(result.Success, Is.False, "Result indicates success");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_a_success_result_for_an_expando_object(DynamicObjectValueProvider sut)
        {
            dynamic obj = new ExpandoObject();
            obj.Foo = "Bar";

            var result = await sut.TryGetValueAsync("Foo", obj);

            Assert.That(result.Success, Is.True, "Result indicates success");
            Assert.That(result.Value, Is.EqualTo("Bar"), "Correct result value");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_result_from_wrapped_service_for_a_non_dynamic_object([Frozen] IGetsValueFromObject wrapped,
                                                                                                        DynamicObjectValueProvider sut,
                                                                                                        GetValueResult expected)
        {
            var obj = new NonDynamicObject();
            Mock.Get(wrapped)
                .Setup(x => x.TryGetValueAsync("Foo", obj, CancellationToken.None))
                .Returns(Task.FromResult(expected));

            var result = await sut.TryGetValueAsync("Foo", obj);

            Assert.That(result, Is.EqualTo(expected));
        }

        #region contained types

        class MyDynamicObject : DynamicObject
        {
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if(binder.Name == "Foo")
                {
                    result = "Bar";
                    return true;
                }

                result = null;
                return false;
            }
        }

        class NonDynamicObject
        {
            public string Foo { get; set; }
        }

        #endregion
    }
}
