using System;

namespace CSF.Zpt.Cli
{
  public interface IApplicationTerminator
  {
    void Terminate();

    void Terminate(int exitCode);
  }
}

