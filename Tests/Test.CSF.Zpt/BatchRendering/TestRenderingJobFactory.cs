using System;
using NUnit.Framework;
using CSF.Zpt.BatchRendering;
using CSF.Zpt;
using Moq;
using Ploeh.AutoFixture;
using System.IO;
using System.Linq;
using CSF.Zpt.Rendering;
using System.Text;

namespace Test.CSF.Zpt.BatchRendering
{
  [TestFixture]
  public class TestRenderingJobFactory
  {
    #region fields

    private IRenderingJobFactory _sut;
    private Mock<IZptDocumentFactory> _docFactory;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _docFactory = new Mock<IZptDocumentFactory>(MockBehavior.Strict);
      _sut = new RenderingJobFactory(_docFactory.Object);
    }

    #endregion

    #region tests

    [Test]
    public void GetRenderingJobs_reads_from_given_stream()
    {
      // Arrange
      Stream capturedStream = null, inputStream = Mock.Of<Stream>();
      _docFactory
        .Setup(x => x.CreateDocument(It.IsAny<Stream>(), It.IsAny<RenderingMode>(), null, null))
        .Callback((Stream s, RenderingMode m, ISourceInfo i, Encoding e) => {
          capturedStream = s;
        })
        .Returns(Mock.Of<IZptDocument>());
      var options = Mock.Of<IBatchRenderingOptions>(x => x.InputStream == inputStream);

      // Act
      var result = _sut.GetRenderingJobs(options, null);
      var job = result.Single();
      job.GetDocument();

      // Assert
      Assert.AreSame(inputStream, capturedStream);
    }

    [TestCase(RenderingMode.Xml,    RenderingMode.Xml)]
    [TestCase(RenderingMode.Html,   RenderingMode.Html)]
    [TestCase(null,                 RenderingMode.Html)]
    public void GetRenderingJobs_uses_correct_rendering_mode_for_streams(RenderingMode? inputMode,
                                                                         RenderingMode expectedMode)
    {
      // Arrange
      _docFactory
        .Setup(x => x.CreateDocument(It.IsAny<Stream>(), expectedMode, null, null))
        .Returns(Mock.Of<IZptDocument>());
      var options = Mock.Of<IBatchRenderingOptions>(x => x.InputStream == Mock.Of<Stream>());

      // Act
      var result = _sut.GetRenderingJobs(options, inputMode);
      var job = result.Single();
      job.GetDocument();

      // Assert
      _docFactory.Verify(x => x.CreateDocument(It.IsAny<Stream>(), expectedMode, null, null), Times.Once());
    }

    #endregion
  }
}

