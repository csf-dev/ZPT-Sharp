using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    [TestFixture,Parallelizable]
    public class ExpressionContextIterativeProcessorTests
    {
        [Test, AutoMoqData]
        public async Task IterateContextAndChildrenAsync_should_process_the_specified_context([Frozen] IProcessesExpressionContext contextProcessor,
                                                                                              ExpressionContext context,
                                                                                              ExpressionContextIterativeProcessor sut)
        {
            await sut.IterateContextAndChildrenAsync(context);
            Mock.Get(contextProcessor)
                .Verify(x => x.ProcessContextAsync(context, CancellationToken.None), Times.Once);
        }

        [Test, AutoMoqData]
        public async Task IterateContextAndChildrenAsync_should_process_child_contexts([Frozen] IProcessesExpressionContext contextProcessor,
                                                                                       [Frozen] IGetsChildExpressionContexts childContextProvider,
                                                                                       ExpressionContext context,
                                                                                       ExpressionContext child1,
                                                                                       ExpressionContext child2,
                                                                                       ExpressionContextIterativeProcessor sut)
        {
            Mock.Get(childContextProvider)
                .Setup(x => x.GetChildContexts(context))
                .Returns(new[] { child1, child2 });

            await sut.IterateContextAndChildrenAsync(context);

            Mock.Get(contextProcessor)
                .Verify(x => x.ProcessContextAsync(child1, CancellationToken.None), Times.Once, $"Processed {nameof(child1)}");
            Mock.Get(contextProcessor)
                .Verify(x => x.ProcessContextAsync(child2, CancellationToken.None), Times.Once, $"Processed {nameof(child2)}");
        }

        [Test, AutoMoqData]
        public async Task IterateContextAndChildrenAsync_should_not_process_child_contexts_if_result_indicates_not_to([Frozen] IProcessesExpressionContext contextProcessor,
                                                                                                                      [Frozen] IGetsChildExpressionContexts childContextProvider,
                                                                                                                      ExpressionContext context,
                                                                                                                      ExpressionContext child1,
                                                                                                                      ExpressionContext child2,
                                                                                                                      ExpressionContextIterativeProcessor sut)
        {
            Mock.Get(childContextProvider)
                .Setup(x => x.GetChildContexts(context))
                .Returns(new[] { child1, child2 });
            Mock.Get(contextProcessor)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(Task.FromResult(ExpressionContextProcessingResult.WithoutChildren));

            await sut.IterateContextAndChildrenAsync(context);

            Mock.Get(contextProcessor)
                .Verify(x => x.ProcessContextAsync(child1, CancellationToken.None), Times.Never, $"Does not process {nameof(child1)}");
            Mock.Get(contextProcessor)
                .Verify(x => x.ProcessContextAsync(child2, CancellationToken.None), Times.Never, $"Does not process {nameof(child2)}");
        }

        [Test, AutoMoqData]
        public async Task IterateContextAndChildrenAsync_should_process_additional_contexts([Frozen] IProcessesExpressionContext contextProcessor,
                                                                                            ExpressionContext context,
                                                                                            ExpressionContext additional1,
                                                                                            ExpressionContext additional2,
                                                                                            ExpressionContextIterativeProcessor sut)
        {
            Mock.Get(contextProcessor)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(new ExpressionContextProcessingResult { AdditionalContexts = new[] { additional1, additional2 } }));

            await sut.IterateContextAndChildrenAsync(context);

            Mock.Get(contextProcessor)
                .Verify(x => x.ProcessContextAsync(additional1, CancellationToken.None), Times.Once, $"Processed {nameof(additional1)}");
            Mock.Get(contextProcessor)
                .Verify(x => x.ProcessContextAsync(additional2, CancellationToken.None), Times.Once, $"Processed {nameof(additional2)}");
        }

        [Test, AutoMoqData]
        public async Task IterateContextAndChildrenAsync_should_process_grandchild_contexts([Frozen] IProcessesExpressionContext contextProcessor,
                                                                                            [Frozen] IGetsChildExpressionContexts childContextProvider,
                                                                                            ExpressionContext context,
                                                                                            ExpressionContext child,
                                                                                            ExpressionContext grandchild1,
                                                                                            ExpressionContext grandchild2,
                                                                                            ExpressionContextIterativeProcessor sut)
        {
            Mock.Get(childContextProvider)
                .Setup(x => x.GetChildContexts(context))
                .Returns(new[] { child });
            Mock.Get(childContextProvider)
                .Setup(x => x.GetChildContexts(child))
                .Returns(new[] { grandchild1, grandchild2 });

            await sut.IterateContextAndChildrenAsync(context);

            Mock.Get(contextProcessor)
                .Verify(x => x.ProcessContextAsync(grandchild1, CancellationToken.None), Times.Once, $"Processed {nameof(grandchild1)}");
            Mock.Get(contextProcessor)
                .Verify(x => x.ProcessContextAsync(grandchild2, CancellationToken.None), Times.Once, $"Processed {nameof(grandchild2)}");
        }

        [Test, AutoMoqData]
        public async Task IterateContextAndChildrenAsync_should_process_additional_contexts_from_child_context([Frozen] IProcessesExpressionContext contextProcessor,
                                                                                                               [Frozen] IGetsChildExpressionContexts childContextProvider,
                                                                                                               ExpressionContext context,
                                                                                                               ExpressionContext child,
                                                                                                               ExpressionContext additional1,
                                                                                                               ExpressionContext additional2,
                                                                                                               ExpressionContextIterativeProcessor sut)
        {
            Mock.Get(childContextProvider)
                .Setup(x => x.GetChildContexts(context))
                .Returns(new[] { child });
            Mock.Get(contextProcessor)
                .Setup(x => x.ProcessContextAsync(child, CancellationToken.None))
                .Returns(() => Task.FromResult(new ExpressionContextProcessingResult { AdditionalContexts = new[] { additional1, additional2 } }));

            await sut.IterateContextAndChildrenAsync(context);

            Mock.Get(contextProcessor)
                .Verify(x => x.ProcessContextAsync(additional1, CancellationToken.None), Times.Once, $"Processed {nameof(additional1)}");
            Mock.Get(contextProcessor)
                .Verify(x => x.ProcessContextAsync(additional2, CancellationToken.None), Times.Once, $"Processed {nameof(additional2)}");
        }

        [Test, AutoMoqData]
        public async Task IterateContextAndChildrenAsync_should_process_sibling_additional_contexts([Frozen] IProcessesExpressionContext contextProcessor,
                                                                                                    ExpressionContext context,
                                                                                                    ExpressionContext additional,
                                                                                                    ExpressionContext sibling1,
                                                                                                    ExpressionContext sibling2,
                                                                                                    ExpressionContextIterativeProcessor sut)
        {
            Mock.Get(contextProcessor)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(new ExpressionContextProcessingResult { AdditionalContexts = new[] { additional } }));
            Mock.Get(contextProcessor)
                .Setup(x => x.ProcessContextAsync(additional, CancellationToken.None))
                .Returns(() => Task.FromResult(new ExpressionContextProcessingResult { AdditionalContexts = new[] { sibling1, sibling2 } }));

            await sut.IterateContextAndChildrenAsync(context);

            Mock.Get(contextProcessor)
                .Verify(x => x.ProcessContextAsync(sibling1, CancellationToken.None), Times.Once, $"Processed {nameof(sibling1)}");
            Mock.Get(contextProcessor)
                .Verify(x => x.ProcessContextAsync(sibling2, CancellationToken.None), Times.Once, $"Processed {nameof(sibling2)}");
        }

        [Test, AutoMoqData]
        public async Task IterateContextAndChildrenAsync_should_process_children_of_additional_context([Frozen] IProcessesExpressionContext contextProcessor,
                                                                                                       [Frozen] IGetsChildExpressionContexts childContextProvider,
                                                                                                       ExpressionContext context,
                                                                                                       ExpressionContext additional,
                                                                                                       ExpressionContext child1,
                                                                                                       ExpressionContext child2,
                                                                                                       ExpressionContextIterativeProcessor sut)
        {
            Mock.Get(contextProcessor)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(() => Task.FromResult(new ExpressionContextProcessingResult { AdditionalContexts = new[] { additional } }));
            Mock.Get(childContextProvider)
                .Setup(x => x.GetChildContexts(additional))
                .Returns(new[] { child1, child2 });

            await sut.IterateContextAndChildrenAsync(context);

            Mock.Get(contextProcessor)
                .Verify(x => x.ProcessContextAsync(child1, CancellationToken.None), Times.Once, $"Processed {nameof(child1)}");
            Mock.Get(contextProcessor)
                .Verify(x => x.ProcessContextAsync(child2, CancellationToken.None), Times.Once, $"Processed {nameof(child1)}");
        }

        [Test, AutoMoqData]
        public async Task IterateContextAndChildrenAsync_should_process_children_before_siblings(ExpressionContext context,
                                                                                                 ExpressionContext sibling1,
                                                                                                 ExpressionContext sibling2,
                                                                                                 ExpressionContext child1,
                                                                                                 ExpressionContext child2)
        {
            var contextProcessor = new Mock<IProcessesExpressionContext>(MockBehavior.Strict).Object;
            var childContextProvider = Mock.Of<IGetsChildExpressionContexts>();
            var sut = new ExpressionContextIterativeProcessor(contextProcessor, childContextProvider);

            Mock.Get(childContextProvider)
                .Setup(x => x.GetChildContexts(context))
                .Returns(new[] { sibling1, sibling2 });
            Mock.Get(childContextProvider)
                .Setup(x => x.GetChildContexts(sibling1))
                .Returns(new[] { child1, child2 });

            var sequence = new MockSequence();
            Mock.Get(contextProcessor)
                .InSequence(sequence)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Returns(Task.FromResult(ExpressionContextProcessingResult.Noop));
            Mock.Get(contextProcessor)
                .InSequence(sequence)
                .Setup(x => x.ProcessContextAsync(sibling1, CancellationToken.None))
                .Returns(Task.FromResult(ExpressionContextProcessingResult.Noop));
            Mock.Get(contextProcessor)
                .InSequence(sequence)
                .Setup(x => x.ProcessContextAsync(child1, CancellationToken.None))
                .Returns(Task.FromResult(ExpressionContextProcessingResult.Noop));
            Mock.Get(contextProcessor)
                .InSequence(sequence)
                .Setup(x => x.ProcessContextAsync(child2, CancellationToken.None))
                .Returns(Task.FromResult(ExpressionContextProcessingResult.Noop));
            Mock.Get(contextProcessor)
                .InSequence(sequence)
                .Setup(x => x.ProcessContextAsync(sibling2, CancellationToken.None))
                .Returns(Task.FromResult(ExpressionContextProcessingResult.Noop));

            await sut.IterateContextAndChildrenAsync(context);

            Mock.Get(contextProcessor)
                .Verify(x => x.ProcessContextAsync(sibling2, CancellationToken.None), Times.Once);
        }

        [Test, AutoMoqData]
        public async Task IterateContextAndChildrenAsync_should_add_processor_to_child_context_error_handler_stack_if_context_processor_is_error_handler(ExpressionContext context,
                                                                                                                                                         ExpressionContext child1,
                                                                                                                                                         ExpressionContext child2)
        {
            var contextProcessorMock = new Mock<IProcessesExpressionContext>();
            contextProcessorMock.As<IHandlesProcessingError>();
            var contextProcessor = contextProcessorMock.Object;
            var childContextProvider = Mock.Of<IGetsChildExpressionContexts>();
            var sut = new ExpressionContextIterativeProcessor(contextProcessor, childContextProvider);

            child1.ErrorHandlers.Clear();
            child2.ErrorHandlers.Clear();

            Mock.Get(childContextProvider)
                .Setup(x => x.GetChildContexts(context))
                .Returns(new[] { child1, child2 });

            Mock.Get(contextProcessor)
                .Setup(x => x.ProcessContextAsync(It.IsAny<ExpressionContext>(), CancellationToken.None))
                .Returns(Task.FromResult(ExpressionContextProcessingResult.Noop));

            await sut.IterateContextAndChildrenAsync(context);

            Assert.That(child1.ErrorHandlers,
                        Has.One.Matches<ErrorHandlingContext>(c => c.Handler == contextProcessor),
                        "First child context has context processor added");
            Assert.That(child2.ErrorHandlers,
                        Has.One.Matches<ErrorHandlingContext>(c => c.Handler == contextProcessor),
                        "Second child context has context processor added");
        }

        [Test, AutoMoqData]
        public async Task IterateContextAndChildrenAsync_should_not_add_processor_to_child_context_error_handler_stack_if_not_an_error_handler(ExpressionContext context,
                                                                                                                                               ExpressionContext child1,
                                                                                                                                               ExpressionContext child2)
        {
            var contextProcessorMock = new Mock<IProcessesExpressionContext>();
            var contextProcessor = contextProcessorMock.Object;
            var childContextProvider = Mock.Of<IGetsChildExpressionContexts>();
            var sut = new ExpressionContextIterativeProcessor(contextProcessor, childContextProvider);

            child1.ErrorHandlers.Clear();
            child2.ErrorHandlers.Clear();

            Mock.Get(childContextProvider)
                .Setup(x => x.GetChildContexts(context))
                .Returns(new[] { child1, child2 });

            Mock.Get(contextProcessor)
                .Setup(x => x.ProcessContextAsync(It.IsAny<ExpressionContext>(), CancellationToken.None))
                .Returns(Task.FromResult(ExpressionContextProcessingResult.Noop));

            await sut.IterateContextAndChildrenAsync(context);

            Assert.That(child1.ErrorHandlers, Has.None.SameAs(contextProcessor), "First child context does not have context processor added");
            Assert.That(child2.ErrorHandlers, Has.None.SameAs(contextProcessor), "Second child context does not have context processor added");
        }

        [Test, AutoMoqData]
        public async Task IterateContextAndChildrenAsync_uses_error_handler_from_stack_to_handle_processing_error([Frozen] IProcessesExpressionContext contextProcessor,
                                                                                                                  [Frozen] IGetsChildExpressionContexts childContextProvider,
                                                                                                                  ExpressionContextIterativeProcessor sut,
                                                                                                                  ExpressionContext context,
                                                                                                                  [Frozen] IHandlesProcessingError handler,
                                                                                                                  [Frozen] ExpressionContext handlerContext,
                                                                                                                  ErrorHandlingContext errorHandler)
        {
            var exception = new Exception("Sample exception");
            context.ErrorHandlers.Push(errorHandler);
            Mock.Get(handler)
                .Setup(x => x.HandleErrorAsync(exception, handlerContext, CancellationToken.None))
                .Returns(() => Task.FromResult(ErrorHandlingResult.Success(ExpressionContextProcessingResult.Noop)));
            Mock.Get(contextProcessor)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Throws(exception);

            await sut.IterateContextAndChildrenAsync(context);

            Mock.Get(handler)
                .Verify(x => x.HandleErrorAsync(exception, handlerContext, CancellationToken.None), Times.Once);
        }

        [Test, AutoMoqData]
        public void IterateContextAndChildrenAsync_throws_if_error_handler_from_stack_cannot_handle_processing_error([Frozen] IProcessesExpressionContext contextProcessor,
                                                                                                                     [Frozen] IGetsChildExpressionContexts childContextProvider,
                                                                                                                     ExpressionContextIterativeProcessor sut,
                                                                                                                     ExpressionContext context,
                                                                                                                     [Frozen] IHandlesProcessingError handler,
                                                                                                                     [Frozen] ExpressionContext handlerContext,
                                                                                                                     ErrorHandlingContext errorHandler)
        {
            var exception = new Exception("Sample exception");
            context.ErrorHandlers.Push(errorHandler);
            Mock.Get(handler)
                .Setup(x => x.HandleErrorAsync(exception, handlerContext, CancellationToken.None))
                .Returns(() => Task.FromResult(ErrorHandlingResult.Failure));
            Mock.Get(contextProcessor)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Throws(exception);

            Assert.That(async () => await sut.IterateContextAndChildrenAsync(context), Throws.Exception.Message.EqualTo("Sample exception"));
        }

        [Test, AutoMoqData]
        public void IterateContextAndChildrenAsync_throws_if_no_error_handlers_in_stack([Frozen] IProcessesExpressionContext contextProcessor,
                                                                                        [Frozen] IGetsChildExpressionContexts childContextProvider,
                                                                                        ExpressionContextIterativeProcessor sut,
                                                                                        ExpressionContext context)
        {
            var exception = new Exception("Sample exception");
            context.ErrorHandlers.Clear();
            Mock.Get(contextProcessor)
                .Setup(x => x.ProcessContextAsync(context, CancellationToken.None))
                .Throws(exception);

            Assert.That(async () => await sut.IterateContextAndChildrenAsync(context), Throws.Exception.Message.EqualTo("Sample exception"));
        }

    }
}
