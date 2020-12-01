using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// Default implementation of <see cref="IParsesPathExpression"/>.
    /// </summary>
    public class PathExpressionParser : IParsesPathExpression
    {
        const char
            AlternateExpressionSeparator = '|',
            PartSeparator = '/',
            InterpolatedSignifier = '?';

        const string
            VariableNamePattern = "^[a-z][a-z0-9_]*$",
            PathPartPattern = "^([a-z0-9 _.,~-]+|\\?[a-z][a-z0-9_]*)$";

        static readonly Regex
            VariableName = new Regex(VariableNamePattern, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase),
            PathPart = new Regex(PathPartPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        /// <summary>
        /// Gets a <see cref="PathExpression"/> model object from a specified <paramref name="expression"/> string.
        /// </summary>
        /// <returns>The parsed model object.</returns>
        /// <param name="expression">The expression string.</param>
        /// <exception cref="CannotParsePathException">If the expression cannot be parsed.</exception>
        public PathExpression Parse(string expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            try
            {
                var alternates = expression
                    .Split(AlternateExpressionSeparator)
                    .Select(x => x.Trim())
                    .Select(GetAlternateExpression)
                    .ToList();
                return new PathExpression(alternates);
            }
            catch(Exception ex)
            {
                throw new CannotParsePathException(String.Format(Resources.ExceptionMessage.CannotParsePath, expression), ex);
            }
        }

        PathExpression.AlternateExpression GetAlternateExpression(string expressionString)
        {
            if (String.IsNullOrEmpty(expressionString))
                throw new ArgumentException(Resources.ExceptionMessage.AlternatePathExpressionCannotBeEmpty, nameof(expressionString));

            var parts = expressionString
                .Split(PartSeparator)
                .Select(GetPart)
                .ToList();

            return new PathExpression.AlternateExpression(parts);
        }

        PathExpression.PathPart GetPart(string partString, int index)
        {
            if (String.IsNullOrEmpty(partString))
                throw new ArgumentException(Resources.ExceptionMessage.PathPartCannotBeEmpty, nameof(partString));

            // The first part of a path must be a valid variable name, with more strict rules for validation.
            // Subsequent parts have more relaxed validation rules.
            if (index == 0)
                AssertPartIsValidVariableName(partString);
            else
                AssertPartIsValid(partString);

            var isInterpolated = partString.First() == InterpolatedSignifier;
            if (!isInterpolated)
                return new PathExpression.PathPart(partString);

            return new PathExpression.PathPart(partString.Substring(1), true);
        }

        void AssertPartIsValidVariableName(string partString)
        {
            if (VariableName.IsMatch(partString)) return;
            var message = String.Format(Resources.ExceptionMessage.InvalidVariableName, partString);
            throw new ArgumentException(message, nameof(partString));
        }

        void AssertPartIsValid(string partString)
        {
            if (PathPart.IsMatch(partString)) return;
            var message = String.Format(Resources.ExceptionMessage.InvalidPathPart, partString);
            throw new ArgumentException(message, nameof(partString));
        }
    }
}
