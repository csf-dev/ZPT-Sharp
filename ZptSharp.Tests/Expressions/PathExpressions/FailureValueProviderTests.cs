using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ZptSharp.Expressions.PathExpressions
{
    [TestFixture,Parallelizable]
    public class FailureValueProviderTests
    {
        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_failure_result(FailureValueProvider sut, string name, object obj)
        {
            var result = await sut.TryGetValueAsync(name, obj);
            Assert.That(result.Success, Is.False);
        }
    }
}
