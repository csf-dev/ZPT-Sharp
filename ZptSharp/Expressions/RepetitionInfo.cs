using ZptSharp.Dom;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// Provides contextual information about a repetition/iteration.  This is used in looping constructs.
    /// </summary>
    public class RepetitionInfo
    {
        /// <summary>
        /// Gets or sets the name of the repetition.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the total number of items in the repetition.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the zero-based index of the current iteration.
        /// </summary>
        /// <value>The index of the current iteration.</value>
        public int CurrentIndex { get; set; }

        /// <summary>
        /// Gets or sets the element associated with the repetition.
        /// </summary>
        /// <value>The element.</value>
        public IElement Element { get; set; }

        /// <summary>
        /// Gets or sets the value associated with the current iteration.
        /// </summary>
        /// <value>The current value.</value>
        public object CurrentValue { get; set; }
    }
}
