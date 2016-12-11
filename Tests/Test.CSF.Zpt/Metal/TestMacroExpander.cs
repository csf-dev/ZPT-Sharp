using System;
using NUnit.Framework;
using Moq;
using CSF.Zpt.Rendering;
using CSF.Zpt.Metal;
using Ploeh.AutoFixture;
using CSF.Zpt.TestUtils.Autofixture;
using CSF.Zpt;
using CSF.Zpt.TestUtils;
using System.Collections.Generic;

namespace Test.CSF.Zpt.Metal
{
  [TestFixture]
  public class TestMacroExpander
  {
    #region fields

    private IFixture _fixture;

    private Mock<IMacroFinder> _finder;
    private Mock<IMacroSubstituter> _substituter;

    private MacroExpander _sut;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _fixture = new Fixture();
      new RenderingContextCustomisation().Customize(_fixture);

      _finder = new Mock<IMacroFinder>();
      _substituter = new Mock<IMacroSubstituter>();

      _sut = new MacroExpander(_finder.Object, _substituter.Object);
    }

    #endregion

    #region tests

    [Test]
    public void GetUsedMacro_uses_macro_finder()
    {
      // Arrange
      var context = _fixture.Create<IRenderingContext>();
      var element = Mock.Of<IZptElement>();
      _finder.Setup(x => x.GetUsedMacro(context)).Returns(element);

      // Act
      var result = _sut.GetUsedMacro(context);

      // Assert
      Assert.AreSame(element, result);
      _finder.Verify(x => x.GetUsedMacro(context), Times.Once());
    }

    [Test]
    public void HandleNoUsedMacro_does_not_change_context()
    {
      // Arrange
      var context = _fixture.Create<IRenderingContext>();

      // Act
      var result = _sut.HandleNoUsedMacro(context);

      // Assert
      Assert.AreSame(context, result);
    }

    [Test]
    public void GetExtendedMacro_gets_extended_macro_when_it_exists()
    {
      // Arrange
      var context = Mock.Of<IRenderingContext>();
      var extended = Mock.Of<IZptElement>();
      _finder.Setup(x => x.GetExtendedMacro(context)).Returns(extended);

      // Act
      var result = _sut.GetExtendedMacro(context);

      // Assert
      Assert.AreSame(extended, result);
    }

    [Test]
    public void GetExtendedMacro_gets_null_when_no_extended_macro_exists()
    {
      // Arrange
      var context = Mock.Of<IRenderingContext>();
      _finder.Setup(x => x.GetExtendedMacro(context)).Returns((IZptElement) null);

      // Act
      var result = _sut.GetExtendedMacro(context);

      // Assert
      Assert.IsNull(result);
    }

    [Test]
    public void GetFullyExtendedContext_returns_same_context_if_no_extended_macro()
    {
      // Arrange
      IRenderingContext macro = Mock.Of<IRenderingContext>(), sibling = Mock.Of<IRenderingContext>();
      Mock.Get(macro)
        .Setup(x => x.CreateSiblingContext(It.IsAny<ZptElement>(), It.IsAny<bool>()))
        .Returns(sibling);

      _finder.Setup(x => x.GetExtendedMacro(macro)).Returns((IZptElement) null);

      IList<IRenderingContext> stack = new List<IRenderingContext>();

      // Act
      var result = _sut.GetFullyExtendedContext(macro, ref stack);

      // Assert
      Assert.AreSame(macro, result);
    }

    [Test]
    public void GetFullyExtendedContext_uses_substitutor_service_when_extended_macro_is_present()
    {
      // Arrange
      IRenderingContext
        macro = Mock.Of<IRenderingContext>(),
        sibling = Mock.Of<IRenderingContext>(),
        substituted = Mock.Of<IRenderingContext>();
      Mock.Get(macro)
        .Setup(x => x.CreateSiblingContext(It.IsAny<IZptElement>(), It.IsAny<bool>()))
        .Returns(sibling);

      IList<IRenderingContext> stack = new List<IRenderingContext>();

      _finder.Setup(x => x.GetExtendedMacro(macro)).Returns(Mock.Of<IZptElement>());
      _substituter.Setup(x => x.MakeSubstitutions(macro, sibling, stack, true)).Returns(substituted);

      // Act
      _sut.GetFullyExtendedContext(macro, ref stack);

      // Assert
      _substituter.Verify(x => x.MakeSubstitutions(macro, sibling, stack, true), Times.Once());
    }

    [Test]
    public void GetFullyExtendedContext_returns_context_from_extender_service_when_extended_macro_is_present()
    {
      // Arrange
      IRenderingContext
        macro = Mock.Of<IRenderingContext>(),
        sibling = Mock.Of<IRenderingContext>(),
        substituted = Mock.Of<IRenderingContext>();
      Mock.Get(macro)
        .Setup(x => x.CreateSiblingContext(It.IsAny<IZptElement>(), It.IsAny<bool>()))
        .Returns(sibling);

      IList<IRenderingContext> stack = new List<IRenderingContext>();

      _finder.Setup(x => x.GetExtendedMacro(macro)).Returns(Mock.Of<IZptElement>());
      _substituter.Setup(x => x.MakeSubstitutions(macro, sibling, stack, true)).Returns(substituted);

      // Act
      var result = _sut.GetFullyExtendedContext(macro, ref stack);

      // Assert
      Assert.AreSame(substituted, result);
    }

    #endregion
  }
}

