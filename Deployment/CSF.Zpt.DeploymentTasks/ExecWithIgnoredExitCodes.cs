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
        
      var tool_path = GenerateFullPathToTool();

      if(tool_path == null)
      {
        return false;
      }

      _exitCode = ExecuteTool(tool_path,
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

