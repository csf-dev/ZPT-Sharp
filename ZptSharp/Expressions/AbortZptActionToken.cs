using System;
namespace ZptSharp.Expressions
{
    /// <summary>
    /// An object which indicates that the current ZPT action should be cancelled/aborted.
    /// </summary>
    public sealed class AbortZptActionToken : IEquatable<AbortZptActionToken>
    {
        /// <summary>
        /// Determines whether the specified <see cref="AbortZptActionToken"/> is equal to the
        /// current <see cref="AbortZptActionToken"/>.
        /// </summary>
        /// <param name="other">The <see cref="AbortZptActionToken"/> to compare with the current <see cref="AbortZptActionToken"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="AbortZptActionToken"/> is equal to the current
        /// <see cref="AbortZptActionToken"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(AbortZptActionToken other) => !(other is null);

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="AbortZptActionToken"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="AbortZptActionToken"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="AbortZptActionToken"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => Equals(obj as AbortZptActionToken);

        /// <summary>
        /// Serves as a hash function for a <see cref="AbortZptActionToken"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode() => 0;

        /// <summary>
        /// Gets a default/singleton instance of the cancellation token.
        /// </summary>
        /// <value>The instance.</value>
        public static AbortZptActionToken Instance { get; } = new AbortZptActionToken();
    }
}
