using System;
using NUnit.Framework;
using Ploeh.AutoFixture;
using CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec;

namespace Test.CSF.Zpt.ExpressionEvaluators.CSharpExpressions.Spec
{
  [TestFixture]
  public class TestExpressionSpecification
  {
    #region fields

    private IFixture _autofixture;
    private ExpressionSpecification _sut, _equalSpec, _differentSpec;

    #endregion

    #region setup

    [TestFixtureSetUp]
    public void FixtureSetup()
    {
      // Implement or delete as required
    }

    [SetUp]
    public void Setup()
    {
      _autofixture = new Fixture();

      SetupSpecifications();
    }

    #endregion

    #region tests

    [Test]
    public void Equals_returns_true_when_two_specifications_are_value_equal()
    {
      // Act & assert
      Assert.IsTrue(_sut.Equals(_equalSpec));
    }

    [Test]
    public void Equals_returns_false_when_two_specifications_are_different()
    {
      // Act & assert
      Assert.IsFalse(_sut.Equals(_differentSpec));
    }

    [Test]
    public void Equals_object_returns_true_when_two_specifications_are_value_equal()
    {
      // Act & assert
      Assert.IsTrue(_sut.Equals((object) _equalSpec));
    }

    [Test]
    public void Equals_object_returns_false_when_two_specifications_are_different()
    {
      // Act & assert
      Assert.IsFalse(_sut.Equals((object) _differentSpec));
    }

    [Test]
    public void GetHashCode_returns_same_value_for_equal_specifications()
    {
      // Arrange
      var expected = _equalSpec.GetHashCode();

      // Act
      var result = _sut.GetHashCode();

      // Assert
      Assert.AreEqual(expected, result);
    }

    #endregion

    #region methods

    private void SetupSpecifications()
    {
      var variables1 = new [] {
        new VariableSpecification(_autofixture.Create<string>()),
        new VariableSpecification(_autofixture.Create<string>(), _autofixture.Create<string>()),
      };
      var references1 = new [] {
        new ReferencedAssemblySpecification(_autofixture.Create<string>()),
      };
      var using1 = new [] {
        new UsingNamespaceSpecification(_autofixture.Create<string>()),
        new UsingNamespaceSpecification(_autofixture.Create<string>(), _autofixture.Create<string>()),
      };

      _sut = new ExpressionSpecification(_autofixture.Create<string>(),
                                         variables1,
                                         references1,
                                         using1);

      var variables2 = new [] {
        new VariableSpecification(variables1[0].Name),
        new VariableSpecification(variables1[1].Name, variables1[1].TypeName),
      };
      var references2 = new [] {
        new ReferencedAssemblySpecification(references1[0].Name),
      };
      var using2 = new [] {
        new UsingNamespaceSpecification(using1[0].Namespace),
        new UsingNamespaceSpecification(using1[1].Namespace, using1[1].Alias),
      };

      _equalSpec = new ExpressionSpecification(_sut.Text,
                                               variables2,
                                               references2,
                                               using2);

      var variables3 = new [] {
        new VariableSpecification(_autofixture.Create<string>()),
        new VariableSpecification(_autofixture.Create<string>(), _autofixture.Create<string>()),
      };
      var references3 = new [] {
        new ReferencedAssemblySpecification(_autofixture.Create<string>()),
      };
      var using3 = new [] {
        new UsingNamespaceSpecification(_autofixture.Create<string>()),
        new UsingNamespaceSpecification(_autofixture.Create<string>(), _autofixture.Create<string>()),
      };

      _differentSpec = new ExpressionSpecification(_autofixture.Create<string>(),
                                                   variables3,
                                                   references3,
                                                   using3);
      
    }

    #endregion
  }
}

