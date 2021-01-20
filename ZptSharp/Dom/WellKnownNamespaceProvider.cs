namespace ZptSharp.Dom
{
    /// <summary>
    /// Implementation of <see cref="IGetsWellKnownNamespace"/> which uses string constants.
    /// </summary>
    public class WellKnownNamespaceProvider : IGetsWellKnownNamespace
    {
        const string
            MetalPrefix = "metal",
            MetalUri = "http://xml.zope.org/namespaces/metal",
            TalPrefix = "tal",
            TalUri = "http://xml.zope.org/namespaces/tal";

        /// <summary>
        /// Gets the namespace for METAL.
        /// </summary>
        /// <value>The METAL namespace.</value>
        public Namespace MetalNamespace => new Namespace(MetalPrefix, MetalUri);

        /// <summary>
        /// Gets the namespace for TAL.
        /// </summary>
        /// <value>The TAL namespace.</value>
        public Namespace TalNamespace => new Namespace(TalPrefix, TalUri);
    }
}
