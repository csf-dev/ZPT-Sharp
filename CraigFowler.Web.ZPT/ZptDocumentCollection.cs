//  
//  ZptDocumentCollection.cs
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
using CraigFowler.Web.ZPT.Tales;
using System.IO;
using System.Text.RegularExpressions;

namespace CraigFowler.Web.ZPT
{
  /// <summary>
  /// <para>A storage class for <see cref="IZptDocument"/> instances, within a hierarchical, structured system.</para>
  /// </summary>
  /// <remarks>
  /// <para>
  /// This type is essentially a flat implementation of <see cref="TalesStructureProvider"/>, strongly-typed to only
  /// store instances of <see cref="IZptDocument"/>.
  /// </para>
  /// </remarks>
  public class ZptDocumentCollection : TalesStructureProvider
  {
    #region constants
    
    /// <summary>
    /// <para>
    /// Read-only.  Runtime constant contains the default filename pattern for matching ZPT document files.
    /// </para>
    /// </summary>
    public static readonly string
      DefaultTemplateDocumentPattern    = String.Format("*{0}", ZptMetadata.ZptTemplateDocumentExtension);
    
    /// <summary>
    /// <para>A pattern that matches file extensions.</para>
    /// </summary>
    private const string
      FILE_EXTENSION_PATTERN            = @"\.[^.]+$";
    
    /// <summary>
    /// <para>A regular expression instance that will match file extensions.</para>
    /// <seealso cref="FILE_EXTENSION_PATTERN"/>
    /// </summary>
    private static readonly Regex
      FileExtension                     = new Regex(FILE_EXTENSION_PATTERN, RegexOptions.Compiled);
    
    #endregion
    
    #region methods

    /// <summary>
    /// <para>
    /// Overloaded.  Stores a <see cref="IZptDocument"/> instance within the structure at a position indicated by the
    /// relative TALES path.
    /// </para>
    /// </summary>
    /// <param name="relativePath">
    /// A <see cref="TalesPath"/> that indicates the path (relative to the position of this structure provider in
    /// a TALES hierarchy) at which to store the <paramref name="item"/>.
    /// </param>
    /// <param name="document">
    /// A <see cref="IZptDocument"/> to store at the specified point within the hierarchy of this instance.
    /// </param>
    public void StoreItem(TalesPath relativePath, IZptDocument document)
    {
      base.StoreItem(relativePath, document);
    }

    /// <summary>
    /// <para>
    /// Overloaded.  Stores a <see cref="IZptDocument"/> instance within the structure at a position indicated by the
    /// relative TALES path.
    /// </para>
    /// </summary>
    /// <param name="relativePath">
    /// A <see cref="TalesPath"/> that indicates the path (relative to the position of this structure provider in
    /// a TALES hierarchy) at which to store the <paramref name="item"/>.
    /// </param>
    /// <param name="collection">
    /// A <see cref="TalesStructureProvider"/> to store at the specified point within the hierarchy of this instance.
    /// </param>
    public void StoreItem(TalesPath relativePath, TalesStructureProvider collection)
    {
      base.StoreItem(relativePath, collection);
    }
    
    /// <summary>
    /// <para>
    /// Overloaded, overridden.  Stores an item within the structure at a position indicated by the relative TALES
    /// path.  This item must be an instance of <see cref="IZptDocument"/>.
    /// </para>
    /// </summary>
    /// <param name="relativePath">
    /// A <see cref="TalesPath"/> that indicates the path (relative to the position of this structure provider in
    /// a TALES hierarchy) at which to store the <paramref name="item"/>.
    /// </param>
    /// <param name="item">
    /// A <see cref="System.Object"/>, which must be castable into either <see cref="IZptDocument"/> or
    /// <see cref="TalesStructureProvider"/>, to store at the specified point within the hierarchy of this instance.
    /// </param>
    public override void StoreItem (TalesPath relativePath, object item)
    {
      if(item is IZptDocument)
      {
        this.StoreItem(relativePath, (IZptDocument) item);
      }
      else if(item is TalesStructureProvider)
      {
        this.StoreItem(relativePath, (TalesStructureProvider) item);
      }
      else
      {
        throw new NotSupportedException("Unsupported type to store within a ZptDocumentCollection.");
      }
    }
    
    #endregion
    
    #region constructor
    
    public ZptDocumentCollection () : base() {}
    
    #endregion
    
    #region static methods
    
