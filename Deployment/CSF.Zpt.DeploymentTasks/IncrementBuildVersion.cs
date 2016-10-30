using System;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using CSF.IniDocuments;
using System.IO;

namespace CSF.Zpt.DeploymentTasks
{
  public class IncrementBuildVersion : Task
  {
    #region properties

    [Required]
    public string VersionFile { get; set; }

    #endregion

    #region methods

    public override bool Execute()
    {
      var doc = IniDocument.Read(new FileInfo(VersionFile));

      string buildNumber;
      var buildSuccess = doc.TryGetValue(ReadVersionNumber.BUILD_NUMBER, out buildNumber);

      if(buildSuccess)
      {
        int parsedBuild;

        if(Int32.TryParse(buildNumber, out parsedBuild))
        {
          doc[ReadVersionNumber.BUILD_NUMBER] = parsedBuild++.ToString();

          doc.Write(VersionFile);

          return true;
        }
      }

      return false;
    }

    #endregion
  }
}

