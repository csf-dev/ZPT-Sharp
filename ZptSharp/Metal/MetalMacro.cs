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
        public Dom.INode Element { get; }

        /// <summary>
        /// Gets a copy of the current macro instance, using a cloned copy of the element.
        /// </summary>
        /// <returns>The copied macro.</returns>
        public MetalMacro GetCopy() => new MetalMacro(Name, Element.GetCopy());

        /// <summary>
        /// Initializes a new instance of the <see cref="MetalMacro"/> class.
        /// </summary>
        /// <param name="name">Macro name.</param>
        /// <param name="element">The element for the macro.</param>
        public MetalMacro(string name, Dom.INode element)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Element = element ?? throw new ArgumentNullException(nameof(element));
        }
    }
}
