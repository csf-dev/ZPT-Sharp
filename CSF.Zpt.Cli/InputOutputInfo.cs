using System;
using System.IO;
using System.Collections.Generic;

namespace CSF.Zpt.Cli
{
  public class InputOutputInfo
  {
    #region properties

    public bool UseStandardInput
    {
      get;
      private set;
    }

    public bool UseStandardOutput
    {
      get;
      private set;
    }

    public IEnumerable<FileSystemInfo> InputPaths
    {
      get;
      private set;
    }

    public IEnumerable<DirectoryInfo> IgnoredPaths
    {
      get;
      private set;
    }

    public FileSystemInfo OutputPath
    {
      get;
      private set;
    }

    public string InputSearchPattern
    {
      get;
      private set;
    }

    public string OutputExtensionOverride
    {
      get;
      private set;
    }

    #endregion

    #region constructors

    public InputOutputInfo(bool useStdin,
                           IEnumerable<FileSystemInfo> inputPaths = null,
                           FileSystemInfo outputPath = null,
                           string inputSearchPattern = null,
                           string outputExtensionOverride = null,
                           IEnumerable<DirectoryInfo> ignoredPaths = null)
    {
      UseStandardInput = useStdin;
      UseStandardOutput = (outputPath == null);
      InputPaths = inputPaths?? new FileSystemInfo[0];
      OutputPath = outputPath;
      InputSearchPattern = inputSearchPattern?? "*";
      OutputExtensionOverride = outputExtensionOverride;
      IgnoredPaths = ignoredPaths?? new DirectoryInfo[0];
    }

    #endregion
  }
}

