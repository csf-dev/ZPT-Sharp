using System;
namespace ZptSharp.Dom
{
    /// <summary>
    /// An object which selects the correct implementation of
    /// <see cref="IReadsAndWritesDocument"/> for a specified filename or path.
    /// </summary>
    public interface IGetsDocumentReaderWriterForFile
    {
        /// <summary>
        /// Gets the document reader/writer for the specified filename.
        /// </summary>
        /// <returns>The document reader/writer.</returns>
        /// <param name="filenameOrPath">A document filename (optionally with its full path).</param>
        IReadsAndWritesDocument GetDocumentProvider(string filenameOrPath);
    }
}
