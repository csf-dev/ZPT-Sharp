namespace ZptSharp.BulkRendering
{
    /// <summary>
    /// An object which can assert that a rendering request is valid.
    /// </summary>
    public interface IValidatesBulkRenderingRequest
    {
        /// <summary>
        /// Asserts that the configuration is valid for use and throws an exception if it is not.
        /// </summary>
        /// <param name="request">The request to validate.</param>
        void AssertIsValid(BulkRenderingRequest request);
    }
}