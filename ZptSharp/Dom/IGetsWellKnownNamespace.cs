namespace ZptSharp.Dom
{
    /// <summary>
    /// A provider for well-known namespaces related to ZPT.
    /// </summary>
    public interface IGetsWellKnownNamespace
    {
        /// <summary>
        /// Gets the namespace for METAL.
        /// </summary>
        /// <value>The METAL namespace.</value>
        Namespace MetalNamespace { get; }

        /// <summary>
        /// Gets the namespace for TAL.
        /// </summary>
        /// <value>The TAL namespace.</value>
        Namespace TalNamespace { get; }
    }
}
