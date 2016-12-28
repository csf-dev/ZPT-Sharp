using System;
using CSF.Zpt.Rendering;
using System.Collections.Generic;
using System.Linq;
using CSF.Zpt.SourceAnnotation;

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
    /// <param name="macroStack">A collection of macros which have been passed-through.</param>
    public virtual IRenderingContext MakeSubstitutions(IRenderingContext sourceContext,
                                                       IRenderingContext macroContext,
                                                       IList<IRenderingContext> macroStack)
    {
      var slotsToFill = GetSlotsToFill(sourceContext, macroContext, macroStack);

      macroContext.Element.RecursivelyCacheSourceInformationInAttributes();

      FillSlots(slotsToFill);

      return ReplaceMacroElement(sourceContext, macroContext);
    }

    /// <summary>
    /// Replaces the defined macro on the source context with the macro context and returns the result. 
    /// </summary>
    /// <returns>The rendering context for the replacement.</returns>
    /// <param name="sourceContext">The source context (which uses a macro).</param>
    /// <param name="macroContext">The macro context (which defines the macro).</param>
    public virtual IRenderingContext ReplaceMacroElement(IRenderingContext sourceContext,
                                                         IRenderingContext macroContext)
    {
      var replacement = sourceContext.Element.ReplaceWith(macroContext.Element);

      replacement.MarkAsImported();

      return sourceContext.CreateSiblingContext(replacement);
    }

    /// <summary>
    /// Fills all of the given slots with their corresponding fillers.
    /// </summary>
    /// <param name="slotsToFill">The slots to fill.</param>
    public virtual void FillSlots(IEnumerable<SlotToFill> slotsToFill)
    {
      if(slotsToFill == null)
      {
        throw new ArgumentNullException(nameof(slotsToFill));
      }

      foreach(var slotAndFiller in slotsToFill)
      {
        FillSlot(slotAndFiller);
      }
    }

    /// <summary>
    /// Fills a single slot with its filler.
    /// </summary>
    /// <returns>The element created by the operation.</returns>
    /// <param name="slotAndFiller">The slot and its filler.</param>
    public virtual IZptElement FillSlot(SlotToFill slotAndFiller)
    {
      var slot = slotAndFiller.Slot.Element;
      var filler = slotAndFiller.Filler.Element;

      filler.RecursivelyCacheSourceInformationInAttributes();

      var fillSlotAttribute = slot.GetMetalAttribute(ZptConstants.Metal.FillSlotAttribute);
      var replacement = slot.ReplaceWith(filler);

      if(fillSlotAttribute != null)
      {
        replacement.SetAttribute(ZptConstants.Metal.Namespace,
                                   ZptConstants.Metal.FillSlotAttribute,
                                   fillSlotAttribute.Value);
      }

      replacement.MarkAsImported();

      return replacement;
    }

    /// <summary>
    /// Gets a collection of the slots which should be filled, and their corresponding fillers.
    /// </summary>
    /// <returns>A collection of <see cref="SlotToFill"/>.</returns>
    /// <param name="sourceContext">The source context, from the original document (to be replaced).</param>
    /// <param name="macroContext">The macro context from which to draw replacements.</param>
    /// <param name="macroStack">A collection of macros which have been passed-through.</param>
    public virtual IEnumerable<SlotToFill> GetSlotsToFill(IRenderingContext sourceContext,
                                                  IRenderingContext macroContext,
                                                  IList<IRenderingContext> macroStack)
    {
      var fillers = GetSlotFillers(sourceContext);
      var definedSlots = GetDefinedSlots(macroContext);

      var slotsDefinedInParentMacros = GetDefinedSlots(macroStack, definedSlots.Keys);
      foreach(var slot in slotsDefinedInParentMacros)
      {
        definedSlots.Add(slot);
      }

      return GetSlotsToFill(sourceContext, fillers, macroContext, definedSlots);
    }

    /// <summary>
    /// Gets a collection of <see cref="SlotToFill"/> from the given source information.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This operation constructs and executes a query across the slots and fillers provided, which matches each filler
    /// to a slot.
    /// </para>
    /// </remarks>
    /// <returns>A collection of the slots matched with fillers.</returns>
    /// <param name="sourceContext">The source rendering context.</param>
    /// <param name="availableFillers">A collection of elements in the source context representing slot fillers.</param>
    /// <param name="macroContext">The macro rendering context.</param>
    /// <param name="availableSlotDefinitions">A collection of elements in the macro context representing slot definitions.</param>
    public virtual IEnumerable<SlotToFill> GetSlotsToFill(IRenderingContext sourceContext,
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

    /// <summary>
    /// Gets a collection of all of the elements which have the METAL <c>fill-slot</c> attribute defined.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The returned dictionary is indexed by the value of the METAL <c>fill-slot</c> attribute.
    /// </para>
    /// </remarks>
    /// <returns>The elements found.</returns>
    /// <param name="context">The rendering context within which to search.</param>
    public virtual IDictionary<string,IZptElement> GetSlotFillers(IRenderingContext context)
    {
      return GetChildElementsByMetalAttributeValue(context, ZptConstants.Metal.FillSlotAttribute);
    }

    /// <summary>
    /// Gets a collection of all of the elements which have the METAL <c>define-slot</c> attribute defined.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The returned dictionary is indexed by the value of the METAL <c>define-slot</c> attribute.
    /// </para>
    /// </remarks>
    /// <returns>The elements found.</returns>
    /// <param name="context">The rendering context within which to search.</param>
    public virtual IDictionary<string,IZptElement> GetDefinedSlots(IRenderingContext context)
    {
      return GetChildElementsByMetalAttributeValue(context, ZptConstants.Metal.DefineSlotAttribute);
    }

    /// <summary>
    /// Gets a collection of all of the elements which have the METAL <c>define-slot</c> attribute defined.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The returned dictionary is indexed by the value of the METAL <c>define-slot</c> attribute.
    /// </para>
    /// </remarks>
    /// <returns>The elements found.</returns>
    /// <param name="macroStack">A collection of macros which have been passed-through.</param>
    /// <param name="slotsFoundSoFar">A collection of the names of the slots found so far.</param>
    public virtual IDictionary<string,IZptElement> GetDefinedSlots(IList<IRenderingContext> macroStack,
                                                                   IEnumerable<string> slotsFoundSoFar)
    {
      IDictionary<string,IZptElement> output = new Dictionary<string,IZptElement>();

      for(int i = 0, len = macroStack.Count; i < len; i++)
      {
        var parentMacro = macroStack[i];

        var newlyFoundSlots = (from slot in GetDefinedSlots(parentMacro)
                               let slotName = slot.Key
                               where
                                  !slotsFoundSoFar.Contains(slotName)
                                  && !output.ContainsKey(slotName)
                               select slot);

        foreach(var slot in newlyFoundSlots)
        {
          output.Add(slot);
        }
      }

      return output;
    }

    /// <summary>
    /// Gets a collection of elements indexed by the value of a given METAL attribute.
    /// </summary>
    /// <returns>The elements found.</returns>
    /// <param name="context">The rendering context within which to search.</param>
    /// <param name="metalAttribute">The name of the METAL attribute for which to search.</param>
    public virtual IDictionary<string,IZptElement> GetChildElementsByMetalAttributeValue(IRenderingContext context,
                                                                                         string metalAttribute)
    {
      return context.Element
        .SearchChildrenByMetalAttribute(metalAttribute)
        .ToDictionary(k => k.GetMetalAttribute(metalAttribute).Value, v => v);
    }

    #endregion
  }
}

