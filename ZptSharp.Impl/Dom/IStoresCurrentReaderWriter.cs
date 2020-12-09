namespace ZptSharp.Dom
{
    /// <summary>
    /// An object which provides access to the current document reader/writer.
    /// This interface is only used within the scope of a rendering request,
    /// and so generally-speaking should not be used as part of the public API.
    /// </summary>
    public interface IStoresCurrentReaderWriter
    {
        /// <summary>
        /// Gets or sets the current document reader/writer.
        /// </summary>
        /// <value>The document reader/writer.</value>
        IReadsAndWritesDocument ReaderWriter { get; set; }

        /// <summary>
        /// Gets the document reader/writer.
        /// </summary>
        /// <returns>The document reader/writer.</returns>
        IReadsAndWritesDocument GetReaderWriter();
    }
}
