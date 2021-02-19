using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using System.Linq;

namespace ZptSharp.BulkRendering
{
    /// <summary>
    /// Implementation of <see cref="IGetsInputFiles" /> which gets input files from a request.
    /// </summary>
    public class InputFilesProvider : IGetsInputFiles
    {
        /// <summary>
        /// Gets a collection of the input files to be processed in the bulk-rendering operation.
        /// </summary>
        /// <param name="request">The bulk-rendering request.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <returns>A collection of input files.</returns>
        public Task<IEnumerable<InputFile>> GetInputFilesAsync(BulkRenderingRequest request, CancellationToken token = default)
        {
            if (request is null)
                throw new System.ArgumentNullException(nameof(request));

            token.ThrowIfCancellationRequested();

            var matcher = new Matcher();
            matcher.AddIncludePatterns(request.IncludedPaths);
            matcher.AddExcludePatterns(request.ExcludedPaths ?? Enumerable.Empty<string>());
            var result = matcher.Execute(GetRootDirectory(request));

            var output = result.Files.Select(x => MapToInputFile(x, request)).ToList();
            return Task.FromResult((IEnumerable<InputFile>) output);
        }

        static InputFile MapToInputFile(FilePatternMatch match, BulkRenderingRequest request)
        {
            var path = match.Path.Replace('/', Path.DirectorySeparatorChar);
            var absolutePath = Path.Combine(request.InputRootPath, path);
            return new InputFile(absolutePath, path);
        }

        static DirectoryInfoBase GetRootDirectory(BulkRenderingRequest request)
            => new DirectoryInfoWrapper(new DirectoryInfo(request.InputRootPath));
    }
}