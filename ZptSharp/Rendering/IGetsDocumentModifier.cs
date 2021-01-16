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
        
        /// <summary>
        /// Gets a document modifier implementation which uses an existing rendering context and not a new context created from a document/model pair.
        /// </summary>
        /// <returns>The document modifier.</returns>
        IModifiesDocumentUsingContext GetDocumentModifierUsingContext();
    }
}
