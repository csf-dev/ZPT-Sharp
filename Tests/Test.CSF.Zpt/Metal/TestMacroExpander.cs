using System;
using NUnit.Framework;
using Moq;
using CSF.Zpt.Rendering;
using CSF.Zpt.Metal;
using Ploeh.AutoFixture;
using CSF.Zpt.TestUtils.Autofixture;
using CSF.Zpt;
using CSF.Zpt.TestUtils;

namespace Test.CSF.Zpt.Metal
{
  [TestFixture]
  public class TestMacroExpander
  {
    #region fields

    private IFixture _fixture;

    private Mock<MacroFinder> _finder;
    private MacroExpander _sut;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _fixture = new Fixture();
      new RenderingContextCustomisation().Customize(_fixture);

      _finder = new Mock<MacroFinder>();
      _sut = new MacroExpander(_finder.Object);
    }

    #endregion

    #region tests

    [Test]
    public void Expand_returns_context_containing_replacement_element()
    {
      // Arrange
      var context = _fixture.Create<RenderingContext>();
      var attribute = Mock.Of<global::CSF.Zpt.Rendering.ZptAttribute>();
      ZptElement
        macro = Mock.Of<ZptElement>(x => x.GetAttribute(ZptConstants.Metal.Namespace,
                                                        ZptConstants.Metal.DefineMacroAttribute) == attribute
                                         && x.SearchChildrenByAttribute(It.IsAny<ZptNamespace>(),
                                                                        It.IsAny<string>()) == new ZptElement[0]);
      _finder.Setup(x => x.GetUsedMacro(context)).Returns(macro);
      Mock.Get(context.Element)
        .Setup(x => x.SearchChildrenByAttribute(It.IsAny<ZptNamespace>(), It.IsAny<string>()))
        .Returns(new ZptElement[0]);
      Mock.Get(context.Element)
        .Setup(x => x.ReplaceWith(macro))
        .Returns(macro);

      // Act
      var result = _sut.ExpandMacros(context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreSame(macro, result.Element, "Correct result");
    }

    [Test]
    public void Expand_replaces_original_element_with_replacement_in_DOM()
    {
      // Arrange
      var context = _fixture.Create<RenderingContext>();
      var attribute = Mock.Of<global::CSF.Zpt.Rendering.ZptAttribute>();
      ZptElement
      macro = Mock.Of<ZptElement>(x => x.GetAttribute(ZptConstants.Metal.Namespace,
                                                        ZptConstants.Metal.DefineMacroAttribute) == attribute
                                    && x.SearchChildrenByAttribute(It.IsAny<ZptNamespace>(),
                                                                        It.IsAny<string>()) == new ZptElement[0]);
      _finder.Setup(x => x.GetUsedMacro(context)).Returns(macro);
      Mock.Get(context.Element)
        .Setup(x => x.SearchChildrenByAttribute(It.IsAny<ZptNamespace>(), It.IsAny<string>()))
        .Returns(new ZptElement[0]);
      Mock.Get(context.Element)
        .Setup(x => x.ReplaceWith(macro))
        .Returns(macro);

      // Act
      _sut.ExpandMacros(context);

      // Assert
      Mock.Get(context.Element).Verify(x => x.ReplaceWith(macro), Times.Once());
    }

    [Test]
    public void Expand_makes_no_changes_when_attribute_not_present()
    {
      // Arrange
      var context = _fixture.Create<RenderingContext>();
      _finder.Setup(x => x.GetUsedMacro(context)).Returns((ZptElement) null);

      // Act
      var result = _sut.ExpandMacros(context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreSame(context, result, "Correct result");
      Mock.Get(context.Element).Verify(x => x.ReplaceWith(It.IsAny<ZptElement>()), Times.Never());
    }

    #endregion
  }
}

