using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Extension methods for the <see cref="ZptElement"/> type and its subtypes.
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
    public static ZptAttribute GetMetalAttribute(this ZptElement element, string attributeName)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }

      return element.GetAttribute(ZptConstants.Metal.Namespace,
                                  attributeName);
    }

    /// <summary>
    /// Recursively searches upwards in the DOM tree, returning the first (closest) ancestor element which has a METAL
    /// attribute matching the given name.
    /// </summary>
    /// <returns>The closest ancestor element, or a <c>null</c> reference if no ancestor was found.</returns>
    /// <param name="element">The element from which to begin the search.</param>
    /// <param name="attributeName">The attribute name.</param>
    public static ZptElement SearchAncestorsByMetalAttribute(this ZptElement element, string attributeName)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }

      return element.SearchAncestorsByAttribute(ZptConstants.Metal.Namespace,
                                                attributeName);
    }

    /// <summary>
    /// Recursively searches the children of the current instance, returning a collection of elements which have a
    /// matching METAL attribute.
    /// </summary>
    /// <returns>The matching child elements.</returns>
    /// <param name="element">The element from which to perform the search.</param>
    /// <param name="attributeName">The attribute name.</param>
    public static ZptElement[] SearchChildrenByMetalAttribute(this ZptElement element, string attributeName)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }

      return element.SearchChildrenByAttribute(ZptConstants.Metal.Namespace,
                                               attributeName);
    }

    /// <summary>
    /// Recursively searches for METAL attributes and removes them from their parent element.
    /// </summary>
    /// <param name="element">The element from which to perform the purge.</param>
    public static void PurgeMetalAttributes(this ZptElement element)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }

      element.PurgeAttributes(ZptConstants.Metal.Namespace);
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
    public static ZptAttribute GetTalAttribute(this ZptElement element, string attributeName)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }

      return element.GetAttribute(ZptConstants.Tal.Namespace,
                                  attributeName);
    }

    /// <summary>
    /// Recursively searches upwards in the DOM tree, returning the first (closest) ancestor element which has a TAL
    /// attribute matching the given name.
    /// </summary>
    /// <returns>The closest ancestor element, or a <c>null</c> reference if no ancestor was found.</returns>
    /// <param name="element">The element from which to begin the search.</param>
    /// <param name="attributeName">The attribute name.</param>
    public static ZptElement SearchAncestorsByTalAttribute(this ZptElement element, string attributeName)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }

      return element.SearchAncestorsByAttribute(ZptConstants.Tal.Namespace,
                                                attributeName);
    }

    /// <summary>
    /// Recursively searches the children of the current instance, returning a collection of elements which have a
    /// matching TAL attribute.
    /// </summary>
    /// <returns>The matching child elements.</returns>
    /// <param name="element">The element from which to perform the search.</param>
    /// <param name="attributeName">The attribute name.</param>
    public static ZptElement[] SearchChildrenByTalAttribute(this ZptElement element, string attributeName)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }

      return element.SearchChildrenByAttribute(ZptConstants.Tal.Namespace,
                                               attributeName);
    }

    /// <summary>
    /// Recursively searches for TAL attributes and removes them from their parent element.
    /// </summary>
    /// <param name="element">The element from which to perform the purge.</param>
    public static void PurgeTalAttributes(this ZptElement element)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }

      element.PurgeAttributes(ZptConstants.Tal.Namespace);
    }

    #endregion
  }
}

