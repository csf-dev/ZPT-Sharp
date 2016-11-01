using System;
using CSF.Zpt.Rendering;
using Moq;
using NUnit.Framework;
using CSF.Zpt;
using CSF.Zpt.Metal;
using Ploeh.AutoFixture;
using CSF.Zpt.TestUtils.Autofixture;
using CSF.Zpt.TestUtils;

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
      new RenderingContextCustomisation().Customize(_fixture);
    }

    #endregion

    #region tests

    [Test]
    public void GetUsedMacro_returns_correct_referenced_element()
    {
      // Arrange
      var attribute = Mock.Of<global::CSF.Zpt.Rendering.ZptAttribute>();
      var referencedElement = Mock.Of<ZptElement>(x => x.Clone() == x);

      var model = _fixture.Create<DummyModel>();
      _fixture.Inject(model);

      var context = _fixture.Create<RenderingContext>();
      Mock.Get(context)
        .Setup(x => x.GetAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.UseMacroAttribute))
        .Returns(attribute);
      Mock.Get(model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), context))
        .Returns(new ExpressionResult(referencedElement));

      var sut = new MacroFinder();

      // Act
      var result = sut.GetUsedMacro(context);

      // Assert
      Assert.AreSame(referencedElement, result);
    }

    [Test]
    public void GetUsedMacro_returns_null_for_evaluation_failure()
    {
      // Arrange
      var attribute = Mock.Of<global::CSF.Zpt.Rendering.ZptAttribute>();
      ZptElement
        originalElement = Mock.Of<ZptElement>(x => x.GetAttribute(ZptConstants.Metal.Namespace,
                                                                  ZptConstants.Metal.UseMacroAttribute) == attribute);

      _fixture.Inject(originalElement);

      var model = _fixture.Create<DummyModel>();
      var context = _fixture.Create<RenderingContext>();
      Mock.Get(model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), context))
        .Throws<IntendedTestingException>();

      var sut = new MacroFinder();

      // Act
      var result = sut.GetUsedMacro(context);

      // Assert
      Assert.IsNull(result);
    }

    [Test]
    public void GetUsedMacro_returns_null_when_attribute_not_present()
    {
      // Arrange
      var mockOriginalElement = new Mock<ZptElement>();
      mockOriginalElement
        .Setup(x => x.GetAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.UseMacroAttribute))
        .Returns((global::CSF.Zpt.Rendering.ZptAttribute)null);
      
      ZptElement originalElement = mockOriginalElement.Object;

      var context = _fixture.Create<RenderingContext>();

      var sut = new MacroFinder();

      // Act
      var result = sut.GetUsedMacro(context);

      // Assert
      Assert.IsNull(result);
    }

    [Test]
    public void GetExtendedMacro_returns_correct_referenced_element()
    {
      // Arrange
      var attribute = Mock.Of<global::CSF.Zpt.Rendering.ZptAttribute>();
      var referencedElement = Mock.Of<ZptElement>(x => x.Clone() == x);

      var model = _fixture.Create<DummyModel>();
      _fixture.Inject(model);

      var context = _fixture.Create<RenderingContext>();
      Mock.Get(context)
        .Setup(x => x.GetAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.ExtendMacroAttribute))
        .Returns(attribute);
      Mock.Get(model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), context))
        .Returns(new ExpressionResult(referencedElement));

      var sut = new MacroFinder();

      // Act
      var result = sut.GetExtendedMacro(context);

      // Assert
      Assert.AreSame(referencedElement, result);
    }

    [Test]
    public void GetExtendedMacro_returns_null_for_evaluation_failure()
    {
      // Arrange
      var attribute = Mock.Of<global::CSF.Zpt.Rendering.ZptAttribute>();
      ZptElement
        originalElement = Mock.Of<ZptElement>(x => x.GetAttribute(ZptConstants.Metal.Namespace,
                                                                  ZptConstants.Metal.ExtendMacroAttribute) == attribute);

      _fixture.Inject(originalElement);
      var model = _fixture.Create<DummyModel>();
      _fixture.Inject(model);

      var context = _fixture.Create<RenderingContext>();
      Mock.Get(model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), context))
        .Throws<IntendedTestingException>();

      var sut = new MacroFinder();

      // Act
      var result = sut.GetExtendedMacro(context);

      // Assert
      Assert.IsNull(result);
    }

    [Test]
    public void GetExtendedMacro_returns_null_when_attribute_not_present()
    {
      // Arrange
      var mockOriginalElement = new Mock<ZptElement>();
      mockOriginalElement
        .Setup(x => x.GetAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.ExtendMacroAttribute))
        .Returns((global::CSF.Zpt.Rendering.ZptAttribute) null);
      
      ZptElement originalElement = mockOriginalElement.Object;
      _fixture.Inject(originalElement);

      var context = _fixture.Create<RenderingContext>();

      var sut = new MacroFinder();

      // Act
      var result = sut.GetExtendedMacro(context);

      // Assert
      Assert.IsNull(result);
    }

    #endregion
  }
}

