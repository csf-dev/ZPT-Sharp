using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Base class for visitor types which visit and potentially modify an <see cref="ZptElement"/> instance.
  /// </summary>
  public abstract class ElementVisitor
  {
    #region methods

    /// <summary>
    /// Visit the given element and perform modifications as required.
    /// </summary>
    /// <returns>A reference to the element which has been visited.  This might be the input <paramref name="element"/> or a replacement.</returns>
    /// <param name="element">The element to visit.</param>
    /// <param name="context">The rendering context provided to the visitor.</param>
    /// <param name="options">The rendering options to use.</param>
    public abstract ZptElement[] Visit(ZptElement element,
                                       RenderingContext context,
                                       RenderingOptions options);

    /// <summary>
    /// Visits the given element, and then recursively visits all of its child elements.
    /// </summary>
    /// <returns>A reference to the element which has been visited.  This might be the input <paramref name="element"/> or a replacement.</returns>
    /// <param name="element">The element to visit.</param>
    /// <param name="context">The rendering context provided to the visitor.</param>
    /// <param name="options">The rendering options to use.</param>
    public virtual ZptElement[] VisitRecursively(ZptElement element,
                                                 RenderingContext context,
                                                 RenderingOptions options)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }
      if(context == null)
      {
        throw new ArgumentNullException("context");
      }

      var output = this.Visit(element, context, options);

      foreach(var item in output)
      {
        var children = item.GetChildElements();
        foreach(var child in children)
        {
          this.VisitRecursively(child, context.CreateChildContext(), options);
        }
      }

      return output;
    }

    /// <summary>
    /// Visits a root element and all of its child element.
    /// </summary>
    /// <returns>The root element(s), after the visiting process is complete.</returns>
    /// <param name="rootElement">Root element.</param>
    /// <param name="context">Context.</param>
    /// <param name="options">Options.</param>
    public virtual ZptElement[] VisitRoot(ZptElement rootElement,
                                          RenderingContext context,
                                          RenderingOptions options)
    {
      return this.VisitRecursively(rootElement, context, options);
    }

    #endregion
  }
}

