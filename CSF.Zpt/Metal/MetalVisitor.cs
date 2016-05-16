using System;
using System.Linq;
using System.Collections.Generic;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Metal
{
  /// <summary>
  /// Implementation of <see cref="IContextVisitor"/> which performs METAL-related functionality.
  /// </summary>
  public class MetalVisitor : ContextVisitorBase
  {
    #region fields

    private MacroExpander _macroExpander;

    #endregion

    #region methods

    /// <summary>
    /// Visit the given context and return a collection of the resultant contexts.
    /// </summary>
    /// <returns>Zero or more <see cref="RenderingContext"/> instances, determined by the outcome of this visit.</returns>
    /// <param name="context">The rendering context to visit.</param>
    public override RenderingContext[] Visit(RenderingContext context)
    {
      if(context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      ZptAttribute attrib;
      if((attrib = context.GetMetalAttribute(ZptConstants.Metal.DefineMacroAttribute)) != null)
      {
        context.MetalModel.AddGlobal(attrib.Value, context.Element);
      }

      return new [] { _macroExpander.Expand(context) };
    }

    /// <summary>
    /// Visit the given context, as well as its child contexts, and return a collection of the resultant contexts.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This operation performs the same work as <see cref="Visit"/>, but it then visits all of the resultant contexts,
    /// recursively moving down the exposed document tree, visiting each context in turn.
    /// </para>
    /// </remarks>
    /// <returns>Zero or more <see cref="RenderingContext"/> instances, determined by the outcome of this visit.</returns>
    /// <param name="context">The rendering context to visit.</param>
    public override RenderingContext[] VisitRecursively(RenderingContext context)
    {
      var output = base.VisitRecursively(context);

      foreach(var item in output)
      {
        item.Element.PurgeMetalAttributes();
        if(item.Element.IsInNamespace(ZptConstants.Metal.Namespace))
        {
          item.Element.Omit();
        }
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

