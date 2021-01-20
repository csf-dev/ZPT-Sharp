namespace ZptSharp.Rendering
{
    /// <summary>
    /// An object which has a parent/container object.
    /// </summary>
    public interface IHasContainer
    {
        /// <summary>
        /// Gets the parent/container object.
        /// </summary>
        /// <returns>The container.</returns>
        object GetContainer();
    }
}
