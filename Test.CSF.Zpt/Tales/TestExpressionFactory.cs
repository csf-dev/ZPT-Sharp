using System;
using NUnit.Framework;
using CSF.Zpt.Tales;

namespace Test.CSF.Zpt.Tales
{
  [TestFixture]
  public class TestExpressionFactory
  {
    #region fields

    private IExpressionFactory _sut;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _sut = new ExpressionFactory();
    }

    #endregion

    #region tests

    //        Source      Prefix  Content
    //        ------      ------  -------
    [TestCase("foo:bar",  "foo",  "bar")]
    [TestCase("bar",      null,   "bar")]
    [TestCase("",         null,   "")]
    public void Create_FromValidSource(string source, string expectedPrefix, string expectedContent)
    {
      // Arrange (nothing to do)

      // Act
      var result = _sut.Create(source);

      // Assert
      Assert.AreEqual(expectedPrefix, result.Prefix);
      Assert.AreEqual(expectedContent, result.Content);
    }

    [TestCase("_invalid:invalid")]
    [TestCase("in_valid:invalid")]
    [TestCase("invalid_:invalid")]
    [TestCase(null)]
    [ExpectedException]
    public void Create_FromInvalidSource(string source)
    {
      // Arrange (nothing to do)

      // Act
      _sut.Create(source);

      // Assert (by observing an exception)
    }

    [TestCase("foo",  "bar")]
    [TestCase(null,   "bar")]
    [TestCase(null,   "")]
    public void Create_FromValidPrefixAndContent(string prefix, string content)
    {
      // Arrange (nothing to do)

      // Act
      var result = _sut.Create(prefix, content);

      // Assert
      Assert.AreEqual(prefix, result.Prefix);
      Assert.AreEqual(content, result.Content);
    }

    [TestCase("_x",  "bar")]
    [TestCase("x_x", "bar")]
    [TestCase("x_",  "")]
    [TestCase("x",   null)]
    [TestCase(null,  null)]
    [ExpectedException]
    public void Create_FromInvalidPrefixAndContent(string prefix, string content)
    {
      // Arrange (nothing to do)

      // Act
      _sut.Create(prefix, content);

      // Assert (by observing an exception)
    }

    [Test]
    public void Create_FromExpression()
    {
      // Arrange
      var expression = new Expression("foo", "bar:baz");

      // Act
      var result = _sut.Create(expression);

      // Assert
      Assert.AreEqual("bar", result.Prefix);
      Assert.AreEqual("baz", result.Content);
    }

    #endregion
  }
}

