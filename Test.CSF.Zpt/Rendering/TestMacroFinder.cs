using System;
using CSF.Zpt.Rendering;
using CSF.Zpt.Tales;
using Moq;
using NUnit.Framework;

namespace Test.CSF.Zpt.Rendering
{
  [TestFixture]
  public class TestMacroFinder
  {
    #region tests

    [Test]
    public void TestGetUsedMacro()
    {
      // Arrange
      var attribute = Mock.Of<global::CSF.Zpt.Rendering.Attribute>();
      Element
        originalElement = Mock.Of<Element>(x => x.GetAttribute(Metal.Namespace,
                                                               Metal.DefaultPrefix,
                                                               Metal.UseMacroAttribute) == attribute),
        referencedElement = Mock.Of<Element>();
      var model = Mock.Of<Model>(x => x.Evaluate(It.IsAny<string>()) == new ExpressionResult(true, referencedElement));

      var sut = new MacroFinder();

      // Act
      var result = sut.GetUsedMacro(originalElement, model);

      // Assert
      Assert.AreSame(referencedElement, result);
    }

    [Test]
    public void TestGetUsedMacroEvaluationFailure()
    {
      // Arrange
      var attribute = Mock.Of<global::CSF.Zpt.Rendering.Attribute>();
      Element
        originalElement = Mock.Of<Element>(x => x.GetAttribute(Metal.Namespace,
                                                               Metal.DefaultPrefix,
                                                               Metal.UseMacroAttribute) == attribute);
      var model = Mock.Of<Model>(x => x.Evaluate(It.IsAny<string>()) == new ExpressionResult(false, null));

      var sut = new MacroFinder();

      // Act
      var result = sut.GetUsedMacro(originalElement, model);

      // Assert
      Assert.IsNull(result);
    }

    [Test]
    public void TestGetUsedMacroNoAttribute()
    {
      // Arrange
      Element
        originalElement = Mock.Of<Element>(x => x.GetAttribute(Metal.Namespace,
                                                               Metal.DefaultPrefix,
                                                               Metal.UseMacroAttribute) == (global::CSF.Zpt.Rendering.Attribute) null);
      var model = Mock.Of<Model>();

      var sut = new MacroFinder();

      // Act
      var result = sut.GetUsedMacro(originalElement, model);

      // Assert
      Assert.IsNull(result);
    }

    [Test]
    public void TestGetExtendedMacro()
    {
      // Arrange
      var attribute = Mock.Of<global::CSF.Zpt.Rendering.Attribute>();
      Element
        originalElement = Mock.Of<Element>(x => x.GetAttribute(Metal.Namespace,
                                                               Metal.DefaultPrefix,
                                                               Metal.ExtendMacroAttribute) == attribute),
        referencedElement = Mock.Of<Element>();
      var model = Mock.Of<Model>(x => x.Evaluate(It.IsAny<string>()) == new ExpressionResult(true, referencedElement));

      var sut = new MacroFinder();

      // Act
      var result = sut.GetExtendedMacro(originalElement, model);

      // Assert
      Assert.AreSame(referencedElement, result);
    }

    [Test]
    public void TestGetExtendedMacroEvaluationFailure()
    {
      // Arrange
      var attribute = Mock.Of<global::CSF.Zpt.Rendering.Attribute>();
      Element
        originalElement = Mock.Of<Element>(x => x.GetAttribute(Metal.Namespace,
                                                               Metal.DefaultPrefix,
                                                               Metal.ExtendMacroAttribute) == attribute);
      var model = Mock.Of<Model>(x => x.Evaluate(It.IsAny<string>()) == new ExpressionResult(false, null));

      var sut = new MacroFinder();

      // Act
      var result = sut.GetExtendedMacro(originalElement, model);

      // Assert
      Assert.IsNull(result);
    }

    [Test]
    public void TestGetExtendedMacroNoAttribute()
    {
      // Arrange
      Element
        originalElement = Mock.Of<Element>(x => x.GetAttribute(Metal.Namespace,
                                                               Metal.DefaultPrefix,
                                                               Metal.ExtendMacroAttribute) == (global::CSF.Zpt.Rendering.Attribute) null);
      var model = Mock.Of<Model>();

      var sut = new MacroFinder();

      // Act
      var result = sut.GetExtendedMacro(originalElement, model);

      // Assert
      Assert.IsNull(result);
    }

    #endregion
  }
}

