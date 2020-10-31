using System;
using System.Collections.Generic;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An object which has may contain child instances of <see cref="IElement"/>.
    /// </summary>
    public interface IHasElements
    {
        /// <summary>
        /// Gets the child elements.
        /// </summary>
        /// <returns>The child elements.</returns>
        IEnumerable<IElement> GetChildElements();
    }
}
