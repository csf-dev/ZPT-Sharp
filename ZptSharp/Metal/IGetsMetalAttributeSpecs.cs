using System;
using ZptSharp.Dom;

namespace ZptSharp.Metal
{
    /// <summary>
    /// An object which gets <see cref="AttributeSpec"/> instances for well-known METAL attributes.
    /// </summary>
    public interface IGetsMetalAttributeSpecs
    {
        /// <summary>
        /// Gets the define-macro attribute spec
        /// </summary>
        /// <value>The define-macro spec.</value>
        AttributeSpec DefineMacro { get; }

        /// <summary>
        /// Gets the extend-macro attribute spec
        /// </summary>
        /// <value>The extend-macro spec.</value>
        AttributeSpec ExtendMacro { get; }

        /// <summary>
        /// Gets the use-macro attribute spec
        /// </summary>
        /// <value>The use-macro spec.</value>
        AttributeSpec UseMacro { get; }

        /// <summary>
        /// Gets the define-slot attribute spec
        /// </summary>
        /// <value>The define-slot spec.</value>
        AttributeSpec DefineSlot { get; }

        /// <summary>
        /// Gets the fill-slot attribute spec
        /// </summary>
        /// <value>The fill-slot spec.</value>
        AttributeSpec FillSlot { get; }
    }
}
