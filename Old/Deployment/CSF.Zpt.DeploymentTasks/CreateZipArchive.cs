using System;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;
using System.IO.Compression;

namespace CSF.Zpt.DeploymentTasks
{
  public class CreateZipArchive : Task
  {
    #region properties

    [Required]
    public ITaskItem[] Files { get; set; }

    [Required]
    public string OutputFilename { get; set; }

    #endregion

    public override bool Execute()
    {
      // Originally taken from https://peteris.rocks/blog/creating-release-zip-archive-with-msbuild/
      // Then modified not to be inline anymore
      try
      {
        using (Stream zipStream = new FileStream(Path.GetFullPath(OutputFilename), FileMode.Create, FileAccess.Write))
        using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
        {
          foreach (ITaskItem fileItem in Files)
          {
            string filename = fileItem.ItemSpec;
            using (Stream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            using (Stream fileStreamInZip = archive.CreateEntry(new FileInfo(filename).Name).Open())
              fileStream.CopyTo(fileStreamInZip);
          }
        }
        return true;
      }
      catch (Exception ex)
      {
        Log.LogErrorFromException(ex);
        return false;
      }
    }
  }
}

