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
    public void Evaluate_returns_expected_result_with_no_variables()
    {
      // Arrange
      var sut = new PythonPoC();

      // Act
      var result = sut.Evaluate("6", new Dictionary<string,object>());

      // Assert
      Assert.AreEqual(6, result);
    }

    [Test]
    public void Evaluate_returns_expected_result_with_one_variable()
    {
      // Arrange
      var sut = new PythonPoC();

      // Act
      var result = sut.Evaluate("one+5", new Dictionary<string,object>{
        { "one",   1 },
      });

      // Assert
      Assert.AreEqual(6, result);
    }

    [Test]
    public void Evaluate_returns_expected_result_with_three_variables()
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

    [Test]
    public void Evaluate_returns_expected_result_twice_with_three_variables()
    {
      // Arrange
      var sut = new PythonPoC();

      // Act
      var variables = new Dictionary<string,object>{
        { "one",   1 },
        { "two",   2 },
        { "three", 3 },
      };
      sut.Evaluate("one+two+three", variables);
      var result = sut.Evaluate("one+two+three", variables);

      // Assert
      Assert.AreEqual(6, result);
    }

    #endregion
  }
}

