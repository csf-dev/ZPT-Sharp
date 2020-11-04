using System;
using System.Collections.Generic;
using System.Linq;
using ZptSharp.Dom;

namespace ZptSharp.Metal
{
    /// <summary>
    /// Default implementation of <see cref="IGetsSlots"/> which gets a
    /// collection of slot-fillers for an element.
    /// </summary>
    public class SlotFinder : IGetsSlots
    {
        readonly IGetsMetalAttributeSpecs specProvider;
        readonly ISearchesForAttributes attributeFinder;

        /// <summary>
        /// Gets the slot fillers from the specified <paramref name="element"/> and its descendents.
        /// </summary>
        /// <returns>The slot fillers.</returns>
        /// <param name="element">Element.</param>
        public IEnumerable<Slot> GetSlotFillers(IElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            return attributeFinder
                .SearchForAttributes(element, specProvider.FillSlot)
                .Select(x => new Slot(x.Value, x.Element))
                .ToList();
        }

        /// <summary>
        /// Gets the defined slots from the specified <paramref name="element"/> and its descendents.
        /// </summary>
        /// <returns>The defined slots.</returns>
        /// <param name="element">Element.</param>
        public IEnumerable<Slot> GetDefinedSlots(IElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            return attributeFinder
                .SearchForAttributes(element, specProvider.DefineSlot)
                .Select(x => new Slot(x.Value, x.Element))
                .ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlotFinder"/> class.
        /// </summary>
        /// <param name="specProvider">Spec provider.</param>
        /// <param name="attributeFinder">Attribute finder.</param>
        public SlotFinder(IGetsMetalAttributeSpecs specProvider,
                          ISearchesForAttributes attributeFinder)
        {
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.attributeFinder = attributeFinder ?? throw new ArgumentNullException(nameof(attributeFinder));
        }
    }
}
