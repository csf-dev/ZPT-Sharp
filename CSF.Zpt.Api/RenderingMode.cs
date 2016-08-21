using System;

namespace CSF.Zpt
{
  /// <summary>
  /// Enumerates the available rendering modes.
  /// </summary>
  public enum RenderingMode
  {
    /// <summary>
    /// Automatically detects between HTML and XML as appropriate.
    /// </summary>
    AutoDetect = 0,

    /// <summary>
    /// Forces the rendering to be treated as HTML.
    /// </summary>
    Html,

    /// <summary>
    /// Forces the rendering to be treated as XML.
    /// </summary>
    Xml,

    /// <summary>
    /// Forces the rendering to be treated as XML, using <c>System.Xml.Linq</c>.
    /// </summary>
    XmlLinq
  }
}

