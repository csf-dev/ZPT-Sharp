using System;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Represents a TALES expression.
  /// </summary>
  public class Expression
  {
    #region properties

    /// <summary>
    /// Gets the expression prefix.
    /// </summary>
    /// <value>The prefix.</value>
    /// 
    public string Prefix { get; private set; }

    /// <summary>
    /// Gets the expression content.
    /// </summary>
    /// <value>The content.</value>
    public string Content { get; private set; }

    #endregion

    #region methods

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current <see cref="CSF.Zpt.Tales.Expression"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents the current <see cref="CSF.Zpt.Tales.Expression"/>.
    /// </returns>
    public override string ToString()
    {
      return String.IsNullOrEmpty(Prefix)? Content : String.Concat(Prefix, ":", Content);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.Expression"/> class.
    /// </summary>
    /// <param name="prefix">Prefix.</param>
    /// <param name="content">Content.</param>
    public Expression(string prefix, string content)
    {
      if(content == null)
      {
        throw new ArgumentNullException(nameof(content));
      }

      Prefix = prefix;
      Content = content;
    }

    #endregion
  }
}

