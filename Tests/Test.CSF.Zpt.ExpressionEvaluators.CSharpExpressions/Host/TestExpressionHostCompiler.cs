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
    public void GetHostCreator_returns_non_null_instance()
    {
      // Arrange
      var model = CreateModel("System.DateTime.Today.ToString()");

      // Act
      var hostCreator = _sut.GetHostCreator(model);

      // Assert
      Assert.NotNull(hostCreator);
    }

    [Test]
    public void GetHostCreator_returns_expression_which_may_be_executed_with_no_parameters()
    {
      // Arrange
      var model = CreateModel("System.DateTime.Today.ToString()");

      // Act
      var result = ExerciseSut(model);

      // Assert
      Assert.AreEqual(DateTime.Today.ToString(), result);
    }

    [Test]
    public void GetHostCreator_returns_expression_which_may_be_executed_with_aliased_namespace()
    {
      // Arrange
      var model = CreateModel("sys.DateTime.Today.ToString()",
                              aliasedNamespaces: new [] { new Tuple<string,string>("System", "sys") });

      // Act
      var result = ExerciseSut(model);

      // Assert
      Assert.AreEqual(DateTime.Today.ToString(), result);
    }

    [Test]
    public void GetHostCreator_returns_expression_which_may_be_executed_with_many_namespaces()
    {
      // Arrange
      var model = CreateModel("DateTime.Today.ToString()",
                              nonAliasedNamespaces: new [] {
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
    public void GetHostCreator_returns_expression_which_may_be_executed_with_some_parameters()
    {
      // Arrange
      var variables = new Dictionary<string,object>();
      variables.Add("text", "The answer");
      variables.Add("number", 30);

      var model = CreateModel("System.String.Format(\"{0} is {1}\", text, number + 12)",
                              dynamicParameterNames: variables.Keys);

      // Act
      var result = ExerciseSut(model, variables);

      // Assert
      Assert.AreEqual("The answer is 42", result);
    }

    [Test]
    public void GetHostCreator_returns_expression_which_may_be_executed_with_complex_parameters()
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
                              dynamicParameterNames: variables.Keys);

      // Act
      var result = ExerciseSut(model, variables);

      // Assert
      Assert.AreEqual(expectedResult, result);
    }

    [Test]
    public void GetHostCreator_returns_expression_which_may_be_executed_with_typed_parameters()
    {
      // Arrange
      var variables = new Dictionary<string,object>();
      variables.Add("amounts", new int[] { 10, 20, 30 });

      var model = CreateModel("amounts.Sum()",
                              typedParameters: new [] { new Tuple<string,string>("amounts", "IEnumerable<int>") },
                              nonAliasedNamespaces: new [] { "System", "System.Collections.Generic", "System.Linq" });

      // Act
      var result = ExerciseSut(model, variables);

      // Assert
      Assert.AreEqual(60, result);
    }

    [Test]
    public void GetHostCreator_returns_expression_which_may_be_executed_with_typed_parameters_and_lambda()
    {
      // Arrange
      var variables = new Dictionary<string,object>();
      variables.Add("amounts", new int[] { 10, 20, 30 });

      var model = CreateModel("String.Join(\"-\", amounts.Select(x => x + 5))",
                              typedParameters: new [] { new Tuple<string,string>("amounts", "IEnumerable<int>") },
                              nonAliasedNamespaces: new [] { "System", "System.Collections.Generic", "System.Linq" });

      // Act
      var result = ExerciseSut(model, variables);

      // Assert
      Assert.AreEqual("15-25-35", result);
    }

    [Test]
    public void GetHostCreator_raises_exception_which_includes_compile_failure_when_expression_is_invalid()
    {
      // Arrange
      // This should raise a compile error, you can't multiply strings, it will raise CS0019
      var model = CreateModel("\"foo\" * \"bar\"");

      // Act & assert
      Assert.That(() => ExerciseSut(model),
                  Throws.Exception.Matches<CSharpExpressionException>(x => {
        var expressionContained = x.Message.Contains("\"foo\" * \"bar\"");
        var errorCodeContained = x.Message.Contains("CS0019");
        var errorMessageContained = x.Message.Contains("cannot be applied to operands");

        return (expressionContained && errorCodeContained && errorMessageContained);
      }),
                 "The exception message must contain the expression and all of the error information.");
    }

    #endregion

    #region methods

    private ExpressionModel CreateModel(string text,
                                        IEnumerable<string> dynamicParameterNames = null,
                                        IEnumerable<string> nonAliasedNamespaces = null,
                                        IEnumerable<Tuple<string,string>> typedParameters = null,
                                        IEnumerable<Tuple<string,string>> aliasedNamespaces = null)
    {
      var dynamicParamNames = dynamicParameterNames?? Enumerable.Empty<string>();
      var typedParams = typedParameters?? Enumerable.Empty<Tuple<string,string>>();
      var paramSpecs = dynamicParamNames
        .Select(x => new VariableSpecification(x))
        .Union(typedParams.Select(x => new VariableSpecification(x.Item1, x.Item2)))
        .ToArray();

      var usingNamespaces = nonAliasedNamespaces?? Enumerable.Empty<string>();
      var usingAliasedNamespaces = aliasedNamespaces?? Enumerable.Empty<Tuple<string,string>>();
      var namespaceSpecs = usingNamespaces
        .Select(x => new UsingNamespaceSpecification(x))
        .Union(usingAliasedNamespaces.Select(x => new UsingNamespaceSpecification(x.Item1, x.Item2)))
        .ToArray();

      var spec = new ExpressionSpecification(text,
                                             paramSpecs,
                                             Enumerable.Empty<ReferencedAssemblySpecification>(),
                                             namespaceSpecs);

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

