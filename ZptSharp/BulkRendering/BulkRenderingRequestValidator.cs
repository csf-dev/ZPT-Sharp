using System;

namespace ZptSharp.BulkRendering
{
    /// <summary>
    /// Implementation of <see cref="IValidatesBulkRenderingRequest" /> which validates a rendering request.
    /// </summary>
    public class BulkRenderingRequestValidator : IValidatesBulkRenderingRequest
    {
        /// <summary>
        /// Asserts that the configuration is valid for use and throws an exception if it is not.
        /// </summary>
        /// <param name="request">The request to validate.</param>
        public void AssertIsValid(BulkRenderingRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            if(request.IncludedPaths == null || request.IncludedPaths.Count == 0)
            {
                var message = String.Format(Resources.ExceptionMessage.BulkRenderingMustIncludeAtLeastOnePath, nameof(BulkRenderingRequest.IncludedPaths));
                throw new BulkRenderingException(message);
            }

            if(String.IsNullOrWhiteSpace(request.OutputPath))
            {
                var message = String.Format(Resources.ExceptionMessage.BulkRenderingMustHaveOutputPath, nameof(BulkRenderingRequest.OutputPath));
                throw new BulkRenderingException(message);
            }
            
            request.InputRootPath = request.InputRootPath ?? Environment.CurrentDirectory;
        }
    }
}