using System;
using System.IO;
using CSF.IO;

namespace CSF.Zpt.BatchRendering
{
  internal class RenderingJob : IRenderingJob
  {
    #region fields

    private readonly Func<IZptDocument> _documentCreator;
    private IZptDocument _document;
    private FileInfo _inputFile;
    private DirectoryInfo _inputRootDirectory;

    #endregion

    #region properties

    public DirectoryInfo InputRootDirectory
    {
      get {
        return _inputRootDirectory;
      }
    }

    #endregion

    #region methods

    public IZptDocument GetDocument()
    {
      if(_document == null)
      {
        _document = _documentCreator();
      }

      return _document;
    }

    public string GetOutputInfo(IBatchRenderingOptions batchOptions)
    {
      if(batchOptions == null)
      {
        throw new ArgumentNullException(nameof(batchOptions));
      }

      string output;

      if(batchOptions.OutputStream != null)
      {
        output = "STDOUT";
      }
      else
      {
        var outputFile = GetOutputFile(batchOptions);
        output = outputFile.FullName;
      }

      return output;
    }

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
      else
      {
        var outputFile = GetOutputFile(batchOptions);
        output = GetOutputStream(outputFile);
      }

      return output;
    }

    private Stream GetOutputStream(FileInfo outputFile)
    {
      var parentDir = outputFile.GetParent();
      if(!parentDir.Exists)
      {
        parentDir.CreateRecursive();
      }

      return outputFile.Open(FileMode.Create);
    }

    private FileInfo GetOutputFile(IBatchRenderingOptions batchOptions)
    {
      FileInfo output;

      if(batchOptions.OutputPath is FileInfo)
      {
        output = (FileInfo) batchOptions.OutputPath;
      }
      else if(batchOptions.OutputPath is DirectoryInfo)
      {
        output = GetOutputFile((DirectoryInfo) batchOptions.OutputPath, batchOptions.OutputExtensionOverride);
      }
      else
      {
        throw new BatchRenderingException(Resources.ExceptionMessages.InvalidBatchRenderingOutputPath);
      }

      return output;
    }

    private FileInfo GetOutputFile(DirectoryInfo outputRoot, string extensionOverride)
    {
      if(_inputFile == null)
      {
        throw new BatchRenderingException(Resources.ExceptionMessages.InputMustBeFileIfOutputIsDirectory);
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

    public RenderingJob(Func<IZptDocument> documentCreator,
                        FileInfo inputFile = null,
                        DirectoryInfo inputRootDirectory = null)
    {
      if(documentCreator == null)
      {
        throw new ArgumentNullException(nameof(documentCreator));
      }

      _documentCreator = documentCreator;
      _inputFile = inputFile;
      _inputRootDirectory = inputRootDirectory;
    }

    #endregion
  }
}

