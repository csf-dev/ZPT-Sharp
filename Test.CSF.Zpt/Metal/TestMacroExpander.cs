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
      new RenderingOptionsCustomisation().Customize(_fixture);

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
      var context = new RenderingContext(_fixture.Create<DummyModel>(),
                                         _fixture.Create<DummyModel>(),
                                         new Mock<ZptElement>().Object,
                                         _fixture.Create<RenderingOptions>());
      var attribute = Mock.Of<global::CSF.Zpt.Rendering.ZptAttribute>();
      ZptElement
      macro = Mock.Of<ZptElement>(x => x.GetAttribute(ZptConstants.Metal.Namespace,
                                                      ZptConstants.Metal.DefineMacroAttribute) == attribute
                                       && x.SearchChildrenByAttribute(It.IsAny<ZptNamespace>(),
                                                                      It.IsAny<string>()) == new ZptElement[0]);
      _finder.Setup(x => x.GetUsedMacro(context.Element, context.MetalModel)).Returns(macro);
      Mock.Get(context.Element)
        .Setup(x => x.SearchChildrenByAttribute(It.IsAny<ZptNamespace>(), It.IsAny<string>()))
        .Returns(new ZptElement[0]);
      Mock.Get(context.Element)
        .Setup(x => x.ReplaceWith(macro))
        .Returns(macro);

      // Act
      var result = _sut.Expand(context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreSame(macro, result.Element, "Correct result");
      Mock.Get(context.Element).Verify(x => x.ReplaceWith(macro), Times.Once());
    }

    [Test]
    public void TestExpandNoUsage()
    {
      // Arrange
      var context = new RenderingContext(_fixture.Create<DummyModel>(),
                                         _fixture.Create<DummyModel>(),
                                         new Mock<ZptElement>().Object,
                                         _fixture.Create<RenderingOptions>());
      _finder.Setup(x => x.GetUsedMacro(context.Element, context.MetalModel)).Returns((ZptElement) null);

      // Act
      var result = _sut.Expand(context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreSame(context, result, "Correct result");
      Mock.Get(context.Element).Verify(x => x.ReplaceWith(It.IsAny<ZptElement>()), Times.Never());
    }

    [Test]
    [Description("Doesn't test any slot-filling or extension functionality but checks that the basics work.")]
    public void TestExpandAndReplace()
    {
      // Arrange
      var context = new RenderingContext(_fixture.Create<DummyModel>(),
                                         _fixture.Create<DummyModel>(),
                                         new Mock<ZptElement>().Object,
                                         _fixture.Create<RenderingOptions>());
      var attribute = Mock.Of<global::CSF.Zpt.Rendering.ZptAttribute>();
      ZptElement
        macro = Mock.Of<ZptElement>(x => x.GetAttribute(ZptConstants.Metal.Namespace,
                                                        ZptConstants.Metal.DefineMacroAttribute) == attribute
                                         && x.SearchChildrenByAttribute(It.IsAny<ZptNamespace>(),
                                                                        It.IsAny<string>()) == new ZptElement[0]);
      Mock.Get(context.Element)
        .Setup(x => x.SearchChildrenByAttribute(It.IsAny<ZptNamespace>(), It.IsAny<string>()))
        .Returns(new ZptElement[0]);
      Mock.Get(context.Element)
        .Setup(x => x.ReplaceWith(macro))
        .Returns(macro);

      // Act
      var result = _sut.ExpandAndReplace(context, macro);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreSame(macro, result.Element, "Correct result");
      Mock.Get(context.Element).Verify(x => x.ReplaceWith(macro), Times.Once());
    }

    #endregion
  }
}

