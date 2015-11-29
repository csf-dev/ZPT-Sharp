using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Static type for TAL constants.
  /// </summary>
  public static class Tal
  {
    #region constants

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

    #endregion
  }
}

