using System;
using ZptSharp.Dom;

namespace ZptSharp.Metal
{
    /// <summary>
    /// Model for content which should fill a METAL slot.
    /// </summary>
    public class SlotFiller
    {
        /// <summary>
        /// Gets the name of the slot to fill.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the DOM element with which to fill the slot.
        /// </summary>
        /// <value>The element.</value>
        public IElement Element { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlotFiller"/> class.
        /// </summary>
        /// <param name="name">Slot name.</param>
        /// <param name="element">Slot filler element.</param>
        public SlotFiller(string name, IElement element)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Element = element ?? throw new ArgumentNullException(nameof(element));
        }
    }
}
