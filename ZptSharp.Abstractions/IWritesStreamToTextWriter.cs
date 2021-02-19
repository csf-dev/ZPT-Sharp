using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp
{
    /// <summary>
    /// An object which writes the contents of a <see cref="Stream"/> to a <see cref="TextWriter"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This service is provided as a convenience for when the developer wishes to
    /// write the contents of a stream into a text writer.
    /// This could be useful when dealing with the result of
    /// <see cref="Dom.IReadsAndWritesDocument.WriteDocumentAsync(Dom.IDocument, Config.RenderingConfig, CancellationToken)"/>.
    /// </para>
    /// </remarks>
    public interface IWritesStreamToTextWriter
    {
        /// <summary>
        /// Writes the contents of the <paramref name="stream"/> to the specified
        /// <paramref name="writer"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The implementation of this method provides a well-performing, asynchronous API
        /// to write the specified stream into the specified text writer.
        /// </para>
        /// </remarks>
        /// <returns>A task which indicates when the stream has been completely written to the text writer.</returns>
        /// <param name="stream">The stream (source).</param>
        /// <param name="writer">The text writer (destination).</param>
        /// <param name="token">An optional cancellation token</param>
        Task WriteToTextWriterAsync(Stream stream, TextWriter writer, CancellationToken token = default);
    }
}
