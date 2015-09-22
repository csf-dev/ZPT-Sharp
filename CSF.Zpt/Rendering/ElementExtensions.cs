using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Extension methods for the <see cref="Element"/> type and its subtypes.
  /// </summary>
  public static class ElementExtensions
  {
    #region extension methods

    /// <summary>
    /// Gets a METAL attribute which matches the given criteria, or a <c>null</c> reference is no matching attribute is
    /// found.
    /// </summary>
    /// <returns>The attribute, or a <c>null</c> reference.</returns>
    /// <param name="element">The element from which to get attributes.</param>
    /// <param name="attributeName">The attribute name.</param>
    public static Attribute GetMetalAttribute(this Element element, string attributeName)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }

      return element.GetAttribute(ZptDocument.MetalNamespace,
                                  ZptDocument.MetalAttributePrefix,
                                  attributeName);
    }

    /// <summary>
    /// Gets a TAL attribute which matches the given criteria, or a <c>null</c> reference is no matching attribute is
    /// found.
    /// </summary>
    /// <returns>The attribute, or a <c>null</c> reference.</returns>
    /// <param name="element">The element from which to get attributes.</param>
    /// <param name="attributeName">The attribute name.</param>
    public static Attribute GetTalAttribute(this Element element, string attributeName)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }

      return element.GetAttribute(ZptDocument.TalNamespace,
                                  ZptDocument.TalAttributePrefix,
                                  attributeName);
    }

    /// <summary>
    /// Recursively searches the children of the current instance, returning a collection of elements which have a
    /// matching METAL attribute.
    /// </summary>
    /// <returns>The matching child elements.</returns>
    /// <param name="element">The element from which to perform the search.</param>
    /// <param name="attributeName">The attribute name.</param>
    public static Element[] SearchChildrenByMetalAttribute(this Element element, string attributeName)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }

      return element.SearchChildrenByAttribute(ZptDocument.MetalNamespace,
                                               ZptDocument.MetalAttributePrefix,
                                               attributeName);
    }

    /// <summary>
    /// Recursively searches the children of the current instance, returning a collection of elements which have a
    /// matching TAL attribute.
    /// </summary>
    /// <returns>The matching child elements.</returns>
    /// <param name="element">The element from which to perform the search.</param>
    /// <param name="attributeName">The attribute name.</param>
    public static Element[] SearchChildrenByTalAttribute(this Element element, string attributeName)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }

      return element.SearchChildrenByAttribute(ZptDocument.TalNamespace,
                                               ZptDocument.TalAttributePrefix,
                                               attributeName);
    }

    #endregion
  }
}

