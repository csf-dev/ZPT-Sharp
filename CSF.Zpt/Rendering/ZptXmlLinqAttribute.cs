using System;
using System.Xml.Linq;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Implementation of <see cref="Attribute"/> for an XML/Linq attribute.
  /// </summary>
  public class ZptXmlLinqAttribute : ZptAttribute
  {
    #region fields

    private XAttribute _original;

    #endregion

    #region properties

    /// <summary>
    /// Gets the attribute value
    /// </summary>
    /// <value>The value.</value>
    public override string Value
    {
      get {
        return _original.Value;
      }
    }

    /// <summary>
    /// Gets the attribute name, including its prefix if applicable.
    /// </summary>
    /// <value>The name.</value>
    public override string Name
    {
      get {
        return _original.Name.ToString();
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

      return (((_original.Name.Namespace.NamespaceName == null
                && nspace.Uri == null)
               || (_original.Name.Namespace.NamespaceName == nspace.Uri
                   || (String.IsNullOrEmpty(nspace.Uri) && String.IsNullOrEmpty(_original.Name.Namespace.NamespaceName))))
              && _original.Name.LocalName == name);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.ZptXmlLinqAttribute"/> class.
    /// </summary>
    /// <param name="original">The original (wrapped) attribute.</param>
    public ZptXmlLinqAttribute(XAttribute original)
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

