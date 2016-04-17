using System;
using System.Linq;

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
    public abstract RenderingContext[] Visit(RenderingContext context);

    /// <summary>
    /// Visits the given element, and then recursively visits all of its child elements.
    /// </summary>
    /// <returns>A reference to the element which has been visited.  This might be the input <paramref name="element"/> or a replacement.</returns>
    /// <param name="element">The element to visit.</param>
    /// <param name="context">The rendering context provided to the visitor.</param>
    /// <param name="options">The rendering options to use.</param>
    public virtual RenderingContext[] VisitRecursively(RenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException("context");
      }

      var output = this.Visit(context);

      var children = output.SelectMany(x => x.GetChildContexts());
      foreach(var child in children)
      {
        this.VisitRecursively(child);
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
    public virtual RenderingContext[] VisitRoot(RenderingContext context)
    {
      return this.VisitRecursively(context);
    }

    #endregion
  }
}

