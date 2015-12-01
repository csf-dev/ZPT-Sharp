using System;
using CSF.Zpt.Rendering;
using NUnit.Framework;
using Moq;
using System.Linq;

namespace Test.CSF.Zpt.Rendering
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
      var elements = Enumerable.Range(0,7).Select(x => new Mock<Element>()).ToArray();
      var topElement = elements[0].Object;
      var secondElements  = elements.Skip(1).Take(2).ToArray();
      var thirdElements   = elements.Skip(3).Take(2);
      var fourthElements  = elements.Skip(5).Take(2);

      var handlers = Enumerable.Range(0, 2).Select(x => new Mock<ITalAttributeHandler>()).ToArray();

      handlers[0]
        .Setup(x => x.Handle(It.IsAny<Element>(), It.IsAny<Model>()))
        .Returns(secondElements.Select(x => x.Object).ToArray());
      handlers[1]
        .Setup(x => x.Handle(secondElements[0].Object, It.IsAny<Model>()))
        .Returns(thirdElements.Select(x => x.Object).ToArray());
      handlers[1]
        .Setup(x => x.Handle(secondElements[1].Object, It.IsAny<Model>()))
        .Returns(fourthElements.Select(x => x.Object).ToArray());

      var expectedElements = thirdElements.Select(x => x.Object).Union(fourthElements.Select(x => x.Object));

      var sut = new TalVisitor(handlers: handlers.Select(x => x.Object).ToArray(), 
                               errorHandler: Mock.Of<ITalAttributeHandler>());

      // Act
      var result = sut.Visit(topElement, Mock.Of<DummyModel>());

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
      var elements = Enumerable
        .Range(0, nestingLevel + 1)
        .Select(x => new Mock<Element>())
        .ToArray();
      for(int i = 0; i < nestingLevel; i++)
      {
        elements[i].Setup(x => x.GetChildElements()).Returns(new [] { elements[i + 1].Object });
      }
      elements[0]
        .Setup(x => x.GetAttribute(Tal.Namespace, Tal.DefaultPrefix, Tal.OnErrorAttribute))
        .Returns(Mock.Of<global::CSF.Zpt.Rendering.Attribute>());

      var generalHandler = new Mock<ITalAttributeHandler>();
      generalHandler
        .Setup(x => x.Handle(It.IsAny<Element>(), It.IsAny<Model>()))
        .Returns((Element ele, Model mod) => new[] { ele });
      generalHandler
        .Setup(x => x.Handle(elements[nestingLevel].Object, It.IsAny<Model>()))
        .Throws<RenderingException>();

      object errorObject = null;

      var errorHandler = new Mock<ITalAttributeHandler>();
      errorHandler
        .Setup(x => x.Handle(elements[0].Object, It.IsAny<Model>()))
        .Callback((Element ele, Model mod) => {
          errorObject = mod.Error;
        });

      var sut = new TalVisitor(handlers: new [] { generalHandler.Object },
                               errorHandler: errorHandler.Object);

      // Act
      sut.VisitRecursively(elements[0].Object, new Mock<DummyModel>() { CallBase = true }.Object);

      // Assert
      errorHandler .Verify(x => x.Handle(elements[0].Object, It.IsAny<Model>()), Times.Once());
      Assert.NotNull(errorObject, "Error object nullability");
      Assert.IsInstanceOf<RenderingException>(errorObject, "Error object type");
    }

    #endregion
  }
}

