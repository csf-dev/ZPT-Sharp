using System;
using System.Linq;
using System.Collections.Generic;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Metal
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
    /// <param name="context">The rendering context provided to the visitor.</param>
    /// <param name="options">The rendering options to use.</param>
    public override RenderingContext[] Visit(RenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException("context");
      }

      return new [] { context.CreateSiblingContext(_macroExpander.Expand(context.Element, context.MetalModel)) };
    }

    /// <summary>
    /// Visits a root element and all of its child element.
    /// </summary>
    /// <returns>The root element(s), after the visiting process is complete.</returns>
    /// <param name="rootElement">Root element.</param>
    /// <param name="context">Context.</param>
    /// <param name="options">Options.</param>
    public override RenderingContext[] VisitRoot(RenderingContext context)
    {
      var output = base.VisitRoot(context);

      foreach(var item in output)
      {
        item.Element.PurgeMetalAttributes();
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Metal.MetalVisitor"/> class.
    /// </summary>
    /// <param name="expander">The macro expander to use.</param>
    public MetalVisitor(MacroExpander expander = null)
    {
      _macroExpander = expander?? new MacroExpander();
    }

    #endregion
  }
}

