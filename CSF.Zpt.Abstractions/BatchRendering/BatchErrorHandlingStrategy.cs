namespace CSF.Zpt.BatchRendering
{
    /// <summary>
    /// Enumerates the possible strategies which an <see cref="IBatchRenderer"/> may use for handling expected errors.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In this scope, a fatal error is something which will fundamentally mean that the batch processing cannot proceed
    /// at all, and that it will not be possible to process any of the inputs at all.
    /// </para>
    /// <para>
    /// A non-fatal error is a problem with a single input/output which will likely only prevent rendering of that single
    /// document, and not the rest of the batch job.
    /// </para>
    /// </remarks>
    public enum BatchErrorHandlingStrategy
    {
        /// <summary>
        /// The default strategy - the batch renderer will raise an exception for any kind of error.
        /// </summary>
        RaiseExceptionForAnyError = 0,

        /// <summary>
        /// The renderer will raise an exception for fatal errors, and will stop rendering early upon reaching a non-fatal error.
        /// </summary>
        RaiseExceptionForFatalStopOnFirstNonFatal,

        /// <summary>
        /// The renderer will raise an exception for fatal errors, but will attempt to render all of the given input documents,
        /// even if some raise non-fatal errors.
        /// </summary>
        RaiseExceptionForFatalContinueOnNonFatal,

        /// <summary>
        /// The renderer will not raise any exceptions for expected circumstances.  It will, however, stop the rendering early
        /// upon reaching a non-fatal error.
        /// </summary>
        NoExpectedExceptionsStopOnFirstNonFatal,

        /// <summary>
        /// The renderer will not raise any exceptions for expected circumstances.  It will also attempt to render all of the
        /// given input documents, even if some raise non-fatal errors.
        /// </summary>
        NoExpectedExceptionsContinueOnNonFatal,
    }
}