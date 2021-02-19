using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;

namespace ZptSharp.Expressions
{
    [TestFixture,Parallelizable]
    public class NamedTalesValueForExpressionContextAdapterTests
    {
        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_built_in_context_if_contexts_requested([Frozen, NoAutoProperties] ExpressionContext context,
                                                                               [Frozen, MockedConfig] RenderingConfig config,
                                                                               [Frozen] IGetsBuiltinContextsProvider builtinContextsProviderFactory,
                                                                               IGetsDictionaryOfNamedTalesValues contextProvider,
                                                                               NamedTalesValueForExpressionContextAdapter sut)
        {
            Mock.Get(builtinContextsProviderFactory).Setup(x => x.GetBuiltinContextsProvider(context, config)).Returns(contextProvider);
            var result = await sut.TryGetValueAsync(NamedTalesValueForExpressionContextAdapter.ContextsName);
            Assert.That(result.Success, Is.True, "Value returned successfully");
            Assert.That(result.Value, Is.SameAs(contextProvider), "Context provider object returned");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_built_in_context_if_contexts_requested_even_if_there_is_a_local_variable([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                                                 [Frozen, MockedConfig] RenderingConfig config,
                                                                                                                 [Frozen] IGetsBuiltinContextsProvider builtinContextsProviderFactory,
                                                                                                                 IGetsDictionaryOfNamedTalesValues contextProvider,
                                                                                                                 object localVariableValue,
                                                                                                                 NamedTalesValueForExpressionContextAdapter sut)
        {
            context.LocalDefinitions.Add(NamedTalesValueForExpressionContextAdapter.ContextsName, localVariableValue);
            Mock.Get(builtinContextsProviderFactory).Setup(x => x.GetBuiltinContextsProvider(context, config)).Returns(contextProvider);
            var result = await sut.TryGetValueAsync(NamedTalesValueForExpressionContextAdapter.ContextsName);
            Assert.That(result.Success, Is.True, "Value returned successfully");
            Assert.That(result.Value, Is.SameAs(contextProvider), "Context provider object returned and not local variable value");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_value_from_local_variable_if_it_exists([Frozen, NoAutoProperties] ExpressionContext context,
                                                                               [Frozen, MockedConfig] RenderingConfig config,
                                                                               string name,
                                                                               object variableValue,
                                                                               NamedTalesValueForExpressionContextAdapter sut)
        {
            context.LocalDefinitions.Add(name, variableValue);
            var result = await sut.TryGetValueAsync(name);
            Assert.That(result.Success, Is.True, "Value returned successfully");
            Assert.That(result.Value, Is.SameAs(variableValue), "Variable value returned");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_value_from_global_variable_if_it_exists([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                [Frozen, MockedConfig] RenderingConfig config,
                                                                                string name,
                                                                                object variableValue,
                                                                                NamedTalesValueForExpressionContextAdapter sut)
        {
            context.GlobalDefinitions.Add(name, variableValue);
            var result = await sut.TryGetValueAsync(name);
            Assert.That(result.Success, Is.True, "Value returned successfully");
            Assert.That(result.Value, Is.SameAs(variableValue), "Variable value returned");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_value_from_built_in_context_if_no_local_or_global_variable([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                                   [Frozen, MockedConfig] RenderingConfig config,
                                                                                                   IGetsDictionaryOfNamedTalesValues contextProvider,
                                                                                                   [Frozen] IGetsBuiltinContextsProvider builtinContextsProviderFactory,
                                                                                                   string name,
                                                                                                   object variableValue,
                                                                                                   NamedTalesValueForExpressionContextAdapter sut)
        {
            Mock.Get(builtinContextsProviderFactory).Setup(x => x.GetBuiltinContextsProvider(context, config)).Returns(contextProvider);
            Mock.Get(contextProvider).Setup(x => x.TryGetValueAsync(name, CancellationToken.None)).Returns(Task.FromResult(GetValueResult.For(variableValue)));
            var result = await sut.TryGetValueAsync(name);
            Assert.That(result.Success, Is.True, "Value returned successfully");
            Assert.That(result.Value, Is.SameAs(variableValue), "Variable value returned");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_value_from_local_variable_over_global_in_case_of_naming_collision([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                                          [Frozen, MockedConfig] RenderingConfig config,
                                                                                                          string name,
                                                                                                          object localValue,
                                                                                                          object globalValue,
                                                                                                          NamedTalesValueForExpressionContextAdapter sut)
        {
            context.LocalDefinitions.Add(name, localValue);
            context.GlobalDefinitions.Add(name, globalValue);
            var result = await sut.TryGetValueAsync(name);
            Assert.That(result.Success, Is.True, "Value returned successfully");
            Assert.That(result.Value, Is.SameAs(localValue), "Local variable value returned");
        }

        [Test, AutoMoqData]
        public async Task TryGetValueAsync_returns_value_from_global_variable_over_built_in_context_in_case_of_naming_collision([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                                                     [Frozen, MockedConfig] RenderingConfig config,
                                                                                                                     IGetsDictionaryOfNamedTalesValues contextProvider,
                                                                                                                     [Frozen] IGetsBuiltinContextsProvider builtinContextsProviderFactory,
                                                                                                                     string name,
                                                                                                                     object globalValue,
                                                                                                                     object builtInValue,
                                                                                                                     NamedTalesValueForExpressionContextAdapter sut)
        {
            Mock.Get(builtinContextsProviderFactory).Setup(x => x.GetBuiltinContextsProvider(context, config)).Returns(contextProvider);
            Mock.Get(contextProvider).Setup(x => x.TryGetValueAsync(name, CancellationToken.None)).Returns(Task.FromResult(GetValueResult.For(builtInValue)));
            context.GlobalDefinitions.Add(name, globalValue);
            var result = await sut.TryGetValueAsync(name);
            Assert.That(result.Success, Is.True, "Value returned successfully");
            Assert.That(result.Value, Is.SameAs(globalValue), "Global variable value returned");
        }

    }
}
