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
    private log4net.ILog _logger;

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
      }
      else
      {
        output = context;
        if(context.Element.IsRoot && context.RenderingOptions.AddSourceFileAnnotation)
        {
          this.AddAnnotationComment(context, true);
        }
      }

      return output;
    }

    /// <summary>
    /// Expands the given macro and uses it to replace the element exposed by the given context.
    /// </summary>
    /// <param name="context">The context to expand.</param>
    /// <param name="macro">The macro element to replace the original.</param>
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

      _logger.DebugFormat("Element to extend: {0}", macroContext.Element.Name);

      var extendedMacro = context.Element.ReplaceWith(macroContext.Element);
      if(context.RenderingOptions.AddSourceFileAnnotation)
      {
        this.AddAnnotationComment(macroContext);
      }

      this.FillSlots(context, extendedMacro, context.RenderingOptions.AddSourceFileAnnotation);

      return context.CreateSiblingContext(extendedMacro);
    }

    /// <summary>
    /// Fills slots defined in the <paramref name="macro"/> with content defined in the <paramref name="sourceElement"/>.
    /// </summary>
    /// <param name="sourceElement">Source element.</param>
    /// <param name="macro">Macro.</param>
    /// <param name="addAnnotation">A value indicating whether or not source annotation is to be added to the result.</param>
    private void FillSlots(RenderingContext sourceContext, ZptElement macro, bool addAnnotation)
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
                           select new { Slot = defineSlot.Value,
                                        Filler = sourceContext.CreateSiblingContext(fillSlot.Value) });

      foreach(var replacement in slotsToHandle)
      {
        replacement.Slot.ReplaceWith(replacement.Filler.Element);
        if(addAnnotation)
        {
          this.AddAnnotationComment(replacement.Filler);
        }
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
      var output = rootElement.SearchChildrenByMetalAttribute(desiredAttribute)
        .Select(x => new {
          Element = x,
          Attribute = x.GetMetalAttribute(desiredAttribute)
        });

      foreach(var eleAttrPair in output)
      {
        _logger.DebugFormat("Element {0}, attribute: {1}", eleAttrPair.Element.Name, eleAttrPair.Attribute);
      }

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
      return (extended != null)? this.ExpandAndReplace(context, extended) : context;
    }

    /// <summary>
    /// Adds the source annotation comment to an element.
    /// </summary>
    /// <param name="element">The element to annotate.</param>
    private void AddAnnotationComment(RenderingContext context, bool skipLineNumber = false)
    {
      _annotator.AddComment(context, skipLineNumber);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Metal.MacroExpander"/> class.
    /// </summary>
    /// <param name="finder">A macro finder instance, or a null reference (in which case one will be constructed).</param>
    public MacroExpander(MacroFinder finder = null,
                         SourceAnnotator annotator = null)
    {
      _logger = log4net.LogManager.GetLogger(this.GetType());

      _macroFinder = finder?? new MacroFinder();
      _annotator = annotator?? new SourceAnnotator();
    }

    #endregion
  }
}

