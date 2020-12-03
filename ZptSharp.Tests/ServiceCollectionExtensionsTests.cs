using System;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

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
    }
}
