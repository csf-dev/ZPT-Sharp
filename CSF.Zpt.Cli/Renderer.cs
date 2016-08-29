using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using CSF.IO;
using CSF.Zpt.Rendering;
using CSF.Zpt.Tales;

namespace CSF.Zpt.Cli
{
  public class Renderer
  {
    #region fields

    private IZptDocumentFactory _documentFactory;

    #endregion

    #region public API

    public void Render(IRenderingOptions options,
                       InputOutputInfo inputOutputInfo,
                       RenderingMode? mode)
    {
      var jobs = GetRenderingJobs(inputOutputInfo, mode);

      foreach(var job in jobs)
      {
        Action<RenderingContext> contextConfigurator = ctx => {
          if(job.SourceDirectory != null)
          {
            var docRoot = new TemplateDirectory(job.SourceDirectory);
            ctx.MetalModel.AddGlobal("documents", docRoot);
          }
        };

        Render(job, options, inputOutputInfo, contextConfigurator);
      }
    }

    private IEnumerable<RenderingJob> GetRenderingJobs(InputOutputInfo inputOutputInfo, RenderingMode? mode)
    {
      IEnumerable<RenderingJob> output;

      if(inputOutputInfo.UseStandardInput)
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

    private IEnumerable<RenderingJob> ReadFromInputPaths(InputOutputInfo inputOutputInfo,
                                                         RenderingMode? mode)
    {
      return inputOutputInfo.InputPaths.SelectMany(x => CreateRenderingJobs(x, inputOutputInfo, mode));
    }

    private IEnumerable<RenderingJob> CreateRenderingJobs(FileSystemInfo inputPath,
                                                          InputOutputInfo inputOutputInfo,
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

    private void Render(RenderingJob job,
                        IRenderingOptions options,
                        InputOutputInfo inputOutputInfo,
                        Action<RenderingContext> contextConfigurator)
    {
      using(var outputStream = GetOutputStream(job, inputOutputInfo))
      using(var writer = new StreamWriter(outputStream, options.OutputEncoding))
      {
        job.Document.Render(writer,
                            options: options,
                            contextConfigurator: contextConfigurator);
      }
    }

    private Stream GetOutputStream(RenderingJob job, InputOutputInfo inputOutputInfo)
    {
      Stream output;

      if(inputOutputInfo.UseStandardOutput)
      {
        output = Console.OpenStandardOutput();
      }
      else if(inputOutputInfo.OutputPath is FileInfo)
      {
        output = ((FileInfo) inputOutputInfo.OutputPath).Open(FileMode.Create);
      }
      else
      {
        var outputFile = GetOutputFile(job, inputOutputInfo);
          
        var parentDir = outputFile.GetParent();
        if(!parentDir.Exists)
        {
          parentDir.CreateRecursive();
        }

        output = outputFile.Open(FileMode.Create);
      }

      return output;
    }

    private FileInfo GetOutputFile(RenderingJob job, InputOutputInfo inputOutputInfo)
    {
      var extension = job.SourceFile.Extension;
      var filenameWithoutExtension = job.SourceFile.Name.Substring(0, job.SourceFile.Name.Length - extension.Length);
      string newFilename;

      if(inputOutputInfo.OutputExtensionOverride != null)
      {
        string newExtension = inputOutputInfo.OutputExtensionOverride;
        if(!newExtension.StartsWith("."))
        {
          newExtension = String.Concat(".", newExtension);
        }

        newFilename = String.Concat(filenameWithoutExtension, newExtension);
      }
      else if(job.Document.Mode == RenderingMode.Xml)
      {
        newFilename = String.Concat(filenameWithoutExtension, ".xml");
      }
      else
      {
        newFilename = String.Concat(filenameWithoutExtension, ".html");
      }

      var tempOutputPath = new FileInfo(System.IO.Path.Combine(job.SourceFile.GetParent().FullName, newFilename));

      var relativePath = tempOutputPath.GetRelative(job.SourceDirectory);
      if(relativePath.StartsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
      {
        relativePath = relativePath.Substring(1);
      }
      var outputPath = inputOutputInfo.OutputPath.FullName;
      return new FileInfo(System.IO.Path.Combine(outputPath, relativePath));
    }

    #endregion

    #region constructors

    public Renderer(IZptDocumentFactory documentFactory = null)
    {
      _documentFactory = documentFactory?? new ZptDocumentFactory();
    }

    #endregion
  }
}

