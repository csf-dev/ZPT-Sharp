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

    private MetalMacroFinder _macroFinder;

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

      bool output;

      var macro = _macroFinder.GetUsedMacro(element, model);

      if(macro != null)
      {
        macro = element.ReplaceWith(macro);

        var slotsToHandle = (from defineSlot in this.GetElementsByValue(macro, Metal.DefineSlotAttribute)
                             join fillSlot in this.GetElementsByValue(element, Metal.FillSlotAttribute)
                             on defineSlot.Key equals fillSlot.Key
                             select new { Slot = defineSlot.Value, Filler = fillSlot.Value });

        foreach(var replacement in slotsToHandle)
        {
          replacement.Slot.ReplaceWith(replacement.Filler);
        }

        output = false;
      }
      else
      {
        output = true;
      }

      return output;
    }

    /// <summary>
    /// Gets a collection of child elements which are decorated by METAL attributes.  These elements are indexed by the
    /// value of that attribute.
    /// </summary>
    /// <returns>A collection of elements, and their attribute values.</returns>
    /// <param name="rootElement">The root element from which to search.</param>
    /// <param name="desiredAttribute">The name of the desired attribute.</param>
    private IDictionary<string,Element> GetElementsByValue(Element rootElement,
                                                           string desiredAttribute)
    {
      return rootElement.SearchChildrenByMetalAttribute(desiredAttribute)
        .Select(x => new {
          Element = x,
          Attribute = x.GetMetalAttribute(desiredAttribute)
        })
        .ToDictionary(k => k.Attribute.Value, v => v.Element);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.MetalVisitor"/> class.
    /// </summary>
    public MetalVisitor()
    {
      _macroFinder = new MetalMacroFinder();
    }

    #endregion
  }
}

