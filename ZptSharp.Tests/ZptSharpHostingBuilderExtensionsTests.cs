using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using ZptSharp.Expressions;

namespace ZptSharp
{
    [TestFixture,Parallelizable]
    public class ZptSharpHostingBuilderExtensionsTests
    {
        [Test]
        public void AddZptSharp_does_not_throw()
        {
            var collection = new ServiceCollection();
            Assert.That(() => collection.AddZptSharp(), Throws.Nothing);
        }

        [Test]
        public void AddStandardZptExpressions_does_not_throw()
        {
            var builder = new ServiceCollection().AddZptSharp();
            Assert.That(() => builder.AddStandardZptExpressions(), Throws.Nothing);
        }

        [Test]
        public void AddZptStringExpressions_adds_using_normal_prefix()
        {
            var builder = new ServiceCollection().AddZptSharp();
            builder.AddZptStringExpressions();
            var provider = builder.ServiceCollection.BuildServiceProvider();

            Assert.That(() => provider.GetRequiredService<IRegistersExpressionEvaluator>().IsRegistered("string"), Is.True);
        }

        [Test]
        public void AddZptStringExpressions_adds_using_short_alias()
        {
            var builder = new ServiceCollection().AddZptSharp();
            builder.AddZptStringExpressions();
            var provider = builder.ServiceCollection.BuildServiceProvider();

            Assert.That(() => provider.GetRequiredService<IRegistersExpressionEvaluator>().IsRegistered("str"), Is.True);
        }
    }
}
