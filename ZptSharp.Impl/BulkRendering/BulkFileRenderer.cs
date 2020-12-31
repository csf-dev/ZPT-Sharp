using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace ZptSharp.BulkRendering
{
    /// <summary>
    /// Implementation of <see cref="IRendersManyFiles" /> which handles requests to
    /// render &amp; write many template files at once.
    /// </summary>
    public class BulkFileRenderer : IRendersManyFiles
    {
        readonly IValidatesBulkRenderingRequest requestValidator;
        readonly IGetsInputFiles inputFilesProvider;
        readonly IRendersInputFile inputFileRenderer;

        /// <summary>
        /// Renders all of the documents described in the <paramref name="request" /> and
        /// gets a result object.
        /// </summary>
        /// <param name="request">The request object, describing the files to be rendered.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <returns>A result/outcome object.</returns>
        public Task<BulkRenderingResult> RenderAllAsync(BulkRenderingRequest request,
                                                        CancellationToken token = default)
        {
            if (request is null)
                throw new System.ArgumentNullException(nameof(request));
            requestValidator.AssertIsValid(request);

            return RenderAllPrivateAsync(request, token);
        }

        async Task<BulkRenderingResult> RenderAllPrivateAsync(BulkRenderingRequest request,
                                                              CancellationToken token)
        {
            var inputFiles = await inputFilesProvider.GetInputFilesAsync(request, token)
                .ConfigureAwait(false);

            var fileResults = await GetFileResultsAsync(request, inputFiles, token)
                .ConfigureAwait(false);
            
            return new BulkRenderingResult(request, fileResults);
        }

        async Task<IEnumerable<BulkRenderingFileResult>> GetFileResultsAsync(BulkRenderingRequest request,
                                                                             IEnumerable<InputFile> inputFiles,
                                                                             CancellationToken token)
        {
            var tasks = new List<Task<BulkRenderingFileResult>>();

            foreach (var inputFile in inputFiles)
                tasks.Add(GetFileResultAsync(request, inputFile, token));

            return await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        async Task<BulkRenderingFileResult> GetFileResultAsync(BulkRenderingRequest request,
                                                               InputFile inputFile,
                                                               CancellationToken token)
        {
            try
            {
                return await inputFileRenderer.RenderAsync(request, inputFile, token)
                    .ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                return BulkRenderingFileResult.Error(inputFile.AbsolutePath, ex);
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BulkFileRenderer" />.
        /// </summary>
        /// <param name="requestValidator">A request validator.</param>
        /// <param name="inputFilesProvider">A service to get the input files from the request.</param>
        /// <param name="inputFileRenderer">A service to render individual input files.</param>
        public BulkFileRenderer(IValidatesBulkRenderingRequest requestValidator,
                                IGetsInputFiles inputFilesProvider,
                                IRendersInputFile inputFileRenderer)
        {
            this.requestValidator = requestValidator ?? throw new System.ArgumentNullException(nameof(requestValidator));
            this.inputFilesProvider = inputFilesProvider ?? throw new System.ArgumentNullException(nameof(inputFilesProvider));
            this.inputFileRenderer = inputFileRenderer ?? throw new System.ArgumentNullException(nameof(inputFileRenderer));
        }
    }
}