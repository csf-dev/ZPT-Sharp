using System;
using System.IO;

namespace CSF.Zpt.BatchRendering
{
  public interface IRenderingJob
  {
    DirectoryInfo InputRootDirectory { get; }

    IZptDocument GetDocument();

    string GetOutputInfo(IBatchRenderingOptions batchOptions);

    Stream GetOutputStream(IBatchRenderingOptions batchOptions);
  }
}

