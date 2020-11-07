using ZptSharp.Rendering;

namespace ZptSharp.Metal
{
    /// <summary>
    /// An object which can get a <see cref="IProcessesExpressionContext"/> suitable for
    /// the METAL macro-expansion process.
    /// </summary>
    public interface IGetsMetalContextProcessor
    {
        /// <summary>
        /// Gets the METAL context processor.
        /// </summary>
        /// <returns>The METAL context processor.</returns>
        IProcessesExpressionContext GetMetalContextProcessor();
    }
}
