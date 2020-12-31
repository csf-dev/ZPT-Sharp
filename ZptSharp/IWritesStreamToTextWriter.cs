using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp
{
    /// <summary>
    /// An object which writes a Stream to a TextWriter.
    /// </summary>
    public interface IWritesStreamToTextWriter
    {
        /// <summary>
        /// Writes the contents of the <paramref name="stream"/> to the specified
        /// <paramref name="writer"/>.
        /// </summary>
        /// <returns>A task which indicates when the work is finished.</returns>
        /// <param name="stream">The stream to write.</param>
        /// <param name="writer">The text writer.</param>
        /// <param name="token">An optional cancellation token</param>
        Task WriteToTextWriterAsync(Stream stream, TextWriter writer, CancellationToken token = default);
    }
}
