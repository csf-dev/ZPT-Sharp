using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using ZptSharp.Expressions;

namespace ZptSharp
{
    [TestFixture,Parallelizable]
    public class ServiceCollectionExtensionsTests
    {
        [Test]
        public void AddZptSharp_does_not_throw()
        {
            var collection = new ServiceCollection();
            Assert.That(() => collection.AddZptSharp(), Throws.Nothing);
        }

        [Test]
        public void UseStandardZptExpressions_does_not_throw()
        {
            var collection = new ServiceCollection();
            collection.AddZptSharp();
            var provider = collection.BuildServiceProvider();

            Assert.That(() => provider.UseStandardZptExpressions(), Throws.Nothing);
        }

        [Test]
        public void UseZptStringExpressions_adds_using_normal_prefix()
        {
            var collection = new ServiceCollection();
            collection.AddZptSharp();
            var provider = collection.BuildServiceProvider();

            provider.UseZptStringExpressions();

            Assert.That(() => provider.GetRequiredService<IRegistersExpressionEvaluator>().IsRegistered("string"), Is.True);
        }

        [Test]
        public void UseZptStringExpressions_adds_using_short_alias()
        {
            var collection = new ServiceCollection();
            collection.AddZptSharp();
            var provider = collection.BuildServiceProvider();

            provider.UseZptStringExpressions();

            Assert.That(() => provider.GetRequiredService<IRegistersExpressionEvaluator>().IsRegistered("str"), Is.True);
        }
    }
}
