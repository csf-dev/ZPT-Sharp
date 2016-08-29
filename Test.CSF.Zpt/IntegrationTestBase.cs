using System;
using System.IO;
using CSF.Zpt;
using CSF.Zpt.Tales;
using NUnit.Framework;
using CSF.Configuration;
using CSF.Zpt.Rendering;
using System.Collections.Generic;
using System.Linq;
using CSF.IO;

namespace Test.CSF.Zpt
{
  public abstract class IntegrationTestBase
  {
    #region fields

    private IZptDocumentFactory _documentFactory;
    private ITemplateFileFactory _templateFactory;
    private IIntegrationTestConfiguration _config;
    private DirectoryInfo _sourcePath, _expectedPath;
    private log4net.ILog _logger;

    #endregion

    #region properties

    protected log4net.ILog Logger
    {
      get {
        return _logger;
      }
    }

    protected IZptDocumentFactory DocumentFactory
    {
      get {
        return _documentFactory;
      }
    }

    protected ITemplateFileFactory TemplateFactory
    {
      get {
        return _templateFactory;
      }
    }

    protected DirectoryInfo SourcePath
    {
      get {
        return _sourcePath;
      }
    }

    protected DirectoryInfo ExpectedPath
    {
      get {
        return _expectedPath;
      }
    }

    #endregion

    #region setup & teardown

    [TestFixtureSetUp]
    public void FixtureSetup()
    {
      PerformDefaultFixtureSetup();
      PerformExtraFixtureSetup();
    }

    [SetUp]
    public void Setup()
    {
      PerformDefaultSetup();
      PerformExtraSetup();
    }

    #endregion

    #region methods

    protected virtual void PerformDefaultFixtureSetup()
    {
      _logger = log4net.LogManager.GetLogger(this.GetType());

      IIntegrationTestConfiguration configuredConfig = ConfigurationHelper.GetSection<IntegrationTestConfiguration>();

      _config = configuredConfig?? new FallbackIntegrationTestConfiguration();

      _sourcePath = GetSourcePath(_config);
      _expectedPath = GetExpectedPath(_config);
    }

    protected virtual void PerformDefaultSetup()
    {
      var fac = new ZptDocumentFactory();
      _documentFactory = fac;
      _templateFactory = fac;
    }

    protected virtual void PerformExtraFixtureSetup()
    {
      // Intentional no-op
    }

    protected virtual void PerformExtraSetup()
    {
      // Intentional no-op
    }

    protected virtual DirectoryInfo GetSourcePath(IIntegrationTestConfiguration config)
    {
      return config.GetSourceDocumentPath();
    }

    protected virtual DirectoryInfo GetExpectedPath(IIntegrationTestConfiguration config)
    {
      return config.GetExpectedOutputPath();
    }

    protected virtual IRenderingOptions GetRenderingOptions(IRenderingContextFactory contextFactory)
    {
      return new RenderingOptions(contextFactory: contextFactory);
    }

    protected virtual IRenderingContextFactory CreateTestEnvironment(DirectoryInfo rootPath)
    {
      var output = new TalesRenderingContextFactory();
      output.RootDocumentPath = rootPath.FullName;
      return output;
    }

    protected virtual string Render(IZptDocument document, IRenderingOptions options)
    {
      return document.Render(options);
    }

    protected bool PerformTestRun(FileInfo sourceDocument,
                                  FileInfo expectedResultDocument)
    {
      bool output = false;
      IZptDocument document = null;
      string expectedRendering, actualRendering = null;
      bool exceptionCaught = false;

      try
      {
        document = _documentFactory.CreateDocument(sourceDocument);
      }
      catch(Exception ex)
      {
        _logger.ErrorFormat("Exception caught whilst loading the source document:{0}{1}{2}",
                            expectedResultDocument.Name,
                            Environment.NewLine,
                            ex);
        exceptionCaught = true;
      }

      if(!exceptionCaught)
      {
        using(var stream = expectedResultDocument.OpenRead())
        using(var reader = new StreamReader(stream))
        {
          expectedRendering = reader.ReadToEnd();
        }

        try
        {
          var rootDir = sourceDocument.GetParent().GetParent();
          var options = GetRenderingOptions(this.CreateTestEnvironment(rootDir));

          actualRendering = this.Render(document, options).Replace(Environment.NewLine, "\n");
          output = (actualRendering == expectedRendering);
        }
        catch(Exception ex)
        {
          _logger.ErrorFormat("Exception caught whilst processing output file:{0}{1}{2}",
                              expectedResultDocument.Name,
                              Environment.NewLine,
                              ex);
          output = false;
          exceptionCaught = true;
        }
      }

      if(!output && !exceptionCaught)
      {
        _logger.ErrorFormat("Unexpected rendering whilst processing expected output:{0}{1}{2}",
                            expectedResultDocument.Name,
                            Environment.NewLine,
                            actualRendering);
      }

      return output;
    }

    protected IEnumerable<Tuple<FileInfo,FileInfo>> GetFilePairsToTest()
    {
      return (from expectedFile in _expectedPath.GetFiles()
              let filename = expectedFile.Name
              join sourceFile in _sourcePath.GetFiles() on
              filename equals sourceFile.Name
              select new Tuple<FileInfo,FileInfo>(sourceFile, expectedFile));
    }

    #endregion
  }
}

