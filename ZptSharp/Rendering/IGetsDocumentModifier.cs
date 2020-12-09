namespace ZptSharp.Rendering
{
    /// <summary>
    /// An object which can get an instance of <see cref="IModifiesDocument"/>, suitable for use
    /// with a specified rendering request.
    /// </summary>
    public interface IGetsDocumentModifier
    {
        /// <summary>
        /// Gets the document modifier suitable for use with the specified request.
        /// </summary>
        /// <returns>The document modifier.</returns>
        /// <param name="request">A rendering request.</param>
        IModifiesDocument GetDocumentModifier(RenderZptDocumentRequest request);
    }
}
