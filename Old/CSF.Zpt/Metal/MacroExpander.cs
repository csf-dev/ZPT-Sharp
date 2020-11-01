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
  public class MacroExpander : IMacroExpander
  {
    #region fields

    private readonly IMacroFinder _macroFinder;
    private readonly IMacroSubstituter _normalSubstituter, _extensionSubstitutor;

    #endregion

    #region methods

    /// <summary>
    /// Expands the given context, replacing it with a new context - representing a macro - if it is required.
    /// </summary>
    /// <param name="context">The context to expand.</param>
    public virtual IRenderingContext ExpandMacros(IRenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      IRenderingContext output;

      var macro = GetUsedMacro(context);

      if(macro != null)
      {
        output = ExtendAndSubstitute(context, macro);
      }
      else
      {
        output = HandleNoUsedMacro(context);
      }

      return output;
    }

    /// <summary>
    /// Extends a given macro and then makes substitutions into the source context.
    /// </summary>
    /// <returns>The resultant rendering context after the substitutions are performed.</returns>
    /// <param name="context">The rendering context.</param>
    /// <param name="macro">The macro element to extend and use for substitutions.</param>
    public virtual IRenderingContext ExtendAndSubstitute(IRenderingContext context,
                                                         IZptElement macro)
    {
      IList<IRenderingContext> discarded = new List<IRenderingContext>();
      return ExtendAndSubstitute(context, macro, ref discarded, _normalSubstituter);
    }

    /// <summary>
    /// Extends a given macro and then makes substitutions into the source context.
    /// </summary>
    /// <returns>The resultant rendering context after the substitutions are performed.</returns>
    /// <param name="context">The rendering context.</param>
    /// <param name="macro">The macro element to extend and use for substitutions.</param>
    /// <param name="macroStack">The collection of macros passed through to get to this point.</param>
    /// <param name="substitutionStrategy">The macro substitution strategy to use.</param>
    public virtual IRenderingContext ExtendAndSubstitute(IRenderingContext context,
                                                         IZptElement macro,
                                                         ref IList<IRenderingContext> macroStack,
                                                         IMacroSubstituter substitutionStrategy)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }
      if(macro == null)
      {
        throw new ArgumentNullException(nameof(macro));
      }
      if(macroStack == null)
      {
        throw new ArgumentNullException(nameof(macroStack));
      }
      if(substitutionStrategy == null)
      {
        throw new ArgumentNullException(nameof(substitutionStrategy));
      }

      var macroContext = context.CreateSiblingContext(macro);
      macroStack.Add(macroContext);

      var extendedContext = GetFullyExtendedContext(macroContext, ref macroStack);

      return substitutionStrategy.MakeSubstitutions(context, extendedContext, macroStack);
    }

    /// <summary>
    /// Takes appropriate actions when there is no use-macro directive present in a given context.
    /// </summary>
    /// <returns>The exposed context (which will be the same as the input context).</returns>
    /// <param name="context">The rendering context.</param>
    public virtual IRenderingContext HandleNoUsedMacro(IRenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      return context;
    }

    /// <summary>
    /// Gets a reference to the macro used by a given rendering context, if any.
    /// </summary>
    /// <returns>The used macro element, or a <c>null</c> reference if there is no used macro.</returns>
    /// <param name="context">The rendering context.</param>
    public IZptElement GetUsedMacro(IRenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      return _macroFinder.GetUsedMacro(context);
    }

    /// <summary>
    /// Gets a reference to a 'parent' macro to be extended by the given context, if any.
    /// </summary>
    /// <returns>The to be extended by the current context, or a <c>null</c> reference if there is no macro extension.</returns>
    /// <param name="context">The rendering context.</param>
    public IZptElement GetExtendedMacro(IRenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      return _macroFinder.GetExtendedMacro(context);
    }

    /// <summary>
    /// Gets an instance of <see cref="IRenderingContext"/> from a context instance representing a METAL macro,
    /// representing a fully-extended chain of macros.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method fully applies METAL extension to the given macro context.  If the macro extends another then the
    /// fully chain of extensions is applied here.  If it does not then the input parameter is returned unchanged.
    /// </para>
    /// </remarks>
    /// <returns>The fully extended context.</returns>
    /// <param name="macroContext">A rendering context representing a macro which might extend other macros.</param>
    /// <param name="macroStack">The collection of macros passed through to get to this point.</param>
    public IRenderingContext GetFullyExtendedContext(IRenderingContext macroContext,
                                                     ref IList<IRenderingContext> macroStack)
    {
      if(macroContext == null)
      {
        throw new ArgumentNullException(nameof(macroContext));
      }

      var extendedMacro = GetExtendedMacro(macroContext);

      if(extendedMacro == null)
      {
        return macroContext;
      }

      return ExtendAndSubstitute(macroContext, extendedMacro, ref macroStack, _extensionSubstitutor);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Metal.MacroExpander"/> class.
    /// </summary>
    public MacroExpander() : this(null,  null, null) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Metal.MacroExpander"/> class.
    /// </summary>
    /// <param name="finder">A macro finder instance, or a null reference (in which case one will be constructed).</param>
    /// <param name="substituter">A substituter service for regular macro substitution, or a null reference (in which case one will be constructed).</param>
    /// <param name="extensionSubstitutor">A substituter service for macro extension, or a null reference (in which case one will be constructed).</param>
    public MacroExpander(IMacroFinder finder = null,
                         IMacroSubstituter substituter = null,
                         IMacroSubstituter extensionSubstitutor = null)
    {
      _macroFinder = finder?? new MacroFinder();
      _normalSubstituter = substituter?? new MacroSubstituter();
      _extensionSubstitutor = extensionSubstitutor?? new MacroExtensionSubstitutor();
    }

    #endregion
  }
}

