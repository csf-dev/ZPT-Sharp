using System;
namespace ZptSharp.Expressions
{
    /// <summary>
    /// An object which indicates that the current ZPT action should be cancelled/aborted.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This object is used only as the return from
    /// <see cref="IEvaluatesExpression.EvaluateExpressionAsync(string, ExpressionContext, System.Threading.CancellationToken)"/>,
    /// when an expression wishes to indicate to ZptSharp that the current operation should be aborted.
    /// </para>
    /// <para>
    /// Each TAL operation reacts differently to receiving this object as the result of an expression.
    /// As a rule of thumb though, when an expression results in an instance of this object, the TAL
    /// operation will attempt to avoid altering the source document.
    /// For example, a <c>tal:content</c> attribute would abort changing the content of the element if the
    /// attribute value evaluates to an instance of this type.
    /// In that case, the original content from the source file would be left in-place, as if the
    /// <c>tal:content</c> attribute had not been present at all.
    /// </para>
    /// <para>
    /// Whilst it is possible to construct new instances of this type, because it has no state and is sealed,
    /// it is pointless to do so.  All instances of this type are considered equal and will all produce the
    /// same hash codes.
    /// It is recommended to use <see cref="Instance"/> in order to refer to this object.
    /// </para>
    /// </remarks>
    public sealed class AbortZptActionToken : IEquatable<AbortZptActionToken>
    {
        /// <summary>
        /// Determines whether the specified <see cref="AbortZptActionToken"/> is equal to the
        /// current <see cref="AbortZptActionToken"/>.
        /// </summary>
        /// <remarks><para>All instances of <see cref="AbortZptActionToken"/> are considered equal by this method.</para></remarks>
        /// <param name="other">The <see cref="AbortZptActionToken"/> to compare with the current <see cref="AbortZptActionToken"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="AbortZptActionToken"/> is equal to the current
        /// <see cref="AbortZptActionToken"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(AbortZptActionToken other) => !(other is null);

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="AbortZptActionToken"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Any instance of <see cref="AbortZptActionToken"/> is considered equal to any other instance of
        /// <see cref="AbortZptActionToken"/> by this method.
        /// This type is not considered equal to any instance of any other type, though.
        /// </para>
        /// </remarks>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="AbortZptActionToken"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="AbortZptActionToken"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => Equals(obj as AbortZptActionToken);

        /// <summary>
        /// Serves as a hash function for a <see cref="AbortZptActionToken"/> object.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method returns a hard-coded hash code of zero.  This means that all instances of this type
        /// will always return the same hash code.  This type is not suitable for use as the key of a hash table.
        /// </para>
        /// </remarks>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode() => 0;

        /// <summary>
        /// Gets a default/singleton instance of the abort-action token.
        /// For clarity, this is the recommended way to get a reference to this type.
        /// </summary>
        /// <value>The instance.</value>
        public static AbortZptActionToken Instance { get; } = new AbortZptActionToken();
    }
}
