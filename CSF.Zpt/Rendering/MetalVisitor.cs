using System;
using CSF.Zpt.Tales;
using System.Linq;
using System.Collections.Generic;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Visitor type which is used to work upon an <see cref="Element"/> and perform METAL-related functionality.
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
    /// <returns><c>true</c> if the element is to remain; <c>false</c> if not</returns>
    /// <param name="element">The element to visit.</param>
    /// <param name="model">The object model provided as context to the visitor.</param>
    public override bool Visit(Element element, Model model)
    {
      if(element == null)
      {
        throw new ArgumentNullException("element");
      }
      if(model == null)
      {
        throw new ArgumentNullException("model");
      }

      var result = _macroExpander.Expand(element, model);
      return (result == element);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.MetalVisitor"/> class.
    /// </summary>
    public MetalVisitor()
    {
      _macroExpander = new MacroExpander();
    }

    #endregion
  }
}

