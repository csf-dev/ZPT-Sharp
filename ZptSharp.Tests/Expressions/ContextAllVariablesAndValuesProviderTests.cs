using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;

namespace ZptSharp.Expressions
{
    [TestFixture,Parallelizable]
    public class ContextAllVariablesAndValuesProviderTests
    {
        [Test, AutoMoqData]
        public async Task GetAllVariablesAsync_does_not_return_null([Frozen] IGetsBuiltinContextsProvider builtinContextsProviderFactory,
                                                                    [MockedConfig, Frozen] Config.RenderingConfig config,
                                                                    ContextAllVariablesAndValuesProvider sut,
                                                                    ExpressionContext context,
                                                                    IGetsDictionaryOfNamedTalesValues builtinContextsProvider,
                                                                    IDictionary<string,object> builtInContexts)
        {
            Mock.Get(builtinContextsProviderFactory)
                .Setup(x => x.GetBuiltinContextsProvider(context, config))
                .Returns(builtinContextsProvider);
            Mock.Get(builtinContextsProvider)
                .Setup(x => x.GetAllNamedValues())
                .Returns(Task.FromResult(builtInContexts));

            var result = await sut.GetAllVariablesAsync(context);

            Assert.That(result, Is.Not.Null);
        }

        [Test, AutoMoqData]
        public async Task GetAllVariablesAsync_prefers_local_variables_to_global([Frozen] IGetsBuiltinContextsProvider builtinContextsProviderFactory,
                                                                                 [MockedConfig, Frozen] Config.RenderingConfig config,
                                                                                 ContextAllVariablesAndValuesProvider sut,
                                                                                 ExpressionContext context,
                                                                                 IGetsDictionaryOfNamedTalesValues builtinContextsProvider,
                                                                                 IDictionary<string, object> builtInContexts)
        {
            Mock.Get(builtinContextsProviderFactory)
                .Setup(x => x.GetBuiltinContextsProvider(context, config))
                .Returns(builtinContextsProvider);
            Mock.Get(builtinContextsProvider)
                .Setup(x => x.GetAllNamedValues())
                .Returns(Task.FromResult(builtInContexts));

            context.LocalDefinitions.Clear();
            context.GlobalDefinitions.Clear();
            context.LocalDefinitions["foo"] = 1234;
            context.GlobalDefinitions["foo"] = 5678;

            var result = await sut.GetAllVariablesAsync(context);

            Assert.That(result, Has.One.Matches<KeyValuePair<string,object>>(x => x.Key == "foo" && Equals(x.Value, 1234)));
        }

        [Test, AutoMoqData]
        public async Task GetAllVariablesAsync_prefers_global_variables_to_builtin([Frozen] IGetsBuiltinContextsProvider builtinContextsProviderFactory,
                                                                                   [MockedConfig, Frozen] Config.RenderingConfig config,
                                                                                   ContextAllVariablesAndValuesProvider sut,
                                                                                   ExpressionContext context,
                                                                                   IGetsDictionaryOfNamedTalesValues builtinContextsProvider)
        {
            Mock.Get(builtinContextsProviderFactory)
                .Setup(x => x.GetBuiltinContextsProvider(context, config))
                .Returns(builtinContextsProvider);
            Mock.Get(builtinContextsProvider)
                .Setup(x => x.GetAllNamedValues())
                .Returns(Task.FromResult<IDictionary<string,object>>(new Dictionary<string, object> { { "foo", 1234 } }));

            context.LocalDefinitions.Clear();
            context.GlobalDefinitions.Clear();
            context.GlobalDefinitions["foo"] = 5678;

            var result = await sut.GetAllVariablesAsync(context);

            Assert.That(result, Has.One.Matches<KeyValuePair<string, object>>(x => x.Key == "foo" && Equals(x.Value, 5678)));
        }
    }
}
