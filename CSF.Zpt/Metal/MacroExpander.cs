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
    private SourceAnnotator _annotator;

    #endregion

    #region methods

    /// <summary>
    /// Expands the given context, replacing it with a new context - representing a macro - if it is required.
    /// </summary>
    /// <param name="context">The context to expand.</param>
    public RenderingContext Expand(RenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      RenderingContext output;

      var macro = _macroFinder.GetUsedMacro(context);
      if(macro != null)
      {
        output = this.ExpandAndReplace(context, macro);
        _annotator.ProcessAnnotation(output,
                                     originalContext: context,
                                     replacementContext: output.CreateSiblingContext(macro));
        LogMacroUsage(macro, context.Element);
      }
      else
      {
        output = context;
        _annotator.ProcessAnnotation(context);
      }

      return output;
    }

    /// <summary>
    /// Expands the given macro and uses it to replace the element exposed by the given context.
    /// </summary>
    /// <param name="context">The context to expand.</param>
    /// <param name="macro">The macro element to replace the original.</param>
    public RenderingContext ExpandAndReplace(RenderingContext context, IZptElement macro)
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

      this.FillSlots(context, extendedMacro);

      return context.CreateSiblingContext(extendedMacro);
    }

    /// <summary>
    /// Fills slots defined in the <paramref name="macro"/> with content defined in the <paramref name="sourceContext"/>.
    /// </summary>
    /// <param name="sourceContext">Source rendering context.</param>
    /// <param name="macro">The macro providing the slots to fill.</param>
    private void FillSlots(RenderingContext sourceContext, IZptElement macro)
    {
      if(sourceContext == null)
      {
        throw new ArgumentNullException(nameof(sourceContext));
      }
      if(macro == null)
      {
        throw new ArgumentNullException(nameof(macro));
      }

      var slotsToHandle = (from defineSlot in this.GetElementsByValue(macro, ZptConstants.Metal.DefineSlotAttribute)
                           join fillSlot in this.GetElementsByValue(sourceContext.Element, ZptConstants.Metal.FillSlotAttribute)
                           on defineSlot.Key equals fillSlot.Key
                           select new { Slot = sourceContext.CreateSiblingContext(defineSlot.Value),
                                        Filler = sourceContext.CreateSiblingContext(fillSlot.Value) });

      foreach(var replacement in slotsToHandle)
      {
        var replacementElement = replacement.Slot.Element.ReplaceWith(replacement.Filler.Element);
        var replacementContext = replacement.Filler.CreateSiblingContext(replacementElement);
        _annotator.ProcessAnnotation(replacementContext,
                                     originalContext: replacement.Slot,
                                     replacementContext: replacement.Filler);
        LogSlotFilling(replacement.Slot.Element, replacement.Filler.Element);
      }
    }

    /// <summary>
    /// Gets a collection of child elements which are decorated by METAL attributes.  These elements are indexed by the
    /// value of that attribute.
    /// </summary>
    /// <returns>A collection of elements, and their attribute values.</returns>
    /// <param name="rootElement">The root element from which to search.</param>
    /// <param name="desiredAttribute">The name of the desired attribute.</param>
    private IDictionary<string,IZptElement> GetElementsByValue(IZptElement rootElement,
                                                           string desiredAttribute)
    {
      var output = rootElement.SearchChildrenByMetalAttribute(desiredAttribute)
        .Select(x => new {
          Element = x,
          Attribute = x.GetMetalAttribute(desiredAttribute)
        });

      return output
        .ToDictionary(k => k.Attribute.Value, v => v.Element);
    }

    /// <summary>
    /// Applies METAL macro extension, recursively extending the given macro where applicable.
    /// </summary>
    /// <returns>A rendering context, exposing the METAL macro element, after all required extension has been applied.</returns>
    /// <param name="context">The context exposing the macro element to expand.</param>
    private RenderingContext ApplyMacroExtension(RenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      var extended = _macroFinder.GetExtendedMacro(context);
      RenderingContext output;

      if(extended != null)
      {
        LogMacroExtension(context.Element, extended);
        output = ExpandAndReplace(context, extended);
      }
      else
      {
        output = context;
      }

      return output;
    }

    private void LogMacroUsage(IZptElement defineMacro, IZptElement useMacro)
    {
      ZptConstants.TraceSource.TraceEvent(System.Diagnostics.TraceEventType.Verbose,
                                          4,
                                          Resources.LogMessageFormats.MacroUsage,
                                          defineMacro.GetMetalAttribute(ZptConstants.Metal.DefineMacroAttribute).Value,
                                          useMacro.GetFullFilePathAndLocation(),
                                          defineMacro.GetFullFilePathAndLocation(),
                                          nameof(MacroExpander),
                                          nameof(LogMacroUsage));
    }

    private void LogMacroExtension(IZptElement defineMacro, IZptElement extendedMacro)
    {
      ZptConstants.TraceSource.TraceEvent(System.Diagnostics.TraceEventType.Verbose,
                                          4,
                                          Resources.LogMessageFormats.MacroExtension,
                                          defineMacro.GetMetalAttribute(ZptConstants.Metal.ExtendMacroAttribute).Value,
                                          extendedMacro.GetFullFilePathAndLocation(),
                                          defineMacro.GetFullFilePathAndLocation(),
                                          nameof(MacroExpander),
                                          nameof(LogMacroExtension));
    }

    private void LogSlotFilling(IZptElement defineSlot, IZptElement fillSlot)
    {
      ZptConstants.TraceSource.TraceEvent(System.Diagnostics.TraceEventType.Verbose,
                                          4,
                                          Resources.LogMessageFormats.SlotFilling,
                                          fillSlot.GetMetalAttribute(ZptConstants.Metal.FillSlotAttribute).Value,
                                          defineSlot.GetFullFilePathAndLocation(),
                                          fillSlot.GetFullFilePathAndLocation(),
                                          nameof(MacroExpander),
                                          nameof(LogSlotFilling));
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Metal.MacroExpander"/> class.
    /// </summary>
    /// <param name="finder">A macro finder instance, or a null reference (in which case one will be constructed).</param>
    /// <param name="annotator">A source annotator instance, or a null reference (in which case one will be constructed).</param>
    public MacroExpander(MacroFinder finder = null,
                         SourceAnnotator annotator = null)
    {
      _macroFinder = finder?? new MacroFinder();
      _annotator = annotator?? new SourceAnnotator();
    }

    #endregion
  }
}

