using System;
using NUnit.Framework;
using CSF.Zpt.ExpressionEvaluators.PythonExpressions;
using System.Collections.Generic;

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
      var result = sut.Evaluate("one+two+three", new Dictionary<string,object>{
        { "one",   1 },
        { "two",   2 },
        { "three", 3 },
      });

      // Assert
      Assert.AreEqual(6, result);
    }

    #endregion
  }
}

