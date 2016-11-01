namespace CSF.Zpt.Rendering
{
    /// <summary>
    /// Interface for a rendering context.
    /// </summary>
    public interface IRenderingContext : IModelValueContainer
    {
        #region properties

        /// <summary>
        /// Gets the object model available to the METAL environment.
        /// </summary>
        /// <value>The METAL model.</value>
        new IModel MetalModel { get; }

        /// <summary>
        /// Gets the object model available to the TAL environment.
        /// </summary>
        /// <value>The TAL model.</value>
        new IModel TalModel { get; }

        /// <summary>
        /// Gets the ZPT element.
        /// </summary>
        /// <value>The element.</value>
        IZptElement Element { get; }

        /// <summary>
        /// Gets the rendering options.
        /// </summary>
        /// <value>The rendering options.</value>
        IRenderingSettings RenderingOptions { get; }

        /// <summary>
        /// Gets the 'virtual' root path for the purpose of source annotation comments.
        /// </summary>
        /// <value>The source annotation root path.</value>
        string SourceAnnotationRootPath { get; }

        #endregion

        #region methods

        /// <summary>
        /// Creates and returns a collection of child contexts, from the current instance.
        /// </summary>
        /// <returns>The child contexts.</returns>
        IRenderingContext[] GetChildContexts();

        /// <summary>
        /// Creates and returns a new sibling rendering context.
        /// </summary>
        /// <returns>The sibling context.</returns>
        /// <param name="element">The ZPT element for which the new context is to be created.</param>
        /// <param name="cloneAttributes">A value indicating whether or not the element's attributes should be cloned or not.</param>
        IRenderingContext CreateSiblingContext(IZptElement element, bool cloneAttributes = false);

        /// <summary>
        /// Gets an attribute matching the given namespace and attribute name.
        /// </summary>
        /// <returns>The attribute, or a <c>null</c> reference if no attribute is found.</returns>
        /// <param name="nspace">The attribute namespace.</param>
        /// <param name="attributeName">The attribute name.</param>
        IZptAttribute GetAttribute(ZptNamespace nspace, string attributeName);

        /// <summary>
        /// Gets the original attributes present upon the element wrapped by the current instance.
        /// </summary>
        /// <returns>The original attributes.</returns>
        OriginalAttributeValuesCollection GetOriginalAttributes();

        #endregion
    }
}