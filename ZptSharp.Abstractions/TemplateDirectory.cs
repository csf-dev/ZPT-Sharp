using System;
using System.IO;

namespace ZptSharp
{
    /// <summary>
    /// A model which represents a filesystem directory containing ZPT documents.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each instance of this class represents a single filesystem directory.
    /// This class has special handling within TALES path expressions, such that it may be treated much as
    /// a filesystem path.
    /// </para>
    /// <para>
    /// If a TALES path expression which makes use of an instance of this type references a document template
    /// file, then that template will automatically be parsed, loaded and its macros made available via a
    /// <c>macros</c> path property.
    /// </para>
    /// </remarks>
    public class TemplateDirectory
    {
        /// <summary>
        /// Gets the filesystem path to the template directory.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateDirectory"/> class
        /// which represents the directory indicated by the specified string path.
        /// </summary>
        /// <param name="path">The path to the directory.</param>
        public TemplateDirectory(string path)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateDirectory"/> class
        /// which represents the directory indicated by the specified <c>DirectoryInfo</c> instance.
        /// </summary>
        /// <param name="directory">A directory info object.</param>
        public TemplateDirectory(DirectoryInfo directory)
        {
            Path = directory?.FullName ?? throw new ArgumentNullException(nameof(directory));
        }
    }
}
