using System;
using System.Xml.Linq;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An implementation of <see cref="IAttribute"/> based upon an XML <see cref="XAttribute"/>.
    /// </summary>
    public class XmlAttribute : AttributeBase
    {
        /// <summary>
        /// Gets the native HAP attribute.
        /// </summary>
        /// <value>The native attribute.</value>
        public XAttribute NativeAttribute { get; }

        /// <summary>
        /// Gets the attribute local name.
        /// </summary>
        /// <value>The name.</value>
        public override string Name => NativeAttribute.Name.LocalName;

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
        /// Gets a value indicating whether or not the current instance is in the specified namespace.
        /// </summary>
        /// <returns><c>true</c>, if the attribute is in the specified namespace, <c>false</c> otherwise.</returns>
        /// <param name="namespace">The namespace.</param>
        public override bool IsInNamespace(Namespace @namespace)
        {
            if (@namespace == null)
                throw new ArgumentNullException(nameof(@namespace));

            return String.Equals(NativeAttribute.Name.Namespace.NamespaceName, @namespace.Uri, StringComparison.InvariantCulture);
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

            return IsInNamespace(spec.Namespace)
                && String.Equals(NativeAttribute.Name.LocalName, spec.Name, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Gets a value indicating whether or not the current instance represents a namespace declaration for the specified namespace.
        /// </summary>
        /// <returns><c>true</c>, if the attribute is a declaration for the specified namespace, <c>false</c> otherwise.</returns>
        /// <param name="namespace">The namespace.</param>
        public override bool IsNamespaceDeclarationFor(Namespace @namespace)
        {
            return NativeAttribute.IsNamespaceDeclaration
                && NativeAttribute.Value == @namespace.Uri;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlAttribute"/> class.
        /// </summary>
        /// <param name="nativeAttribute">Native attribute.</param>
        public XmlAttribute(XAttribute nativeAttribute)
        {
            NativeAttribute = nativeAttribute ?? throw new ArgumentNullException(nameof(nativeAttribute));
        }
    }
}
