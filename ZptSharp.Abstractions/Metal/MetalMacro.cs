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
        /// Gets the DOM node for the root of the macro.
        /// </summary>
        /// <value>The node.</value>
        public Dom.INode Node { get; }

        /// <summary>
        /// Gets a copy of the current macro instance, using a cloned copy of the node.
        /// </summary>
        /// <returns>The copied macro.</returns>
        public MetalMacro GetCopy() => new MetalMacro(Name, Node.GetCopy());

        /// <summary>
        /// Initializes a new instance of the <see cref="MetalMacro"/> class.
        /// </summary>
        /// <param name="name">Macro name.</param>
        /// <param name="node">The node for the macro.</param>
        public MetalMacro(string name, Dom.INode node)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Node = node ?? throw new ArgumentNullException(nameof(node));
        }
    }
}
