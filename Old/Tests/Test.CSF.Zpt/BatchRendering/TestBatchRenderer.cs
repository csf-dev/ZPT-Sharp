using System;
using NUnit.Framework;
using CSF.Zpt.BatchRendering;
using CSF.Zpt;
using CSF.Zpt.Rendering;
using Moq;
using System.Linq;
using System.IO;
using CSF.Zpt.Tales;

namespace Test.CSF.Zpt.BatchRendering
{
  [TestFixture]
  public class TestBatchRenderer
  {
    #region fields

    private BatchRenderer _sut;

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

    [Test]
    public void GetContextConfigurator_adds_documents_directory_when_path_is_provided()
    {
      // Arrange
      var path = new DirectoryInfo(Environment.CurrentDirectory);
      var job = Mock.Of<IRenderingJob>(x => x.InputRootDirectory == path);

      var model = new Mock<IModelValueStore>();
      model.Setup(x => x.AddGlobal(It.IsAny<string>(), It.IsAny<object>()));

      var ctx = new Mock<IModelValueContainer>();
      ctx.SetupGet(x => x.MetalModel).Returns(model.Object);

      // Act
      var result = _sut.GetContextConfigurator(job);
      result(ctx.Object);

      // Assert
      model
        .Verify(x => x.AddGlobal("documents", It.Is<TemplateDirectory>(t => t.DirectoryInfo == path)),
                Times.Once());
    }

    [Test]
    public void GetContextConfigurator_does_not_add_documents_directory_when_path_is_null()
    {
      // Arrange
      var job = new Mock<IRenderingJob>().Object;
      Mock.Get(job)
          .SetupGet(x => x.InputRootDirectory)
          .Returns((DirectoryInfo) null);

      var model = new Mock<IModelValueStore>();
      model.Setup(x => x.AddGlobal(It.IsAny<string>(), It.IsAny<object>()));

      var ctx = new Mock<IModelValueContainer>();
      ctx.SetupGet(x => x.MetalModel).Returns(model.Object);

      // Act
      var result = _sut.GetContextConfigurator(job);
      result(ctx.Object);

      // Assert
      model
        .Verify(x => x.AddGlobal("documents", It.IsAny<object>()),
                Times.Never());
    }

    #endregion
  }
}

