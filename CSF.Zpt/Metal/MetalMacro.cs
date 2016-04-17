using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Metal
{
  /// <summary>
  /// Represents a METAL macro and its name.
  /// </summary>
  public class MetalMacro
  {
    #region properties

    /// <summary>
    /// Gets the macro name.
    /// </summary>
    /// <value>The name.</value>
    public string Name
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the element.
    /// </summary>
    /// <value>The element.</value>
    public ZptElement Element
    {
      get;
      private set;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Metal.MetalMacro"/> class.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="element">Element.</param>
    public MetalMacro(string name, ZptElement element)
    {
      if(name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      this.Name = name;
      this.Element = element;
    }

    #endregion
  }
}

