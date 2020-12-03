using System;
using System.Collections.Generic;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// Model for a path expression, providing access to the individual parts.
    /// </summary>
    public class PathExpression
    {
        /// <summary>
        /// Gets a collection of the alternate expressions which make up this overall path expression.
        /// </summary>
        /// <value>The alternate expressions.</value>
        public IReadOnlyList<AlternateExpression> Alternates { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathExpression"/> class from a list of alternate expressions.
        /// </summary>
        /// <param name="alternates">The alternates.</param>
        public PathExpression(IList<AlternateExpression> alternates)
        {
            Alternates = new List<AlternateExpression>(alternates);
        }

        /// <summary>
        /// Model for an alternate expression, which would be part of a larger <see cref="PathExpression"/>.
        /// </summary>
        public class AlternateExpression
        {
            /// <summary>
            /// Gets the parts within this expression.
            /// </summary>
            /// <value>The path parts within this expression.</value>
            public IReadOnlyList<PathPart> Parts { get; }

            /// <summary>
            /// Initializes a new instance of the
            /// <see cref="AlternateExpression"/> class.
            /// </summary>
            /// <param name="parts">The path parts.</param>
            public AlternateExpression(IReadOnlyList<PathPart> parts)
            {
                Parts = parts ?? throw new ArgumentNullException(nameof(parts));
            }

            /// <summary>
            /// Initializes a new instance of the
            /// <see cref="AlternateExpression"/> class.
            /// </summary>
            /// <param name="parts">The path parts.</param>
            public AlternateExpression(params PathPart[] parts)
            {
                Parts = parts ?? throw new ArgumentNullException(nameof(parts));
            }
        }

        /// <summary>
        /// Base type for a single part of a path expression.
        /// </summary>
        public class PathPart
        {
            /// <summary>
            /// Gets the name of the path part.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; }

            /// <summary>
            /// Gets a value indicating whether this <see cref="PathPart"/> represents an interpolated path name.
            /// </summary>
            /// <value><c>true</c> if this part is interpolated; otherwise, <c>false</c>.</value>
            public bool IsInterpolated { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="PathPart"/> class.
            /// </summary>
            /// <param name="name">The name of the path part.</param>
            /// <param name="isInterpolated"><c>true</c> if this path part represents an interpolated name; <c>false</c> otherwise.</param>
            public PathPart(string name, bool isInterpolated = false)
            {
                Name = name ?? throw new ArgumentNullException(nameof(name));
                IsInterpolated = isInterpolated;
            }
        }
    }
}
