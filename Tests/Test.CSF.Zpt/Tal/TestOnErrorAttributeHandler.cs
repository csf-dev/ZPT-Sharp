using System;
using NUnit.Framework;
using CSF.Zpt.Tal;
using CSF.Zpt.Rendering;
using Ploeh.AutoFixture;
using Moq;
using CSF.Zpt.TestUtils;
using CSF.Zpt.TestUtils.Autofixture;
using CSF.Zpt;

namespace Test.CSF.Zpt.Tal
{
  [TestFixture]
  public class TestOnErrorAttributeHandler
  {
    #region fields

    private IFixture _autofixture;
    private Mock<ZptElement> _element;
    private RenderingContext _context;
    private DummyModel _model;

    private OnErrorAttributeHandler _sut;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _autofixture = new Fixture();
      new DummyModelCustomisation().Customize(_autofixture);

      _model = _autofixture.Create<DummyModel>();
      _element = new Mock<ZptElement>() { CallBase = true };
      _context = Mock.Of<RenderingContext>(x => x.Element == _element.Object && x.TalModel == _model);

      _element.Setup(x => x.Remove());
      _element.Setup(x => x.RemoveAllChildren());
      _element.Setup(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>())).Returns(new ZptElement[0]);
      _element.Setup(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()));

      _sut = new OnErrorAttributeHandler();
    }

    #endregion

    #region no attribute

    [Test]
    public void Handle_takes_no_action_when_attribute_not_present()
    {
      // Arrange
      _element
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OnErrorAttribute))
        .Returns((ZptAttribute) null);

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Contexts.Length, "Count of results");
      Assert.AreSame(_context, result.Contexts[0], "Correct element returned");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Never());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
    }

    #endregion

    #region cancels action

    [Test]
    public void Handle_takes_no_action_when_expression_cancels_action()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OnErrorAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(ZptConstants.CancellationToken));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Contexts.Length, "Count of results");
      Assert.AreSame(_context, result.Contexts[0], "Correct element returned");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Never());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
    }

    [Test]
    public void Handle_takes_no_action_when_text_based_expression_cancels_action()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OnErrorAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "text bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(ZptConstants.CancellationToken));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Contexts.Length, "Count of results");
      Assert.AreSame(_context, result.Contexts[0], "Correct element returned");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Never());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
    }

    [Test]
    public void Handle_takes_no_action_when_structure_based_expression_cancels_action()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OnErrorAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "structure bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(ZptConstants.CancellationToken));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Contexts.Length, "Count of results");
      Assert.AreSame(_context, result.Contexts[0], "Correct element returned");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Never());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
    }

    #endregion

    #region attribute evaluates null

    [Test]
    public void Handle_removes_all_children_when_expression_is_null()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OnErrorAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(null));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Contexts.Length, "Count of results");
      Assert.AreSame(_context, result.Contexts[0], "Correct element returned");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Once());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
    }

    [Test]
    public void Handle_removes_all_children_when_text_expression_is_null()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OnErrorAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "text bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(null));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Contexts.Length, "Count of results");
      Assert.AreSame(_context, result.Contexts[0], "Correct element returned");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Once());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
    }

    [Test]
    public void Handle_removes_all_children_when_structure_expression_is_null()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OnErrorAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "structure bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(null));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Contexts.Length, "Count of results");
      Assert.AreSame(_context, result.Contexts[0], "Correct element returned");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Once());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
    }

    #endregion

    #region attribute evaluates to something

    [Test]
    public void Handle_replaces_children_with_text_when_expression_has_value()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OnErrorAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "bar"));

      string value = _autofixture.Create<string>();

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(value));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Contexts.Length, "Count of results");
      Assert.AreSame(_context, result.Contexts[0], "Correct element returned");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Never());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(value, false),
                      Times.Once());
    }

    [Test]
    public void Handle_replaces_children_with_text_when_text_based_expression_has_value()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OnErrorAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "text bar"));

      string value = _autofixture.Create<string>();

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(value));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Contexts.Length, "Count of results");
      Assert.AreSame(_context, result.Contexts[0], "Correct element returned");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Never());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(value, false),
                      Times.Once());
    }

    [Test]
    public void Handle_replaces_children_with_markup_when_structure_based_expression_has_value()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.OnErrorAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "structure bar"));

      string value = _autofixture.Create<string>();

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(value));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Contexts.Length, "Count of results");
      Assert.AreSame(_context, result.Contexts[0], "Correct element returned");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Never());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(value, true),
                      Times.Once());
    }

    #endregion
  }
}
