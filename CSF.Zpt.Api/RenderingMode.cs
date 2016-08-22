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
    /// <remarks>
    /// <para>
    /// By default, for documents detected as XML, the <see cref="XmlLinq"/> implementation will be favoured over
    /// the plain <see cref="Xml"/> one.  This better-adheres to ZPT's features, only use the <c>System.Xml</c>
    /// implementation if you really need it.
    /// </para>
    /// </remarks>
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

