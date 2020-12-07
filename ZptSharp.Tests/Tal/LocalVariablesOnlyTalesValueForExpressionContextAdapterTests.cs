using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using NUnit.Framework;
using ZptSharp.Expressions;

namespace ZptSharp.Tal
{
    [TestFixture,Parallelizable]
    public class LocalVariablesOnlyTalesValueForExpressionContextAdapterTests
    {
        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_result_from_local_variable([Frozen] ExpressionContext context,
                                                                              LocalVariablesOnlyTalesValueForExpressionContextAdapter sut)
        {
            context.LocalDefinitions["foo"] = "bar";

            var result = await sut.TryGetValueAsync("foo");
            Assert.That(result.Value, Is.EqualTo("bar"));
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_does_not_return_value_from_global_variable([Frozen] ExpressionContext context,
                                                                                      LocalVariablesOnlyTalesValueForExpressionContextAdapter sut)
        {
            context.GlobalDefinitions["foo"] = "bar";

            var result = await sut.TryGetValueAsync("foo");
            Assert.That(result.Success, Is.False);
        }
    }
}
