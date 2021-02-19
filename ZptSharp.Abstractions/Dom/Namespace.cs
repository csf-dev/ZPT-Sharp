using System;
namespace ZptSharp.Dom
{
    /// <summary>
    /// Represents a namespace for an <see cref="IDocument"/>, <see cref="INode"/> or <see cref="IAttribute"/>.
    /// </summary>
    public sealed class Namespace : IEquatable<Namespace>
    {
        /// <summary>
        /// Gets the full URI for this namespace.
        /// </summary>
        /// <value>The URI.</value>
        public string Uri { get; }

        /// <summary>
        /// Gets the short namespace prefix.
        /// </summary>
        /// <value>The prefix.</value>
        public string Prefix { get; }

        /// <summary>
        /// Determines whether the specified <see cref="Namespace"/> is equal to the current <see cref="Namespace"/>.
        /// </summary>
        /// <param name="other">The <see cref="Namespace"/> to compare with the current <see cref="Namespace"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="Namespace"/> is equal to the current
        /// <see cref="Namespace"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Namespace other)
        {
            if (ReferenceEquals(other, this)) return true;
            if (other is null) return false;

            if (Uri != null) return Equals(Uri, other.Uri);
            if (other.Uri != null) return false;
            return Equals(Prefix, other.Prefix);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="Namespace"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="Namespace"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="Namespace"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => Equals(obj as Namespace);

        /// <summary>
        /// Serves as a hash function for a <see cref="Namespace"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            if (Uri != null) return Uri.GetHashCode();
            if (Prefix != null) return Prefix.GetHashCode();
            return 0;
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="Namespace"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="Namespace"/>.</returns>
        public override string ToString()
        {
            if (Uri == null && Prefix == null) return String.Empty;
            if (Uri == null) return Prefix;
            if (Prefix == null) return Uri;
            return $"{Prefix}:{Uri}";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Namespace"/> class.
        /// </summary>
        /// <param name="prefix">Prefix.</param>
        /// <param name="uri">URI.</param>
        public Namespace(string prefix = null, string uri = null)
        {
            Prefix = prefix;
            Uri = uri;
        }

        /// <summary>
        /// Operator overload for testing equality between two <see cref="Namespace"/> instances.
        /// </summary>
        /// <param name="first">The first instance.</param>
        /// <param name="second">The second instance.</param>
        public static bool operator ==(Namespace first, Namespace second) => Equals(first, second);

        /// <summary>
        /// Operator overload for testing inequality between two <see cref="Namespace"/> instances.
        /// </summary>
        /// <param name="first">The first instance.</param>
        /// <param name="second">The second instance.</param>
        public static bool operator !=(Namespace first, Namespace second) => !(first == second);

    }
}
