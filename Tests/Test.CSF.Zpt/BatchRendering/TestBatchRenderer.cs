using System;
using NUnit.Framework;
using CSF.Zpt.BatchRendering;
using CSF.Zpt;
using CSF.Zpt.Rendering;
using Moq;
using System.Linq;
using System.IO;

namespace Test.CSF.Zpt.BatchRendering
{
  [TestFixture]
  public class TestBatchRenderer
  {
    #region fields

    private IBatchRenderer _sut;

    private Mock<IRenderingJobFactory> _jobFactory;
    private Mock<IRenderingSettingsFactory> _settingsFactory;
    private Mock<IBatchRenderingOptionsValidator> _optionsValidator;

    private Stream _outputStream;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      _jobFactory = new Mock<IRenderingJobFactory>();
      _settingsFactory = new Mock<IRenderingSettingsFactory>();
      _optionsValidator = new Mock<IBatchRenderingOptionsValidator>();

      _sut = new BatchRenderer(_jobFactory.Object,
                               _settingsFactory.Object,
                               _optionsValidator.Object);

      _outputStream = new MemoryStream();
    }

    [TearDown]
    public void Teardown()
    {
      if(_outputStream != null)
      {
        _outputStream.Dispose();
      }
    }

    #endregion

    #region tests

    [Test]
    public void Render_validates_batch_options()
    {
      // Arrange
      _jobFactory
        .Setup(x => x.GetRenderingJobs(It.IsAny<IBatchRenderingOptions>(), It.IsAny<RenderingMode?>()))
        .Returns(Enumerable.Empty<IRenderingJob>());

      // Act
      _sut.Render(Mock.Of<IBatchRenderingOptions>());

      // Assert
      _optionsValidator.Verify(x => x.Validate(It.IsAny<IBatchRenderingOptions>()), Times.Once());
    }

    [Test]
    public void Render_uses_job_factory()
    {
      // Arrange
      _jobFactory
        .Setup(x => x.GetRenderingJobs(It.IsAny<IBatchRenderingOptions>(), It.IsAny<RenderingMode?>()))
        .Returns(Enumerable.Empty<IRenderingJob>());

      // Act
      _sut.Render(Mock.Of<IBatchRenderingOptions>());

      // Assert
      _jobFactory
        .Verify(x => x.GetRenderingJobs(It.IsAny<IBatchRenderingOptions>(), It.IsAny<RenderingMode?>()),
                Times.Once());
    }

    #endregion
  }
}

