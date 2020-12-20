using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Metal
{
    [TestFixture,Parallelizable]
    public class AddDefinedMacroToGlobalScopeProcessorDecoratorTests
    {
        [Test, AutoMoqData]
        public async Task ProcessContextAsync_adds_to_global_context_if_node_has_define_macro_attribute_and_is_the_root_context([Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                                                                                   [Frozen] ISearchesForAttributes attributeFinder,
                                                                                                                                   AddDefinedMacroToGlobalScopeProcessorDecorator sut,
                                                                                                                                   AttributeSpec spec,
                                                                                                                                   ExpressionContext context,
                                                                                                                                   INode node,
                                                                                                                                   INode clone,
                                                                                                                                   IAttribute attribute,
                                                                                                                                   string name)
        {
            Mock.Get(specProvider).SetupGet(x => x.DefineMacro).Returns(spec);
            context.CurrentNode = node;
            Mock.Get(attributeFinder).Setup(x => x.SearchForAttributes(node, spec)).Returns(() => new[] { attribute });
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            Mock.Get(attribute).SetupGet(x => x.Value).Returns(name);
            Mock.Get(attribute).SetupGet(x => x.Node).Returns(node);
            Mock.Get(node).Setup(x => x.GetCopy()).Returns(clone);
            context.GlobalDefinitions.Clear();
            context.IsRootContext = true;

            await sut.ProcessContextAsync(context);

            Assert.That(context.GlobalDefinitions,
                        Has.One.Matches<KeyValuePair<string, object>>(x => x.Key == name
                                                                        && (x.Value is MetalMacro macro)
                                                                        && macro.Name == name
                                                                        && macro.Node == clone));
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_does_not_add_to_global_context_if_it_is_not_the_root_context([Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                                                           [Frozen] ISearchesForAttributes attributeFinder,
                                                                                                           AddDefinedMacroToGlobalScopeProcessorDecorator sut,
                                                                                                           AttributeSpec spec,
                                                                                                           ExpressionContext context,
                                                                                                           INode node,
                                                                                                           IAttribute attribute,
                                                                                                           string name)
        {
            Mock.Get(specProvider).SetupGet(x => x.DefineMacro).Returns(spec);
            context.CurrentNode = node;
            Mock.Get(attributeFinder).Setup(x => x.SearchForAttributes(node, spec)).Returns(() => new[] { attribute });
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            Mock.Get(attribute).SetupGet(x => x.Value).Returns(name);
            Mock.Get(attribute).SetupGet(x => x.Node).Returns(node);
            context.GlobalDefinitions.Clear();
            context.IsRootContext = false;

            await sut.ProcessContextAsync(context);

            Assert.That(context.GlobalDefinitions, Is.Empty);
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_does_not_add_to_global_context_if_node_has_no_define_macro_attribute([Frozen] IGetsMetalAttributeSpecs specProvider,
                                                                                                                      AddDefinedMacroToGlobalScopeProcessorDecorator sut,
                                                                                                                      AttributeSpec spec,
                                                                                                                      ExpressionContext context,
                                                                                                                      INode node,
                                                                                                                      IAttribute attribute)
        {
            Mock.Get(specProvider).SetupGet(x => x.DefineMacro).Returns(spec);
            context.CurrentNode = node;
            Mock.Get(node).SetupGet(x => x.Attributes).Returns(new List<IAttribute> { attribute });
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(false);
            context.GlobalDefinitions.Clear();

            await sut.ProcessContextAsync(context);

            Assert.That(context.GlobalDefinitions, Has.Count.Zero);
        }
    }
}
