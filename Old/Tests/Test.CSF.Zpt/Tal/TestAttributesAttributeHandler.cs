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
  public class TestAttributesAttributeHandler
  {
    #region fields

    private IFixture _autofixture;
    private Mock<ZptElement> _element;
    private RenderingContext _context;
    private DummyModel _model;

    private AttributesAttributeHandler _sut;

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

      _sut = new AttributesAttributeHandler();
    }

    #endregion

    #region tests

    [Test]
    public void Handle_makes_no_changes_when_attribute_not_present()
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.AttributesAttribute))
        .Returns((ZptAttribute) null);

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Contexts.Length, "Count of results");
      Assert.AreSame(_context, result.Contexts[0], "Correct element returned");
      _element.Verify(x => x.SetAttribute(It.IsAny<ZptNamespace>(), It.IsAny<string>() ,It.IsAny<string>()),
                      Times.Never());
      _element.Verify(x => x.SetAttribute(It.IsAny<string>(), It.IsAny<string>()),
                      Times.Never());
      _element.Verify(x => x.RemoveAttribute(It.IsAny<ZptNamespace>(), It.IsAny<string>()),
                      Times.Never());
      _element.Verify(x => x.RemoveAttribute(It.IsAny<string>()),
                      Times.Never());
    }

    //        Value               Prefix      Name      Expression
    //        -----               ------      ----      ----------
    [TestCase("foo bar",          null,       "foo",    "bar")]
    [TestCase("pre:foo bar",      "pre",      "foo",    "bar")]
    [TestCase("pre:foo bar/baz",  "pre",      "foo",    "bar/baz")]
    public void Handle_sets_single_attribute(string value,
                                             string expectedPrefix,
                                             string expectedName,
                                             string expectedExpression)
    {
      // Arrange
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.AttributesAttribute))
        .Returns(Mock.Of<ZptAttribute>(a => a.Value == value));
      
      var expressionResult = _autofixture.Create<string>();
      Mock.Get(_model)
        .Setup(x => x.Evaluate(expectedExpression, _context))
        .Returns(new ExpressionResult(expressionResult));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Contexts.Length, "Count of results");
      Assert.AreSame(_context, result.Contexts[0], "Correct element returned");
      Mock.Get(_model).Verify(x => x.Evaluate(expectedExpression, _context), Times.Once());
      _element.Verify(x => x.SetAttribute(It.Is<ZptNamespace>(ns => ns.Prefix == expectedPrefix), expectedName, expressionResult), Times.Once());
      _element.Verify(x => x.RemoveAttribute(It.IsAny<ZptNamespace>(), It.IsAny<string>()),
                      Times.Never());
      _element.Verify(x => x.RemoveAttribute(It.IsAny<string>()),
                      Times.Never());
    }

    [TestCase(null, "foo", "bar")]
    [TestCase("pre", "foo", "bar")]
    public void Handle_removes_attribute_when_expression_is_null(string prefix, string name, string expression)
    {
      // Arrange
      string attributeValue = (prefix != null)? String.Format("{0}:{1} {2}", prefix, name, expression) : String.Format("{0} {1}", name, expression);
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.AttributesAttribute))
        .Returns(Mock.Of<ZptAttribute>(a => a.Value == attributeValue));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(expression, _context))
        .Returns(new ExpressionResult(null));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Contexts.Length, "Count of results");
      Assert.AreSame(_context, result.Contexts[0], "Correct element returned");
      Mock.Get(_model).Verify(x => x.Evaluate(expression, _context), Times.Once());
      _element.Verify(x => x.RemoveAttribute(It.Is<ZptNamespace>(ns => ns.Prefix == prefix), name), Times.Once());
      _element.Verify(x => x.SetAttribute(It.IsAny<ZptNamespace>(), It.IsAny<string>(), It.IsAny<string>()),
                      Times.Never());
      _element.Verify(x => x.SetAttribute(It.IsAny<string>(), It.IsAny<string>()),
                      Times.Never());
    }

    [TestCase(null, "foo", "bar")]
    [TestCase("pre", "foo", "bar")]
    public void Handle_makes_no_changes_when_expression_cancels_action(string prefix, string name, string expression)
    {
      // Arrange
      string attributeValue = (prefix != null)? String.Format("{0}:{1} {2}", prefix, name, expression) : String.Format("{0} {1}", name, expression);
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.AttributesAttribute))
        .Returns(Mock.Of<ZptAttribute>(a => a.Value == attributeValue));

      Mock.Get(_model)
        .Setup(x => x.Evaluate(expression, _context))
        .Returns(new ExpressionResult(ZptConstants.CancellationToken));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Contexts.Length, "Count of results");
      Assert.AreSame(_context, result.Contexts[0], "Correct element returned");
      Mock.Get(_model).Verify(x => x.Evaluate(expression, _context), Times.Once());
      _element.Verify(x => x.SetAttribute(It.IsAny<ZptNamespace>(), It.IsAny<string>(), It.IsAny<string>()),
                      Times.Never());
      _element.Verify(x => x.SetAttribute(It.IsAny<string>(), It.IsAny<string>()),
                      Times.Never());
      _element.Verify(x => x.RemoveAttribute(It.IsAny<ZptNamespace>(), It.IsAny<string>()),
                      Times.Never());
      _element.Verify(x => x.RemoveAttribute(It.IsAny<string>()),
                      Times.Never());
    }

    [Test]
    [Description("This test verifies a multiple of different scenarios together")]
    public void Handle_behaves_correctly_for_multiple_attributes_and_scenarios()
    {
      // Arrange
      var attributeValue = @"one expressionOne;
                             two expressionTwo;
                             three expression;;WithSemicolon;
                             prefix:four expressionFour;";
      Mock.Get(_context)
        .Setup(x => x.GetAttribute(ZptConstants.Tal.Namespace,
                                   ZptConstants.Tal.AttributesAttribute))
        .Returns(Mock.Of<ZptAttribute>(a => a.Value == attributeValue));

      object
        expressionOneResult = _autofixture.Create<string>(),
        expressionTwoResult = null,
        expressionThreeResult = ZptConstants.CancellationToken,
        expressionFourResult = _autofixture.Create<string>();

      Mock.Get(_model)
        .Setup(x => x.Evaluate("expressionOne", _context))
        .Returns(new ExpressionResult(expressionOneResult));
      Mock.Get(_model)
        .Setup(x => x.Evaluate("expressionTwo", _context))
        .Returns(new ExpressionResult(expressionTwoResult));
      Mock.Get(_model)
        .Setup(x => x.Evaluate("expression;WithSemicolon", _context))
        .Returns(new ExpressionResult(expressionThreeResult));
      Mock.Get(_model)
        .Setup(x => x.Evaluate("expressionFour", _context))
        .Returns(new ExpressionResult(expressionFourResult));

      // Act
      var result = _sut.Handle(_context);

      // Assert
      Assert.NotNull(result, "Result nullability");
      Assert.AreEqual(1, result.Contexts.Length, "Count of results");
      Assert.AreSame(_context, result.Contexts[0], "Correct element returned");
      _element.Verify(x => x.SetAttribute(It.Is<ZptNamespace>(ns => ns.Prefix == null), "one", (string) expressionOneResult), Times.Once());
      _element.Verify(x => x.RemoveAttribute(It.Is<ZptNamespace>(ns => ns.Prefix == null), "two"), Times.Once());
      _element.Verify(x => x.SetAttribute(It.Is<ZptNamespace>(ns => ns.Prefix == null), "three", It.IsAny<string>()), Times.Never());
      _element.Verify(x => x.SetAttribute(It.Is<ZptNamespace>(ns => ns.Prefix == "prefix"), "four", (string) expressionFourResult), Times.Once());
    }

    #endregion
  }
}

