using System;
namespace ZptSharp.Rendering
{
    /// <summary>
    /// Implementation of <see cref="IDocumentSourceInfo"/> for a document which was retrieved from a file on disk.
    /// </summary>
    public sealed class FileSourceInfo : IDocumentSourceInfo, IHasContainer
    {
        /// <summary>
        /// Gets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath { get; }

        /// <summary>
        /// Gets the name of the source.
        /// </summary>
        /// <value>The name of the document source.</value>
        public string Name => FilePath;

        /// <summary>
        /// Determines whether the specified <see cref="IDocumentSourceInfo"/> is equal to the
        /// current <see cref="FileSourceInfo"/>.
        /// </summary>
        /// <param name="other">The <see cref="IDocumentSourceInfo"/> to compare with the current <see cref="FileSourceInfo"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="IDocumentSourceInfo"/> is equal to the current
        /// <see cref="FileSourceInfo"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(IDocumentSourceInfo other)
        {
            if (other is null) return false;
            if (ReferenceEquals(other, this)) return true;
            if (!(other is FileSourceInfo fileInfo)) return false;

            return fileInfo.FilePath == FilePath;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="FileSourceInfo"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="FileSourceInfo"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="FileSourceInfo"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => Equals(obj as IDocumentSourceInfo);

        /// <summary>
        /// Serves as a hash function for a <see cref="FileSourceInfo"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode() => FilePath.GetHashCode();

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="FileSourceInfo"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current <see cref="FileSourceInfo"/>.</returns>
        public override string ToString() => FilePath;

        /// <summary>
        /// Gets the parent/container object, in this case representing a <see cref="System.IO.DirectoryInfo"/>.
        /// </summary>
        /// <returns>The container.</returns>
        public object GetContainer() => new TemplateDirectory(new System.IO.FileInfo(FilePath).Directory);

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSourceInfo"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public FileSourceInfo(string filePath)
        {
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }
    }
}
