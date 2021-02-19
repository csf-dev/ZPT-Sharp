using System;
namespace ZptSharp.Rendering
{
    /// <summary>
    /// An object which encapsulates information about the source of a single DOM node.
    /// </summary>
    public sealed class NodeSourceInfo : IEquatable<NodeSourceInfo>
    {
        /// <summary>
        /// Gets source information about the document where this node originated.
        /// </summary>
        /// <value>The document.</value>
        public IDocumentSourceInfo Document { get; }

        /// <summary>
        /// Gets the line number for the beginning of the node.
        /// </summary>
        /// <value>The line number.</value>
        public int? StartTagLineNumber { get; }

        /// <summary>
        /// Gets the line number for the end of the node.
        /// </summary>
        /// <value>The line number.</value>
        public int? EndTagLineNumber { get; }

        /// <summary>
        /// Creates a child <see cref="NodeSourceInfo"/> which uses the same
        /// document as the current instance, but different line number(s).
        /// </summary>
        /// <returns>The child source info.</returns>
        /// <param name="startTagLineNumber">The line number for the start-tag.</param>
        /// <param name="endTagLineNumber">The line number for the end-tag.</param>
        public NodeSourceInfo CreateChild(int? startTagLineNumber = null, int? endTagLineNumber = null)
            => new NodeSourceInfo(Document, startTagLineNumber, endTagLineNumber);

        /// <summary>
        /// Determines whether the specified <see cref="NodeSourceInfo"/> is equal to the
        /// current <see cref="NodeSourceInfo"/>.  Will only return <c>true</c> if the objects are reference-equal.
        /// </summary>
        /// <param name="other">The <see cref="NodeSourceInfo"/> to compare with the current <see cref="NodeSourceInfo"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="NodeSourceInfo"/> is equal to the current
        /// <see cref="NodeSourceInfo"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(NodeSourceInfo other)
        {
            if (StartTagLineNumber == null || other?.StartTagLineNumber == null
             || EndTagLineNumber == null || other?.EndTagLineNumber == null)
                return ReferenceEquals(this, other);

            return Equals(Document, other.Document)
                && Equals(StartTagLineNumber, other.StartTagLineNumber)
                && Equals(EndTagLineNumber, other.EndTagLineNumber);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="NodeSourceInfo"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="NodeSourceInfo"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="NodeSourceInfo"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => Equals(obj as NodeSourceInfo);

        /// <summary>
        /// Serves as a hash function for a <see cref="NodeSourceInfo"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            // If the line number info is unavailable then we only
            // consider instances equal if they are reference equal.
            // Hence the call to the base hashcode function.
            if (StartTagLineNumber == null || EndTagLineNumber == null) return base.GetHashCode();

            return Document.GetHashCode() ^ StartTagLineNumber.Value ^ EndTagLineNumber.Value;
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="NodeSourceInfo"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current <see cref="NodeSourceInfo"/>.</returns>
        public override string ToString() => StartTagLineNumber.HasValue ? $"{Document.ToString()}:{StartTagLineNumber.Value}" : Document.ToString();

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeSourceInfo"/> class.
        /// </summary>
        /// <param name="document">Document.</param>
        /// <param name="startTagLineNumber">The line number for the start-tag.</param>
        /// <param name="endTagLineNumber">The line number for the end-tag.</param>
        public NodeSourceInfo(IDocumentSourceInfo document, int? startTagLineNumber = null, int? endTagLineNumber = null)
        {
            Document = document ?? throw new System.ArgumentNullException(nameof(document));
            StartTagLineNumber = startTagLineNumber;
            EndTagLineNumber = endTagLineNumber;
        }
    }
}
