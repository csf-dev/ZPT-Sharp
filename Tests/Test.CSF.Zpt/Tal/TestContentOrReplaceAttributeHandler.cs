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
  public class TestContentOrReplaceAttributeHandler
  {
    #region fields

    private IFixture _autofixture;
    private Mock<ZptElement> _element;
    private RenderingContext _context;
    private DummyModel _model;

    private ContentOrReplaceAttributeHandler _sut;

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

      _sut = new ContentOrReplaceAttributeHandler();
    }

    #endregion

    #region no attribute

    [Test]
    public void Handle_makes_no_change_when_attribute_not_present()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ContentAttribute))
        .Returns((ZptAttribute) null);
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ReplaceAttribute))
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
    public void Handle_makes_no_change_with_content_attribute_when_attribute_cancels_action()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ContentAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "bar"));
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ReplaceAttribute))
        .Returns((ZptAttribute) null);

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
    public void Handle_makes_no_change_with_content_text_attribute_when_attribute_cancels_action()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ContentAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "text bar"));
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ReplaceAttribute))
        .Returns((ZptAttribute) null);

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
    public void Handle_makes_no_change_with_content_structure_attribute_when_attribute_cancels_action()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ContentAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "structure bar"));
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ReplaceAttribute))
        .Returns((ZptAttribute) null);

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
    public void Handle_makes_no_change_with_replace_attribute_when_attribute_cancels_action()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ContentAttribute))
        .Returns((ZptAttribute) null);
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ReplaceAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(ZptConstants.CancellationToken));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(0, result.Contexts.Length, "Count of results");

      _element.Verify(x => x.Omit(),
                      Times.Once());
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
    public void Handle_makes_no_change_with_replace_text_attribute_when_attribute_cancels_action()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ContentAttribute))
        .Returns((ZptAttribute) null);
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ReplaceAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "text bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(ZptConstants.CancellationToken));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(0, result.Contexts.Length, "Count of results");

      _element.Verify(x => x.Omit(),
                      Times.Once());
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
    public void Handle_makes_no_change_with_replace_structure_attribute_when_attribute_cancels_action()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ContentAttribute))
        .Returns((ZptAttribute) null);
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ReplaceAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "structure bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(ZptConstants.CancellationToken));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(0, result.Contexts.Length, "Count of results");

      _element.Verify(x => x.Omit(),
                      Times.Once());
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
    public void Handle_with_content_attribute_removes_all_children_when_expression_is_null()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ContentAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "bar"));
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ReplaceAttribute))
        .Returns((ZptAttribute) null);

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
    public void Handle_with_text_content_attribute_removes_all_children_when_expression_is_null()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ContentAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "text bar"));
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ReplaceAttribute))
        .Returns((ZptAttribute) null);

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
    public void Handle_with_structure_content_attribute_removes_all_children_when_expression_is_null()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ContentAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "structure bar"));
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ReplaceAttribute))
        .Returns((ZptAttribute) null);

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
    public void Handle_with_replace_attribute_removes_element_when_expression_is_null()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ContentAttribute))
        .Returns((ZptAttribute) null);
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ReplaceAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(null));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(0, result.Contexts.Length, "Count of results");

      _element.Verify(x => x.Remove(),
                      Times.Once());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Never());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
    }

    [Test]
    public void Handle_with_text_replace_attribute_removes_element_when_expression_is_null()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ContentAttribute))
        .Returns((ZptAttribute) null);
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ReplaceAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "text bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(null));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(0, result.Contexts.Length, "Count of results");

      _element.Verify(x => x.Remove(),
                      Times.Once());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Never());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
    }

    [Test]
    public void Handle_with_structure_replace_attribute_removes_element_when_expression_is_null()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ContentAttribute))
        .Returns((ZptAttribute) null);
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ReplaceAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "structure bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(null));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(0, result.Contexts.Length, "Count of results");

      _element.Verify(x => x.Remove(),
                      Times.Once());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Never());
      _element.Verify(x => x.ReplaceWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
      _element.Verify(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
    }

    #endregion

    #region attribute evaluates to something

    [Test]
    public void Handle_with_content_attribute_replaces_children_with_text_value()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ContentAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "bar"));
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ReplaceAttribute))
        .Returns((ZptAttribute) null);

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
    public void Handle_with_text_content_attribute_replaces_children_with_text_value()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ContentAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "text bar"));
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ReplaceAttribute))
        .Returns((ZptAttribute) null);

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
    public void Handle_with_structure_content_attribute_replaces_children_with_markup_value()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ContentAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "structure bar"));
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ReplaceAttribute))
        .Returns((ZptAttribute) null);

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

    [Test]
    public void Handle_with_replace_attribute_replaces_self_with_text_value()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ContentAttribute))
        .Returns((ZptAttribute) null);
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ReplaceAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "bar"));

      string value = _autofixture.Create<string>();

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(value));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Never());
      _element.Verify(x => x.ReplaceWith(value, false),
                      Times.Once());
      _element.Verify(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
    }

    [Test]
    public void Handle_with_text_replace_attribute_replaces_self_with_text_value()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ContentAttribute))
        .Returns((ZptAttribute) null);
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ReplaceAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "text bar"));

      string value = _autofixture.Create<string>();

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(value));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Never());
      _element.Verify(x => x.ReplaceWith(value, false),
                      Times.Once());
      _element.Verify(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
    }

    [Test]
    public void Handle_with_structure_replace_attribute_replaces_self_with_markup_value()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ContentAttribute))
        .Returns((ZptAttribute) null);
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ReplaceAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "structure bar"));

      string value = _autofixture.Create<string>();

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(value));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");

      _element.Verify(x => x.Remove(),
                      Times.Never());
      _element.Verify(x => x.RemoveAllChildren(),
                      Times.Never());
      _element.Verify(x => x.ReplaceWith(value, true),
                      Times.Once());
      _element.Verify(x => x.ReplaceChildrenWith(It.IsAny<string>(), It.IsAny<bool>()),
                      Times.Never());
    }

    #endregion

    #region error scenarios

    [Test]
    [ExpectedException(typeof(ParserException))]
    public void Handle_raises_error_when_both_content_and_replace_attribute_present()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ContentAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>());
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.ReplaceAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>());

      // Act
      _sut.Handle(_context);

      // Assert (by observing an exception)
    }

    #endregion
  }
}

