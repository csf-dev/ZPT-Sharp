using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;
using ZptSharp.Expressions;

namespace ZptSharp.PathExpressions
{
    [TestFixture,Parallelizable]
    public class DecoratorBasedObjectValueProviderTests
    {
        [Test, AutoMoqData]
        public void TryGetValueAsync_returns_result_for_an_object_and_does_not_throw([MockedConfig,Frozen] RenderingConfig config,
                                                                                     [Frozen] IGetsBuiltinContextsProvider builtinContextsProviderFactory,
                                                                                     DecoratorBasedObjectValueProvider sut,
                                                                                     string name,
                                                                                     object obj)
        {
            Assert.That(() => sut.TryGetValueAsync(name, obj).Result, Throws.Nothing);
        }
    }
}
