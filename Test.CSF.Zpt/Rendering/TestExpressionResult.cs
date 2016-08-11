using System;
using CSF.Zpt.Rendering;
using NUnit.Framework;

namespace Test.CSF.Zpt.Rendering
{
  [TestFixture]
  public class TestExpressionResult
  {
    #region tests

    [TestCase("",     true)]
    [TestCase(" ",    true)]
    [TestCase(null,   false)]
    [TestCase(1,      true)]
    [TestCase(0,      false)]
    [TestCase(true,   true)]
    [TestCase(false,  false)]
    public void TestGetResultAsBoolean(object value, bool expectedValue)
    {
      // Arrange
      var sut = new ExpressionResult(value);

      // Act
      var result = sut.GetValueAsBoolean();

      // Assert
      Assert.AreEqual(expectedValue, result);
    }

    #endregion
  }
}

