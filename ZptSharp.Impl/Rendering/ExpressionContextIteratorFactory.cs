using System;
namespace ZptSharp.Rendering
{
    /// <summary>
    /// A factory implementation which creates <see cref="IIteratesExpressionContexts"/> using a
    /// specified implementation of <see cref="IProcessesExpressionContext"/>.
    /// </summary>
    public class ExpressionContextIteratorFactory : IGetsExpressionContextIterator
    {
        readonly IGetsChildExpressionContexts childContextProvider;

        /// <summary>
        /// Gets an expression context iterator which uses the specified <paramref name="processor"/>.
        /// </summary>
        /// <returns>The context iterator.</returns>
        /// <param name="processor">The processor to be used upon each iteration.</param>
        public IIteratesExpressionContexts GetContextIterator(IProcessesExpressionContext processor)
        {
            if (processor == null)
                throw new ArgumentNullException(nameof(processor));

            return new ExpressionContextIterator(processor, childContextProvider);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionContextIteratorFactory"/> class.
        /// </summary>
        /// <param name="childContextProvider">Child context provider.</param>
        public ExpressionContextIteratorFactory(IGetsChildExpressionContexts childContextProvider)
        {
            this.childContextProvider = childContextProvider ?? throw new ArgumentNullException(nameof(childContextProvider));
        }
    }
}
