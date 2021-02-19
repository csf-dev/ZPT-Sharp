using System;
using ZptSharp.Dom;

namespace ZptSharp.Metal
{
    /// <summary>
    /// Implementation of <see cref="IGetsMetalAttributeSpecs"/> which uses string constants and
    /// a <see cref="IGetsWellKnownNamespace"/>.
    /// </summary>
    public class MetalAttributeSpecProvider : IGetsMetalAttributeSpecs
    {
        const string
            DefineMacroAttribute = "define-macro",
            ExtendMacroAttribute = "extend-macro",
            UseMacroAttribute = "use-macro",
            DefineSlotAttribute = "define-slot",
            FillSlotAttribute = "fill-slot";

        readonly IGetsWellKnownNamespace namespaceProvider;

        /// <summary>
        /// Gets the define-macro attribute spec
        /// </summary>
        /// <value>The define-macro spec.</value>
        public AttributeSpec DefineMacro
            => new AttributeSpec(DefineMacroAttribute, namespaceProvider.MetalNamespace);

        /// <summary>
        /// Gets the extend-macro attribute spec
        /// </summary>
        /// <value>The extend-macro spec.</value>
        public AttributeSpec ExtendMacro
            => new AttributeSpec(ExtendMacroAttribute, namespaceProvider.MetalNamespace);

        /// <summary>
        /// Gets the use-macro attribute spec
        /// </summary>
        /// <value>The use-macro spec.</value>
        public AttributeSpec UseMacro
            => new AttributeSpec(UseMacroAttribute, namespaceProvider.MetalNamespace);

        /// <summary>
        /// Gets the define-slot attribute spec
        /// </summary>
        /// <value>The define-slot spec.</value>
        public AttributeSpec DefineSlot
            => new AttributeSpec(DefineSlotAttribute, namespaceProvider.MetalNamespace);

        /// <summary>
        /// Gets the fill-slot attribute spec
        /// </summary>
        /// <value>The fill-slot spec.</value>
        public AttributeSpec FillSlot
            => new AttributeSpec(FillSlotAttribute, namespaceProvider.MetalNamespace);

        /// <summary>
        /// Initializes a new instance of the <see cref="MetalAttributeSpecProvider"/> class.
        /// </summary>
        /// <param name="namespaceProvider">Namespace provider.</param>
        public MetalAttributeSpecProvider(IGetsWellKnownNamespace namespaceProvider)
        {
            this.namespaceProvider = namespaceProvider ?? throw new ArgumentNullException(nameof(namespaceProvider));
        }
    }
}
