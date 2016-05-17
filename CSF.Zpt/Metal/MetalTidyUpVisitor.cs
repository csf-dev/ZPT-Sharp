using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Metal
{
  /// <summary>
  /// Context visitor that tidies up METAL elements, removing any elements or attributes that should not be rendered.
  /// </summary>
  public class MetalTidyUpVisitor : NoOpVisitor
  {
    #region overrides

    /// <summary>
    /// Visits a rendering context and returns a collection of contexts which represent the result of that visit.
    /// </summary>
    /// <returns>The rendering contexts instances which are exposed after the visiting process is complete.</returns>
    /// <param name="context">The rendering context to visit.</param>
    public override RenderingContext[] VisitContext(RenderingContext context)
    {
      context.Element.PurgeMetalElements();
      context.Element.PurgeMetalAttributes();

      return new [] { context };
    }

    #endregion
  }
}

