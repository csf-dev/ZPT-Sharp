using System;
namespace ZptSharp.Dom
{
    /// <summary>
    /// A really simple class which holds a reference to an instance of <see cref="IReadsAndWritesDocument"/>.
    /// This is used for dependency-injecting a reader.writer object into per-scope services which need it.
    /// No logic except for <see cref="Rendering.ZptRequestRenderer"/> and the Bootstrap DI module should
    /// actually reference this type.
    /// </summary>
    public class ReaderWriterServiceLocator : IStoresCurrentReaderWriter
    {
        /// <summary>
        /// Gets or sets the current document reader/writer.
        /// </summary>
        /// <value>The document reader/writer.</value>
        public IReadsAndWritesDocument ReaderWriter { get; set; }

        /// <summary>
        /// Gets the document reader/writer.
        /// </summary>
        /// <returns>The document reader/writer.</returns>
        public IReadsAndWritesDocument GetReaderWriter()
            => ReaderWriter ?? throw new InvalidOperationException(Resources.ExceptionMessage.MissingReaderWriter);
    }
}
