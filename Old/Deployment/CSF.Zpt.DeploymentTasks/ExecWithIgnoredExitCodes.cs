using System;
using Microsoft.Build.Tasks;
using Microsoft.Build.Framework;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Zpt.DeploymentTasks
{
  public class ExecWithIgnoredExitCodes : Exec
  {
    #region constants

    private const char SEPARATOR = ',';

    #endregion

    #region fields

    private int? _exitCode;

    #endregion

    #region properties

    [Required]
    public string IgnoredExitCodes { get; set; }


    [Output]
    public new int ExitCode
    {
      get {
        if(_exitCode.HasValue)
        {
          return _exitCode.Value;
        }
        else
        {
          return base.ExitCode;
        }
      }
    }

    #endregion

    #region methods

    public override bool Execute()
    {
      if(SkipTaskExecution())
      {
        return true;
      }
        
      _exitCode = ExecuteTool(GenerateFullPathToTool(),
                              GenerateResponseFileCommands(),
                              GenerateCommandLineCommands());

      if(_exitCode.Value > 0)
      {
        var ignoredCodes = GetIgnoredExitCodes();
        if(ignoredCodes.Contains(_exitCode.Value))
        {
          return true;
        }
        else
        {
          return HandleTaskExecutionErrors();
        }
      }

      return true;
    }

    protected override string GetWorkingDirectory()
    {
      if(!String.IsNullOrWhiteSpace(WorkingDirectory))
        return WorkingDirectory;
      
      return Environment.CurrentDirectory;
    }

    private IEnumerable<int> GetIgnoredExitCodes()
    {
      return IgnoredExitCodes
        .Split(SEPARATOR)
        .Select(x => {
          int output;

          if(!Int32.TryParse(x, out output))
          {
            output = 0;
          }

          return output;
        })
        .Where(x => x > 0)
        .ToArray();
    }

    #endregion
  }
}

