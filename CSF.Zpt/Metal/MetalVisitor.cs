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

      return new [] { _macroExpander.Expand(context) };
    }

    /// <summary>
    /// Visits a rendering context and returns a collection of contexts which represent the result of that visit.
    /// </summary>
    /// <remarks>
    /// <para>
    /// After the base class functionality has finished executing, this visit purges the elements (exposed by their
    /// contexts) of all METAL attributes.
    /// </para>
    /// </remarks>
    /// <returns>The rendering contexts instances which are exposed after the visiting process is complete.</returns>
    /// <param name="context">The rendering context to visit.</param>
    public override RenderingContext[] VisitContext(RenderingContext context)
    {
      var output = base.VisitContext(context);

      foreach(var item in output)
      {
        item.Element.PurgeMetalAttributes();
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

