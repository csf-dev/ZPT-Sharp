using System;
using NUnit.Framework;
using Moq;
using CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec;
using CSF.Zpt.ExpressionEvaluators.CSharpExpressions;
using System.Collections.Generic;
using CSF.Zpt.Tales;
using Ploeh.AutoFixture;
using System.Linq;

namespace Test.CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec
{
  [TestFixture]
  public class TestExpressionSpecificationFactory
  {
    #region fields

    private IExpressionSpecificationFactory _sut;
    private Mock<IExpressionConfiguration> _config;
    private IDictionary<string,object> _variableDefinitions;
    private ITalesModel _model;
    private IFixture _autoFixture;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _autoFixture = new Fixture();

      _variableDefinitions = new Dictionary<string, object>();

      var model = new Mock<ITalesModel>();
      model.Setup(x => x.GetAllDefinitions()).Returns(_variableDefinitions);
      _model = model.Object;

      _config = new Mock<IExpressionConfiguration>();
      _config.Setup(x => x.GetImportedNamespaces()).Returns(Enumerable.Empty<UsingNamespaceSpecification>());
      _config.Setup(x => x.GetReferencedAssemblies()).Returns(Enumerable.Empty<ReferencedAssemblySpecification>());

      _sut = new ExpressionSpecificationFactory(_config.Object);
    }

    #endregion

    #region tests

    [Test]
    public void CreateExpressionSpecification_returns_instance_with_no_definitions()
    {
      // Arrange


      // Act
      var result = ExerciseSut();

      // Assert
      Assert.NotNull(result);
    }

    [Test]
    public void CreateExpressionSpecification_assigns_untyped_variables()
    {
      // Arrange
      _variableDefinitions.Add("foo", 1);
      _variableDefinitions.Add("bar", 2);

      // Act
      var result = ExerciseSut();

      // Assert
      Assert.AreEqual(2, result.Variables.Count(), "Count of variables");
      Assert.IsTrue(result.Variables.Any(x => x.Name == "foo" && x.IsDynamicType),
                    "First expected variable present");
      Assert.IsTrue(result.Variables.Any(x => x.Name == "bar" && x.IsDynamicType),
                    "Second expected variable present");
    }

    [Test]
    public void CreateExpressionSpecification_assigns_typed_variables()
    {
      // Arrange
      _variableDefinitions.Add("foo", 1);
      _variableDefinitions.Add("bar", 2);
      _variableDefinitions.Add(_autoFixture.Create<string>(),
                               new VariableTypeDefinition("foo", "int"));
      _variableDefinitions.Add(_autoFixture.Create<string>(),
                               new VariableTypeDefinition("bar", "int"));

      // Act
      var result = ExerciseSut();

      // Assert
      Assert.AreEqual(2, result.Variables.Count(), "Count of variables");
      Assert.IsTrue(result.Variables.Any(x => x.Name == "foo" && x.TypeName == "int"),
                    "First expected variable present");
      Assert.IsTrue(result.Variables.Any(x => x.Name == "bar" && x.TypeName == "int"),
                    "Second expected variable present");
    }

    [Test]
    public void CreateExpressionSpecification_assigns_assembly_references()
    {
      // Arrange
      _variableDefinitions.Add(_autoFixture.Create<string>(),
                               new ReferencedAssemblySpecification("SomeAssembly.dll"));

      // Act
      var result = ExerciseSut();

      // Assert
      Assert.AreEqual(1, result.Assemblies.Count(), "Count of referenced assemblies");
      Assert.IsTrue(result.Assemblies.Any(x => x.Name == "SomeAssembly.dll"), "Expected assembly found");
    }

    [Test]
    public void CreateExpressionSpecification_merges_with_configured_assemblies()
    {
      // Arrange
      _variableDefinitions.Add(_autoFixture.Create<string>(),
                               new ReferencedAssemblySpecification("SomeAssembly.dll"));

      _config
        .Setup(x => x.GetReferencedAssemblies())
        .Returns(new [] { new ReferencedAssemblySpecification("ConfiguredAssembly.dll") });

      // Act
      var result = ExerciseSut();

      // Assert
      Assert.AreEqual(2, result.Assemblies.Count(), "Count of referenced assemblies");
      Assert.IsTrue(result.Assemblies.Any(x => x.Name == "SomeAssembly.dll"), "First expected assembly found");
      Assert.IsTrue(result.Assemblies.Any(x => x.Name == "ConfiguredAssembly.dll"), "Second expected assembly found");
    }

