using System;
using Test.CSF.Zpt.Rendering;
using CSF.Zpt.Rendering;
using NUnit.Framework;
using Moq;
using System.Linq;
using CSF.Zpt.Tal;
using CSF.Zpt;
using Ploeh.AutoFixture;
using CSF.Zpt.TestUtils.Autofixture;
using CSF.Zpt.TestUtils;

namespace Test.CSF.Zpt.Tal
{
  [TestFixture]
  public class TestTalVisitor
  {
    #region fields

    private log4net.ILog _logger;

    #endregion

    #region setup

    [TestFixtureSetUp]
    public void FixtureSetup()
    {
      _logger = log4net.LogManager.GetLogger(this.GetType());
    }

    #endregion

    #region tests

    [Test]
    public void Visit_applies_handlers_to_every_context_in_a_context_hierarchy()
    {
      // Arrange
      var fixture = new Fixture();
      new RenderingContextCustomisation().Customize(fixture);

      var contexts = Enumerable.Range(0,7).Select(x => fixture.Create<RenderingContext>()).ToArray();
      var topContext = contexts[0];
      RenderingContext[]
        secondContexts  = contexts.Skip(1).Take(2).ToArray(),
        thirdContexts   = contexts.Skip(3).Take(2).ToArray(),
        fourthContexts  = contexts.Skip(5).Take(2).ToArray();

      var handlers = Enumerable.Range(0, 2).Select(x => new Mock<IAttributeHandler>(MockBehavior.Strict)).ToArray();

      handlers[0]
        .Setup(x => x.Handle(topContext))
        .Returns((RenderingContext ctx) => new AttributeHandlingResult(secondContexts, true));
      handlers[1]
        .Setup(x => x.Handle(secondContexts[0]))
        .Returns((RenderingContext ctx) => new AttributeHandlingResult(thirdContexts, true));
      handlers[1]
        .Setup(x => x.Handle(secondContexts[1]))
        .Returns((RenderingContext ctx) => new AttributeHandlingResult(fourthContexts, true));

      var expectedContexts = thirdContexts.Union(fourthContexts);

      var sut = new TalVisitor(handlers: handlers.Select(x => x.Object).ToArray(), 
                               errorHandler: Mock.Of<IAttributeHandler>());

      // Act
      var result = sut.Visit(topContext);

      // Assert
      Assert.NotNull(result, "Result nullability");
      try
      {
        CollectionAssert.AreEquivalent(expectedContexts, result, "All expected contexts returned");
      }
      catch(AssertionException)
      {
        _logger.ErrorFormat("{0} contexts returned", result.Count());
        throw;
      }
    }

    [Test]
    [Description("This test ensures that if new contexts are created (to be visited), that they are processed also")]
    public void Visit_visits_newly_exposes_contexts()
    {
      // Arrange
      var fixture = new Fixture();
      new RenderingContextCustomisation().Customize(fixture);

      var contexts = Enumerable.Range(0,3).Select(x => fixture.Create<RenderingContext>()).ToArray();
      RenderingContext
        topContext = contexts[0],
        secondContext = contexts[1],
        thirdContext = contexts[2];

      var handler = new Mock<IAttributeHandler>(MockBehavior.Strict);

      handler
        .Setup(x => x.Handle(topContext))
        .Returns(new AttributeHandlingResult(new RenderingContext[0], true, new [] { secondContext }));
      handler
        .Setup(x => x.Handle(secondContext))
        .Returns(new AttributeHandlingResult(new [] { thirdContext }, true));

      var expectedElements = new [] { thirdContext };

      var sut = new TalVisitor(handlers: new [] { handler.Object }, 
                               errorHandler: Mock.Of<IAttributeHandler>());

      // Act
      var result = sut.Visit(topContext);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.IsTrue(expectedElements.Intersect(result).Count() == expectedElements.Count(),
                    "All expected elements contained");
      Assert.IsTrue(result.Intersect(expectedElements).Count() == result.Count(),
                    "No unwanted elements contained");
    }

    [TestCase(0)]
    [TestCase(2)]
    [TestCase(5)]
    public void VisitRecursively_uses_error_handler_when_present_and_exposes_error_object(int nestingLevel)
    {
      // Arrange
      var fixture = new Fixture();
      new RenderingContextCustomisation().Customize(fixture);
      new DummyModelCustomisation().Customize(fixture);

      var contexts = Enumerable
        .Range(0, nestingLevel + 1)
        .Select(x => fixture.Create<IRenderingContext>())
        .ToArray();
      for(int i = 0; i < nestingLevel; i++)
      {
        Mock.Get(contexts[i]).Setup(x => x.GetChildContexts()).Returns(new [] { contexts[i + 1] });
      }
      Mock.Get(contexts[nestingLevel])
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace, ZptConstants.Tal.OnErrorAttribute))
        .Returns(Mock.Of<ZptAttribute>());

      var generalHandler = new Mock<IAttributeHandler>();
      generalHandler
        .Setup(x => x.Handle(It.IsAny<IRenderingContext>()))
        .Returns((IRenderingContext ctx) => new AttributeHandlingResult(new[] { ctx }, true));
      generalHandler
        .Setup(x => x.Handle(contexts[nestingLevel]))
        .Throws<RenderingException>();

      object errorObject = null;

      var errorHandler = new Mock<IAttributeHandler>();
      errorHandler
        .Setup(x => x.Handle(contexts[nestingLevel]))
        .Callback((IRenderingContext ele) => {
          errorObject = ele.TalModel.Error;
        });

      var sut = new TalVisitor(handlers: new [] { generalHandler.Object },
                               errorHandler: errorHandler.Object);

      // Act
      sut.VisitRecursively(contexts[0]);

      // Assert
      errorHandler.Verify(x => x.Handle(contexts[nestingLevel]), Times.Once());
      Assert.NotNull(errorObject, "Error object nullability");
      Assert.IsInstanceOf<RenderingException>(errorObject, "Error object type");
    }

    #endregion
  }
}
