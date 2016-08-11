using System;
using NUnit.Framework;
using CSF.Zpt.Tales;
using Moq;
using Ploeh.AutoFixture;
using CSF.Zpt.Rendering;

namespace Test.CSF.Zpt.Tales
{
  [TestFixture]
  public class TestPathExpressionEvaluator
  {
    #region fields

    private IFixture _autofixture;
    private Mock<ITalesModel> _model;
    private RenderingContext _element;
    private PathExpressionEvaluator _sut;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _autofixture = new Fixture();
      _model = new Mock<ITalesModel>();
      _element = Mock.Of<RenderingContext>();
      _sut = new PathExpressionEvaluator(SimpleEvaluatorRegistry.Default);
    }

    #endregion

    #region tests

    [Test]
    public void TestEvaluate_One()
    {
      // Arrange
      var expression = new Expression(null, "foo/ToString");
      object expectedResult = _autofixture.Create<int>();
      _model.Setup(x => x.TryGetRootObject("foo", _element, out expectedResult)).Returns(true);

      // Act
      var result = _sut.Evaluate(expression, _element, _model.Object);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(expectedResult.ToString(), result.Value, "Result value");
    }

    [Test]
    public void TestEvaluate_Two()
    {
      // Arrange
      var expression = new Expression(null, "foo/Day");
      object expectedResult = _autofixture.Create<DateTime>();
      _model.Setup(x => x.TryGetRootObject("foo", _element, out expectedResult)).Returns(true);

      // Act
      var result = _sut.Evaluate(expression, _element, _model.Object);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(((DateTime) expectedResult).Day, result.Value, "Result value");
    }

    [Test]
    public void TestEvaluate_Three()
    {
      // Arrange
      var expression = new Expression(null, "foo/Day/ToString");
      object expectedResult = _autofixture.Create<DateTime>();
      _model.Setup(x => x.TryGetRootObject("foo", _element, out expectedResult)).Returns(true);

      // Act
      var result = _sut.Evaluate(expression, _element, _model.Object);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(((DateTime) expectedResult).Day.ToString(), result.Value, "Result value");
    }

    [Test]
    public void TestEvaluate_Four()
    {
      // Arrange
      var expression = new Expression(null, "foo/?prop/ToString");
      object expectedResult = _autofixture.Create<DateTime>(), propName = "Day";
      _model.Setup(x => x.TryGetRootObject("foo", _element, out expectedResult)).Returns(true);
      _model.Setup(x => x.TryGetRootObject("prop", _element, out propName)).Returns(true);

      // Act
      var result = _sut.Evaluate(expression, _element, _model.Object);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(((DateTime) expectedResult).Day.ToString(), result.Value, "Result value");
    }

    [Test]
    [ExpectedException(typeof(ModelEvaluationException))]
    public void TestEvaluate_Five()
    {
      // Arrange
      var expression = new Expression(null, "bar/?prop/ToString");
      object expectedResult = _autofixture.Create<DateTime>(), propName = "Day";
      _model.Setup(x => x.TryGetRootObject("foo", _element, out expectedResult)).Returns(true);
      _model.Setup(x => x.TryGetRootObject("prop", _element, out propName)).Returns(true);

      // Act
      _sut.Evaluate(expression, _element, _model.Object);

      // Assert (by observing an exception)
    }

    [Test]
    public void TestEvaluate_Six()
    {
      // Arrange
      var expression = new Expression(null, "bar/?prop/ToString | foo/?prop/ToString");
      object expectedResult = _autofixture.Create<DateTime>(), propName = "Day";
      _model.Setup(x => x.TryGetRootObject("foo", _element, out expectedResult)).Returns(true);
      _model.Setup(x => x.TryGetRootObject("prop", _element, out propName)).Returns(true);

      // Act
      var result = _sut.Evaluate(expression, _element, _model.Object);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(((DateTime) expectedResult).Day.ToString(), result.Value, "Result value");
    }

    [Test]
    [ExpectedException(typeof(ModelEvaluationException))]
    public void TestEvaluate_Seven()
    {
      // Arrange
      var expression = new Expression(null, "bar/?prop/ToString | foo/?flob/ToString");
      object expectedResult = _autofixture.Create<DateTime>(), propName = "Day";
      _model.Setup(x => x.TryGetRootObject("foo", _element, out expectedResult)).Returns(true);
      _model.Setup(x => x.TryGetRootObject("prop", _element, out propName)).Returns(true);

      // Act
      var result = _sut.Evaluate(expression, _element, _model.Object);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(((DateTime) expectedResult).Day.ToString(), result.Value, "Result value");
    }

    [Test]
    public void TestEvaluate_Eight()
    {
      // Arrange
      var expression = new Expression(null, "foo/?flob/ToString | foo/?prop/ToString");
      object expectedResult = _autofixture.Create<DateTime>(), propName = "Day";
      _model.Setup(x => x.TryGetRootObject("foo", _element, out expectedResult)).Returns(true);
      _model.Setup(x => x.TryGetRootObject("prop", _element, out propName)).Returns(true);

      // Act
      var result = _sut.Evaluate(expression, _element, _model.Object);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(((DateTime) expectedResult).Day.ToString(), result.Value, "Result value");
    }

    #endregion
  }
}

