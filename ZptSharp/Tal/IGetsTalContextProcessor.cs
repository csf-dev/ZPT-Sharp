using ZptSharp.Rendering;

namespace ZptSharp.Tal
{
    /// <summary>
    /// An object which can get a <see cref="IHandlesProcessingError"/> suitable for
    /// the TAL document-manipulation process.
    /// </summary>
    public interface IGetsTalContextProcessor
    {
        /// <summary>
        /// Gets the TAL context processor.
        /// </summary>
        /// <returns>The TAL context processor.</returns>
        IHandlesProcessingError GetTalContextProcessor();
    }
}
