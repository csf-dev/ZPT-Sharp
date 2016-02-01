using System;
using NUnit.Framework;
using CSF.Zpt.Tales;

namespace Test.CSF.Zpt.Tales
{
  [TestFixture]
  public class TestExpression
  {
    #region tests

    [TestCase("foo:bar",  "foo")]
    [TestCase("bar",      null)]
    public void TestGetPrefix(string source, string expectedPrefix)
    {
      // Arrange
      var expression = new Expression(source);

      // Act
      var result = expression.GetPrefix();

      // Assert
      Assert.AreEqual(expectedPrefix, result);
    }

    [TestCase("foo:bar",  "bar")]
    [TestCase("bar",      "bar")]
    public void TestGetContent(string source, string expectedContent)
    {
      // Arrange
      var expression = new Expression(source);

      // Act
      var result = expression.GetContent();

      // Assert
      Assert.AreEqual(expectedContent, result);
    }

    #endregion
  }
}

