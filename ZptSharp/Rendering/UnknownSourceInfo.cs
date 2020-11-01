using System;
namespace ZptSharp.Rendering
{
    /// <summary>
    /// Implementation of <see cref="IDocumentSourceInfo"/> for a document which has an unidentifiable source.
    /// </summary>
    public sealed class UnknownSourceInfo : IDocumentSourceInfo
    {
        const string DisplayName = "<unknown>";

        /// <summary>
        /// Gets the name of the source.
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
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current <see cref="UnknownSourceInfo"/>.</returns>
        public override string ToString() => Name;
    }
}
