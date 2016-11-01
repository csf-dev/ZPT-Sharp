using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CSF.IO;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Default implementation of <see cref="IRenderingJobFactory"/>.
  /// </summary>
  public class RenderingJobFactory : IRenderingJobFactory
  {
    #region fields

    private IZptDocumentFactory _documentFactory;

    #endregion

    #region methods

    /// <summary>
    /// Gets the rendering jobs from the batch rendering options.
    /// </summary>
    /// <returns>The rendering jobs.</returns>
    /// <param name="inputOutputInfo">Input output info.</param>
    /// <param name="mode">Mode.</param>
    public IEnumerable<IRenderingJob> GetRenderingJobs(IBatchRenderingOptions inputOutputInfo,
                                                       RenderingMode? mode)
    {
      IEnumerable<IRenderingJob> output;

      if(inputOutputInfo.InputStream != null)
      {
        output = ReadFromStream(inputOutputInfo.InputStream, mode);
      }
      else
      {
        output = ReadFromInputPaths(inputOutputInfo, mode);
      }

      return output;
    }

    private IEnumerable<IRenderingJob> ReadFromStream(Stream stream, RenderingMode? mode)
    {
      Func<IZptDocument> documentCreator;

      if(mode == RenderingMode.Xml)
      {
        documentCreator = () => _documentFactory.CreateDocument(stream, RenderingMode.Xml);
      }
      else
      {
        documentCreator = () => _documentFactory.CreateDocument(stream, RenderingMode.Html);
      }

      return new [] { new RenderingJob(documentCreator) };
    }

    private IEnumerable<IRenderingJob> ReadFromInputPaths(IBatchRenderingOptions inputOutputInfo,
                                                          RenderingMode? mode)
    {
      return inputOutputInfo.InputPaths.SelectMany(x => CreateRenderingJobs(x, inputOutputInfo, mode));
    }

    private IEnumerable<IRenderingJob> CreateRenderingJobs(FileSystemInfo inputPath,
                                                           IBatchRenderingOptions inputOutputInfo,
                                                           RenderingMode? mode)
    {
      IEnumerable<IRenderingJob> output;

      if(inputPath != null && (inputPath is FileInfo))
      {
        output = new [] { CreateRenderingJob((FileInfo) inputPath, mode, inputPath.GetParent()) };
      }
      else if(inputPath != null && (inputPath is DirectoryInfo))
      {
        var dir = (DirectoryInfo) inputPath;

        output = (from file in dir.GetFiles(inputOutputInfo.InputSearchPattern, SearchOption.AllDirectories)
                  where  !inputOutputInfo.IgnoredPaths.Any(x => file.IsChildOf(x))
                  select CreateRenderingJob(file, mode, dir));
      }
      else
      {
        output = new IRenderingJob[0];
      }

      return output;
    }

    private IRenderingJob CreateRenderingJob(FileInfo file, RenderingMode? mode, DirectoryInfo sourceDirectory)
    {
      Func<IZptDocument> documentCreator = () => _documentFactory.CreateDocument(file, renderingMode: mode);

      return new RenderingJob(documentCreator, file, sourceDirectory);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.BatchRendering.RenderingJobFactory"/> class.
    /// </summary>
    /// <param name="documentFactory">Document factory.</param>
    public RenderingJobFactory(IZptDocumentFactory documentFactory = null)
    {
      _documentFactory = documentFactory?? new ZptDocumentFactory();
    }

    #endregion
  }
}

