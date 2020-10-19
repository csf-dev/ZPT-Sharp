using System;
namespace ZptSharp.Rendering
{
    /// <summary>
    /// A factory service for creating instances of <see cref="IIteratesExpressionContexts"/> which
    /// use a custom expression context processor.
    /// </summary>
    public interface IGetsExpressionContextIterator
    {
        /// <summary>
        /// Gets an expression context iterator which uses the specified <paramref name="processor"/>.
        /// </summary>
        /// <returns>The context iterator.</returns>
        /// <param name="processor">The processor to be used upon each iteration.</param>
        IIteratesExpressionContexts GetContextIterator(IProcessesExpressionContext processor);
    }
}
