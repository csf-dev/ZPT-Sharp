using System;
using CSF.Zpt.Rendering;
using Moq;
using NUnit.Framework;
using CSF.Zpt;
using CSF.Zpt.Metal;
using Ploeh.AutoFixture;
using Test.CSF.Zpt.Util.Autofixture;
using Test.CSF.Zpt.Util;

namespace Test.CSF.Zpt.Metal
{
  [TestFixture]
  public class TestMacroFinder
  {
    #region fields

    private IFixture _fixture;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _fixture = new Fixture();
      new DummyModelCustomisation().Customize(_fixture);
    }

    #endregion

    #region tests

    [Test]
    public void TestGetUsedMacro()
    {
      // Arrange
      var attribute = Mock.Of<global::CSF.Zpt.Rendering.ZptAttribute>();
      ZptElement
      originalElement = Mock.Of<ZptElement>(x => x.GetAttribute(ZptConstants.Metal.Namespace,
                                                                ZptConstants.Metal.DefaultPrefix,
                                                                ZptConstants.Metal.UseMacroAttribute) == attribute),
        referencedElement = Mock.Of<ZptElement>();
      var model = _fixture.Create<DummyModel>();
      Mock.Get(model).Setup(x => x.Evaluate(It.IsAny<string>())).Returns(new ExpressionResult(true, referencedElement));

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
      var attribute = Mock.Of<global::CSF.Zpt.Rendering.ZptAttribute>();
      ZptElement
      originalElement = Mock.Of<ZptElement>(x => x.GetAttribute(ZptConstants.Metal.Namespace,
                                                                ZptConstants.Metal.DefaultPrefix,
                                                                ZptConstants.Metal.UseMacroAttribute) == attribute);
      var model = _fixture.Create<DummyModel>();
      Mock.Get(model).Setup(x => x.Evaluate(It.IsAny<string>())).Returns(new ExpressionResult(false, null));

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
      ZptElement
      originalElement = Mock.Of<ZptElement>(x => x.GetAttribute(ZptConstants.Metal.Namespace,
                                                                ZptConstants.Metal.DefaultPrefix,
                                                                ZptConstants.Metal.UseMacroAttribute) == (global::CSF.Zpt.Rendering.ZptAttribute) null);
      var model = _fixture.Create<DummyModel>();

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
      var attribute = Mock.Of<global::CSF.Zpt.Rendering.ZptAttribute>();
      ZptElement
      originalElement = Mock.Of<ZptElement>(x => x.GetAttribute(ZptConstants.Metal.Namespace,
                                                                ZptConstants.Metal.DefaultPrefix,
                                                                ZptConstants.Metal.ExtendMacroAttribute) == attribute),
        referencedElement = Mock.Of<ZptElement>();
      var model = _fixture.Create<DummyModel>();
      Mock.Get(model).Setup(x => x.Evaluate(It.IsAny<string>())).Returns(new ExpressionResult(true, referencedElement));

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
      var attribute = Mock.Of<global::CSF.Zpt.Rendering.ZptAttribute>();
      ZptElement
      originalElement = Mock.Of<ZptElement>(x => x.GetAttribute(ZptConstants.Metal.Namespace,
                                                                ZptConstants.Metal.DefaultPrefix,
                                                                ZptConstants.Metal.ExtendMacroAttribute) == attribute);
      var model = _fixture.Create<DummyModel>();
      Mock.Get(model).Setup(x => x.Evaluate(It.IsAny<string>())).Returns(new ExpressionResult(false, null));

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
      ZptElement
      originalElement = Mock.Of<ZptElement>(x => x.GetAttribute(ZptConstants.Metal.Namespace,
                                                                ZptConstants.Metal.DefaultPrefix,
                                                                ZptConstants.Metal.ExtendMacroAttribute) == (global::CSF.Zpt.Rendering.ZptAttribute) null);
      var model = _fixture.Create<DummyModel>();

      var sut = new MacroFinder();

      // Act
      var result = sut.GetExtendedMacro(originalElement, model);

      // Assert
      Assert.IsNull(result);
    }

    #endregion
  }
}

