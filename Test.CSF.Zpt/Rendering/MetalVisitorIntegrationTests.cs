using System;
using NUnit.Framework;
using CSF.Zpt.Rendering;
using Moq;
using System.Reflection;
using CSF.Reflection;

namespace Test.CSF.Zpt.Rendering
{
  [TestFixture]
  [Category("Integration")]
  public class MetalVisitorIntegrationTests
  {
    #region fields

    private Mock<Model> _model;

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
      _model = new Mock<Model>(MockBehavior.Strict);
    }

    #endregion

    #region tests

    [Test]
    public void TestVisit()
    {
      // Arrange
      var sourceFile = Mock.Of<SourceFileInfo>();
      var document = new XmlElement(_documentOne.DocumentElement, sourceFile);
      var macroOne = new XmlElement(_documentTwo.DocumentElement.ChildNodes[0], sourceFile);
      var macroTwo = new XmlElement(_documentTwo.DocumentElement.ChildNodes[1], sourceFile);
      var macroBase = new XmlElement(_documentTwo.DocumentElement.ChildNodes[2], sourceFile);

      _model.Setup(x => x.Evaluate("macro-one")).Returns(new ExpressionResult(true, macroOne));
      _model.Setup(x => x.Evaluate("macro-two")).Returns(new ExpressionResult(true, macroTwo));
      _model.Setup(x => x.Evaluate("macro-base")).Returns(new ExpressionResult(true, macroBase));

      var sut = new MetalVisitor();

      // Act
      sut.VisitRecursively(document, _model.Object);
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

