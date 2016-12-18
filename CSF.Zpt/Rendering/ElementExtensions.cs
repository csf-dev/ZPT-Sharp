using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Extension methods for the <see cref="IZptElement"/> type and its subtypes.
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
    public static IZptAttribute GetMetalAttribute(this IZptElement element, string attributeName)
    {
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      return element.GetAttribute(ZptConstants.Metal.Namespace, attributeName);
    }

    /// <summary>
    /// Recursively searches upwards in the DOM tree, returning the first (closest) ancestor element which has a METAL
    /// attribute matching the given name.
    /// </summary>
    /// <returns>The closest ancestor element, or a <c>null</c> reference if no ancestor was found.</returns>
    /// <param name="element">The element from which to begin the search.</param>
    /// <param name="attributeName">The attribute name.</param>
    public static IZptElement SearchAncestorsByMetalAttribute(this IZptElement element, string attributeName)
    {
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      return element.SearchAncestorsByAttribute(ZptConstants.Metal.Namespace, attributeName);
    }

    /// <summary>
    /// Recursively searches the children of the current instance, returning a collection of elements which have a
    /// matching METAL attribute.
    /// </summary>
    /// <returns>The matching child elements.</returns>
    /// <param name="element">The element from which to perform the search.</param>
    /// <param name="attributeName">The attribute name.</param>
    public static IZptElement[] SearchChildrenByMetalAttribute(this IZptElement element, string attributeName)
    {
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      return element.SearchChildrenByAttribute(ZptConstants.Metal.Namespace, attributeName);
    }

    /// <summary>
    /// Recursively searches for METAL attributes and removes them from their parent element.
    /// </summary>
    /// <param name="element">The element from which to perform the purge.</param>
    public static void PurgeMetalAttributes(this IZptElement element)
    {
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      element.PurgeAttributes(ZptConstants.Metal.Namespace);
    }

    /// <summary>
    /// Recursively searches for elements in the METAL namespace and removes them from their parent element.
    /// </summary>
    /// <param name="element">The element from which to perform the purge.</param>
    public static void PurgeMetalElements(this IZptElement element)
    {
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      element.PurgeElements(ZptConstants.Metal.Namespace);
    }

    #endregion

    #region TAL extension methods

    /// <summary>
    /// Recursively searches upwards in the DOM tree, returning the first (closest) ancestor element which has a TAL
    /// attribute matching the given name.
    /// </summary>
    /// <returns>The closest ancestor element, or a <c>null</c> reference if no ancestor was found.</returns>
    /// <param name="element">The element from which to begin the search.</param>
    /// <param name="attributeName">The attribute name.</param>
    public static IZptElement SearchAncestorsByTalAttribute(this IZptElement element, string attributeName)
    {
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      return element.SearchAncestorsByAttribute(ZptConstants.Tal.Namespace, attributeName);
    }

    /// <summary>
    /// Recursively searches the children of the current instance, returning a collection of elements which have a
    /// matching TAL attribute.
    /// </summary>
    /// <returns>The matching child elements.</returns>
    /// <param name="element">The element from which to perform the search.</param>
    /// <param name="attributeName">The attribute name.</param>
    public static IZptElement[] SearchChildrenByTalAttribute(this IZptElement element, string attributeName)
    {
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      return element.SearchChildrenByAttribute(ZptConstants.Tal.Namespace, attributeName);
    }

    /// <summary>
    /// Recursively searches for TAL attributes and removes them from their parent element.
    /// </summary>
    /// <param name="element">The element from which to perform the purge.</param>
    public static void PurgeTalAttributes(this IZptElement element)
    {
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      element.PurgeAttributes(ZptConstants.Tal.Namespace);
    }

    /// <summary>
    /// Recursively searches for elements in the TAL namespace and removes them from their parent element.
    /// </summary>
    /// <param name="element">The element from which to perform the purge.</param>
    public static void PurgeTalElements(this IZptElement element)
    {
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      element.PurgeElements(ZptConstants.Tal.Namespace);
    }

    #endregion

    #region source annotation extension methods

    /// <summary>
    /// Marks the given element as being imported into its parent document (IE: it represents a context switch).
    /// </summary>
    /// <param name="element">The element to mark as imported.</param>
    public static void MarkAsImported(this IZptElement element)
    {
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      element.SetAttribute(ZptConstants.SourceAnnotation.Namespace,
                           ZptConstants.SourceAnnotation.ElementIsImported,
                           Boolean.TrueString);
    }

    /// <summary>
    /// Recursively searches for source annotation attributes and removes them from their parent element.
    /// </summary>
    /// <param name="element">The element from which to perform the purge.</param>
    public static void PurgeSourceAnnotationAttributes(this IZptElement element)
    {
      if(element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      element.PurgeAttributes(ZptConstants.SourceAnnotation.Namespace);
    }

    #endregion
  }
}

