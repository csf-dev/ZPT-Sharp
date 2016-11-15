﻿using System;
using CSF.Zpt.Rendering;
using System.Diagnostics;

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
      private static readonly ZptNamespace _namespace;

      /// <summary>
      /// Gets the namespace for METAL: Macro Expansion Template Attribute Language.
      /// </summary>
      /// <value>The namespace.</value>
      public static ZptNamespace Namespace
      {
        get {
          return _namespace;
        }
      }

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

      /// <summary>
      /// Initializes the <see cref="T:CSF.Zpt.ZptConstants+Metal"/> class.
      /// </summary>
      static Metal()
      {
        _namespace = new ZptNamespace("metal", "http://xml.zope.org/namespaces/metal");
      }
    }

    /// <summary>
    /// Static type for TAL constants.
    /// </summary>
    public static class Tal
    {
      private static readonly ZptNamespace _namespace;

      /// <summary>
      /// Gets the namespace for TAL: Template Attribute Language.
      /// </summary>
      /// <value>The namespace.</value>
      public static ZptNamespace Namespace
      {
        get {
          return _namespace;
        }
      }

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

      /// <summary>
      /// Initializes the <see cref="T:CSF.Zpt.ZptConstants+Tal"/> class.
      /// </summary>
      static Tal()
      {
        _namespace = new ZptNamespace("tal", "http://xml.zope.org/namespaces/tal");
      }
    }

    /// <summary>
    /// Gets a special singleton value that indicates the "cancel the current action" token.
    /// </summary>
    public static readonly object CancellationToken = new CancellationToken();

    /// <summary>
    /// Gets the name of the <see cref="TraceSource"/> which is used for all ZPT-related messages.
    /// </summary>
    public static readonly string TraceSourceName = typeof(ZptConstants).Namespace;

    /// <summary>
    /// Gets a <c>System.Diagnostics.TraceSource</c> instance which is used for all ZPT-related messages.
    /// </summary>
    public static readonly TraceSource TraceSource = new TraceSource(TraceSourceName);
  }
}
