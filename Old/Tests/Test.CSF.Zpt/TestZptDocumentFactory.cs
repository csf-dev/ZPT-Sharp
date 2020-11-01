using System;
using NUnit.Framework;
using CSF.Zpt;
using Moq;
using System.IO;
using System.Text;
using Ploeh.AutoFixture;
using CSF.Zpt.Rendering;

namespace Test.CSF.Zpt
{
  [TestFixture]
  public class TestZptDocumentFactory
  {
    #region fields

    private IZptDocumentFactory _sut;

    private Mock<IZptDocumentProviderService> _providerService;
    private Mock<IZptDocumentProvider> _provider;
    private IFixture _autofixture;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _autofixture = new Fixture();

      _providerService = new Mock<IZptDocumentProviderService>(MockBehavior.Strict);
      _provider = new Mock<IZptDocumentProvider>();

      _sut = new ZptDocumentFactory(_providerService.Object);
    }

    #endregion

    #region tests

    [TestCase(  "foo.html",   true,   RenderingMode.Html)]
    [TestCase(  "foo.pt",     true,   RenderingMode.Html)]
    [TestCase(  "foo.xhtml",  true,   RenderingMode.Xml)]
    [TestCase(  "foo.xml",    true,   RenderingMode.Xml)]
    [TestCase(  "foo.zzz",    false,  0)]
    public void TryDetectMode_returns_expected_result(string filename, bool expectedResult, RenderingMode expectedMode)
    {
      // Arrange
      RenderingMode actualMode;
      var file = new FileInfo(filename);

      // Act
      bool result = _sut.TryDetectMode(file, out actualMode);

      // Assert
      Assert.AreEqual(expectedResult, result, "Correct result");
      if(expectedResult)
      {
        Assert.AreEqual(expectedMode, actualMode, "Detected mode");
      }
    }

    [Test]
    public void CreateDocument_with_file_and_html_mode_uses_appropriate_provider()
    {
      // Arrange
      var createdDocument = Mock.Of<IZptDocument>();
      _provider.Setup(x => x.CreateDocument(It.IsAny<FileInfo>(), It.IsAny<Encoding>())).Returns(createdDocument);

      _providerService.Setup(x => x.GetDefaultHtmlProvider()).Returns(_provider.Object);

      var file = new FileInfo(_autofixture.Create<string>());

      // Act
      var document = _sut.CreateDocument(file, renderingMode: RenderingMode.Html);

      // Assert
      Assert.AreSame(createdDocument, document);
    }

    [Test]
    public void CreateDocument_with_file_and_xml_mode_uses_appropriate_provider()
    {
      // Arrange
      var createdDocument = Mock.Of<IZptDocument>();
      _provider.Setup(x => x.CreateDocument(It.IsAny<FileInfo>(), It.IsAny<Encoding>())).Returns(createdDocument);

      _providerService.Setup(x => x.GetDefaultXmlProvider()).Returns(_provider.Object);

      var file = new FileInfo(_autofixture.Create<string>());

      // Act
      var document = _sut.CreateDocument(file, renderingMode: RenderingMode.Xml);

      // Assert
      Assert.AreSame(createdDocument, document);
    }

    [Test]
    public void CreateDocument_with_file_and_provider_type_uses_appropriate_provider()
    {
      // Arrange
      var createdDocument = Mock.Of<IZptDocument>();
      _provider.Setup(x => x.CreateDocument(It.IsAny<FileInfo>(), It.IsAny<Encoding>())).Returns(createdDocument);

      _providerService.Setup(x => x.GetProvider(typeof(string))).Returns(_provider.Object);

      var file = new FileInfo(_autofixture.Create<string>());

      // Act
      var document = _sut.CreateDocument(file, typeof(string));

      // Assert
      Assert.AreSame(createdDocument, document);
    }

    [Test]
    public void CreateDocument_with_stream_and_html_mode_uses_appropriate_provider()
    {
      // Arrange
      var createdDocument = Mock.Of<IZptDocument>();
      _provider.Setup(x => x.CreateDocument(It.IsAny<Stream>(), It.IsAny<ISourceInfo>(), It.IsAny<Encoding>())).Returns(createdDocument);

      _providerService.Setup(x => x.GetDefaultHtmlProvider()).Returns(_provider.Object);

      var stream = new MemoryStream();

      // Act
      var document = _sut.CreateDocument(stream, RenderingMode.Html);

      // Assert
      Assert.AreSame(createdDocument, document);
    }

    [Test]
    public void CreateDocument_with_stream_and_xml_mode_uses_appropriate_provider()
    {
      // Arrange
      var createdDocument = Mock.Of<IZptDocument>();
      _provider.Setup(x => x.CreateDocument(It.IsAny<Stream>(), It.IsAny<ISourceInfo>(), It.IsAny<Encoding>())).Returns(createdDocument);

      _providerService.Setup(x => x.GetDefaultXmlProvider()).Returns(_provider.Object);

      var stream = new MemoryStream();

      // Act
      var document = _sut.CreateDocument(stream, RenderingMode.Xml);

      // Assert
      Assert.AreSame(createdDocument, document);
    }

    [Test]
    public void CreateDocument_with_stream_and_provider_type_uses_appropriate_provider()
    {
      // Arrange
      var createdDocument = Mock.Of<IZptDocument>();
      _provider.Setup(x => x.CreateDocument(It.IsAny<Stream>(), It.IsAny<ISourceInfo>(), It.IsAny<Encoding>())).Returns(createdDocument);

      _providerService.Setup(x => x.GetProvider(typeof(string))).Returns(_provider.Object);

      var stream = new MemoryStream();

      // Act
      var document = _sut.CreateDocument(stream, typeof(string));

      // Assert
      Assert.AreSame(createdDocument, document);
    }

    #endregion
  }
}

