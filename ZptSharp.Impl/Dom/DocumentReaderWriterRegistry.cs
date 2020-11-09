using System;
using System.Collections.Generic;
using System.Linq;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An in-memory implementation of both <see cref="IRegistersDocumentReaderWriter"/> and
    /// <see cref="IGetsDocumentReaderWriterForFile"/>.  This essentially implements the strategy
    /// pattern, facilitating the choosing of an appropriate implementation of
    /// <see cref="IReadsAndWritesDocument"/> for a given filename or path.
    /// </summary>
    public class DocumentReaderWriterRegistry : IRegistersDocumentReaderWriter, IGetsDocumentReaderWriterForFile
    {
        readonly HashSet<IReadsAndWritesDocument> registry = new HashSet<IReadsAndWritesDocument>();

        /// <summary>
        /// Gets the registry backing store.  This is intentionally not revealed via interfaces.
        /// </summary>
        /// <value>The underlying registry backing store.</value>
        public IReadOnlyCollection<IReadsAndWritesDocument> Registry => registry;

        /// <summary>
        /// Gets the document reader/writer for the specified filename.
        /// </summary>
        /// <returns>The document reader/writer.</returns>
        /// <param name="filenameOrPath">A document filename (optionally with its full path).</param>
        public IReadsAndWritesDocument GetDocumentProvider(string filenameOrPath)
            => Registry.FirstOrDefault(x => x.CanReadWriteForFilename(filenameOrPath));

        /// <summary>
        /// Registers an implementation of <see cref="IReadsAndWritesDocument"/>.
        /// </summary>
        /// <param name="readerWriter">Reader writer.</param>
        public void RegisterDocumentReaderWriter(IReadsAndWritesDocument readerWriter)
        {
            if (readerWriter == null) throw new ArgumentNullException(nameof(readerWriter));

            registry.Add(readerWriter);
        }
    }
}
