using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CSF.Zpt.BatchRendering;

namespace CSF.Zpt.Cli
{
  public class BatchRenderingOptionsCreator : IBatchRenderingOptionsCreator
  {
    #region constants

    private const char IGNORED_PATH_SEPARATOR = ';';

    #endregion

    #region fields

    private DirectoryInfo _relativeBase;

    #endregion

    #region methods

    public IBatchRenderingOptions GetBatchOptions(CommandLineOptions options)
    {
      var inputFiles = options.InputPaths.Select(GetInputFile).ToArray();
      var useStdin = ReadFromStandardInput(inputFiles);
      var ignoredPaths = GetIgnoredPaths(options);

      var outputPath = GetOutputPath(options);

      return new BatchRenderingOptions(inputStream: useStdin? Console.OpenStandardInput() : null,
                                       outputStream: useStdin? Console.OpenStandardOutput() : null,
                                       inputPaths: inputFiles,
                                       outputPath: outputPath,
                                       inputSearchPattern: options.InputFilenamePattern,
                                       outputExtensionOverride: options.OutputFilenameExtension,
                                       ignoredPaths: ignoredPaths);
    }

    private FileSystemInfo GetInputFile(string path)
    {
      FileSystemInfo output;
      var absolutePath = MakeAbsolutePath(path);

      if(File.Exists(absolutePath))
      {
        output = new FileInfo(absolutePath);
      }
      else if(Directory.Exists(absolutePath))
      {
        output = new DirectoryInfo(absolutePath);
      }
      else
      {
        output = null;
      }

      return output;
    }

    private bool ReadFromStandardInput(IEnumerable<FileSystemInfo> inputFiles)
    {
      return !inputFiles.Any() || inputFiles.All(x => x.Name == "-");
    }

    private FileSystemInfo GetOutputPath(CommandLineOptions options)
    {
      FileSystemInfo output;

      if(options.OutputPath == null)
      {
        output = null;
      }
      else
      {
        var absolutePath = MakeAbsolutePath(options.OutputPath);
        if(Directory.Exists(absolutePath))
        {
          output = new DirectoryInfo(absolutePath);
        }
        else
        {
          var file = new FileInfo(absolutePath);
          output = file.Directory.Exists? file : null;
        }
      }

      return output;
    }

    private IEnumerable<DirectoryInfo> GetIgnoredPaths(CommandLineOptions options)
    {
      IEnumerable<DirectoryInfo> output;

      if(String.IsNullOrEmpty(options.IgnoredPaths))
      {
        output = new DirectoryInfo[0];
      }
      else
      {
        output = options.IgnoredPaths
          .Split(IGNORED_PATH_SEPARATOR)
          .Select(x => {
          var absolutePath = MakeAbsolutePath(x);
          return Directory.Exists(absolutePath)? new DirectoryInfo(absolutePath) : null;
        });
      }

      return output;
    }

    private string MakeAbsolutePath(string path)
    {
      return Path.IsPathRooted(path)? path : Path.Combine(_relativeBase.FullName, path);
    }

    #endregion

    #region constructor

    public BatchRenderingOptionsCreator()
    {
      _relativeBase = new DirectoryInfo(System.Environment.CurrentDirectory);
    }

    #endregion
  }
}

