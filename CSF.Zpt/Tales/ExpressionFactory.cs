using System;
using System.Text.RegularExpressions;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Creator for <see cref="Expression"/> instances.
  /// </summary>
  public class ExpressionFactory : IExpressionFactory
  {
    #region constants

    private const string
      PREFIX_PATTERN        = @"^([a-zA-Z0-9]+):",
      VALID_PREFIX_PATTERN  = @"^[a-zA-Z0-9]+$",
      CONTENT_PATTERN       = @"^(?:[a-zA-Z0-9]+:)?(.*)";
    private static readonly Regex
      PrefixMatcher         = new Regex(PREFIX_PATTERN, RegexOptions.Compiled),
      PrefixValidator       = new Regex(VALID_PREFIX_PATTERN, RegexOptions.Compiled),
      ContentMatcher        = new Regex(CONTENT_PATTERN, RegexOptions.Compiled | RegexOptions.Singleline);

    #endregion

    #region methods

    /// <summary>
    /// Creates an expression from the given prefix and content.
    /// </summary>
    /// <param name="prefix">Prefix.</param>
    /// <param name="content">Content.</param>
    public Expression Create(string prefix, string content)
    {
      if(content == null)
      {
        throw new ArgumentNullException(nameof(content));
      }

      if(prefix != null && !PrefixValidator.IsMatch(prefix))
      {
        throw new FormatException(Resources.ExceptionMessages.InvalidTalesExpressionPrefix);
      }

      return new Expression(prefix, content);
    }

    /// <summary>
    /// Create an expression from the specified source string.
    /// </summary>
    /// <param name="source">Source.</param>
    public Expression Create(string source)
    {
      if(source == null)
      {
        throw new ArgumentNullException(nameof(source));
      }

      var prefixMatch = PrefixMatcher.Match(source);
      string
        prefix = prefixMatch.Success? prefixMatch.Groups[1].Value : null,
        content = ContentMatcher.Match(source).Groups[1].Value;

      return Create(prefix, content);
    }

    /// <summary>
    /// Create an expression from the content of another expression.
    /// </summary>
    /// <param name="expression">The expression from which to create another expression.</param>
    public Expression Create(Expression expression)
    {
      if(expression == null)
      {
        throw new ArgumentNullException(nameof(expression));
      }

      return Create(expression.Content);
    }

    #endregion
  }
}

