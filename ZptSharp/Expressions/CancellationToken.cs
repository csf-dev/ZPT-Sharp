using System;
namespace ZptSharp.Expressions
{
    /// <summary>
    /// An object which indicates that the current ZPT action should be cancelled/aborted.
    /// </summary>
    public class CancellationToken : IEquatable<CancellationToken>
    {
        /// <summary>
        /// Determines whether the specified <see cref="CancellationToken"/> is equal to the
        /// current <see cref="CancellationToken"/>.
        /// </summary>
        /// <param name="other">The <see cref="CancellationToken"/> to compare with the current <see cref="CancellationToken"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="CancellationToken"/> is equal to the current
        /// <see cref="CancellationToken"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(CancellationToken other) => !(other is null);

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="CancellationToken"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="CancellationToken"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="CancellationToken"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => Equals(obj as CancellationToken);

        /// <summary>
        /// Serves as a hash function for a <see cref="CancellationToken"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode() => 0;

        /// <summary>
        /// Gets a default/singleton instance of the cancellation token.
        /// </summary>
        /// <value>The instance.</value>
        public static CancellationToken Instance { get; } = new CancellationToken();
    }
}
