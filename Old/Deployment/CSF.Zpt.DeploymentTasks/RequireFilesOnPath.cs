using System;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;
using System.Linq;

namespace CSF.Zpt.DeploymentTasks
{
  public class RequireFilesOnPath : Task
  {
    #region constants

    private const string PATH_VARIABLE = "PATH";

    #endregion

    #region fields

    private readonly string[] environmentPaths = Environment
      .GetEnvironmentVariable(PATH_VARIABLE)
      .Split(Path.PathSeparator);

    #endregion

    #region properties

    [Required]
    public ITaskItem[] Files { get; set; }

    #endregion

    #region methods

    public override bool Execute()
    {
      var allFiles = Files
        .Select(x => x.ItemSpec)
        .ToArray();

      bool output = allFiles
        .All(x => ExistsOnPath(x));

      if(!output)
      {
        var items = allFiles.Select(x => String.Format(Resources.ErrorMessages.MissingFilesOnPathItemFormat, x));
        string message = String.Concat(Resources.ErrorMessages.MissingFilesOnPathIntro,
                                       Environment.NewLine,
                                       String.Join(Environment.NewLine, items));

        Console.Error.WriteLine(message);
      }

      return output;
    }

    private bool ExistsOnPath(string filename)
    {
      return GetActualPath(filename) != null;
    }

    private string GetActualPath(string filename)
    {
      foreach(var path in environmentPaths)
      {
        var attemptedPath = Path.Combine(path, filename);
        if(File.Exists(attemptedPath))
        {
          return attemptedPath;
        }
      }

      return null;
    }

    #endregion
  }
}

