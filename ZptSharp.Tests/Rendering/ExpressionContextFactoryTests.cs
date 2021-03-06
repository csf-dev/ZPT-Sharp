﻿using System;
using System.Linq;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Config;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    [TestFixture,Parallelizable]
    public class ExpressionContextFactoryTests
    {
        [Test, AutoMoqData]
        public void GetExpressionContext_returns_context_with_correct_document_root_node_and_model(IDocument document,
                                                                                                      object model,
                                                                                                      [Frozen] IServiceProvider serviceProvider,
                                                                                                      INode node,
                                                                                                      ExpressionContextFactory sut)
        {
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(RenderingConfig))).Returns(() => RenderingConfig.Default);
            Mock.Get(document).SetupGet(x => x.RootNode).Returns(node);
            var result = sut.GetExpressionContext(document, model);

            Assert.That(result.TemplateDocument, Is.SameAs(document), $"{nameof(ExpressionContext.TemplateDocument)} is correct");
            Assert.That(result.CurrentNode, Is.SameAs(node), $"{nameof(ExpressionContext.CurrentNode)} is correct");
            Assert.That(result.Model, Is.SameAs(model), $"{nameof(ExpressionContext.Model)} is correct");
        }

        [Test, AutoMoqData]
        public void GetExpressionContext_returns_context_configured_with_config_action_if_it_is_specified(IDocument document,
                                                                                                          [MockedConfig, Frozen] RenderingConfig config,
                                                                                                          [Frozen] IServiceProvider serviceProvider,
                                                                                                          object model,
                                                                                                          INode node,
                                                                                                          ExpressionContextFactory sut,
                                                                                                          object val)
        {
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(RenderingConfig))).Returns(() => config);
            Mock.Get(document).SetupGet(x => x.RootNode).Returns(node);
            Mock.Get(config).SetupGet(x => x.ContextBuilder).Returns((c, s) => c.AddToRootContext("Foo", val));
            var result = sut.GetExpressionContext(document, model);

            Assert.That(result.GlobalDefinitions["Foo"], Is.SameAs(val));
        }

        [Test, AutoMoqData]
        public void GetChildContexts_returns_a_context_for_each_child_node(ExpressionContext context,
                                                                              INode child1,
                                                                              INode child2,
                                                                              ExpressionContextFactory sut)
        {
            Mock.Get(child1).SetupGet(x => x.IsElement).Returns(true);
            Mock.Get(child2).SetupGet(x => x.IsElement).Returns(true);
            Mock.Get(context.CurrentNode).SetupGet(x => x.ChildNodes).Returns(() => new[] { child1, child2 });

            Assert.That(() => sut.GetChildContexts(context), Has.Count.EqualTo(2));
        }

        [Test, AutoMoqData]
        public void GetChildContexts_does_not_create_contexts_for_nodes_which_are_not_nodes(ExpressionContext context,
                                                                                               INode child1,
                                                                                               INode child2,
                                                                                               ExpressionContextFactory sut)
        {
            Mock.Get(child1).SetupGet(x => x.IsElement).Returns(true);
            Mock.Get(child2).SetupGet(x => x.IsElement).Returns(false);
            Mock.Get(context.CurrentNode).SetupGet(x => x.ChildNodes).Returns(() => new[] { child1, child2 });

            Assert.That(() => sut.GetChildContexts(context), Has.Count.EqualTo(1));
        }

        [Test, AutoMoqData]
        public void GetChildContexts_returns_a_context_with_correct_properties(ExpressionContext context,
                                                                               INode child1,
                                                                               ExpressionContextFactory sut)
        {
            Mock.Get(child1).SetupGet(x => x.IsElement).Returns(true);
            Mock.Get(context.CurrentNode).SetupGet(x => x.ChildNodes).Returns(() => new[] { child1 });

            var result = sut.GetChildContexts(context).Single();

            Assert.That(result.CurrentNode, Is.SameAs(child1), $"{nameof(ExpressionContext.CurrentNode)} is correct");
            Assert.That(result.TemplateDocument, Is.SameAs(context.TemplateDocument), $"{nameof(ExpressionContext.TemplateDocument)} is correct");
            Assert.That(result.Model, Is.SameAs(context.Model), $"{nameof(ExpressionContext.Model)} is correct");
            Assert.That(result.LocalDefinitions, Is.EqualTo(context.LocalDefinitions), $"{nameof(ExpressionContext.LocalDefinitions)} is correct");
            Assert.That(result.GlobalDefinitions, Is.EqualTo(context.GlobalDefinitions), $"{nameof(ExpressionContext.GlobalDefinitions)} is correct");
            Assert.That(result.Repetitions, Is.EqualTo(context.Repetitions), $"{nameof(ExpressionContext.Repetitions)} is correct");
        }
    }
}
