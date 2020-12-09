using System;
using System.IO;
using System.Text;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Utility class for converting string document representations to/from streams.
    /// </summary>
    public static class DocumentToFromStream
    {
        /// <summary>This matches the default buffer size for a built-in <see cref="StreamWriter"/>.</summary>
        const int BufferSize = 1024;

        /// <summary>
        /// Gets a stream from the specified document string.
        /// </summary>
        /// <returns>The document as a stream.</returns>
        /// <param name="document">The document.</param>
        /// <param name="encoding">Optional.  The text encoding to use.</param>
        public static Stream ToStream(string document, Encoding encoding = null)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream, encoding ?? Encoding.UTF8, BufferSize, true))
            {
                writer.Write(document);
                writer.Flush();
            }
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Gets a document string from the specified stream.
        /// </summary>
        /// <returns>The document string.</returns>
        /// <param name="stream">The document stream.</param>
        /// <param name="encoding">Optional.  The text encoding to use.</param>
        public static string FromStream(Stream stream, Encoding encoding = null)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            using (var reader = new StreamReader(stream, encoding ?? Encoding.UTF8))
                return reader.ReadToEnd();
        }
    }
}
