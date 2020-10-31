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
    public class MemoryDocumentReaderWriterRegistry : IRegistersDocumentReaderWriter, IGetsDocumentReaderWriterForFile
    {
        readonly List<IReadsAndWritesDocument> registry = new List<IReadsAndWritesDocument>();

        /// <summary>
        /// Gets the document reader/writer for the specified filename.
        /// </summary>
        /// <returns>The document reader/writer.</returns>
        /// <param name="filenameOrPath">A document filename (optionally with its full path).</param>
        public IReadsAndWritesDocument GetDocumentProvider(string filenameOrPath)
            => registry.FirstOrDefault(x => x.CanReadWriteForFilename(filenameOrPath));

        /// <summary>
        /// Registers an implementation of <see cref="T:ZptSharp.Dom.IReadsAndWritesDocument"/>.
        /// </summary>
        /// <param name="readerWriter">Reader writer.</param>
        public void RegisterDocumentReaderWriter(IReadsAndWritesDocument readerWriter)
        {
            if (readerWriter == null) throw new ArgumentNullException(nameof(readerWriter));
            if (registry.Contains(readerWriter)) return;

            registry.Add(readerWriter);
        }
    }
}
