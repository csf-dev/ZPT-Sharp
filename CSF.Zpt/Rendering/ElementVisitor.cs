using System;
using CSF.Zpt.Tales;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Base class for visitor types which visit and potentially modify an <see cref="Element"/> instance.
  /// </summary>
  public abstract class ElementVisitor
  {
    #region methods

    /// <summary>
    /// Visit the given element and perform modifications as required.
    /// </summary>
    /// <returns>A reference to the element which has been visited.  This might be the input <paramref name="element"/> or a replacement.</returns>
    /// <param name="element">The element to visit.</param>
    /// <param name="model">The object model provided as context to the visitor.</param>
    public abstract Element Visit(Element element, Model model);

    /// <summary>
    /// Visits the given element, and then recursively visits all of its child elements.
    /// </summary>
    /// <param name="element">The element to visit.</param>
    /// <param name="model">The object model provided as context to the visitor.</param>
    public virtual void VisitRecursively(Element element, Model model)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }
      if(model == null)
      {
        throw new ArgumentNullException("model");
      }

      var visited = this.Visit(element, model);

      var children = visited.GetChildElements();
      foreach(var child in children)
      {
        this.VisitRecursively(child, model);
      }
    }

    #endregion
  }
}

