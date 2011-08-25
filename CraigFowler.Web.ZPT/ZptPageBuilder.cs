//  
//  PageBuilder.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2011 Craig Fowler
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace CraigFowler.Web.ZPT
{
  /// <summary>
  /// <para>
  /// ZPT web page builder.  This class handles the (bulk) assembly of ZPT document files (stored within a directory
  /// structure on the filesystem) and outputting the rendered documents (as static XHTML files) into an output
  /// directory.
  /// </para>
  /// </summary>
  public class ZptPageBuilder
  {
    #region constants
    
    private static readonly string
      DEFAULT_INPUT_FILENAME_PATTERN = String.Format("*{0}", ZptMetadata.ZptTemplateDocumentExtension),
      DEFAULT_IGNORE_MARKER_FILENAME = ".zpt-builder-ignore",
      DEFAULT_OUTPUT_DIRECTORY_NAME = "zpt-output",
      DEFAULT_OUTPUT_FILENAME_EXTENSION = ".xhtml";
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Gets and sets the path in which to discover input files to be loaded as ZPT documents and built.</para>
    /// </summary>
    public string InputPath
    {
      get;
      set;
    }
    
    /// <summary>
    /// <para>
    /// Gets and sets a filename/search pattern (a glob-style pattern) that will find ZPT documents within the
    /// <see cref="InputPath"/>.
    /// </para>
    /// <para>
    /// By default this pattern is <c>*.pt</c>.
    /// </para>
    /// </summary>
    public string InputFilenamePattern
    {
      get;
      set;
    }
    
    /// <summary>
    /// <para>Gets and sets the path in which to output to built/assembled pages.</para>
    /// <para>
    /// If this path is not specified then an <c>output</c> directory will be created alongside the
    /// <see cref="InputPath"/>.
    /// </para>
    /// </summary>
    public string OutputPath
    {
      get;
      set;
    }
    
    /// <summary>
    /// <para>Gets and sets the filename extension (including the leading period) to give output filenames.</para>
    /// <para>By default this extension is <c>.xhtml</c>.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Correctly, the output files should be XHTML files (assuming nothing invalid is done within them).  However,
    /// the <c>.xhtml</c> extension can be less-than-useful as (depending on the configuration of how the pages are
    /// served) it could encourage webservers to serve the pages using the XHTML MIME type
    /// (<c>application/xml+xhtml</c>) which confuses and confounds all versions of Microsoft Internet Explorer below
    /// IE9.
    /// </para>
    /// <para>
    /// This property makes it trivial to switch to a more useful default filename extension if desired.
    /// </para>
    /// </remarks>
    public string OutputFilenameExtension
    {
      get;
      set;
    }
    
    /// <summary>
    /// <para>
    /// Gets and sets a filename that - if detected whilst searching the <see cref="InputPath"/> for template files to
    /// build - indicates that the directory that the 'marker' file appears within should be ignored and not processed.
    /// </para>
    /// <para>By default this filename is <c>.zpt-builder-ignore</c>.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// When processing directories of documents that make use of METAL macros, it is likely that some subdirectories of
    /// the <see cref="InputPath"/> will contain ZPT documents which exist only reusable METAL macros.  Typically these
    /// supporting documents should not be processed on their own as part of the output.
    /// </para>
    /// <para>
    /// By placing a placeholder file (it does not need to have any content, the presence of a file with the
    /// appropriate filename is sufficient) in the root of any directory structure that contains these supporting-only
    /// template documents, they can be ignored when building the output.
    /// </para>
    /// <para>
    /// Any time a file with the filename specified by this property is found whilst searching for template documents,
    /// all files and subdirectories in the directory that contains the placeholder file will be ignored and not
    /// directly processed as output.  This does not affect the visibility of these documents to other template
    /// documents (so macros made available by these ignored documents are still usable by output pages) but no output
    /// will be explicitly created for ignored documents.
    /// </para>
    /// </remarks>
    public string IgnoreMarkerFilename
    {
      get;
      set;
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets the collection of <see cref="System.String"/> to <see cref="System.Object"/> definitions that
    /// have come from the command-line.  These will be sent to the
    /// <see cref="CraigFowler.Web.ZPT.Tales.TalesContext.Options"/> property of each of the rendered documents.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property allows commandline-based utilities to pass options from the commandline into the rendered ZPT
    /// documents.  Non-commandline applications of this class should leave this collection empty.
    /// </para>
    /// </remarks>
    public Dictionary<string,object> CommandLineOptions
    {
      get;
      private set;
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  Gets a collection of <see cref="System.String"/> to <see cref="System.Object"/> definitions that
    /// will be made available to the <see cref="CraigFowler.Web.ZPT.Tales.TalesContext"/> on every rendered document.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Non-commandline utilities wishing to make objects available to rendered documents should favour this property
    /// over <see cref="CommandLineOptions"/> in order to make objects available to the TALES contexts of rendered
    /// documents.
    /// </para>
    /// </remarks>
    public Dictionary<string,object> GlobalDefinitions
    {
      get;
      private set;
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>
    /// Processes all of the ZPT documents within <see cref="InputPath"/>, building them as (X)HTML documents using ZPT
    /// and saving the output to the <see cref="OutputPath"/>.
    /// </para>
    /// </summary>
    public void BuildPages()
    {
      DirectoryInfo inputPath, outputPath;
      ZptDocumentCollection documentCollection;
      FileInfo[] allCandidateDocuments;
      List<DirectoryInfo> ignoredDirectories;
      
      this.PerformPreBuildTasks(out inputPath, out outputPath);
      
      // Perform a search for all of the files that we are interested in.
      documentCollection = ZptDocumentCollection.CreateFromFilesystem(inputPath, this.InputFilenamePattern);
      ignoredDirectories = this.GetIgnoredDirectories(inputPath, this.IgnoreMarkerFilename);
      allCandidateDocuments = inputPath.GetFiles(this.InputFilenamePattern, SearchOption.AllDirectories);
      
      // Now loop through all of the files to be processed and build them.
      foreach(FileInfo file in allCandidateDocuments)
      {
        if(!this.IsIgnored(file, ignoredDirectories))
        {
          this.BuildSingleDocument(file, inputPath, outputPath, documentCollection);
        }
      }
    }
    
    /// <summary>
    /// <para>Performs pre-build tasks and validates the state of this object to begin building pages.</para>
    /// </summary>
    /// <param name="inputPath">
    /// A <see cref="DirectoryInfo"/> that represents the input directory within which to discover page files.
    /// </param>
    /// <param name="outputPath">
    /// A <see cref="DirectoryInfo"/> that represents the output directory to store assembled pages.
    /// </param>
    private void PerformPreBuildTasks(out DirectoryInfo inputPath, out DirectoryInfo outputPath)
    {
      // Create a DirectoryInfo instance for the input directory that contains the pages to build.
      inputPath = new DirectoryInfo(this.InputPath);
      if(!inputPath.Exists)
      {
        string message = String.Format("The input document path '{0}' does not exist.", inputPath.FullName);
        throw new DirectoryNotFoundException(message);
      }
      
      // Create a DirectoryInfo instance for the output directory that will contain our rendered pages.
      if(this.OutputPath == null)
      {
        DirectoryInfo inputParent = inputPath.Parent;
        DirectoryInfo[] viableOutputDirectories = inputParent.GetDirectories(DEFAULT_OUTPUT_DIRECTORY_NAME,
                                                                             SearchOption.TopDirectoryOnly);
        
        if(viableOutputDirectories.Length == 0)
        {
          outputPath = inputParent.CreateSubdirectory(DEFAULT_OUTPUT_DIRECTORY_NAME);
        }
        else
        {
          outputPath = viableOutputDirectories[0];
        }
      }
      else
      {
        outputPath = new DirectoryInfo(this.OutputPath);
      }
      
      // At this point the output directory should definitely exist, just check that's true in case something is wrong.
      if(!outputPath.Exists)
      {
        string message = String.Format("The output document path '{0}' does not exist (or could not be created).",
                                       outputPath.FullName);
        throw new DirectoryNotFoundException(message);
      }
      
      // Clear the output directory ready to receive the new output.
      foreach(DirectoryInfo subdirectory in outputPath.GetDirectories())
      {
        subdirectory.Delete(true);
      }
      foreach(FileInfo file in outputPath.GetFiles())
      {
        file.Delete();
      }
      
      // Validate some of the properties of this instance.
      if(this.IgnoreMarkerFilename == null)
      {
        throw new InvalidOperationException("The filename for the 'ignored directories marker' cannot be null.");
      }
      else if(this.InputFilenamePattern == null)
      {
        throw new InvalidOperationException("The pattern for matching input files cannot be null.");
      }
    }
    
    /// <summary>
    /// <para>
    /// Gets a collection of the directories of document templates that should be ignored for processing output.
    /// </para>
    /// <seealso cref="IgnoreMarkerFilename"/>
    /// </summary>
    /// <param name="inputPath">
    /// A <see cref="DirectoryInfo"/>
    /// </param>
    /// <param name="ignoreMarkerFilename">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A collection of <see cref="DirectoryInfo"/>
    /// </returns>
    private List<DirectoryInfo> GetIgnoredDirectories(DirectoryInfo inputPath, string ignoreMarkerFilename)
    {
      List<DirectoryInfo> output = new List<DirectoryInfo>();
      FileInfo[] allIgnoreMarkers;
      
      allIgnoreMarkers = inputPath.GetFiles(ignoreMarkerFilename, SearchOption.AllDirectories);
      
      foreach(FileInfo marker in allIgnoreMarkers)
      {
        output.Add(marker.Directory);
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Gets whether or not the <paramref name="documentFile"/> should be ignored or not.</para>
    /// </summary>
    /// <param name="documentFile">
    /// A <see cref="FileInfo"/>
    /// </param>
    /// <param name="ignoredDirectories">
    /// A collection of <see cref="DirectoryInfo"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    private bool IsIgnored(FileInfo documentFile, List<DirectoryInfo> ignoredDirectories)
    {
      bool output = false;
      
      /* Perform a linear search through the ignored directories and if we find a directory that matches the left-hand
       * portion of the filename then indicate that we have found an ignored file and immediately break off the search.
       * 
       * HACK: This isn't really totally cross-platform is it?  Perhaps we can improve this at some point?
       * 
       * It assumes that the filesystem is written left-to-right and that a file is "in a directory" (or a subdirectory
       * thereof) if it's full name/path begins with that directory.  This happens to be true for Windows and
       * Unix/Linux/MacOSX filesystems but I still don't feel right doing it like this.
       */
      foreach(DirectoryInfo directory in ignoredDirectories)
      {
        if(documentFile.FullName.StartsWith(directory.FullName))
        {
          output = true;
          break;
        }
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>
    /// Builds, renders and saves a single ZPT document (stored in <paramref name="documentFile"/>) to the same
    /// relative path within the <paramref name="outputBasePath"/>, with the file extension changed to
    /// <see cref="OutputFilenameExtension"/>.
    /// </para>
    /// </summary>
    /// <param name="documentFile">
    /// A <see cref="FileInfo"/>
    /// </param>
    /// <param name="inputBasePath">
    /// A <see cref="DirectoryInfo"/>
    /// </param>
    /// <param name="outputBasePath">
    /// A <see cref="DirectoryInfo"/>
    /// </param>
    /// <param name="documentCollection">
    /// A <see cref="ZptDocumentCollection"/>
    /// </param>
    private void BuildSingleDocument(FileInfo documentFile,
                                     DirectoryInfo inputBasePath,
                                     DirectoryInfo outputBasePath,
                                     ZptDocumentCollection documentCollection)
    {
      IZptDocument document = ZptDocument.DocumentFactory(documentFile);
      ITemplateDocument template;
      string relativeInputFilePath, relativeOutputFilePath;
      FileInfo outputFile;
      
      // Figure out where we are saving the output file.
      relativeInputFilePath = documentFile.FullName.Substring(inputBasePath.FullName.Length + 1);
      relativeOutputFilePath = Path.ChangeExtension(relativeInputFilePath, this.OutputFilenameExtension);
      outputFile = new FileInfo(Path.Combine(outputBasePath.FullName, relativeOutputFilePath));
      
      // Add definitions to the root TALES context of the rendered documents, including the 'documents' collection.
      template = document.GetTemplateDocument();
      if(template == null)
      {
        throw new InvalidOperationException(String.Format("The document at {0} could not be parsed.",
                                                          documentFile.FullName));
      }
      else if(template.TalesContext == null)
      {
        throw new InvalidOperationException(String.Format("The document at {0} has a null TALES context.",
                                                          documentFile.FullName));
      }
      template.TalesContext.AddDefinition("documents", documentCollection);
      foreach(string key in this.CommandLineOptions.Keys)
      {
        template.TalesContext.Options.Add(key, this.CommandLineOptions[key]);
      }
      foreach(string key in this.GlobalDefinitions.Keys)
      {
        template.TalesContext.AddDefinition(key, this.GlobalDefinitions[key]);
      }
      
      // Just in case the target output file's parent directory does not exist then create it.
      Directory.CreateDirectory(outputFile.Directory.FullName);
      
      // Render the actual output and save it in the output file.
      using(XmlWriter writer = new XmlTextWriter(outputFile.FullName, Encoding.UTF8))
      {
        document.GetTemplateDocument().Render(writer);
      }
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// <para>Initialises this instance with sane default values.</para>
    /// </summary>
    public ZptPageBuilder ()
    {
      this.InputPath = null;
      this.InputFilenamePattern = DEFAULT_INPUT_FILENAME_PATTERN;
      this.OutputPath = null;
      this.IgnoreMarkerFilename = DEFAULT_IGNORE_MARKER_FILENAME;
      this.OutputFilenameExtension = DEFAULT_OUTPUT_FILENAME_EXTENSION;
      
      this.CommandLineOptions = new Dictionary<string, object>();
      this.GlobalDefinitions = new Dictionary<string, object>();
    }
    
    /// <summary>
    /// <para>Initialises this instance for a given input file path.</para>
    /// </summary>
    /// <param name="inputPath">
    /// A <see cref="System.String"/>
    /// </param>
    public ZptPageBuilder (string inputPath) : this()
    {
      this.InputPath = inputPath;
    }
    
    #endregion
  }
}

