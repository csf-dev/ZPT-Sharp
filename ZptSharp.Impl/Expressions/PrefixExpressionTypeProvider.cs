using System;
using System.Text.RegularExpressions;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// Implementation of <see cref="IGetsExpressionType"/> which reads the expression prefix.
    /// This class also implements <see cref="IRemovesPrefixFromExpression"/>.
    /// </summary>
    public class PrefixExpressionTypeProvider : IGetsExpressionType, IRemovesPrefixFromExpression
    {
        const string PrefixPattern = @"^([a-z][a-z0-9]*):";
        static readonly Regex Prefix = new Regex(PrefixPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        /// <summary>
        /// Gets the expression type for the specified <paramref name="expression"/>.
        /// </summary>
        /// <returns>The expression type.</returns>
        /// <param name="expression">An expression.</param>
        public string GetExpressionType(string expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            var match = Prefix.Match(expression);
            return match.Success ? match.Groups[1].Value : null;
        }

        /// <summary>
        /// Gets the expression with its expression-type prefix removed (if it had one).
        /// </summary>
        /// <returns>The expression with its prefix removed.</returns>
        /// <param name="expression">An expression which might have a prefix.</param>
        public string GetExpressionWithoutPrefix(string expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            return Prefix.Replace(expression, String.Empty);
        }
    }
}
