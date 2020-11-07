using System;
using System.Collections.Generic;

namespace ZptSharp.PathExpressions
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
        /// Initializes a new instance of the <see cref="PathExpression"/> class from a collection of path parts.
        /// </summary>
        /// <param name="parts">The path parts.</param>
        public PathExpression(params PathPart[] parts)
        {
            var alternate = new AlternateExpression(parts);
            Alternates = new[] { alternate };
        }

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
        }

        /// <summary>
        /// Base type for a single part of a path expression.
        /// </summary>
        public abstract class PathPart { }

        /// <summary>
        /// A part of a path expression which indicates a traversible name.
        /// </summary>
        public class NamedPathPart : PathPart
        {
            /// <summary>
            /// Gets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="NamedPathPart"/> class.
            /// </summary>
            /// <param name="name">Name.</param>
            public NamedPathPart(string name)
            {
                Name = name ?? throw new ArgumentNullException(nameof(name));
            }
        }

        /// <summary>
        /// A part of a path expression which exposes a traversible name via an
        /// interpolated value (which is, itself, a path expression).
        /// </summary>
        public class InterpolatedPathPart : PathPart
        {
            /// <summary>
            /// The path expression used to get the interpolated part-name.
            /// </summary>
            /// <value>The expression.</value>
            public PathExpression Expression { get; }

            /// <summary>
            /// Initializes a new instance of the
            /// <see cref="InterpolatedPathPart"/> class.
            /// </summary>
            /// <param name="expression">Expression.</param>
            public InterpolatedPathPart(PathExpression expression)
            {
                Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            }
        }
    }
}
