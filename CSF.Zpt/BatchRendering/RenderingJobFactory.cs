using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CSF.IO;

namespace CSF.Zpt.BatchRendering
{
  internal class RenderingJobFactory
  {
    #region fields

    private IZptDocumentFactory _documentFactory;

    #endregion

    #region methods

    public IEnumerable<RenderingJob> GetRenderingJobs(IBatchRenderingOptions inputOutputInfo,
                                                      RenderingMode? mode)
    {
      IEnumerable<RenderingJob> output;

      if(inputOutputInfo.InputStream != null)
      {
        output = ReadFromStandardInput(mode);
      }
      else
      {
        output = ReadFromInputPaths(inputOutputInfo, mode);
      }

      return output;
    }

    private IEnumerable<RenderingJob> ReadFromStandardInput(RenderingMode? mode)
    {
      IZptDocument output;

      using(var stream = Console.OpenStandardInput())
      {
        if(mode == RenderingMode.Xml)
        {
          output = _documentFactory.CreateDocument(stream, RenderingMode.Xml);
        }
        else
        {
          output = _documentFactory.CreateDocument(stream, RenderingMode.Html);
        }
      }

      return new [] { new RenderingJob(output) };
    }

    private IEnumerable<RenderingJob> ReadFromInputPaths(IBatchRenderingOptions inputOutputInfo,
                                                         RenderingMode? mode)
    {
      return inputOutputInfo.InputPaths.SelectMany(x => CreateRenderingJobs(x, inputOutputInfo, mode));
    }

    private IEnumerable<RenderingJob> CreateRenderingJobs(FileSystemInfo inputPath,
                                                          IBatchRenderingOptions inputOutputInfo,
                                                          RenderingMode? mode)
    {
      IEnumerable<RenderingJob> output;

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
        output = new RenderingJob[0];
      }

      return output;
    }

    private RenderingJob CreateRenderingJob(FileInfo file, RenderingMode? mode, DirectoryInfo sourceDirectory)
    {
      return new RenderingJob(_documentFactory.CreateDocument(file, renderingMode: mode), file, sourceDirectory);
    }

    #endregion

    #region constructor

    public RenderingJobFactory()
    {
      _documentFactory = ZptDocumentFactory.DefaultDocumentFactory;
    }

    #endregion
  }
}

