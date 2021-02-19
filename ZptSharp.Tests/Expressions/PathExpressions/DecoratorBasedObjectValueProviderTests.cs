using System;
using AutoFixture.NUnit3;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;
using ZptSharp.Expressions;

namespace ZptSharp.Expressions.PathExpressions
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

        [Test, AutoMoqData]
        public void TryGetValueAsync_does_not_throw_if_object_is_null([MockedConfig, Frozen] RenderingConfig config,
                                                                      [Frozen] IGetsBuiltinContextsProvider builtinContextsProviderFactory,
                                                                      DecoratorBasedObjectValueProvider sut,
                                                                      string name)
        {
            Assert.That(() => sut.TryGetValueAsync(name, null).Result, Throws.Nothing);
        }
    }
}
