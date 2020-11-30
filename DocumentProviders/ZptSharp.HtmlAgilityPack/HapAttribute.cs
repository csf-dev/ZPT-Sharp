using System;
using HtmlAgilityPack;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An implementation of <see cref="IAttribute"/> based upon an HTML Agility Pack <see cref="HtmlAttribute"/>.
    /// </summary>
    public class HapAttribute : AttributeBase
    {
        const char PrefixSeparator = ':';

        /// <summary>
        /// Gets the native HAP attribute.
        /// </summary>
        /// <value>The native attribute.</value>
        public HtmlAttribute NativeAttribute { get; }

        /// <summary>
        /// Gets the attribute name, including any relevant prefix.
        /// </summary>
        /// <value>The name.</value>
        public override string Name => NativeAttribute.OriginalName;

        /// <summary>
        /// Gets the attribute value.
        /// </summary>
        /// <value>The value.</value>
        public override string Value
        {
            get => NativeAttribute.Value;
            set => NativeAttribute.Value = value;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current instance matches a specified attribute.
        /// </summary>
        /// <returns><see langword="true"/> if the current instance matches the specified attribute; <see langword="false"/> if it does not.</returns>
        /// <param name="spec">The spec to match against.</param>
        public override bool Matches(AttributeSpec spec)
        {
            if (spec == null)
                throw new ArgumentNullException(nameof(spec));

            var (prefix, localName) = GetAttributeName(NativeAttribute.OriginalName);

            return IsInNamespace(spec.Namespace)
                && String.Equals(spec.Name, localName, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Gets a value indicating whether or not the current instance is in the specified namespace.
        /// </summary>
        /// <returns><c>true</c>, if the attribute is in the specified namespace, <c>false</c> otherwise.</returns>
        /// <param name="namespace">The namespace.</param>
        public override bool IsInNamespace(Namespace @namespace)
        {
            if (@namespace == null)
                throw new ArgumentNullException(nameof(@namespace));

            var (prefix, localName) = GetAttributeName(NativeAttribute.OriginalName);

            return String.Equals(@namespace.Prefix, prefix, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Gets a value indicating whether or not the current instance represents a namespace declaration for the specified namespace.
        /// </summary>
        /// <returns><c>true</c>, if the attribute is a declaration for the specified namespace, <c>false</c> otherwise.</returns>
        /// <param name="namespace">The namespace.</param>
        public override bool IsNamespaceDeclarationFor(Namespace @namespace) => false;

        (string, string) GetAttributeName(string name)
        {
            if (name == null) return (null, null);
            var val = name?.Split(new[] { PrefixSeparator }, 2);
            return (val[0], (val.Length > 1) ? val[1] : null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HapAttribute"/> class.
        /// </summary>
        /// <param name="nativeAttribute">Native attribute.</param>
        public HapAttribute(HtmlAttribute nativeAttribute)
        {
            NativeAttribute = nativeAttribute ?? throw new ArgumentNullException(nameof(nativeAttribute));
        }
    }
}
