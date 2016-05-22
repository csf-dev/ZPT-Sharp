using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using CSF.IO;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Cli
{
  public class Renderer
  {
    #region fields

    private IZptDocumentFactory _documentFactory;

    #endregion

    #region public API

    public void Render(RenderingOptions options,
                       InputOutputInfo inputOutputInfo,
                       RenderingMode mode,
                       bool addDocumentsMetalGlobal)
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    private IEnumerable<Tuple<FileInfo,IZptDocument>> GetInputDocuments(InputOutputInfo inputOutputInfo,
                                                                        RenderingMode mode)
    {
      IEnumerable<Tuple<FileInfo,IZptDocument>> output;

      if(inputOutputInfo.UseStandardInput)
      {
        output = ReadFromStandardInput(mode);
      }
      else
      {
        output = ReadFromInputPaths(inputOutputInfo, mode);
      }

      return output;
    }

    private IEnumerable<Tuple<FileInfo,IZptDocument>> ReadFromStandardInput(RenderingMode mode)
    {
      IZptDocument output;

      using(var stream = Console.OpenStandardInput())
      {
        if(mode == RenderingMode.Xml)
        {
          output = _documentFactory.CreateXmlDocument(stream);
        }
        else
        {
          output = _documentFactory.CreateHtmlDocument(stream);
        }
      }

      return new [] { new Tuple<FileInfo, IZptDocument>(null, output) };
    }

    private IEnumerable<Tuple<FileInfo,IZptDocument>> ReadFromInputPaths(InputOutputInfo inputOutputInfo,
                                                                         RenderingMode mode)
    {
      return inputOutputInfo.InputPaths.SelectMany(x => GetDocuments(x, inputOutputInfo, mode));
    }

    private IEnumerable<Tuple<FileInfo,IZptDocument>> GetDocuments(FileSystemInfo inputPath,
                                                                   InputOutputInfo inputOutputInfo,
                                                                   RenderingMode mode)
    {
      IEnumerable<Tuple<FileInfo,IZptDocument>> output;

      if(inputPath != null && (inputPath is FileInfo))
      {
        output = new [] { GetDocument((FileInfo) inputPath, mode) };
      }
      else if(inputPath != null && (inputPath is DirectoryInfo))
      {
        var dir = (DirectoryInfo) inputPath;
        output = (from file in dir.GetFiles(inputOutputInfo.InputSearchPattern, SearchOption.AllDirectories)
                  from ignoredDirectory in inputOutputInfo.IgnoredPaths
                  where !file.IsChildOf(ignoredDirectory)
                  select GetDocument(file, mode));
      }
      else
      {
        output = new Tuple<FileInfo,IZptDocument>[0];
      }

      return output;
    }

    private Tuple<FileInfo,IZptDocument> GetDocument(FileInfo file, RenderingMode mode)
    {
      IZptDocument output;

      if(mode == RenderingMode.Xml)
      {
        output = _documentFactory.CreateXmlDocument(file);
      }
      else if(mode == RenderingMode.Html)
      {
        output = _documentFactory.CreateHtmlDocument(file);
      }
      else
      {
        output = _documentFactory.CreateDocument(file);
      }

      return new Tuple<FileInfo, IZptDocument>(file, output);
    }

    #endregion
  }
}

