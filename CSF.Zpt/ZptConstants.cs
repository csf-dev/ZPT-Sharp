using System;

namespace CSF.Zpt
{
  /// <summary>
  /// Static type holding constant values shared amongst the ZPT system.
  /// </summary>
  public static class ZptConstants
  {
    /// <summary>
    /// Static type for METAL constants.
    /// </summary>
    public static class Metal
    {
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
    }

    /// <summary>
    /// Static type for TAL constants.
    /// </summary>
    public static class Tal
    {
      /// <summary>
      /// Gets the XML namespace for TAL: Template Attribute Language.
      /// </summary>
      public static readonly string Namespace = "http://xml.zope.org/namespaces/tal";

      /// <summary>
      /// Gets the name of the default attribute/element name prefix.
      /// </summary>
      public static readonly string DefaultPrefix = "tal";

      /// <summary>
      /// Gets the name of the define attribute.
      /// </summary>
      public static readonly string DefineAttribute = "define";

      /// <summary>
      /// Gets the name of the attributes attribute.
      /// </summary>
      public static readonly string AttributesAttribute = "attributes";

      /// <summary>
      /// Gets the name of the condition attribute.
      /// </summary>
      public static readonly string ConditionAttribute = "condition";

      /// <summary>
      /// Gets the name of the replace attribute.
      /// </summary>
      public static readonly string ReplaceAttribute = "replace";

      /// <summary>
      /// Gets the name of the content attribute.
      /// </summary>
      public static readonly string ContentAttribute = "content";

      /// <summary>
      /// Gets the name of the repeat attribute.
      /// </summary>
      public static readonly string RepeatAttribute = "repeat";

      /// <summary>
      /// Gets the name of the on-error attribute.
      /// </summary>
      public static readonly string OnErrorAttribute = "on-error";

      /// <summary>
      /// Gets the name of the omit-tag attribute.
      /// </summary>
      public static readonly string OmitTagAttribute = "omit-tag";
    }
  }
}

