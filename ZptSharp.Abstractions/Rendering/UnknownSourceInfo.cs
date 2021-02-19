using System;
namespace ZptSharp.Rendering
{
    /// <summary>
    /// Implementation of <see cref="IDocumentSourceInfo"/> for a document which has an unidentifiable source.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class is used when (for any reason) the source information is not provided.
    /// This typically means that either an optional parameter was omitted, or a <see langword="null"/> was explicitly passed.
    /// Instances of this type will always indicate that the source is <c>&lt;unknown&gt;</c> and are only
    /// equal in the case that they are reference equal.
    /// </para>
    /// </remarks>
    public sealed class UnknownSourceInfo : IDocumentSourceInfo
    {
        const string DisplayName = "<unknown>";

        /// <summary>
        /// Gets the name of the source, which for this type will always return the string <c>&lt;unknown&gt;</c>.
        /// </summary>
        /// <value>The name of the document source.</value>
        public string Name => DisplayName;

        /// <summary>
        /// Determines whether the specified <see cref="IDocumentSourceInfo"/> is equal to the
        /// current <see cref="UnknownSourceInfo"/>.  Will only return <c>true</c> if the objects are reference-equal.
        /// </summary>
        /// <param name="other">The <see cref="IDocumentSourceInfo"/> to compare with the current <see cref="UnknownSourceInfo"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="IDocumentSourceInfo"/> is equal to the current
        /// <see cref="UnknownSourceInfo"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(IDocumentSourceInfo other) => ReferenceEquals(other, this);

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="UnknownSourceInfo"/>.
        /// For this type, it will always return the string <c>&lt;unknown&gt;</c>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current <see cref="UnknownSourceInfo"/>.</returns>
        public override string ToString() => Name;
    }
}
