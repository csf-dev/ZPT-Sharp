namespace ZptSharp.Rendering
{
    /// <summary>
    /// An object which can get an instance of <see cref="IModifiesDocument"/>, suitable for use
    /// with a specified rendering request.
    /// </summary>
    public interface IGetsDocumentModifier
    {
        /// <summary>
        /// Gets a document modifier implementation.
        /// </summary>
        /// <returns>The document modifier.</returns>
        IModifiesDocument GetDocumentModifier();
    }
}
