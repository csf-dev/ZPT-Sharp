using System;
using NUnit.Framework;
using Moq;
using CSF.Zpt.Rendering;
using CSF.Zpt.Metal;
using Ploeh.AutoFixture;
using Test.CSF.Zpt.Util.Autofixture;
using CSF.Zpt;
using Test.CSF.Zpt.Util;

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
      new DummyModelCustomisation().Customize(_fixture);

      _finder = new Mock<MacroFinder>();
      _sut = new MacroExpander(_finder.Object);
    }

    #endregion

    #region tests

    [Test]
    [Description("Doesn't test any slot-filling or extension functionality but checks that the basics work.")]
    public void TestExpand()
    {
      // Arrange
      var model = _fixture.Create<DummyModel>();
      var attribute = Mock.Of<global::CSF.Zpt.Rendering.ZptAttribute>();
      var original = new Mock<ZptElement>();
      ZptElement
      macro = Mock.Of<ZptElement>(x => x.GetAttribute(ZptConstants.Metal.Namespace,
                                                      ZptConstants.Metal.DefineMacroAttribute) == attribute
                                       && x.SearchChildrenByAttribute(It.IsAny<ZptNamespace>(),
                                                                      It.IsAny<string>()) == new ZptElement[0]);
      _finder.Setup(x => x.GetUsedMacro(original.Object, model)).Returns(macro);
      original
        .Setup(x => x.SearchChildrenByAttribute(It.IsAny<ZptNamespace>(), It.IsAny<string>()))
        .Returns(new ZptElement[0]);
      original
        .Setup(x => x.ReplaceWith(macro))
        .Returns(macro);

      // Act
      var result = _sut.Expand(original.Object, model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreSame(macro, result, "Correct result");
      original.Verify(x => x.ReplaceWith(macro), Times.Once());
    }

    [Test]
    public void TestExpandNoUsage()
    {
      // Arrange
      var model = _fixture.Create<DummyModel>();
      var original = new Mock<ZptElement>();
      _finder.Setup(x => x.GetUsedMacro(original.Object, model)).Returns((ZptElement) null);

      // Act
      var result = _sut.Expand(original.Object, model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreSame(original.Object, result, "Correct result");
      original.Verify(x => x.ReplaceWith(It.IsAny<ZptElement>()), Times.Never());
    }

    [Test]
    [Description("Doesn't test any slot-filling or extension functionality but checks that the basics work.")]
    public void TestExpandAndReplace()
    {
      // Arrange
      var model = _fixture.Create<DummyModel>();
      var attribute = Mock.Of<global::CSF.Zpt.Rendering.ZptAttribute>();
      var original = new Mock<ZptElement>();
      ZptElement
        macro = Mock.Of<ZptElement>(x => x.GetAttribute(ZptConstants.Metal.Namespace,
                                                        ZptConstants.Metal.DefineMacroAttribute) == attribute
                                         && x.SearchChildrenByAttribute(It.IsAny<ZptNamespace>(),
                                                                        It.IsAny<string>()) == new ZptElement[0]);
      original
        .Setup(x => x.SearchChildrenByAttribute(It.IsAny<ZptNamespace>(), It.IsAny<string>()))
        .Returns(new ZptElement[0]);
      original
        .Setup(x => x.ReplaceWith(macro))
        .Returns(macro);

      // Act
      var result = _sut.ExpandAndReplace(original.Object, macro, model);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreSame(macro, result, "Correct result");
      original.Verify(x => x.ReplaceWith(macro), Times.Once());
    }

    #endregion
  }
}

