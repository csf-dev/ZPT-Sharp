using System;
namespace ZptSharp.Rendering
{
    /// <summary>
    /// A factory implementation which creates <see cref="IIterativelyProcessesExpressionContexts"/> using a
    /// specified implementation of <see cref="IProcessesExpressionContext"/>.
    /// </summary>
    public class IterativeExpressionContextProcessorFactory : IGetsIterativeExpressionContextProcessor
    {
        readonly IGetsChildExpressionContexts childContextProvider;

        /// <summary>
        /// Gets an expression context iterator which uses the specified <paramref name="processor"/>.
        /// </summary>
        /// <returns>The context iterator.</returns>
        /// <param name="processor">The processor to be used upon each iteration.</param>
        public IIterativelyProcessesExpressionContexts GetContextIterator(IProcessesExpressionContext processor)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor));

            return new ExpressionContextIterativeProcessor(processor, childContextProvider);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IterativeExpressionContextProcessorFactory"/> class.
        /// </summary>
        /// <param name="childContextProvider">Child context provider.</param>
        public IterativeExpressionContextProcessorFactory(IGetsChildExpressionContexts childContextProvider)
        {
            this.childContextProvider = childContextProvider ?? throw new ArgumentNullException(nameof(childContextProvider));
        }
    }
}
