using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Zpt.Rendering;
using CSF.Zpt.Resources;

namespace CSF.Zpt.Metal
{
  /// <summary>
  /// Expands a METAL macro and splices it into its source document.
  /// </summary>
  public class MacroExpander
  {
    #region fields

    private MacroFinder _macroFinder;

    #endregion

    #region methods

    /// <summary>
    /// Expands the given element, replacing it with a macro if it is required.
    /// </summary>
    /// <param name="original">The original element.</param>
    /// <param name="model">The METAL object model.</param>
    public RenderingContext Expand(RenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      var macro = _macroFinder.GetUsedMacro(context.Element, context.MetalModel);
      return (macro != null)? this.ExpandAndReplace(context, macro) : context;
    }

    /// <summary>
    /// Expands the given macro and uses it to replace the original element.
    /// </summary>
    /// <param name="original">The original element.</param>
    /// <param name="macro">The macro element to replace the original.</param>
    /// <param name="model">The METAL object model.</param>
    public RenderingContext ExpandAndReplace(RenderingContext context, ZptElement macro)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }
      if(macro == null)
      {
        throw new ArgumentNullException(nameof(macro));
      }
      if(macro.GetMetalAttribute(ZptConstants.Metal.DefineMacroAttribute) == null)
      {
        string message = String.Format(ExceptionMessages.MetalMacroMustBeDefined,
                                       ZptConstants.Metal.DefineMacroAttribute,
                                       ZptConstants.Metal.Namespace);
        throw new ArgumentException(message, "macro");
      }

      var macroContext = context.CreateSiblingContext(macro);

      macroContext = this.ApplyMacroExtension(macroContext);
      var extendedMacro = context.Element.ReplaceWith(macroContext.Element);
      this.FillSlots(context.Element, extendedMacro);

      return context.CreateSiblingContext(extendedMacro);
    }

    /// <summary>
    /// Fills slots defined in the <paramref name="macro"/> with content defined in the <paramref name="sourceElement"/>.
    /// </summary>
    /// <param name="sourceElement">Source element.</param>
    /// <param name="macro">Macro.</param>
    private void FillSlots(ZptElement sourceElement, ZptElement macro)
    {
      if(sourceElement == null)
      {
        throw new ArgumentNullException(nameof(sourceElement));
      }
      if(macro == null)
      {
        throw new ArgumentNullException(nameof(macro));
      }

      var slotsToHandle = (from defineSlot in this.GetElementsByValue(macro, ZptConstants.Metal.DefineSlotAttribute)
                           join fillSlot in this.GetElementsByValue(sourceElement, ZptConstants.Metal.FillSlotAttribute)
                           on defineSlot.Key equals fillSlot.Key
                           select new { Slot = defineSlot.Value, Filler = fillSlot.Value });

      foreach(var replacement in slotsToHandle)
      {
        replacement.Slot.ReplaceWith(replacement.Filler);
      }
    }

    /// <summary>
    /// Gets a collection of child elements which are decorated by METAL attributes.  These elements are indexed by the
    /// value of that attribute.
    /// </summary>
    /// <returns>A collection of elements, and their attribute values.</returns>
    /// <param name="rootElement">The root element from which to search.</param>
    /// <param name="desiredAttribute">The name of the desired attribute.</param>
    private IDictionary<string,ZptElement> GetElementsByValue(ZptElement rootElement,
                                                           string desiredAttribute)
    {
      return rootElement.SearchChildrenByMetalAttribute(desiredAttribute)
        .Select(x => new {
          Element = x,
          Attribute = x.GetMetalAttribute(desiredAttribute)
        })
        .ToDictionary(k => k.Attribute.Value, v => v.Element);
    }

    /// <summary>
    /// Applies METAL macro extension, recursively extending the given macro where applicable.
    /// </summary>
    /// <returns>The METAL macro element, after all required extension has been applied.</returns>
    /// <param name="macro">The macro element.</param>
    /// <param name="model">The METAL object model.</param>
    private RenderingContext ApplyMacroExtension(RenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      var extended = _macroFinder.GetExtendedMacro(context.Element, context.MetalModel);
      return (extended != null)? this.ExpandAndReplace(context, extended) : context;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Metal.MacroExpander"/> class.
    /// </summary>
    /// <param name="finder">A macro finder instance, or a null reference (in which case one will be constructed).</param>
    public MacroExpander(MacroFinder finder = null)
    {
      _macroFinder = finder?? new MacroFinder();
    }

    #endregion
  }
}

