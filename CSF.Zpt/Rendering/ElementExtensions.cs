using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Extension methods for the <see cref="Element"/> type and its subtypes.
  /// </summary>
  public static class ElementExtensions
  {
    #region METAL extension methods

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

      return element.GetAttribute(Metal.Namespace,
                                  Metal.DefaultPrefix,
                                  attributeName);
    }

    /// <summary>
    /// Recursively searches upwards in the DOM tree, returning the first (closest) ancestor element which has a METAL
    /// attribute matching the given name.
    /// </summary>
    /// <returns>The closest ancestor element, or a <c>null</c> reference if no ancestor was found.</returns>
    /// <param name="element">The element from which to begin the search.</param>
    /// <param name="attributeName">The attribute name.</param>
    public static Element SearchAncestorsByMetalAttribute(this Element element, string attributeName)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }

      return element.SearchAncestorsByAttribute(Metal.Namespace,
                                                Metal.DefaultPrefix,
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

      return element.SearchChildrenByAttribute(Metal.Namespace,
                                               Metal.DefaultPrefix,
                                               attributeName);
    }

    /// <summary>
    /// Recursively searches for METAL attributes and removes them from their parent element.
    /// </summary>
    /// <param name="element">The element from which to perform the purge.</param>
    public static void PurgeMetalAttributes(this Element element)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }

      element.PurgeAttributes(Metal.Namespace, Metal.DefaultPrefix);
    }

    #endregion

    #region TAL extension methods

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

      return element.GetAttribute(Tal.Namespace,
                                  Tal.DefaultPrefix,
                                  attributeName);
    }

    /// <summary>
    /// Recursively searches upwards in the DOM tree, returning the first (closest) ancestor element which has a TAL
    /// attribute matching the given name.
    /// </summary>
    /// <returns>The closest ancestor element, or a <c>null</c> reference if no ancestor was found.</returns>
    /// <param name="element">The element from which to begin the search.</param>
    /// <param name="attributeName">The attribute name.</param>
    public static Element SearchAncestorsByTalAttribute(this Element element, string attributeName)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }

      return element.SearchAncestorsByAttribute(Tal.Namespace,
                                                Tal.DefaultPrefix,
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

      return element.SearchChildrenByAttribute(Tal.Namespace,
                                               Tal.DefaultPrefix,
                                               attributeName);
    }

    /// <summary>
    /// Recursively searches for TAL attributes and removes them from their parent element.
    /// </summary>
    /// <param name="element">The element from which to perform the purge.</param>
    public static void PurgeTalAttributes(this Element element)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }

      element.PurgeAttributes(Tal.Namespace, Tal.DefaultPrefix);
    }

    #endregion
  }
}

