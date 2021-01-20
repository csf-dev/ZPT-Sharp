namespace ZptSharp.BulkRendering
{
    /// <summary>
    /// Describes a single file to be rendered in a bulk-rendering operation.
    /// </summary>
    public class InputFile
    {
        /// <summary>
        /// Gets the absolute path to the input file.
        /// </summary>
        /// <value>The absolute path.</value>
        public string AbsolutePath { get; }

        /// <summary>
        /// Gets a path to the input file, relative to the 'root' directory for the operation.
        /// </summary>
        /// <value>The relative path.</value>
        public string RelativePath { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="InputFile" />.
        /// </summary>
        /// <param name="absolutePath">The absolute path.</param>
        /// <param name="relativePath">The relative path.</param>
        public InputFile(string absolutePath, string relativePath)
        {
            this.AbsolutePath = absolutePath ?? throw new System.ArgumentNullException(nameof(absolutePath));
            this.RelativePath = relativePath ?? throw new System.ArgumentNullException(nameof(relativePath));
        }
    }
}