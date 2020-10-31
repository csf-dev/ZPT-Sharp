using System;
namespace ZptSharp.Metal
{
    /// <summary>
    /// Describes a METAL macro.
    /// </summary>
    public class MetalMacro
    {
        /// <summary>
        /// Gets the name of the macro.
        /// </summary>
        /// <value>The macro name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the DOM element for the root of the macro.
        /// </summary>
        /// <value>The element.</value>
        public Dom.IElement Element { get; }
    }
}
