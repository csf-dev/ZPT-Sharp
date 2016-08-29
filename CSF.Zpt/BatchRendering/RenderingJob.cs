using System;
using System.IO;
using CSF.IO;

namespace CSF.Zpt.BatchRendering
{
  internal class RenderingJob
  {
    #region fields

    private IZptDocument _document;
    private FileInfo _inputFile;
    private DirectoryInfo _inputRootDirectory;

    #endregion

    #region properties

    public IZptDocument Document
    {
      get {
        return _document;
      }
    }

    public DirectoryInfo InputRootDirectory
    {
      get {
        return _inputRootDirectory;
      }
    }

    #endregion

    #region methods

    public Stream GetOutputStream(IBatchRenderingOptions batchOptions)
    {
      if(batchOptions == null)
      {
        throw new ArgumentNullException(nameof(batchOptions));
      }

      Stream output;

      if(batchOptions.OutputStream != null)
      {
        output = batchOptions.OutputStream;
      }
      else if(batchOptions.OutputPath is FileInfo)
      {
        output = ((FileInfo) batchOptions.OutputPath).Open(FileMode.Create);
      }
      else if(batchOptions.OutputPath is DirectoryInfo)
      {
        var outputFile = GetOutputFile((DirectoryInfo) batchOptions.OutputPath, batchOptions.OutputExtensionOverride);

        var parentDir = outputFile.GetParent();
        if(!parentDir.Exists)
        {
          parentDir.CreateRecursive();
        }

        output = outputFile.Open(FileMode.Create);
      }
      else
      {
        // TODO: Better exception here
        throw new InvalidOperationException();
      }

      return output;

    }

    private FileInfo GetOutputFile(DirectoryInfo outputRoot, string extensionOverride)
    {
      if(_inputFile == null)
      {
        // TODO: Better exception here
        throw new InvalidOperationException();
      }

      var extension = _inputFile.Extension;
      var filenameWithoutExtension = _inputFile.Name.Substring(0, _inputFile.Name.Length - extension.Length);
      string newFilename;

      if(extensionOverride != null)
      {
        string newExtension = extensionOverride;
        if(!newExtension.StartsWith("."))
        {
          newExtension = String.Concat(".", newExtension);
        }

        newFilename = String.Concat(filenameWithoutExtension, newExtension);
      }
      else if(_document.Mode == RenderingMode.Xml)
      {
        newFilename = String.Concat(filenameWithoutExtension, ".xml");
      }
      else
      {
        newFilename = String.Concat(filenameWithoutExtension, ".html");
      }

      var tempOutputPath = new FileInfo(System.IO.Path.Combine(_inputFile.GetParent().FullName, newFilename));

      var relativePath = tempOutputPath.GetRelative(_inputRootDirectory);
      if(relativePath.StartsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
      {
        relativePath = relativePath.Substring(1);
      }
      var outputPath = outputRoot.FullName;
      return new FileInfo(System.IO.Path.Combine(outputPath, relativePath));
    }

    #endregion

    #region constructor

    public RenderingJob(IZptDocument document,
                        FileInfo inputFile = null,
                        DirectoryInfo inputRootDirectory = null)
    {
      if(document == null)
      {
        throw new ArgumentNullException(nameof(document));
      }

      _document = document;
      _inputFile = inputFile;
      _inputRootDirectory = inputRootDirectory;
    }

    #endregion
  }
}

