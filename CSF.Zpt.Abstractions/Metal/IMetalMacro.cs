using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Metal
{
  /// <summary>
  /// Represents a METAL macro and its name.
  /// </summary>
  public interface IMetalMacro
  {
    /// <summary>
    /// Gets the macro name.
    /// </summary>
    /// <value>The name.</value>
    string Name { get; }

    /// <summary>
    /// Gets the element.
    /// </summary>
    /// <value>The element.</value>
    IZptElement Element { get; }
  }
}
