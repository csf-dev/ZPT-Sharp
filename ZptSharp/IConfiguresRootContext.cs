namespace ZptSharp
{
    /// <summary>
    /// An object which may be used to add arbitrary objects/values
    /// to the root ZPT context for a rendering operation.  These
    /// are implementation-specific values which enrich the context
    /// aside from the main model.
    /// </summary>
    public interface IConfiguresRootContext
    {
        /// <summary>
        /// Adds a named value to the root ZPT context.
        /// </summary>
        /// <param name="name">The name of the value to be added.</param>
        /// <param name="value">The value to be added for the name.</param>
        void AddToRootContext(string name, object value);
    }
}
