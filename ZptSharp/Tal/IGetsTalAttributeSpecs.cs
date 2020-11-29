using ZptSharp.Dom;

namespace ZptSharp.Tal
{
    /// <summary>
    /// An object which gets <see cref="AttributeSpec"/> instances for well-known TAL attributes.
    /// </summary>
    public interface IGetsTalAttributeSpecs
    {
        /// <summary>
        /// Gets the attributes attribute spec
        /// </summary>
        /// <value>The attributes spec.</value>
        AttributeSpec Attributes { get; }

        /// <summary>
        /// Gets the condition attribute spec
        /// </summary>
        /// <value>The condition spec.</value>
        AttributeSpec Condition { get; }

        /// <summary>
        /// Gets the content attribute spec
        /// </summary>
        /// <value>The content spec.</value>
        AttributeSpec Content { get; }

        /// <summary>
        /// Gets the replace attribute spec
        /// </summary>
        /// <value>The replace spec.</value>
        AttributeSpec Replace { get; }

        /// <summary>
        /// Gets the define attribute spec
        /// </summary>
        /// <value>The define spec.</value>
        AttributeSpec Define { get; }

        /// <summary>
        /// Gets the omit-tag attribute spec
        /// </summary>
        /// <value>The omit-tag spec.</value>
        AttributeSpec OmitTag { get; }

        /// <summary>
        /// Gets the on-error attribute spec
        /// </summary>
        /// <value>The on-error spec.</value>
        AttributeSpec OnError { get; }

        /// <summary>
        /// Gets the repeat attribute spec
        /// </summary>
        /// <value>The repeat spec.</value>
        AttributeSpec Repeat { get; }

    }
}
