using System;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;

namespace ZptSharp.Expressions
{
    [TestFixture,Parallelizable]
    public class ContextValueProviderTests
    {
        [Test, AutoMoqData]
        public void TryGetValue_returns_built_in_context_if_contexts_requested([Frozen, NoAutoProperties] ExpressionContext context,
                                                                               [Frozen, MockedConfig] RenderingConfig config,
                                                                               [Frozen] IGetsBuiltinContextsProvider builtinContextsProviderFactory,
                                                                               IGetsNamedTalesValue contextProvider,
                                                                               ContextValueProvider sut)
        {
            Mock.Get(builtinContextsProviderFactory).Setup(x => x.GetBuiltinContextsProvider(context, config)).Returns(contextProvider);
            var result = sut.TryGetValue(ContextValueProvider.BuiltinContexts, out var value);
            Assert.That(result, Is.True, "Value returned successfully");
            Assert.That(value, Is.SameAs(contextProvider), "Context provider object returned");
        }

        [Test, AutoMoqData]
        public void TryGetValue_returns_built_in_context_if_contexts_requested_even_if_there_is_a_local_variable([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                                                 [Frozen, MockedConfig] RenderingConfig config,
                                                                                                                 [Frozen] IGetsBuiltinContextsProvider builtinContextsProviderFactory,
                                                                                                                 IGetsNamedTalesValue contextProvider,
                                                                                                                 object localVariableValue,
                                                                                                                 ContextValueProvider sut)
        {
            context.LocalDefinitions.Add(ContextValueProvider.BuiltinContexts, localVariableValue);
            Mock.Get(builtinContextsProviderFactory).Setup(x => x.GetBuiltinContextsProvider(context, config)).Returns(contextProvider);
            var result = sut.TryGetValue(ContextValueProvider.BuiltinContexts, out var value);
            Assert.That(result, Is.True, "Value returned successfully");
            Assert.That(value, Is.SameAs(contextProvider), "Context provider object returned and not local variable value");
        }

        [Test, AutoMoqData]
        public void TryGetValue_returns_value_from_local_variable_if_it_exists([Frozen, NoAutoProperties] ExpressionContext context,
                                                                               [Frozen, MockedConfig] RenderingConfig config,
                                                                               string name,
                                                                               object variableValue,
                                                                               ContextValueProvider sut)
        {
            context.LocalDefinitions.Add(name, variableValue);
            var result = sut.TryGetValue(name, out var value);
            Assert.That(result, Is.True, "Value returned successfully");
            Assert.That(value, Is.SameAs(variableValue), "Variable value returned");
        }

        [Test, AutoMoqData]
        public void TryGetValue_returns_value_from_global_variable_if_it_exists([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                [Frozen, MockedConfig] RenderingConfig config,
                                                                                string name,
                                                                                object variableValue,
                                                                                ContextValueProvider sut)
        {
            context.GlobalDefinitions.Add(name, variableValue);
            var result = sut.TryGetValue(name, out var value);
            Assert.That(result, Is.True, "Value returned successfully");
            Assert.That(value, Is.SameAs(variableValue), "Variable value returned");
        }

        [Test, AutoMoqData]
        public void TryGetValue_returns_value_from_built_in_context_if_no_local_or_global_variable([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                                   [Frozen, MockedConfig] RenderingConfig config,
                                                                                                   IGetsNamedTalesValue contextProvider,
                                                                                                   [Frozen] IGetsBuiltinContextsProvider builtinContextsProviderFactory,
                                                                                                   string name,
                                                                                                   object variableValue,
                                                                                                   ContextValueProvider sut)
        {
            Mock.Get(builtinContextsProviderFactory).Setup(x => x.GetBuiltinContextsProvider(context, config)).Returns(contextProvider);
            Mock.Get(contextProvider).Setup(x => x.TryGetValue(name, out variableValue)).Returns(true);
            var result = sut.TryGetValue(name, out var value);
            Assert.That(result, Is.True, "Value returned successfully");
            Assert.That(value, Is.SameAs(variableValue), "Variable value returned");
        }

        [Test, AutoMoqData]
        public void TryGetValue_returns_value_from_local_variable_over_global_in_case_of_naming_collision([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                                          [Frozen, MockedConfig] RenderingConfig config,
                                                                                                          string name,
                                                                                                          object localValue,
                                                                                                          object globalValue,
                                                                                                          ContextValueProvider sut)
        {
            context.LocalDefinitions.Add(name, localValue);
            context.GlobalDefinitions.Add(name, globalValue);
            var result = sut.TryGetValue(name, out var value);
            Assert.That(result, Is.True, "Value returned successfully");
            Assert.That(value, Is.SameAs(localValue), "Local variable value returned");
        }

        [Test, AutoMoqData]
        public void TryGetValue_returns_value_from_global_variable_over_built_in_context_in_case_of_naming_collision([Frozen, NoAutoProperties] ExpressionContext context,
                                                                                                                     [Frozen, MockedConfig] RenderingConfig config,
                                                                                                                     IGetsNamedTalesValue contextProvider,
                                                                                                                     [Frozen] IGetsBuiltinContextsProvider builtinContextsProviderFactory,
                                                                                                                     string name,
                                                                                                                     object globalValue,
                                                                                                                     object builtInValue,
                                                                                                                     ContextValueProvider sut)
        {
            Mock.Get(builtinContextsProviderFactory).Setup(x => x.GetBuiltinContextsProvider(context, config)).Returns(contextProvider);
            Mock.Get(contextProvider).Setup(x => x.TryGetValue(name, out builtInValue)).Returns(true);
            context.GlobalDefinitions.Add(name, globalValue);
            var result = sut.TryGetValue(name, out var value);
            Assert.That(result, Is.True, "Value returned successfully");
            Assert.That(value, Is.SameAs(globalValue), "Global variable value returned");
        }

    }
}
