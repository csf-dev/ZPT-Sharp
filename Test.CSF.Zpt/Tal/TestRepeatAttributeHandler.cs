using System;
using NUnit.Framework;
using Moq;
using Ploeh.AutoFixture;
using CSF.Zpt.Tal;
using CSF.Zpt.Rendering;
using Test.CSF.Zpt.Util;
using Test.CSF.Zpt.Util.Autofixture;
using CSF.Zpt;
using System.Linq;

namespace Test.CSF.Zpt.Tal
{
  [TestFixture]
  public class TestRepeatAttributeHandler
  {
    #region fields

    private IFixture _autofixture;
    private Mock<ZptElement> _element, _parent, _clone;
    private RenderingContext _context;
    private DummyModel _model;

    private RepeatAttributeHandler _sut;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _autofixture = new Fixture();
      new DummyModelCustomisation().Customize(_autofixture);

      _model = _autofixture.Create<DummyModel>();
      _element = new Mock<ZptElement>() { CallBase = true };
      _parent = new Mock<ZptElement>() { CallBase = true };
      _clone = new Mock<ZptElement>() { CallBase = true };
      _context = Mock.Of<RenderingContext>(x => x.Element == _element.Object && x.TalModel == _model);

      _element.Setup(x => x.Remove());
      _element.Setup(x => x.Clone()).Returns(_clone.Object);
      _element.Setup(x => x.GetParentElement()).Returns(_parent.Object);
      _parent.Setup(x => x.InsertBefore(It.IsAny<ZptElement>(), _clone.Object)).Returns(_clone.Object);

      _sut = new RepeatAttributeHandler();
    }

    #endregion

    #region tests

    [Test]
    public void TestHandleNoAttribute()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.RepeatAttribute))
        .Returns((ZptAttribute) null);

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Contexts.Length, "Count of results");
      Assert.AreSame(_context, result.Contexts[0], "Correct element returned");

      _element.Verify(x => x.Remove(), Times.Never());
      _parent.Verify(x => x.InsertBefore(It.IsAny<ZptElement>(), It.IsAny<ZptElement>()), Times.Never());
    }

    [Test]
    public void TestHandleCancelsAction()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.RepeatAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "foo bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(ZptConstants.CancellationToken));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");

      _element.Verify(x => x.Remove(), Times.Never());
      _parent.Verify(x => x.InsertBefore(It.IsAny<ZptElement>(), It.IsAny<ZptElement>()), Times.Never());

      Assert.AreEqual(1, result.Contexts.Length, "Count of results");
      Assert.AreSame(_context, result.Contexts[0], "Correct element returned");
    }

    [Test]
    public void TestHandleNullSequences()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.RepeatAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "foo bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(null));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");

      _element.Verify(x => x.Remove(), Times.Never());
      _parent.Verify(x => x.InsertBefore(It.IsAny<ZptElement>(), It.IsAny<ZptElement>()), Times.Never());

      Assert.AreEqual(1, result.Contexts.Length, "Count of results");
      Assert.AreSame(_context, result.Contexts[0], "Correct element returned");
    }

    [Test]
    public void TestHandleEmptySequence()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.RepeatAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "foo bar"));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(It.IsAny<string>(), _context))
        .Returns(new ExpressionResult(new string[0]));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");

      _element.Verify(x => x.Remove(), Times.Once());
      _parent.Verify(x => x.InsertBefore(It.IsAny<ZptElement>(), It.IsAny<ZptElement>()), Times.Never());

      Assert.AreEqual(0, result.Contexts.Length, "Count of results");
    }

    [Test]
    public void TestHandleSequence()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.RepeatAttribute))
        .Returns((ZptAttribute) Mock.Of<ZptAttribute>(x => x.Value == "foo bar"));
      Mock.Get(_context)
        .Setup(x => x.CreateSiblingContext(It.IsAny<ZptElement>(), false))
        .Returns(_context);

      Mock.Get(_model)
        .Setup(x => x.Evaluate("bar", _context))
        .Returns(new ExpressionResult(new [] { "One", "Two", "Three", "Four" }));
      Mock.Get(_model).Setup(x => x.AddRepetitionInfo(It.IsAny<IRepetitionInfo>()));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");

      _element.Verify(x => x.Remove(), Times.Once());
      _parent.Verify(x => x.InsertBefore(_element.Object, _clone.Object), Times.Exactly(4));
      Mock.Get(_model)
        .Verify(x => x.AddRepetitionInfo(It.Is<IRepetitionInfo>(r => r.Name == "foo")),
                Times.Exactly(4));
      
      Assert.AreEqual(4, result.Contexts.Length, "Count of results");
    }

    #endregion
  }
}

