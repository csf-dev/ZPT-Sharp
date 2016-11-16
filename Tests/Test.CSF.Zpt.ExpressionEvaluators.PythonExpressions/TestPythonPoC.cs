using System;
using NUnit.Framework;
using CSF.Zpt.ExpressionEvaluators.PythonExpressions;

namespace Test.CSF.Zpt.ExpressionEvaluators.PythonExpressions
{
  [TestFixture]
  public class TestPythonPoC
  {
    #region tests

    [Test]
    public void Evaluate_returns_expected_result()
    {
      // Arrange
      var sut = new PythonPoC();

      // Act
      var result = sut.Evaluate();

      // Assert
      Assert.AreEqual(6, result);
    }

    #endregion
  }
}

