using System;
using System.Text.RegularExpressions;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Represents a TALES expression.
  /// </summary>
  public class Expression
  {
    #region constants

    private const string
      PREFIX_PATTERN        = @"^([a-zA-Z0-9]+):",
      VALID_PREFIX_PATTERN  = @"^[a-zA-Z0-9]+$",
      CONTENT_PATTERN       = @"^(?:[a-zA-Z0-9]+:)?(.*)";
    private static readonly Regex
      PrefixMatcher         = new Regex(PREFIX_PATTERN, RegexOptions.Compiled),
      PrefixValidator       = new Regex(VALID_PREFIX_PATTERN, RegexOptions.Compiled),
      ContentMatcher        = new Regex(CONTENT_PATTERN, RegexOptions.Compiled);

    #endregion

    #region fields

    private string _source;

    #endregion

    #region properties

    /// <summary>
    /// Gets the <c>string</c> source of this expression.
    /// </summary>
    /// <value>The source.</value>
    public string Source
    {
      get {
        return _source;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets the expression prefix.
    /// </summary>
    /// <returns>The prefix.</returns>
    public string GetPrefix()
    {
      string output;
      var match = PrefixMatcher.Match(this.Source);

      if(match.Success && match.Groups[1].Value.Length > 0)
      {
        output = match.Groups[1].Value;
      }
      else
      {
        output = null;
      }

      return output;
    }

    /// <summary>
    /// Gets the expression content.
    /// </summary>
    /// <returns>The content.</returns>
    public string GetContent()
    {
      return ContentMatcher.Match(this.Source).Groups[1].Value;
    }

    /// <summary>
    /// Gets the expression content, as a new expression instance.
    /// </summary>
    /// <returns>The content, as a new expression.</returns>
    public Expression GetContentAsExpression()
    {
      var content = this.GetContent();
      return new Expression(content);
    }

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current <see cref="CSF.Zpt.Tales.Expression"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents the current <see cref="CSF.Zpt.Tales.Expression"/>.
    /// </returns>
    public override string ToString()
    {
      return this.Source;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.Expression"/> class.
    /// </summary>
    /// <param name="source">Source.</param>
    public Expression(string source)
    {
      if(source == null)
      {
        throw new ArgumentNullException("source");
      }

      _source = source;
    }

    #endregion

    #region static methods

    /// <summary>
    /// Creates an expression from the given prefix and content.
    /// </summary>
    /// <param name="prefix">Prefix.</param>
    /// <param name="content">Content.</param>
    public static Expression Create(string prefix, string content)
    {
      if(content == null)
      {
        throw new ArgumentNullException("content");
      }
      if(prefix != null
         && !PrefixValidator.IsMatch(prefix))
      {
        throw new FormatException(Resources.ExceptionMessages.InvalidTalesExpressionPrefix);
      }

      return new Expression((prefix != null)? String.Concat(prefix, ":", content) : content);
    }

    #endregion
  }
}

