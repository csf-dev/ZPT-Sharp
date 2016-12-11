using System;
using CSF.Zpt.Rendering;
using System.Linq;

namespace CSF.Zpt.Metal
{
  /// <summary>
  /// Implementation of <see cref="IMacroSubstituter"/> suitable for use in macro extension.
  /// </summary>
  public class MacroExtensionSubstitutor : MacroSubstituter
  {
    /// <summary>
    /// Fills a single slot with its filler.
    /// </summary>
    /// <returns>The element created by the operation.</returns>
    /// <param name="slotAndFiller">The slot and its filler.</param>
    public override IZptElement FillSlot(SlotToFill slotAndFiller)
    {
      var slot = slotAndFiller.Slot.Element;
      var filler = slotAndFiller.Filler.Element;

      var replacement = base.FillSlot(slotAndFiller);

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

      return replacement;
    }
  }
}

