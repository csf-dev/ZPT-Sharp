using System;
using CSF.Zpt.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Zpt.Metal
{
  /// <summary>
  /// Default implementation of <see cref="IMacroSubstituter"/>.
  /// </summary>
  public class MacroSubstituter : IMacroSubstituter
  {
    #region methods

    /// <summary>
    /// Makes the substitutions from the macro into the given source.
    /// </summary>
    /// <remarks>
    /// <para>
    /// These substitutions include replacing the source context with the content of the macro, but also filling
    /// slots which have been defined by the macro and for which filler content is found within the source context.
    /// </para>
    /// </remarks>
    /// <returns>A rendering context representing the result of the substitutions.</returns>
    /// <param name="sourceContext">The source context, from the original document (to be replaced).</param>
    /// <param name="macroContext">The macro context from which to draw replacements.</param>
    public IRenderingContext MakeSubstitutions(IRenderingContext sourceContext,
                                               IRenderingContext macroContext,
                                               IList<IRenderingContext> macroStack,
                                               bool isMacroExtension)
    {
      var slotsToFill = GetSlotsToFill(sourceContext, macroContext, macroStack);
      return MakeSubstitutions(sourceContext, macroContext, slotsToFill, isMacroExtension);
    }

    public IRenderingContext MakeSubstitutions(IRenderingContext sourceContext,
                                               IRenderingContext macroContext,
                                               IEnumerable<SlotToFill> slotsToFill,
                                               bool isMacroExtension)
    {
      foreach(var slotAndFiller in slotsToFill)
      {
        FillSlot(slotAndFiller, isMacroExtension);
      }

      var replacedSourceElement = sourceContext.Element.ReplaceWith(macroContext.Element);
      return sourceContext.CreateSiblingContext(replacedSourceElement);
    }

    public void FillSlot(SlotToFill slotAndFiller, bool isMacroExtension)
    {
      var slot = slotAndFiller.Slot.Element;
      var filler = slotAndFiller.Filler.Element;

      var fillSlotAttribute = slot.GetMetalAttribute(ZptConstants.Metal.FillSlotAttribute);
      var replacement = slot.ReplaceWith(filler);
      if(fillSlotAttribute != null)
      {
        replacement.SetAttribute(ZptConstants.Metal.Namespace,
                                   ZptConstants.Metal.FillSlotAttribute,
                                   fillSlotAttribute.Value);
      }

      if(isMacroExtension)
      {
        var slotIsRedefined = filler.SearchChildrenByMetalAttribute(ZptConstants.Metal.DefineSlotAttribute)
          .Where(x => x.GetAttribute(ZptConstants.Metal.Namespace,
                                       ZptConstants.Metal.DefineSlotAttribute).Value == slotAndFiller.Name)
          .Any();

        if(!slotIsRedefined)
        {
          replacement.SetAttribute(ZptConstants.Metal.Namespace,
                                     ZptConstants.Metal.DefineSlotAttribute,
                                     slotAndFiller.Name);
        }
      }
    }

    public IEnumerable<SlotToFill> GetSlotsToFill(IRenderingContext sourceContext,
                                                  IRenderingContext macroContext,
                                                  IList<IRenderingContext> macroStack)
    {
      var fillers = GetSlotFillers(sourceContext);
      var definedSlots = GetDefinedSlots(macroContext);

      for(int i = 0, len = macroStack.Count; i < len; i++)
      {
        var extendedParent = macroStack[i];
        var uniqueUnfilledSlots = GetDefinedSlots(extendedParent).Where(x => !definedSlots.ContainsKey(x.Key));
        
        foreach(var slot in uniqueUnfilledSlots)
        {
          definedSlots.Add(slot);
        }
      }

      return GetSlotsToFill(sourceContext, fillers, macroContext, definedSlots);
    }

    public IEnumerable<SlotToFill> GetSlotsToFill(IRenderingContext sourceContext,
                                                  IDictionary<string,IZptElement> availableFillers,
                                                  IRenderingContext macroContext,
                                                  IDictionary<string,IZptElement> availableSlotDefinitions)
    {
      return (from slotElement in availableSlotDefinitions
              join fillerElement in availableFillers
                on slotElement.Key equals fillerElement.Key
              let slot = sourceContext.CreateSiblingContext(slotElement.Value)
              let filler = macroContext.CreateSiblingContext(fillerElement.Value)
              select new SlotToFill(slot, filler, slotElement.Key))
        .ToArray();
    }

    public IDictionary<string,IZptElement> GetSlotFillers(IRenderingContext context)
    {
      return GetChildElementsByMetalAttributeValue(context, ZptConstants.Metal.FillSlotAttribute);
    }

    public IDictionary<string,IZptElement> GetDefinedSlots(IRenderingContext context)
    {
      return GetChildElementsByMetalAttributeValue(context, ZptConstants.Metal.DefineSlotAttribute);
    }

    public IDictionary<string,IZptElement> GetChildElementsByMetalAttributeValue(IRenderingContext context,
                                                                                 string metalAttribute)
    {
      return context.Element
        .SearchChildrenByMetalAttribute(metalAttribute)
        .ToDictionary(k => k.GetMetalAttribute(metalAttribute).Value, v => v);
    }

    #endregion
  }
}

