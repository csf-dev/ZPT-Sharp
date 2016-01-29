using System;
using Test.CSF.Zpt.Rendering;
using CSF.Zpt.Rendering;
using NUnit.Framework;
using Moq;
using System.Linq;
using CSF.Zpt.Tal;
using CSF.Zpt;
using Ploeh.AutoFixture;
using Test.CSF.Zpt.Util.Autofixture;

namespace Test.CSF.Zpt.Tal
{
  [TestFixture]
  public class TestTalVisitor
  {
    #region tests

    [Test]
    [Description("This test ensures that all elements returned by handlers are processed")]
    public void TestVisit()
    {
      // Arrange
      var fixture = new Fixture();
      new RenderingContextCustomisation().Customize(fixture);

      var elements = Enumerable.Range(0,7).Select(x => new Mock<ZptElement>()).ToArray();
      var topElement = elements[0].Object;
      var secondElements  = elements.Skip(1).Take(2).ToArray();
      var thirdElements   = elements.Skip(3).Take(2);
      var fourthElements  = elements.Skip(5).Take(2);

      var handlers = Enumerable.Range(0, 2).Select(x => new Mock<IAttributeHandler>(MockBehavior.Strict) ).ToArray();

      handlers[0]
        .Setup(x => x.Handle(It.IsAny<ZptElement>(), It.IsAny<Model>()))
        .Returns(new AttributeHandlingResult(secondElements.Select(x => x.Object).ToArray(), true));
      handlers[1]
        .Setup(x => x.Handle(secondElements[0].Object, It.IsAny<Model>()))
        .Returns(new AttributeHandlingResult(thirdElements.Select(x => x.Object).ToArray(), true));
      handlers[1]
        .Setup(x => x.Handle(secondElements[1].Object, It.IsAny<Model>()))
        .Returns(new AttributeHandlingResult(fourthElements.Select(x => x.Object).ToArray(), true));

      var expectedElements = thirdElements.Select(x => x.Object).Union(fourthElements.Select(x => x.Object));

      var sut = new TalVisitor(handlers: handlers.Select(x => x.Object).ToArray(), 
                               errorHandler: Mock.Of<IAttributeHandler>());

      // Act
      var result = sut.Visit(topElement, fixture.Create<RenderingContext>(), fixture.Create<RenderingOptions>());

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.IsTrue(expectedElements.Intersect(result).Count() == expectedElements.Count(),
                    "All expected elements contained");
      Assert.IsTrue(result.Intersect(expectedElements).Count() == result.Count(),
                    "No unwanted elements contained");
    }

    [Test]
    [Description("This test ensures that if new elements are created (to be visited), that they are processed also")]
    public void TestVisit_NewlyExposedElements()
    {
      // Arrange
      var fixture = new Fixture();
      new RenderingContextCustomisation().Customize(fixture);

      var elements = Enumerable.Range(0,3).Select(x => new Mock<ZptElement>()).ToArray();
      ZptElement
        topElement = elements[0].Object,
        secondElement = elements[1].Object,
        thirdElement = elements[2].Object;

      var handler = new Mock<IAttributeHandler>(MockBehavior.Strict);

      handler
        .Setup(x => x.Handle(topElement, It.IsAny<Model>()))
        .Returns(new AttributeHandlingResult(new ZptElement[0], true, new [] { secondElement }));
      handler
        .Setup(x => x.Handle(secondElement, It.IsAny<Model>()))
        .Returns(new AttributeHandlingResult(new [] { thirdElement }, true));

      var expectedElements = new [] { thirdElement };

      var sut = new TalVisitor(handlers: new [] { handler.Object }, 
                               errorHandler: Mock.Of<IAttributeHandler>());

      // Act
      var result = sut.Visit(topElement, fixture.Create<RenderingContext>(), fixture.Create<RenderingOptions>());

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
    public void TestVisitRecursivelyWithError(int nestingLevel)
    {
      // Arrange
      var fixture = new Fixture();
      new RenderingContextCustomisation().Customize(fixture);

      var elements = Enumerable
        .Range(0, nestingLevel + 1)
        .Select(x => new Mock<ZptElement>())
        .ToArray();
      for(int i = 0; i < nestingLevel; i++)
      {
        elements[i].Setup(x => x.GetChildElements()).Returns(new [] { elements[i + 1].Object });
      }
      elements[0]
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace, ZptConstants.Tal.OnErrorAttribute))
        .Returns(Mock.Of<global::CSF.Zpt.Rendering.ZptAttribute>());

      var generalHandler = new Mock<IAttributeHandler>();
      generalHandler
        .Setup(x => x.Handle(It.IsAny<ZptElement>(), It.IsAny<Model>()))
        .Returns((ZptElement ele, Model mod) => new AttributeHandlingResult(new[] { ele }, true));
      generalHandler
        .Setup(x => x.Handle(elements[nestingLevel].Object, It.IsAny<Model>()))
        .Throws<RenderingException>();

      object errorObject = null;

      var errorHandler = new Mock<IAttributeHandler>();
      errorHandler
        .Setup(x => x.Handle(elements[0].Object, It.IsAny<Model>()))
        .Callback((ZptElement ele, Model mod) => {
          errorObject = mod.Error;
        });

      var sut = new TalVisitor(handlers: new [] { generalHandler.Object },
                               errorHandler: errorHandler.Object);

      // Act
      sut.VisitRecursively(elements[0].Object, fixture.Create<RenderingContext>(), fixture.Create<RenderingOptions>());

      // Assert
      errorHandler .Verify(x => x.Handle(elements[0].Object, It.IsAny<Model>()), Times.Once());
      Assert.NotNull(errorObject, "Error object nullability");
      Assert.IsInstanceOf<RenderingException>(errorObject, "Error object type");
    }

    #endregion
  }
}

