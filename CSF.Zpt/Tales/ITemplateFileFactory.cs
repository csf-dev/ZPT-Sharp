using System;
using System.IO;
using System.Text;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Interface for a type which creates instances of <see cref="TemplateFile"/>.
  /// </summary>
  public interface ITemplateFileFactory
  {
    #region methods

    /// <summary>
    /// Gets a value indicating whether or not the current instance can create a <see cref="TemplateFile"/> from a given
    /// source file.
    /// </summary>
    /// <returns><c>true</c> if this instance can create a document from the given file; otherwise, <c>false</c>.</returns>
    /// <param name="sourceFile">The source file.</param>
    bool CanCreateFromFile(FileInfo sourceFile);

    /// <summary>
    /// Creates a template file from the given source file.
    /// </summary>
    /// <param name="sourceFile">The source file containing the document to create.</param>
    /// <param name="encoding">The text encoding to use in reading the source file.</param>
    TemplateFile CreateTemplateFile(FileInfo sourceFile, Encoding encoding = null);

    #endregion
  }
}

