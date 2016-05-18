using System;
using NUnit.Framework;
using CSF.Zpt.Rendering;
using Test.CSF.Zpt.Rendering;
using Moq;
using System.Reflection;
using CSF.Reflection;
using Test.CSF.Zpt.Util;
using CSF.Zpt.Metal;
using Ploeh.AutoFixture;
using Test.CSF.Zpt.Util.Autofixture;

namespace Test.CSF.Zpt.Metal
{
  [TestFixture]
  [Category("Integration")]
  public class MetalVisitorIntegrationTests
  {
    #region fields

    private IFixture _fixture;
    private DummyModel _model;

    private string _xmlStringOne, _xmlStringTwo;
    private System.Xml.XmlDocument _documentOne, _documentTwo;

    private log4net.ILog _logger;

    #endregion

    #region setup

    [TestFixtureSetUp]
    public void FixtureSetup()
    {
      _logger = log4net.LogManager.GetLogger(this.GetType());

      _xmlStringOne = Assembly.GetExecutingAssembly().GetManifestResourceText(this.GetType(),
                                                                              "MetalVisitorIntegrationTests.1.xml");
      _xmlStringTwo = Assembly.GetExecutingAssembly().GetManifestResourceText(this.GetType(),
                                                                              "MetalVisitorIntegrationTests.2.xml");

      _documentOne = new System.Xml.XmlDocument();
      _documentTwo = new System.Xml.XmlDocument();

      _documentOne.LoadXml(_xmlStringOne);
      _documentTwo.LoadXml(_xmlStringTwo);
    }

    [SetUp]
    public void Setup()
    {
      _fixture = new Fixture();
      new DummyModelCustomisation().Customize(_fixture);
      new RenderingOptionsCustomisation().Customize(_fixture);

      _model = _fixture.Create<DummyModel>();
    }

    #endregion

    #region tests

    [Test]
    public void TestVisitRoot()
    {
      // Arrange
      var sourceFile = Mock.Of<SourceFileInfo>();
      var document = new ZptXmlElement(_documentOne.DocumentElement, sourceFile);
      var macroOne = new ZptXmlElement(_documentTwo.DocumentElement.ChildNodes[0], sourceFile);
      var macroTwo = new ZptXmlElement(_documentTwo.DocumentElement.ChildNodes[1], sourceFile);
      var macroBase = new ZptXmlElement(_documentTwo.DocumentElement.ChildNodes[2], sourceFile);

      _model.AddLocal("macro-one", macroOne);
      _model.AddLocal("macro-two", macroTwo);
      _model.AddLocal("macro-base", macroBase);

      var sut = new MetalVisitor();
      var ctx = new RenderingContext(_model,
                                     _fixture.Create<DummyModel>(),
                                     document,
                                     _fixture.Create<DefaultRenderingOptions>());

      // Act
      sut.VisitContext(ctx);
      new MetalTidyUpVisitor().VisitContext(ctx);

      var result = document.ToString();

      // Assert
      string expected = @"<root xmlns=""http://ns.csf-dev.com/sample"">
  <ele>
    <base>Base content</base>
    <blurb>
      Extra slot decoration
      <bar>Macro one, slot one content</bar></blurb>
    <base>Base content</base>
    <bar>Macro one, slot two content</bar>
    <base>Base content</base>
  </ele>
  <foo />
  <ele>
    <baz>BAZ content</baz>
    <bar>Macro two, slot one content</bar>
    <baz>BAZ content</baz>
    <bar>Macro two, slot two content</bar>
    <baz>BAZ content</baz>
  </ele>
</root>";

      try
      {
        Assert.AreEqual(expected, result);
      }
      catch(AssertionException)
      {
        _logger.Error(result);
        throw;
      }
    }

    #endregion
  }
}

