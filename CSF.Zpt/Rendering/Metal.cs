using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Static type for METAL constants.
  /// </summary>
  public static class Metal
  {
    #region constants

    /// <summary>
    /// Gets the XML namespace for METAL: Macro Expansion Template Attribute Language.
    /// </summary>
    public static readonly string Namespace = "http://xml.zope.org/namespaces/metal";

    /// <summary>
    /// Gets the name of the default attribute/element name prefix.
    /// </summary>
    public static readonly string DefaultPrefix = "metal";

    /// <summary>
    /// Gets the name of the define-macro attribute.
    /// </summary>
    public static readonly string DefineMacroAttribute = "define-macro";

    /// <summary>
    /// Gets the name of the extend-macro attribute.
    /// </summary>
    public static readonly string ExtendMacroAttribute = "extend-macro";

    /// <summary>
    /// Gets the name of the use-macro attribute.
    /// </summary>
    public static readonly string UseMacroAttribute = "use-macro";

    /// <summary>
    /// Gets the name of the define-slot attribute.
    /// </summary>
    public static readonly string DefineSlotAttribute = "define-slot";

    /// <summary>
    /// Gets the name of the fill-slot attribute.
    /// </summary>
    public static readonly string FillSlotAttribute = "fill-slot";

    /// <summary>
    /// Gets the name of the 'modelname' variable in the METAL model.
    /// </summary>
    public static readonly string MacroNameModelName = "macroname";

    #endregion
  }
}

