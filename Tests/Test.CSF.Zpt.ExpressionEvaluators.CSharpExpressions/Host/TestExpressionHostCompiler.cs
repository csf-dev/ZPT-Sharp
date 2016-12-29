using System;
using NUnit.Framework;
using CSF.Zpt.ExpressionEvaluators.CSharpExpressions;
using CSF.Zpt.ExpressionEvaluators.CSharpFramework;
using System.Collections.Generic;
using System.Linq;
using Moq;
using CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec;
using CSF.Zpt.Tales;
using CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Host;

namespace Test.CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Host
{
  [TestFixture]
  public class TestExpressionHostCompiler
  {
    #region fields

    private IExpressionHostCompiler _sut;
    private int _nextId;

    #endregion

    #region setup

    [TestFixtureSetUp]
    public void FixtureSetup()
    {
      _nextId = 1;
    }

    [SetUp]
    public void Setup()
    {
      _sut = new ExpressionHostCompiler();
    }

    #endregion

    #region tests

    [Test]
    public void Create_returns_non_null_with_no_parameters()
    {
      // Arrange
      var model = CreateModel("System.DateTime.Today.ToString()");

      // Act
      var hostCreator = _sut.GetHostCreator(model);

      // Assert
      Assert.NotNull(hostCreator);
    }

    [Test]
    public void Create_returns_expression_which_may_be_executed_with_no_parameters()
    {
      // Arrange
      var model = CreateModel("System.DateTime.Today.ToString()");

      // Act
      var result = ExerciseSut(model);

      // Assert
      Assert.AreEqual(DateTime.Today.ToString(), result);
    }

    [Test]
    public void Create_returns_expression_which_may_be_executed_with_many_namespaces()
    {
      // Arrange
      var model = CreateModel("DateTime.Today.ToString()",
                              namespaces: new [] {
                                "System",
                                "System.Collections",
                                "System.Collections.Generic",
                                "System.Linq",
                              });

      // Act
      var result = ExerciseSut(model);

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

      var model = CreateModel("System.String.Format(\"{0} is {1}\", text, number + 12)",
                              parameters: variables.Keys);

      // Act
      var result = ExerciseSut(model, variables);

      // Assert
      Assert.AreEqual("The answer is 42", result);
    }

    [Test]
    public void Create_returns_expression_which_may_be_executed_with_complex_parameters()
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

      var model = CreateModel("(referenceDate - person.DateOfBirth).TotalDays",
                              parameters: variables.Keys);

      // Act
      var result = ExerciseSut(model, variables);

      // Assert
      Assert.AreEqual(expectedResult, result);
    }

    #endregion

    #region methods

    private ExpressionModel CreateModel(string text,
                                        IEnumerable<string> parameters = null,
                                        IEnumerable<string> namespaces = null)
    {
      var expressionParams = parameters?? Enumerable.Empty<string>();
      var expressionNamespaces = namespaces?? Enumerable.Empty<string>();

      var spec = new ExpressionSpecification(text,
                                             expressionParams.Select(x => new VariableSpecification(x)),
                                             Enumerable.Empty<ReferencedAssemblySpecification>(),
                                             expressionNamespaces.Select(x => new UsingNamespaceSpecification(x)));

      return new ExpressionModel(_nextId ++, spec);
    }

    private object ExerciseSut(ExpressionModel model, IDictionary<string,object> variables = null)
    {
      var allVariables = variables?? new Dictionary<string, object>();

      var hostCreator = _sut.GetHostCreator(model);
      var host = hostCreator.CreateHostInstance();
      foreach(var kvp in allVariables)
      {
        host.SetVariableValue(kvp.Key, kvp.Value);
      }
      return host.Evaluate();
    }

    #endregion
  }
}

