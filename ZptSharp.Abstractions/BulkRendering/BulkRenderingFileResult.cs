using System;

namespace ZptSharp.BulkRendering
{
    /// <summary>
    /// Describes the result of a bulk-rendering operation for a single input file.
    /// </summary>
    public class BulkRenderingFileResult
    {
        /// <summary>
        /// Gets the path to the input file.
        /// </summary>
        /// <value>The input file path.</value>
        public string InputFile { get; }
        
        /// <summary>
        /// Gets the path to the output file, if <see cref="IsSuccess" /> is <see langword="true" />.
        /// </summary>
        /// <value>The output file path.</value>
        public string OutputFile { get; }
        
        /// <summary>
        /// Gets the exception encountered whilst rendering this file, if <see cref="IsSuccess" />
        /// is <see langword="false" />.
        /// </summary>
        /// <value>The rendering exception.</value>
        public Exception Exception { get; }

        /// <summary>
        /// Gets a value indicating whether or not this file was successfully rendered and saved.
        /// </summary>
        /// <value></value>
        public bool IsSuccess { get; }
        
        BulkRenderingFileResult(string inputFile, string outputFile = null, Exception exception = null)
        {
            InputFile = inputFile ?? throw new ArgumentNullException(nameof(inputFile));
            OutputFile = outputFile;
            Exception = exception;
            IsSuccess = exception == null;
        }

        /// <summary>
        /// Creates an instance of <see cref="BulkRenderingFileResult" /> for a successful rendering.
        /// </summary>
        /// <param name="inputFile">The input file path.</param>
        /// <param name="outputFile">The output file path.</param>
        /// <returns>A bulk-rendering file result.</returns>
        public static BulkRenderingFileResult Success(string inputFile, string outputFile)
            => new BulkRenderingFileResult(inputFile, outputFile);

        /// <summary>
        /// Creates an instance of <see cref="BulkRenderingFileResult" /> for a failed rendering.
        /// </summary>
        /// <param name="inputFile">The input file path.</param>
        /// <param name="exception">The exception encountered whilst rendering.</param>
        /// <returns>A bulk-rendering file result.</returns>
        public static BulkRenderingFileResult Error(string inputFile, Exception exception)
            => new BulkRenderingFileResult(inputFile, exception: exception);

    }
}