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

        /// <summary>
        /// Initializes a new instance of the <see cref="MetalMacro"/> class.
        /// </summary>
        /// <param name="name">Macro name.</param>
        /// <param name="element">The element for the macro.</param>
        public MetalMacro(string name, Dom.IElement element)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Element = element ?? throw new ArgumentNullException(nameof(element));
        }
    }
}
