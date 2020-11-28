using System;
namespace ZptSharp.Dom
{
    /// <summary>
    /// Extension methods for <see cref="IReplacesNode"/>.
    /// </summary>
    public static class NodeReplacerExtensions
    {
        /// <summary>
        /// Replace the specified node with the specified replacement.
        /// </summary>
        /// <param name="toReplace">The node to replace.</param>
        /// <param name="replacement">The replacement node.</param>
        public static void Replace(this IReplacesNode replacer, INode toReplace, INode replacement)
        {
            if (replacer == null)
                throw new ArgumentNullException(nameof(replacer));

            replacer.Replace(toReplace, new[] { replacement });
        }
    }
}
