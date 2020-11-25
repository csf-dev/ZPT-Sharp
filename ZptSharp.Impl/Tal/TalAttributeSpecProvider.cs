using System;
using ZptSharp.Dom;

namespace ZptSharp.Tal
{
    /// <summary>
    /// Implementation of <see cref="IGetsTalAttributeSpecs"/> which gets attribute
    /// specifications for the TAL attributes.
    /// </summary>
    public class TalAttributeSpecProvider : IGetsTalAttributeSpecs
    {
        const string
            AttributesAttribute = "attributes",
            ConditionAttribute = "condition",
            ContentAttribute = "content",
            ReplaceAttribute = "replace",
            DefineAttribute = "define",
            OmitTagAttribute = "omit-tag",
            OnErrorAttribute = "on-error",
            RepeatAttribute = "repeat";

        readonly IGetsWellKnownNamespace namespaceProvider;

        /// <summary>
        /// Gets the attributes attribute spec
        /// </summary>
        /// <value>The attributes spec.</value>
        public AttributeSpec Attributes
            => new AttributeSpec(AttributesAttribute, namespaceProvider.TalNamespace);

        /// <summary>
        /// Gets the condition attribute spec
        /// </summary>
        /// <value>The condition spec.</value>
        public AttributeSpec Condition
            => new AttributeSpec(ConditionAttribute, namespaceProvider.TalNamespace);

        /// <summary>
        /// Gets the content attribute spec
        /// </summary>
        /// <value>The content spec.</value>
        public AttributeSpec Content
            => new AttributeSpec(ContentAttribute, namespaceProvider.TalNamespace);

        /// <summary>
        /// Gets the replace attribute spec
        /// </summary>
        /// <value>The replace spec.</value>
        public AttributeSpec Replace
            => new AttributeSpec(ReplaceAttribute, namespaceProvider.TalNamespace);

        /// <summary>
        /// Gets the define attribute spec
        /// </summary>
        /// <value>The define spec.</value>
        public AttributeSpec Define
            => new AttributeSpec(DefineAttribute, namespaceProvider.TalNamespace);

        /// <summary>
        /// Gets the omit-tag attribute spec
        /// </summary>
        /// <value>The omit-tag spec.</value>
        public AttributeSpec OmitTag
            => new AttributeSpec(OmitTagAttribute, namespaceProvider.TalNamespace);

        /// <summary>
        /// Gets the on-error attribute spec
        /// </summary>
        /// <value>The on-error spec.</value>
        public AttributeSpec OnError
            => new AttributeSpec(OnErrorAttribute, namespaceProvider.TalNamespace);

        /// <summary>
        /// Gets the repeat attribute spec
        /// </summary>
        /// <value>The repeat spec.</value>
        public AttributeSpec Repeat
            => new AttributeSpec(RepeatAttribute, namespaceProvider.TalNamespace);

        /// <summary>
        /// Initializes a new instance of the <see cref="TalAttributeSpecProvider"/> class.
        /// </summary>
        /// <param name="namespaceProvider">Namespace provider.</param>
        public TalAttributeSpecProvider(IGetsWellKnownNamespace namespaceProvider)
        {
            this.namespaceProvider = namespaceProvider ?? throw new ArgumentNullException(nameof(namespaceProvider));
        }
    }
}
