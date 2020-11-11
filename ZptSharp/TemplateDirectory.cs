using System;
using System.IO;

namespace ZptSharp
{
    /// <summary>
    /// A model which indicates a filesystem directory containing ZPT documents.
    /// </summary>
    public class TemplateDirectory
    {
        /// <summary>
        /// Gets the filesystem path to the template directory.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateDirectory"/> class.
        /// </summary>
        /// <param name="path">The path to the directory.</param>
        public TemplateDirectory(string path)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateDirectory"/> class.
        /// </summary>
        /// <param name="directory">A directory info object.</param>
        public TemplateDirectory(DirectoryInfo directory)
        {
            Path = directory?.FullName ?? throw new ArgumentNullException(nameof(directory));
        }
    }
}
