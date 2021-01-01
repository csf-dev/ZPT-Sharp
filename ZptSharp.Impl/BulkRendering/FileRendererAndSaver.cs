using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System;
using CSF;
using System.Linq;

namespace ZptSharp.BulkRendering
{
    /// <summary>
    /// Implementation of <see cref="IRendersInputFile" /> which renders the file and
    /// saves the output.
    /// </summary>
    public class FileRendererAndSaver : IRendersInputFile
    {
        private readonly IRendersZptFile fileRenderer;
        private readonly IWritesStreamToTextWriter streamCopier;

        /// <summary>
        /// Renders the template described by <paramref name="inputFile" /> asynchronously
        /// and returns a result object.
        /// </summary>
        /// <param name="request">The original request to bulk-render files.</param>
        /// <param name="inputFile">The file to be rendered.</param>
        /// <param name="token">An optional cancellation token</param>
        /// <returns>A result object indicating the outcome.</returns>
        public Task<BulkRenderingFileResult> RenderAsync(BulkRenderingRequest request, InputFile inputFile, CancellationToken token = default)
        {
            if (request is null)
                throw new System.ArgumentNullException(nameof(request));
            if (inputFile is null)
                throw new System.ArgumentNullException(nameof(inputFile));

            return RenderPrivateAsync(request, inputFile, token);
        }

        async Task<BulkRenderingFileResult> RenderPrivateAsync(BulkRenderingRequest request, InputFile inputFile, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var outputStream = await fileRenderer.RenderAsync(inputFile.AbsolutePath,
                                                              request.Model,
                                                              request.RenderingConfig,
                                                              token)
                .ConfigureAwait(false);

            var outputPath = GetOutputPath(request, inputFile);
            EnsureOutputPathExists(outputPath);
            using(var writer = new StreamWriter(outputPath))
                await streamCopier.WriteToTextWriterAsync(outputStream, writer, token).ConfigureAwait(false);

            return BulkRenderingFileResult.Success(inputFile.AbsolutePath, outputPath);
        }

        static void EnsureOutputPathExists(string outputPath)
        {
            var fileInfo = new FileInfo(outputPath);
            var dir = fileInfo.Directory;
            Directory.CreateDirectory(dir.FullName);
        }

        static string GetOutputPath(BulkRenderingRequest request, InputFile inputFile)
        {
            var outputPath = Path.Combine(request.OutputPath, inputFile.RelativePath);
            if(String.IsNullOrEmpty(request.OutputFileExtension)) return outputPath;

            var file = new FileInfo(outputPath);
            var newExtension = request.OutputFileExtension.TrimStart('.');

            var extensionChanger = FilenameExtensionBuilder.Parse(file.Name);
            if(!extensionChanger.Extensions.Any())
                extensionChanger.Extensions.Add(newExtension);
            else
                extensionChanger.Extensions[extensionChanger.Extensions.Count - 1] = newExtension;
            
            return Path.Combine(file.Directory.FullName, extensionChanger.ToString());
        }

        /// <summary>
        /// Initializes a new instance of <see cref="FileRendererAndSaver" />.
        /// </summary>
        /// <param name="fileRenderer">A single-file renderer service.</param>
        /// <param name="streamCopier">A stream-copier service.</param>
        public FileRendererAndSaver(IRendersZptFile fileRenderer,
                                    IWritesStreamToTextWriter streamCopier)
        {
            this.fileRenderer = fileRenderer ?? throw new System.ArgumentNullException(nameof(fileRenderer));
            this.streamCopier = streamCopier ?? throw new System.ArgumentNullException(nameof(streamCopier));
        }
    }
}