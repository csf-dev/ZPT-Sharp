using System;
using System.IO;

namespace CSF.Zpt.BatchRendering
{
  public interface IRenderingJob
  {
    IZptDocument Document { get; }

    DirectoryInfo InputRootDirectory { get; }

    string GetOutputInfo(IBatchRenderingOptions batchOptions);

    Stream GetOutputStream(IBatchRenderingOptions batchOptions);
  }
}

