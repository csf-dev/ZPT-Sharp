using System;
using System.IO;

namespace CSF.Zpt.Cli
{
  public class RenderingJob
  {
    #region properties

    public IZptDocument Document
    {
      get;
      private set;
    }

    public FileInfo SourceFile
    {
      get;
      private set;
    }

    public DirectoryInfo SourceDirectory
    {
      get;
      private set;
    }

    #endregion

    #region constructor

    public RenderingJob(IZptDocument document,
                        FileInfo sourceFile = null,
                        DirectoryInfo sourceDirectory = null)
    {
      Document = document;
      SourceFile = sourceFile;
      SourceDirectory = sourceDirectory;
    }

    #endregion
  }
}

