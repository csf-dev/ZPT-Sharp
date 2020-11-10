using System;
using AngleSharp.Dom;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An implementation of <see cref="IAttribute"/> based upon an AngleSharp <see cref="IAttr"/>.
    /// </summary>
    public class AngleSharpAttribute : AttributeBase
    {
        /// <summary>
        /// Gets the native AngleSharp attribute.
        /// </summary>
        /// <value>The native attribute.</value>
        public IAttr NativeAttribute { get; }

        /// <summary>
        /// Gets the attribute name, including any relevant prefix.
        /// </summary>
        /// <value>The name.</value>
        public override string Name => NativeAttribute.Name;

        /// <summary>
        /// Gets the attribute value.
        /// </summary>
        /// <value>The value.</value>
        public override string Value => NativeAttribute.Value;

        /// <summary>
        /// Gets a value indicating whether or not the current instance matches a specified attribute.
        /// </summary>
        /// <returns><see langword="true"/> if the current instance matches the specified attribute; <see langword="false"/> if it does not.</returns>
        /// <param name="spec">The spec to match against.</param>
        public override bool Matches(AttributeSpec spec)
        {
            if (spec == null)
                throw new ArgumentNullException(nameof(spec));

            return String.Equals(spec.Namespace.Prefix, NativeAttribute.Prefix, StringComparison.CurrentCultureIgnoreCase)
                && String.Equals(spec.Name, NativeAttribute.LocalName, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AngleSharpAttribute"/> class.
        /// </summary>
        /// <param name="nativeAttribute">The native DOM attribute.</param>
        /// <param name="element">The element to which this attribute belongs.</param>
        public AngleSharpAttribute(IAttr nativeAttribute, IElement element) : base(element)
        {
            NativeAttribute = nativeAttribute ?? throw new ArgumentNullException(nameof(nativeAttribute));
        }
    }
}
