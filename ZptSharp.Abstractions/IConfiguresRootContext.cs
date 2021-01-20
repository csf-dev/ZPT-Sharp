namespace ZptSharp
{
    /// <summary>
    /// An object which may be used to add additional named values to the root TALES context.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Any values added by this will be available at the root TALES context of the rendering operation.
    /// These values are essentially additional pre-defined variables.
    /// </para>
    /// <para>
    /// This type is mainly used via <see cref="Config.RenderingConfig.ContextBuilder"/>.
    /// The 'context builder' property of a rendering configuration is the recommended mechanism of
    /// expanding the root rendering context with additional data.
    /// </para>
    /// </remarks>
    /// <seealso cref="Config.RenderingConfig.RootContextsProvider"/>
    public interface IConfiguresRootContext
    {
        /// <summary>
        /// Adds a named value to the root ZPT context.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This named value will be available from the root context within rendering operations.
        /// That means that it will essentially be a pre-defined variable available throughout the
        /// source document.  It may also be accessed via the special <c>CONTEXTS</c> keyword.
        /// </para>
        /// </remarks>
        /// <param name="name">The name of the value to be added.</param>
        /// <param name="value">The value to be added for the name.</param>
        void AddToRootContext(string name, object value);
    }
}
