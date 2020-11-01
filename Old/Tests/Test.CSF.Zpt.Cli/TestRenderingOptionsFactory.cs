using System;
using NUnit.Framework;
using CSF.Zpt.Cli;
using Moq;
using Ploeh.AutoFixture;

namespace Test.CSF.Zpt.Cli
{
  [TestFixture]
  public class TestRenderingOptionsFactory
  {
    #region fields

    private IRenderingOptionsFactory _sut;
    private IFixture _autofixture;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _autofixture = new Fixture();
      _sut = new RenderingOptionsFactory();
    }

    #endregion

    #region tests

    [Theory]
    public void GetOptions_returns_expected_source_annotation_value(bool value)
    {
      // Arrange
      var input = new CommandLineOptions() {
        EnableSourceAnnotation = value
      };

      // Act
      var result = _sut.GetOptions(input);

      // Assert
      Assert.AreEqual(value, result.AddSourceFileAnnotation);
    }

    [Theory]
    public void GetOptions_returns_expected_omit_xml_declaration_value(bool value)
    {
      // Arrange
      var input = new CommandLineOptions() {
        OmitXmlDeclarations = value
      };

      // Act
      var result = _sut.GetOptions(input);

      // Assert
      Assert.AreEqual(value, result.OmitXmlDeclaration);
    }

    [Test]
    public void GetOptions_returns_expected_context_visitor_type_names()
    {
      // Arrange
      var value = _autofixture.Create<string>();
      var input = new CommandLineOptions() {
        ContextVisitorClassNames = value
      };

      // Act
      var result = _sut.GetOptions(input);

      // Assert
      Assert.AreEqual(value, result.ContextVisitorTypes);
    }

    [Test]
    public void GetOptions_returns_expected_rendering_context_factory_type_names()
    {
      // Arrange
      var value = _autofixture.Create<string>();
      var input = new CommandLineOptions() {
        RenderingContextFactoryClassName = value
      };

      // Act
      var result = _sut.GetOptions(input);

      // Assert
      Assert.AreEqual(value, result.RenderingContextFactoryType);
    }

    [Test]
    public void GetOptions_returns_expected_output_encoding()
    {
      // Arrange
      var value = _autofixture.Create<string>();
      var input = new CommandLineOptions() {
        OutputEncoding = value
      };

      // Act
      var result = _sut.GetOptions(input);

      // Assert
      Assert.AreEqual(value, result.OutputEncodingName);
    }

    [Test]
    public void GetOptions_returns_expected_keyword_options()
    {
      // Arrange

      var input = new CommandLineOptions() {
        KeywordOptions = "foo=bar;wibble=wobble;spork=splife"
      };

      // Act
      var result = _sut.GetOptions(input);

      // Assert
      Assert.AreEqual(3, result.KeywordOptions.Count, "Count of keyword options");
      Assert.AreEqual("bar",    result.KeywordOptions["foo"],     "First option as expected");
      Assert.AreEqual("wobble", result.KeywordOptions["wibble"],  "Second option as expected");
      Assert.AreEqual("splife", result.KeywordOptions["spork"],   "Third option as expected");
    }

    #endregion
  }
}

