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
    public class NamedTalesValueProviderTests
    {
        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_result_from_wrapped_service_if_object_is_not_IGetsNamedTalesValue([Frozen] IGetsValueFromObject wrapped,
                                                                                                                     NamedTalesValueProvider sut,
                                                                                                                     string name,
                                                                                                                     object obj,
                                                                                                                     GetValueResult expected)
        {
            Mock.Get(wrapped)
                .Setup(x => x.TryGetValueAsync(name, obj, CancellationToken.None))
                .Returns(Task.FromResult(expected));

            var result = await sut.TryGetValueAsync(name, obj);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_result_from_object_if_it_is_IGetsNamedTalesValue(NamedTalesValueProvider sut,
                                                                                                    string name,
                                                                                                    IGetsNamedTalesValue obj,
                                                                                                    GetValueResult expected)
        {
            Mock.Get(obj)
                .Setup(x => x.TryGetValueAsync(name, CancellationToken.None))
                .Returns(Task.FromResult(expected));

            var result = await sut.TryGetValueAsync(name, obj);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
