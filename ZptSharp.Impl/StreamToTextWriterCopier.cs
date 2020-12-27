using System;
using System.IO;
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
        const int defaultBufferSize = 1024;

        readonly int bufferSize;

        /// <summary>
        /// Writes the contents of the <paramref name="stream"/> to the specified
        /// <paramref name="writer"/>.
        /// </summary>
        /// <returns>A task which indicates when the work is finished.</returns>
        /// <param name="stream">The stream to write.</param>
        /// <param name="writer">The text writer.</param>
        public Task WriteToTextWriterAsync(Stream stream, TextWriter writer)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            return WriteToTextWriterPrivateAsync(stream, writer);
        }

        async Task WriteToTextWriterPrivateAsync(Stream stream, TextWriter writer)
        {
            if (writer is StreamWriter streamWriter)
                await CopyToStreamWriterAsync(stream, streamWriter);
            else
                await CopyToTextWriterAsync(stream, writer);
        }

        async Task CopyToStreamWriterAsync(Stream stream, StreamWriter streamWriter)
        {
            await streamWriter.FlushAsync();
            await stream.CopyToAsync(streamWriter.BaseStream);
            await streamWriter.FlushAsync();
        }

        async Task CopyToTextWriterAsync(Stream stream, TextWriter textWriter)
        {
            using (var reader = new StreamReader(stream))
            {
                for (var buffer = new char[bufferSize]; !reader.EndOfStream;)
                {
                    var charactersRead = await reader.ReadAsync(buffer, 0, bufferSize);
                    await textWriter.WriteAsync(buffer, 0, charactersRead);
                }

                await textWriter.FlushAsync();
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
