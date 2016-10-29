using System;
using CSF.Zpt.Metal;
using NUnit.Framework;
using Moq;
using CSF.Zpt.Rendering;
using CSF.Zpt;

namespace Test.CSF.Zpt.Metal
{
  [TestFixture]
  public class TestMetalVisitor
  {
    #region fields

    private MetalVisitor _sut;
    private Mock<MacroExpander> _expander;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _expander = new Mock<MacroExpander>() {
        CallBase = false
      };
      _sut = new MetalVisitor(_expander.Object);
    }

    #endregion

    #region tests

    [Test]
    public void Visit_uses_expander()
    {
      // Arrange
      var context = Mock.Of<IRenderingContext>();

      // Act
      _sut.Visit(context);

      // Assert
      _expander.Verify(x => x.Expand(context), Times.Once());
    }

    [Test]
    public void Visit_returns_context_from_expander()
    {
      // Arrange
      IRenderingContext contextOne = Mock.Of<IRenderingContext>(), contextTwo = Mock.Of<IRenderingContext>();
      _expander.Setup(x => x.Expand(contextOne)).Returns(contextTwo);

      // Act
      var result = _sut.Visit(contextOne);

      // Assert
      Assert.AreEqual(1, result.Length, "Count of result contexts");
      Assert.AreSame(contextTwo, result[0], "Expected context");
    }

    [Test]
    public void Visit_adds_macro_definition_where_present()
    {
      // Arrange
      var context = new Mock<IRenderingContext>() {
        DefaultValue = DefaultValue.Mock,
      };
      var attrib = Mock.Of<IZptAttribute>(x => x.Name == ZptConstants.Metal.DefineMacroAttribute && x.Value == "foo");
      context
        .Setup(x => x.GetAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.DefineMacroAttribute))
        .Returns(attrib);
      var model = Mock.Get(context.Object.MetalModel);

      // Act
      _sut.Visit(context.Object);

      // Assert
      model.Verify(x => x.AddGlobal("foo", context.Object.Element), Times.Once());
    }

    [Test]
    public void Visit_does_not_change_model_where_no_definition_present()
    {
      // Arrange
      var context = new Mock<IRenderingContext>() {
        DefaultValue = DefaultValue.Mock,
      };
      context
        .Setup(x => x.GetAttribute(ZptConstants.Metal.Namespace, ZptConstants.Metal.DefineMacroAttribute))
        .Returns((IZptAttribute) null);
      var model = Mock.Get(context.Object.MetalModel);

      // Act
      _sut.Visit(context.Object);

      // Assert
      model.Verify(x => x.AddGlobal(It.IsAny<string>(), It.IsAny<object>()), Times.Never());
    }

    #endregion
  }
}

