using System;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Factory delegate used to create a template file from a <c>System.IO.FileInfo</c>.
  /// </summary>
  /// <param name="sourceFile">The source file.</param>
  public delegate TemplateFile TemplateFileCreator(System.IO.FileInfo sourceFile);
}

