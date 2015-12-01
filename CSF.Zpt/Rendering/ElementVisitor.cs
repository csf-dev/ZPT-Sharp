using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Base class for visitor types which visit and potentially modify an <see cref="ZptElement"/> instance.
  /// </summary>
  public abstract class ElementVisitor
  {
    #region fields

    private RenderingOptions _options;

    #endregion

    #region properties

    /// <summary>
    /// Gets the rendering options.
    /// </summary>
    /// <value>The rendering options.</value>
    protected virtual RenderingOptions RenderingOptions
    {
      get {
        return _options;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Visit the given element and perform modifications as required.
    /// </summary>
    /// <returns>A reference to the element which has been visited.  This might be the input <paramref name="element"/> or a replacement.</returns>
    /// <param name="element">The element to visit.</param>
    /// <param name="model">The object model provided as context to the visitor.</param>
    public abstract ZptElement[] Visit(ZptElement element, Model model);

    /// <summary>
    /// Visits the given element, and then recursively visits all of its child elements.
    /// </summary>
    /// <returns>A reference to the element which has been visited.  This might be the input <paramref name="element"/> or a replacement.</returns>
    /// <param name="element">The element to visit.</param>
    /// <param name="model">The object model provided as context to the visitor.</param>
    public virtual ZptElement[] VisitRecursively(ZptElement element, Model model)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }
      if(model == null)
      {
        throw new ArgumentNullException("model");
      }

      var output = this.Visit(element, model);

      foreach(var item in output)
      {
        var children = item.GetChildElements();
        foreach(var child in children)
        {
          this.VisitRecursively(child, model.CreateChildModel());
        }
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.ElementVisitor"/> class.
    /// </summary>
    /// <param name="options">Rendering options.</param>
    public ElementVisitor(RenderingOptions options)
    {
      _options = options?? RenderingOptions.Default;
    }

    #endregion
  }
}