    [Test]
    public void CreateExpressionSpecification_merging_does_not_create_duplicate_assembly_references()
    {
      // Arrange
      _variableDefinitions.Add(_autoFixture.Create<string>(),
                               new ReferencedAssemblySpecification("SomeAssembly.dll"));

      _config
        .Setup(x => x.GetReferencedAssemblies())
        .Returns(new [] { new ReferencedAssemblySpecification("ConfiguredAssembly.dll"),
                          new ReferencedAssemblySpecification("SomeAssembly.dll") });

      // Act
      var result = ExerciseSut();

      // Assert
      Assert.AreEqual(2, result.Assemblies.Count(), "Count of referenced assemblies");
      Assert.IsTrue(result.Assemblies.Any(x => x.Name == "SomeAssembly.dll"), "First expected assembly found");
      Assert.IsTrue(result.Assemblies.Any(x => x.Name == "ConfiguredAssembly.dll"), "Second expected assembly found");
    }

    [Test]
    public void CreateExpressionSpecification_assigns_imported_namespaces()
    {
      // Arrange
      _variableDefinitions.Add(_autoFixture.Create<string>(),
                               new UsingNamespaceSpecification("MyNamespace"));

      // Act
      var result = ExerciseSut();

      // Assert
      Assert.AreEqual(1, result.Namespaces.Count(), "Count of namespaces");
      Assert.IsTrue(result.Namespaces.Any(x => x.Namespace == "MyNamespace" && !x.HasAlias), "Expected namespace found");
    }

    [Test]
    public void CreateExpressionSpecification_merges_with_configured_namespaces()
    {
      // Arrange
      _variableDefinitions.Add(_autoFixture.Create<string>(),
                               new UsingNamespaceSpecification("MyNamespace"));

      _config
        .Setup(x => x.GetImportedNamespaces())
        .Returns(new [] { new UsingNamespaceSpecification("ConfiguredNamespace") });

      // Act
      var result = ExerciseSut();

      // Assert
      Assert.AreEqual(2, result.Namespaces.Count(), "Count of namespaces");
      Assert.IsTrue(result.Namespaces.Any(x => x.Namespace == "MyNamespace" && !x.HasAlias), "First expected namespace found");
      Assert.IsTrue(result.Namespaces.Any(x => x.Namespace == "ConfiguredNamespace" && !x.HasAlias), "Second expected namespace found");
    }

    [Test]
    public void CreateExpressionSpecification_merging_does_not_create_duplicate_imported_namespaces()
    {
      // Arrange
      _variableDefinitions.Add(_autoFixture.Create<string>(),
                               new UsingNamespaceSpecification("MyNamespace"));

      _config
        .Setup(x => x.GetImportedNamespaces())
        .Returns(new [] { new UsingNamespaceSpecification("ConfiguredNamespace"),
                          new UsingNamespaceSpecification("MyNamespace") });

      // Act
      var result = ExerciseSut();

      // Assert
      Assert.AreEqual(2, result.Namespaces.Count(), "Count of namespaces");
      Assert.IsTrue(result.Namespaces.Any(x => x.Namespace == "MyNamespace" && !x.HasAlias), "First expected namespace found");
      Assert.IsTrue(result.Namespaces.Any(x => x.Namespace == "ConfiguredNamespace" && !x.HasAlias), "Second expected namespace found");
    }

    [Test]
    public void CreateExpressionSpecification_assigns_aliased_namespaces()
    {
      // Arrange
      _variableDefinitions.Add(_autoFixture.Create<string>(),
                               new UsingNamespaceSpecification("MyNamespace", "my"));

      // Act
      var result = ExerciseSut();

      // Assert
      Assert.AreEqual(1, result.Namespaces.Count(), "Count of namespaces");
      Assert.IsTrue(result.Namespaces.Any(x => x.Namespace == "MyNamespace" && x.Alias == "my"), "Expected namespace found");
    }

    #endregion

    #region methods

    private ExpressionSpecification ExerciseSut()
    {
      return _sut.CreateExpressionSpecification(_autoFixture.Create<string>(), _model);
    }

    #endregion
  }
}

