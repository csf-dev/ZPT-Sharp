using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp
{
    /// <summary>
    /// Implementation of <see cref="IWritesStreamToTextWriter"/> which uses either a
    /// stream-copy (if the text writer is in fact a <see cref="StreamWriter"/>),
    /// or otherwise a buffered read &amp; write if the text writer is any other implementation.
    /// </summary>
    public class StreamToTextWriterCopier : IWritesStreamToTextWriter
    {
        // This is the same buffer size as used here https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.copytoasync?view=net-5.0
        const int defaultBufferSize = 81920;

        readonly int bufferSize;

        /// <summary>
        /// Writes the contents of the <paramref name="stream"/> to the specified
        /// <paramref name="writer"/>.
        /// </summary>
        /// <returns>A task which indicates when the work is finished.</returns>
        /// <param name="stream">The stream to write.</param>
        /// <param name="writer">The text writer.</param>
        /// <param name="token">An optional cancellation token</param>
        public Task WriteToTextWriterAsync(Stream stream, TextWriter writer, CancellationToken token = default)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            return WriteToTextWriterPrivateAsync(stream, writer, token);
        }

        async Task WriteToTextWriterPrivateAsync(Stream stream, TextWriter writer, CancellationToken token)
        {
            if (writer is StreamWriter streamWriter)
                await CopyToStreamWriterAsync(stream, streamWriter, token).ConfigureAwait(false);
            else
                await CopyToTextWriterAsync(stream, writer, token).ConfigureAwait(false);
        }

        static async Task CopyToStreamWriterAsync(Stream stream, StreamWriter streamWriter, CancellationToken token)
        {
            await streamWriter.FlushAsync().ConfigureAwait(false);
            await stream.CopyToAsync(streamWriter.BaseStream, defaultBufferSize, token).ConfigureAwait(false);
            await streamWriter.FlushAsync().ConfigureAwait(false);
        }

        async Task CopyToTextWriterAsync(Stream stream, TextWriter textWriter, CancellationToken token)
        {
            using (var reader = new StreamReader(stream))
            {
                for (var buffer = new char[bufferSize]; !reader.EndOfStream;)
                {
                    token.ThrowIfCancellationRequested();
                    var charactersRead = await reader.ReadAsync(buffer, 0, bufferSize).ConfigureAwait(false);
                    await textWriter.WriteAsync(buffer, 0, charactersRead).ConfigureAwait(false);
                }

                await textWriter.FlushAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamToTextWriterCopier"/> class.
        /// </summary>
        public StreamToTextWriterCopier() : this(defaultBufferSize) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamToTextWriterCopier"/> class.
        /// </summary>
        /// <param name="bufferSize">Buffer size.</param>
        public StreamToTextWriterCopier(int bufferSize)
        {
            this.bufferSize = bufferSize;
        }
    }
}
