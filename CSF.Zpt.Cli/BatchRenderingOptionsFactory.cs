using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CSF.Zpt.BatchRendering;
using CSF.Zpt.Cli.Exceptions;
using CSF.Zpt.Cli.Resources;

namespace CSF.Zpt.Cli
{
  public class BatchRenderingOptionsFactory : IBatchRenderingOptionsFactory
  {
    #region constants

    private const char IGNORED_PATH_SEPARATOR = ';';
    private const string STD_INPUT_SIGNIFIER = "-";

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
                                       ignoredPaths: ignoredPaths,
                                       renderingMode: options.GetRenderingMode());
    }

    private FileSystemInfo GetInputFile(string path)
    {
      FileSystemInfo output;

      if(path == STD_INPUT_SIGNIFIER)
      {
        return null;
      }

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
        throw new InvalidInputPathException(ExceptionMessages.InvalidInputFile) {
          Path = path
        };
      }

      return output;
    }

    private bool ReadFromStandardInput(IEnumerable<FileSystemInfo> inputFiles)
    {
      return inputFiles.Count() == 1 && inputFiles.All(x => x == null);
    }

    private FileSystemInfo GetOutputPath(CommandLineOptions options)
    {
      FileSystemInfo output;

      if(String.IsNullOrEmpty(options.OutputPath))
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
          FileInfo file;
          try
          {
            file = new FileInfo(absolutePath);
          }
          catch(Exception ex)
          {
            throw new InvalidOutputPathException(ExceptionMessages.InvalidOutputFile, ex);
          }

          if(file.Directory.Exists)
          {
            output = file;
          }
          else
          {
            throw new InvalidOutputPathException(ExceptionMessages.InvalidOutputFile);
          }
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

    public BatchRenderingOptionsFactory(DirectoryInfo baseDirectory = null)
    {
      _relativeBase = baseDirectory?? new DirectoryInfo(System.Environment.CurrentDirectory);
    }

    #endregion
  }
}

