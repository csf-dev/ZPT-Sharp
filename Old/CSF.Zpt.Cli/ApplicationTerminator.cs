using System;

namespace CSF.Zpt.Cli
{
  public class ApplicationTerminator : IApplicationTerminator
  {
    public static readonly int
      SuccessfulOperationExitCode = 0,
      ExpectedErrorExitCode = 1,
      UnexpectedErrorExitCode = 2;

    public void Terminate()
    {
      this.Terminate(SuccessfulOperationExitCode);
    }

    public void Terminate(int exitCode)
    {
      Environment.Exit(exitCode);
    }
  }
}

