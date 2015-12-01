using System;
using System.Linq;
using System.Collections.Generic;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Visitor type which is used to work upon an <see cref="ZptElement"/> and perform METAL-related functionality.
  /// </summary>
  public class MetalVisitor : ElementVisitor
  {
    #region fields

    private MacroExpander _macroExpander;

    #endregion

    #region methods

    /// <summary>
    /// Visit the given element and perform modifications as required.
    /// </summary>
    /// <returns>A reference to the element which has been visited.  This might be the input <paramref name="element"/> or a replacement.</returns>
    /// <param name="element">The element to visit.</param>
    /// <param name="model">The object model provided as context to the visitor.</param>
    public override ZptElement[] Visit(ZptElement element, Model model)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }
      if(model == null)
      {
        throw new ArgumentNullException("model");
      }

      return new [] { _macroExpander.Expand(element, model) };
    }

    /// <summary>
    /// Visits the given element, and then recursively visits all of its child elements.
    /// </summary>
    /// <returns>A reference to the element which has been visited.  This might be the input <paramref name="element"/> or a replacement.</returns>
    /// <param name="element">The element to visit.</param>
    /// <param name="model">The object model provided as context to the visitor.</param>
    public override ZptElement[] VisitRecursively(ZptElement element, Model model)
    {
      var output = base.VisitRecursively(element, model);

      foreach(var item in output)
      {
        item.PurgeMetalAttributes();
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.MetalVisitor"/> class.
    /// </summary>
    /// <param name="expander">The macro expander to use.</param>
    /// <param name="options">The rendering options.</param>
    public MetalVisitor(MacroExpander expander = null,
                        RenderingOptions options = null) : base(options: options)
    {
      _macroExpander = expander?? new MacroExpander();
    }

    #endregion
  }
}

