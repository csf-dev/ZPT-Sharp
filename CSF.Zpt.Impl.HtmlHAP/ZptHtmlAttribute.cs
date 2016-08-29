using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Impl
{
  /// <summary>
  /// Implementation of <see cref="Attribute"/> for an HTML attribute.
  /// </summary>
  public class ZptHtmlAttribute : ZptAttribute
  {
    #region fields

    private HtmlAgilityPack.HtmlAttribute _original;

    #endregion

    #region properties

    /// <summary>
    /// Gets the attribute value
    /// </summary>
    /// <value>The value.</value>
    public override string Value
    {
      get {
        return HtmlAgilityPack.HtmlEntity.DeEntitize(_original.Value);
      }
    }

    /// <summary>
    /// Gets the attribute name, including its prefix if applicable.
    /// </summary>
    /// <value>The name.</value>
    public override string Name
    {
      get {
        return _original.Name;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Determines whether this instance matches the given namespace and attribute name or not.
    /// </summary>
    /// <returns><c>true</c> if this instance matches the specified namespace and name; otherwise, <c>false</c>.</returns>
    /// <param name="nspace">The attribute namespace.</param>
    /// <param name="name">The attribute name.</param>
    public override bool IsMatch(ZptNamespace nspace, string name)
    {
      if(nspace == null)
      {
        throw new ArgumentNullException(nameof(nspace));
      }

      return ZptHtmlElement.GetNameWithPrefix(nspace, name) == this.Name;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.ZptHtmlAttribute"/> class.
    /// </summary>
    /// <param name="original">The original (wrapped) attribute.</param>
    public ZptHtmlAttribute(HtmlAgilityPack.HtmlAttribute original)
    {
      if(original == null)
      {
        throw new ArgumentNullException(nameof(original));
      }

      _original = original;
    }

    #endregion
  }
}

