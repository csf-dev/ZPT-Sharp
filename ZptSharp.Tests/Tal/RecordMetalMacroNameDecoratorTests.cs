using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Dom;
using ZptSharp.Expressions;
using ZptSharp.Metal;
using ZptSharp.Rendering;

namespace ZptSharp.Tal
{
    [TestFixture,Parallelizable]
    public class RecordMetalMacroNameDecoratorTests
    {
        [Test, AutoMoqData]
        public async Task ProcessContextAsync_records_macroname_when_there_is_a_define_macro_attribute([Frozen] IHandlesProcessingError wrapped,
                                                                                                       [Frozen] IGetsMetalAttributeSpecs metalSpecProvider,
                                                                                                       RecordMetalMacroNameDecorator sut,
                                                                                                       [StubDom] ExpressionContext context,
                                                                                                       [StubDom] IAttribute attribute,
                                                                                                       AttributeSpec spec,
                                                                                                       string macroName)
        {
            Mock.Get(metalSpecProvider).SetupGet(x => x.DefineMacro).Returns(spec);
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            context.CurrentElement.Attributes.Add(attribute);
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(ExpressionContextProcessingResult.Noop));
            attribute.Value = macroName;

            await sut.ProcessContextAsync(context);

            Assert.That(context.LocalDefinitions,
                        Has.One.Matches<KeyValuePair<string, object>>(x => x.Key == "macroname" && Equals(x.Value, macroName)));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_does_not_record_macroname_when_there_is_no_define_macro_attribute([Frozen] IHandlesProcessingError wrapped,
                                                                                                                [Frozen] IGetsMetalAttributeSpecs metalSpecProvider,
                                                                                                                RecordMetalMacroNameDecorator sut,
                                                                                                                [StubDom] ExpressionContext context,
                                                                                                                AttributeSpec spec)
        {
            Mock.Get(metalSpecProvider).SetupGet(x => x.DefineMacro).Returns(spec);
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(ExpressionContextProcessingResult.Noop));
            context.CurrentElement.Attributes.Clear();

            await sut.ProcessContextAsync(context);

            Assert.That(context.LocalDefinitions,
                        Has.None.Matches<KeyValuePair<string, object>>(x => x.Key == "macroname"));
        }
    }
}
