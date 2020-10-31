using System;
namespace ZptSharp.Dom
{
    /// <summary>
    /// An object which maintains a registry of the currently-available implementations
    /// of <see cref="IReadsAndWritesDocument"/>.
    /// </summary>
    public interface IRegistersDocumentReaderWriter
    {
        /// <summary>
        /// Registers an implementation of <see cref="IReadsAndWritesDocument"/>.
        /// </summary>
        /// <param name="readerWriter">Reader writer.</param>
        void RegisterDocumentReaderWriter(IReadsAndWritesDocument readerWriter);
    }
}
