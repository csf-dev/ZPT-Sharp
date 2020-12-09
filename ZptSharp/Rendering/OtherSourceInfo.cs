using System;
namespace ZptSharp.Rendering
{
    /// <summary>
    /// Implementation of <see cref="IDocumentSourceInfo"/> for a document which was retrieved from a location that may
    /// be identified, but is not a file on disk.
    /// </summary>
    public sealed class OtherSourceInfo : IDocumentSourceInfo
    {
        /// <summary>
        /// Gets the identifier for the source location.
        /// </summary>
        /// <value>The source location.</value>
        public string SourceLocation { get; }

        /// <summary>
        /// Gets the name of the source.
        /// </summary>
        /// <value>The name of the document source.</value>
        public string Name => SourceLocation;

        /// <summary>
        /// Determines whether the specified <see cref="IDocumentSourceInfo"/> is equal to the
        /// current <see cref="OtherSourceInfo"/>.
        /// </summary>
        /// <param name="other">The <see cref="IDocumentSourceInfo"/> to compare with the current <see cref="OtherSourceInfo"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="IDocumentSourceInfo"/> is equal to the current
        /// <see cref="OtherSourceInfo"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(IDocumentSourceInfo other)
        {
            if (other is null) return false;
            if (ReferenceEquals(other, this)) return true;
            if (!(other is OtherSourceInfo fileInfo)) return false;

            return fileInfo.SourceLocation == SourceLocation;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="OtherSourceInfo"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="OtherSourceInfo"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="OtherSourceInfo"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => Equals(obj as IDocumentSourceInfo);

        /// <summary>
        /// Serves as a hash function for a <see cref="OtherSourceInfo"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode() => SourceLocation.GetHashCode();

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="OtherSourceInfo"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current <see cref="OtherSourceInfo"/>.</returns>
        public override string ToString() => SourceLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="OtherSourceInfo"/> class.
        /// </summary>
        /// <param name="sourceLocation">A string identifying the source location.</param>
        public OtherSourceInfo(string sourceLocation)
        {
            SourceLocation = sourceLocation ?? throw new ArgumentNullException(nameof(sourceLocation));
        }
    }
}
