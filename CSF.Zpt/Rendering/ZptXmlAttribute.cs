using System;
using System.Xml;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Implementation of <see cref="Attribute"/> for an XML attribute.
  /// </summary>
  public class ZptXmlAttribute : ZptAttribute
  {
    #region fields

    private XmlAttribute _original;

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
        return _original.Name;
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.ZptXmlAttribute"/> class.
    /// </summary>
    /// <param name="original">The original (wrapped) attribute.</param>
    public ZptXmlAttribute(XmlAttribute original)
    {
      if(original == null)
      {
        throw new ArgumentNullException("original");
      }

      _original = original;
    }

    #endregion
  }
}

