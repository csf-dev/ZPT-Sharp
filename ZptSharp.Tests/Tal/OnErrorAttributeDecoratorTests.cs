using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Autofixture;
using ZptSharp.Dom;
using ZptSharp.Expressions;
using ZptSharp.Rendering;

namespace ZptSharp.Tal
{
    [TestFixture,Parallelizable]
    public class OnErrorAttributeDecoratorTests
    {
        [Test, AutoMoqData]
        public void ProcessContextAsync_returns_wrapped_result_if_it_does_not_throw([Frozen] IHandlesProcessingError wrapped,
                                                                                    OnErrorAttributeDecorator sut,
                                                                                    [StubDom] ExpressionContext context,
                                                                                    ExpressionContextProcessingResult wrappedResult)
        {
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(Task.FromResult(wrappedResult));
            Assert.That(() => sut.ProcessContextAsync(context).Result, Is.SameAs(wrappedResult));
        }

        [Test, AutoMoqData]
        public void ProcessContextAsync_rethrows_if_element_has_no_onError_attribute([Frozen] IHandlesProcessingError wrapped,
                                                                                     [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                     OnErrorAttributeDecorator sut,
                                                                                     AttributeSpec spec,
                                                                                     [StubDom] ExpressionContext context,
                                                                                     [StubDom] IAttribute attribute,
                                                                                     string message)
        {
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Throws(new Exception(message));
            Mock.Get(specProvider).SetupGet(x => x.OnError).Returns(spec);
            context.CurrentElement.Attributes.Clear();
            context.CurrentElement.Attributes.Add(attribute);
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(false);

            // It won't throw the exact same exception because it will be inside an aggregate exception
            Assert.That(() => sut.ProcessContextAsync(context).Result,
                        Throws.InstanceOf<AggregateException>().And.InnerException.Message.EqualTo(message));
        }

        [Test, AutoMoqData]
        public void ProcessContextAsync_returns_noop_result_if_attribute_aborts_action([Frozen] IHandlesProcessingError wrapped,
                                                                                       [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                       [Frozen] IEvaluatesDomValueExpression evaluator,
                                                                                       OnErrorAttributeDecorator sut,
                                                                                       AttributeSpec spec,
                                                                                       [StubDom] ExpressionContext context,
                                                                                       [StubDom] IAttribute attribute,
                                                                                       string expression)
        {
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Throws<Exception>();
            Mock.Get(specProvider).SetupGet(x => x.OnError).Returns(spec);
            context.CurrentElement.Attributes.Clear();
            context.CurrentElement.Attributes.Add(attribute);
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            Mock.Get(attribute).SetupGet(x => x.Value).Returns(expression);
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult(new DomValueExpressionResult(abortAction: true)));

            Assert.That(() => sut.ProcessContextAsync(context).Result,
                        Is.InstanceOf<ExpressionContextProcessingResult>()
                            .And.Property(nameof(ExpressionContextProcessingResult.AdditionalContexts)).Empty);
        }

        [Test, AutoMoqData]
        public async Task ProcessContextAsync_replaces_children_with_error_handling_result([Frozen] IHandlesProcessingError wrapped,
                                                                                           [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                           [Frozen] IEvaluatesDomValueExpression evaluator,
                                                                                           OnErrorAttributeDecorator sut,
                                                                                           AttributeSpec spec,
                                                                                           [StubDom] ExpressionContext context,
                                                                                           [StubDom] IAttribute attribute,
                                                                                           string expression,
                                                                                           [StubDom] INode existingChild,
                                                                                           [StubDom] INode replacement)
        {
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Throws<Exception>();
            Mock.Get(specProvider).SetupGet(x => x.OnError).Returns(spec);
            context.CurrentElement.Attributes.Clear();
            context.CurrentElement.Attributes.Add(attribute);
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            Mock.Get(attribute).SetupGet(x => x.Value).Returns(expression);
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult(new DomValueExpressionResult(new[] { replacement })));
            context.CurrentElement.ChildNodes.Clear();
            context.CurrentElement.ChildNodes.Add(existingChild);

            await sut.ProcessContextAsync(context);

            Assert.That(context.CurrentElement.ChildNodes, Is.EqualTo(new[] { replacement }));
        }

        [Test, AutoMoqData]
        public void ProcessContextAsync_throws_OnErrorHandlingException_if_evaluator_throws([Frozen] IHandlesProcessingError wrapped,
                                                                                            [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                            [Frozen] IEvaluatesDomValueExpression evaluator,
                                                                                            [Frozen, MockLogger] Microsoft.Extensions.Logging.ILogger<OnErrorAttributeDecorator> logger,
                                                                                            OnErrorAttributeDecorator sut,
                                                                                            AttributeSpec spec,
                                                                                            [StubDom] ExpressionContext context,
                                                                                            [StubDom] IAttribute attribute,
                                                                                            string expression)
        {
            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Throws<Exception>();
            Mock.Get(specProvider).SetupGet(x => x.OnError).Returns(spec);
            context.CurrentElement.Attributes.Clear();
            context.CurrentElement.Attributes.Add(attribute);
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            Mock.Get(attribute).SetupGet(x => x.Value).Returns(expression);
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Throws<Exception>();

            Assert.That(() => sut.ProcessContextAsync(context).Result,
                        Throws.InstanceOf<AggregateException>().And.InnerException.InstanceOf<OnErrorHandlingException>());
        }


        [Test, AutoMoqData]
        public async Task ProcessContextAsync_sets_error_object_into_context_when_it_catches_an_error([Frozen] IHandlesProcessingError wrapped,
                                                                                                      [Frozen] IGetsTalAttributeSpecs specProvider,
                                                                                                      [Frozen] IEvaluatesDomValueExpression evaluator,
                                                                                                      OnErrorAttributeDecorator sut,
                                                                                                      AttributeSpec spec,
                                                                                                      [StubDom] ExpressionContext context,
                                                                                                      [StubDom] IAttribute attribute,
                                                                                                      string expression,
                                                                                                      [StubDom] INode existingChild,
                                                                                                      [StubDom] INode replacement)
        {
            var exception = new InvalidOperationException("Sample exception");

            Mock.Get(wrapped)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Throws(exception);
            Mock.Get(specProvider).SetupGet(x => x.OnError).Returns(spec);
            context.CurrentElement.Attributes.Clear();
            context.CurrentElement.Attributes.Add(attribute);
            Mock.Get(attribute).Setup(x => x.Matches(spec)).Returns(true);
            Mock.Get(attribute).SetupGet(x => x.Value).Returns(expression);
            Mock.Get(evaluator)
                .Setup(x => x.EvaluateExpressionAsync(expression, context, CancellationToken.None))
                .Returns(() => Task.FromResult(new DomValueExpressionResult(new[] { replacement })));
            context.CurrentElement.ChildNodes.Clear();
            context.CurrentElement.ChildNodes.Add(existingChild);

            await sut.ProcessContextAsync(context);

            Assert.That(context.Error, Is.InstanceOf<OnErrorExceptionAdapter>(), "Object is of correct type");
            Assert.That(() => (OnErrorExceptionAdapter) context.Error,
                        Has.Property("type").EqualTo(typeof(InvalidOperationException))
                       .And.Property("value").SameAs(exception)
                       .And.Property("traceback").EqualTo(exception.StackTrace));
        }

    }
}
