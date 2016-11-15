﻿using System;
using NUnit.Framework;
using CSF.Zpt.ExpressionEvaluators.CSharpExpressions;
using CSF.Zpt.ExpressionEvaluators.CSharpFramework;
using System.Collections.Generic;
using System.Linq;

namespace Test.CSF.Zpt.ExpressionEvaluators.CSharpExpressions
{
  [TestFixture]
  public class TestCSharpExpressionFactory
  {
    #region fields

    private CSharpExpressionFactory _sut;
    private int _nextId;

    #endregion

    #region setup

    [TestFixtureSetUp]
    public void FixtureSetup()
    {
      _sut = new CSharpExpressionFactory();
      _nextId = 1;
    }

    [SetUp]
    public void Setup()
    {
      
    }

    #endregion

    #region tests

    [Test]
    public void Create_returns_non_null_with_no_parameters()
    {
      // Arrange
      var model = CreateModel("DateTime.Today.ToString()", Enumerable.Empty<string>());

      // Act
      var result = _sut.Create(model);

      // Assert
      Assert.NotNull(result);
    }

    [Test]
    public void Create_returns_expression_which_may_be_executed_with_no_parameters()
    {
      // Arrange
      var model = CreateModel("DateTime.Today.ToString()", Enumerable.Empty<string>());

      // Act
      var expression = _sut.Create(model);
      var result = expression.Evaluate(new Dictionary<string, object>());

      // Assert
      Assert.AreEqual(DateTime.Today.ToString(), result);
    }

    [Test]
    public void Create_returns_expression_which_may_be_executed_with_some_parameters()
    {
      // Arrange
      var variables = new Dictionary<string,object>();
      variables.Add("text", "The answer");
      variables.Add("number", 30);

      var model = CreateModel("String.Format(\"{0} is {1}\", text, number + 12)", variables.Keys);

      // Act
      var expression = _sut.Create(model);
      var result = expression.Evaluate(variables);

      // Assert
      Assert.AreEqual("The answer is 42", result);
    }

    [Test]
    public void Create_returns_expression_which_may_be_executed_with_typed_parameters()
    {
      // Arrange
      var person = new Person() {
        DateOfBirth = new DateTime(1980, 1, 1),
      };
      var referenceDate = DateTime.Today;
      var expectedResult = (referenceDate - person.DateOfBirth).TotalDays;

      var variables = new Dictionary<string,object>();
      variables.Add("person", person);
      variables.Add("referenceDate", referenceDate);

      var model = CreateModel("(referenceDate - person.DateOfBirth).TotalDays", variables.Keys);

      // Act
      var expression = _sut.Create(model);
      var result = expression.Evaluate(variables);

      // Assert
      Assert.AreEqual(expectedResult, result);
    }

    #endregion

    #region methods

    private ExpressionModel CreateModel(string text, IEnumerable<string> parameters)
    {
      return new ExpressionModel(_nextId ++,
                                 text,
                                 parameters.OrderBy(x => x).ToArray());
    }

    #endregion
  }
}