    /// <summary>
    /// <para>
    /// Overloaded.  Creates a new <see cref="ZptDocumentCollection"/> from the ZPT template files found within a
    /// given path.
    /// </para>
    /// </summary>
    /// <param name="path">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="ZptDocumentCollection"/>
    /// </returns>
    public static ZptDocumentCollection CreateFromFilesystem(string path)
    {
      return CreateFromFilesystem(new DirectoryInfo(path), DefaultTemplateDocumentPattern);
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Creates a new <see cref="ZptDocumentCollection"/> from the ZPT template files found within a
    /// given path.
    /// </para>
    /// </summary>
    /// <param name="basePath">
    /// A <see cref="DirectoryInfo"/>
    /// </param>
    /// <returns>
    /// A <see cref="ZptDocumentCollection"/>
    /// </returns>
    public static ZptDocumentCollection CreateFromFilesystem(DirectoryInfo basePath)
    {
      return CreateFromFilesystem(basePath, DefaultTemplateDocumentPattern);
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Creates a new <see cref="ZptDocumentCollection"/> from the ZPT template files found within a
    /// given path, matching a given filename pattern.
    /// </para>
    /// </summary>
    /// <param name="basePath">
    /// A <see cref="DirectoryInfo"/>
    /// </param>
    /// <param name="filenamePattern">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="ZptDocumentCollection"/>
    /// </returns>
    public static ZptDocumentCollection CreateFromFilesystem(DirectoryInfo basePath, string filenamePattern)
    {
      ZptDocumentCollection output = new ZptDocumentCollection();
      
      CreateFromFilesystem(basePath, filenamePattern, ref output);
      
      return output;
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Creates a new <see cref="ZptDocumentCollection"/> from the ZPT template files found within a
    /// given path, matching a given filename pattern.  This overload will populate an existing collection with the
    /// view files found.
    /// </para>
    /// </summary>
    /// <param name="basePath">
    /// A <see cref="DirectoryInfo"/>
    /// </param>
    /// <param name="filenamePattern">
    /// A <see cref="System.String"/>
    /// </param>
    /// <param name="output">
    /// A <see cref="ZptDocumentCollection"/>
    /// </param>
    protected static void CreateFromFilesystem(DirectoryInfo basePath,
                                               string filenamePattern,
                                               ref ZptDocumentCollection output)
    {
      TalesPath relativePath;
      ZptMetadata metadata;
      string searchPattern;

      if(output == null)
      {
        throw new ArgumentNullException("output");
      }
      
      if(basePath == null)
      {
        throw new ArgumentNullException("basePath");
      }
      else if(!basePath.Exists)
      {
        throw new DirectoryNotFoundException("The base path for ZPT template documents could not be found.");
      }
      
      /* The search pattern is a glob-style filename pattern.  Typically it is the same as the filenamePattern
       * parameter but if the parameter was null then we use the glob pattern "*", meaning 'all files'.
       */
      searchPattern = (filenamePattern != null)? filenamePattern : "*";
      
      /* Perform a recursive search through the base path (and all subdirectories) for matching files.
       * 
       * For each file, get its metadata (note that if a file has no explicit metadata file then a default metadata
       * will be created by the GetMetadata method) and then store the file within the output, at a location
       * determined by its relative path from the base document path.
       */
      foreach(FileInfo file in basePath.GetFiles(searchPattern, SearchOption.AllDirectories))
      {
        relativePath = GetRelativePath(basePath, file);
        metadata = ZptMetadata.GetMetadata(file.FullName);
        
        output.StoreItem(relativePath, ZptDocument.DocumentFactory(metadata));
      }
    }
    
    /// <summary>
    /// <para>
    /// Creates and returns a <see cref="TalesPath"/> that represents the path of <paramref name="file"/>, relative to
    /// a <paramref name="basePath"/>.
    /// </para>
    /// </summary>
    /// <param name="basePath">
    /// A <see cref="DirectoryInfo"/>
    /// </param>
    /// <param name="file">
    /// A <see cref="FileInfo"/>
    /// </param>
    /// <returns>
    /// A <see cref="TalesPath"/>
    /// </returns>
    private static TalesPath GetRelativePath(DirectoryInfo basePath, FileInfo file)
    {
      TalesPath output = TalesPath.GetRelativePath(basePath, file);
      
      // Strip the filename extension from the last part of the path
      output.Parts[output.Parts.Count - 1] = TalesPath.CreatePart(FileExtension.Replace(output.Parts[output.Parts.Count - 1].Text, String.Empty));
      
      return output;
    }
    
    #endregion
  }
}

