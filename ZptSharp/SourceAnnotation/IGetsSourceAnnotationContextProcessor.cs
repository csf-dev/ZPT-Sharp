using ZptSharp.Rendering;

namespace ZptSharp.SourceAnnotation
{
    /// <summary>
    /// An object which can get a <see cref="IProcessesExpressionContext"/> suitable for
    /// the source-annotation process, which adds comments to a document indicating the source info.
    /// </summary>
    public interface IGetsSourceAnnotationContextProcessor
    {
        /// <summary>
        /// Gets the source-annotation context processor.
        /// </summary>
        /// <returns>The source-annotation context processor.</returns>
        IProcessesExpressionContext GetSourceAnnotationContextProcessor();
    }
}
